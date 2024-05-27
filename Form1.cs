using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using SharpConfig;
using System.Text.RegularExpressions;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YoutubeMp3Converter
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (listBoxLinks.Items.Count == 0)
            {
                MessageBox.Show("Não há links para baixar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(tbLocation.Text))
            {
                MessageBox.Show("Por favor, selecione uma pasta de saída.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "downloads");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            labelStatus.Text = "Iniciando processo de download em sequência...";
            try
            {
                List<string> videoUrls = listBoxLinks.Items.Cast<string>().ToList();
                await DownloadVideosFromListBox(videoUrls, tbLocation.Text);
                labelStatus.Text = "Downloads em sequência concluídos com sucesso.";
            }
            catch (Exception ex)
            {
                labelStatus.Text = $"Ocorreu um erro: {ex.Message}";
            }
        }

        private async Task DownloadVideosFromListBox(List<string> videoUrls, string outputDirectory)
        {
            foreach (var videoUrl in videoUrls)
            {
                try
                {
                    await DownloadAndConvertVideo(videoUrl, outputDirectory);
                    listBoxLinks.Items.Remove(videoUrl); // Remove o link da lista após o download
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocorreu um erro ao baixar o vídeo {videoUrl}: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task DownloadAndConvertVideo(string videoUrl, string outputDirectory)
        {
            // Se o caminho da pasta de saída for nulo ou vazio, saia do método
            if (string.IsNullOrEmpty(outputDirectory))
            {
                MessageBox.Show("Por favor, selecione uma pasta de saída.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var youtube = new YoutubeClient();

            // Obtém o ID do vídeo a partir da URL
            labelStatus.Text = "Obtendo ID do video.";
            var video = await youtube.Videos.GetAsync(videoUrl);
            var videoId = video.Id;

            // Obtém os streams do vídeo
            labelStatus.Text = "Obtendo informações do video.";
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
            var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestBitrate();

            // Obtém o título do vídeo
            var videoTitle = await GetVideoTitle(videoUrl);

            // Define o caminho do arquivo temporário de download
            labelStatus.Text = "Definindo camnho do video.";
            var tempFilePath = Path.Combine(outputDirectory, $"{videoTitle}.{streamInfo.Container.Name}");

            // Obtém o tamanho do vídeo em bytes
            long videoSize = streamInfo.Size.Bytes;

            // Prepara a ProgressBar
            pbDownload.Maximum = 100; // Vamos usar 100 como o valor máximo da ProgressBar
            pbDownload.Value = 0;

            // Baixa o vídeo
            var progressHandler = new Progress<double>(progress =>
            {
                // Atualiza a ProgressBar com base no progresso do download
                pbDownload.Value = (int)(progress * 100);
            });

            labelStatus.Text = "Iniciando download do video.";
            await youtube.Videos.Streams.DownloadAsync(streamInfo, tempFilePath, progressHandler);

            // Define o caminho do arquivo de saída MP3 com o título do vídeo
            var outputFilePath = Path.Combine(outputDirectory, $"{videoTitle}.mp3");

            // Chama o método para converter o vídeo para MP3
            await ConvertVideoToMP3(tempFilePath, outputFilePath, videoTitle);

            // Remove o arquivo temporário
            if (!cbSaveVideo.Checked)
            {
                File.Delete(tempFilePath);
            }
        }

        private async Task<string> GetVideoTitle(string videoUrl)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(videoUrl);

            // Remove caracteres especiais do título usando expressão regular
            string title = video.Title;
            title = Regex.Replace(title, @"[^\w\d\s]", ""); // Remove todos os caracteres que não são letras, números ou espaços

            return title;
        }

        private async Task ConvertVideoToMP3(string tempFilePath, string outputFilePath, string videoTitle)
        {
            SetStatusLabel("Iniciando conversão do vídeo para MP3.");

            await Task.Run(() =>
            {
                using (var engine = new Engine())
                {
                    var inputOptions = new ConversionOptions { Seek = TimeSpan.FromSeconds(30) };
                    var outputOptions = new MediaFile { Filename = outputFilePath };

                    engine.Convert(new MediaFile { Filename = tempFilePath }, outputOptions);
                }
            });

            // Após a conversão, atualiza a interface do usuário
            SetStatusLabel("Conversão concluída.");
        }

        private void SetStatusLabel(string text)
        {
            if (labelStatus.InvokeRequired)
            {
                labelStatus.Invoke(new Action(() => labelStatus.Text = text));
            }
            else
            {
                labelStatus.Text = text;
            }
        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFolder = folderDialog.SelectedPath;
                    tbLocation.Text = selectedFolder; // Atualiza o texto na caixa de texto
                }
            }
        }

        private void AddVideoToListBox(string videoUrl)
        {
            listBoxLinks.Items.Add(videoUrl);
            tbLink.Text = ""; // Limpa o campo de texto após adicionar o link
        }

        private void btnAddVideo_Click(object sender, EventArgs e)
        {
            string videoUrl = tbLink.Text;
            if (string.IsNullOrEmpty(videoUrl))
            {
                MessageBox.Show("Por favor, insira a URL do vídeo do YouTube.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AddVideoToListBox(videoUrl);
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            listBoxLinks.Items.Clear();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            string configFilePath = "config.ini";

            if (File.Exists(configFilePath))
            {
                Configuration config = Configuration.LoadFromFile("config.ini");
                Section section = config["General"];

                string outputDirectory = section["directory"].GetValue<string>();
                bool isCheckboxChecked = section["isCheckboxChecked"].GetValue<bool>();

                tbLocation.Text = outputDirectory;
                cbSaveVideo.Checked = isCheckboxChecked;
            }
            else
            {
                CreateConfig();
            }
        }

        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var myConfig = new Configuration();
            myConfig["General"]["directory"].StringValue = tbLocation.Text;
            myConfig["General"]["isCheckboxChecked"].BoolValue = cbSaveVideo.Checked;

            myConfig.SaveToFile("config.ini");
        }

        private void CreateConfig()
        {
            var myConfig = new Configuration();
            myConfig["General"]["directory"].StringValue = "";
            myConfig["General"]["isCheckboxChecked"].BoolValue = false;

            //Salva arquivo de configuração
            myConfig.SaveToFile("config.ini");
        }
    }
}
