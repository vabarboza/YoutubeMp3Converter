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
                MessageBox.Show("N�o h� links para baixar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(tbLocation.Text))
            {
                MessageBox.Show("Por favor, selecione uma pasta de sa�da.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "downloads");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            labelStatus.Text = "Iniciando processo de download em sequ�ncia...";
            try
            {
                List<string> videoUrls = listBoxLinks.Items.Cast<string>().ToList();
                await DownloadVideosFromListBox(videoUrls, tbLocation.Text);
                labelStatus.Text = "Downloads em sequ�ncia conclu�dos com sucesso.";
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
                    listBoxLinks.Items.Remove(videoUrl); // Remove o link da lista ap�s o download
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocorreu um erro ao baixar o v�deo {videoUrl}: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async Task DownloadAndConvertVideo(string videoUrl, string outputDirectory)
        {
            // Se o caminho da pasta de sa�da for nulo ou vazio, saia do m�todo
            if (string.IsNullOrEmpty(outputDirectory))
            {
                MessageBox.Show("Por favor, selecione uma pasta de sa�da.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var youtube = new YoutubeClient();

            // Obt�m o ID do v�deo a partir da URL
            labelStatus.Text = "Obtendo ID do video.";
            var video = await youtube.Videos.GetAsync(videoUrl);
            var videoId = video.Id;

            // Obt�m os streams do v�deo
            labelStatus.Text = "Obtendo informa��es do video.";
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
            var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestBitrate();

            // Obt�m o t�tulo do v�deo
            var videoTitle = await GetVideoTitle(videoUrl);

            // Define o caminho do arquivo tempor�rio de download
            labelStatus.Text = "Definindo camnho do video.";
            var tempFilePath = Path.Combine(outputDirectory, $"{videoTitle}.{streamInfo.Container.Name}");

            // Obt�m o tamanho do v�deo em bytes
            long videoSize = streamInfo.Size.Bytes;

            // Prepara a ProgressBar
            pbDownload.Maximum = 100; // Vamos usar 100 como o valor m�ximo da ProgressBar
            pbDownload.Value = 0;

            // Baixa o v�deo
            var progressHandler = new Progress<double>(progress =>
            {
                // Atualiza a ProgressBar com base no progresso do download
                pbDownload.Value = (int)(progress * 100);
            });

            labelStatus.Text = "Iniciando download do video.";
            await youtube.Videos.Streams.DownloadAsync(streamInfo, tempFilePath, progressHandler);

            // Define o caminho do arquivo de sa�da MP3 com o t�tulo do v�deo
            var outputFilePath = Path.Combine(outputDirectory, $"{videoTitle}.mp3");

            // Chama o m�todo para converter o v�deo para MP3
            await ConvertVideoToMP3(tempFilePath, outputFilePath, videoTitle);

            // Remove o arquivo tempor�rio
            if (!cbSaveVideo.Checked)
            {
                File.Delete(tempFilePath);
            }
        }

        private async Task<string> GetVideoTitle(string videoUrl)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(videoUrl);

            // Remove caracteres especiais do t�tulo usando express�o regular
            string title = video.Title;
            title = Regex.Replace(title, @"[^\w\d\s]", ""); // Remove todos os caracteres que n�o s�o letras, n�meros ou espa�os

            return title;
        }

        private async Task ConvertVideoToMP3(string tempFilePath, string outputFilePath, string videoTitle)
        {
            SetStatusLabel("Iniciando convers�o do v�deo para MP3.");

            await Task.Run(() =>
            {
                using (var engine = new Engine())
                {
                    var inputOptions = new ConversionOptions { Seek = TimeSpan.FromSeconds(30) };
                    var outputOptions = new MediaFile { Filename = outputFilePath };

                    engine.Convert(new MediaFile { Filename = tempFilePath }, outputOptions);
                }
            });

            // Ap�s a convers�o, atualiza a interface do usu�rio
            SetStatusLabel("Convers�o conclu�da.");
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
            tbLink.Text = ""; // Limpa o campo de texto ap�s adicionar o link
        }

        private void btnAddVideo_Click(object sender, EventArgs e)
        {
            string videoUrl = tbLink.Text;
            if (string.IsNullOrEmpty(videoUrl))
            {
                MessageBox.Show("Por favor, insira a URL do v�deo do YouTube.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            //Salva arquivo de configura��o
            myConfig.SaveToFile("config.ini");
        }
    }
}
