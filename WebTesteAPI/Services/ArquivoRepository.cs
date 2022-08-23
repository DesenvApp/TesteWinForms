using Microsoft.Extensions.Logging;
using NLog;
using WebTesteAPI.Controllers;

namespace WebTesteAPI.Services
{
    public class ArquivoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ArquivoController> _logger;

        public ArquivoRepository(ILogger<ArquivoController> logger)
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _logger = logger;   
            
        }

        public string CalculaLista(List<string> Lista)
        {
            string sResultado = string.Empty;

            try
            {
                _logger.LogInformation("Busca o nome do arquivo no appsettings.jon");

                string strNomeArquivo = string.Format("{0}{1}", _configuration.GetSection("PathDestino").Value,
                                                                _configuration.GetSection("ArquivoDestino").Value);

                _logger.LogInformation("Inicio da aplicação da regra de negócio");
                using (var stream = File.CreateText(strNomeArquivo))
                {
                    stream.WriteLine("R");

                    foreach (string s in Lista)
                    {
                        double numero = double.Parse(s);
                        double valor = (0.1 * numero) + numero;
                        stream.WriteLine(valor.ToString());
                    }
                }
                _logger.LogInformation("fim da aplicação da regra de negócio");

                sResultado = "Arquivo enviado com sucesso.";
                _logger.LogInformation(sResultado);
            }
            catch (Exception ex)
            {                
                sResultado = string.Format("Não foi possível enviar o arquivo.{0}{1}", Environment.NewLine, ex.Message);
                _logger.LogInformation(sResultado);
            }
 
            return sResultado;
        }
    }
}
