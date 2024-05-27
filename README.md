<p align="center">
  <img src="https://raw.githubusercontent.com/vabarboza/YoutubeMp3Converter/master/Resources/image.png" alt="YouTube MP3 Converter Logo">
</p>

# Youtube Mp3 Converter

YoutubeMp3Converter é uma aplicação Windows Forms que permite aos usuários baixar vídeos do YouTube e convertê-los para o formato MP3. A aplicação também salva e carrega configurações, incluindo o diretório de saída e o estado de uma checkbox.

## Funcionalidades

- Adicionar links de vídeos do YouTube a uma lista.
- Baixar e converter vídeos para MP3.
- Monitorar o progresso do download e da conversão usando uma ProgressBar.
- Salvar e carregar configurações do aplicativo em um arquivo `config.ini`.

## Pré-requisitos

- [.NET Core 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MediaToolkit](https://www.nuget.org/packages/MediaToolkit/)
- [YoutubeExplode](https://www.nuget.org/packages/YoutubeExplode/)
- [SharpConfig](https://www.nuget.org/packages/SharpConfig/)

## Instalação

1. Clone o repositório:

    ```sh
    git clone https://github.com/seu-usuario/YoutubeMp3Converter.git
    ```

2. Abra o projeto no Visual Studio.

3. Restaure os pacotes NuGet:

    ```sh
    Install-Package MediaToolkit
    Install-Package YoutubeExplode
    Install-Package SharpConfig
    ```

## Uso

### Interface do Usuário

1. **Adicionar Vídeo**: Insira o URL do vídeo do YouTube no campo `Link do Video` e clique em `Adicionar Video` para adicionar o vídeo à lista `Lista de Links`.
2. **Selecionar Diretório de Saída**: Clique em `Selecionar` para escolher o diretório onde os arquivos MP3 serão salvos. O caminho será exibido no campo `Local de Saida`.
3. **Baixar e Converter**: Clique em `Baixar e Converter` para iniciar o download e a conversão dos vídeos na lista. O progresso será exibido na `Barra de Progresso` e o status será mostrado em `Status`.
4. **Salvar Configurações**: As configurações serão salvas automaticamente ao fechar o aplicativo, incluindo o diretório de saída e o estado da checkbox `Salvar Videos`.

### Arquivo de Configuração

As configurações são salvas em um arquivo `config.ini` no diretório de trabalho do aplicativo. O arquivo contém a configuração do diretório de saída e o estado da checkbox.

Exemplo de `config.ini`:

```ini
[General]
directory = C:\Users\SeuUsuario\Music
isCheckboxChecked = true
```


### Contribuição
Contribuições são bem-vindas! Sinta-se à vontade para abrir uma issue ou enviar um pull request.

### Licença
Este projeto está licenciado sob a licença MIT - veja o arquivo LICENSE para mais detalhes.
