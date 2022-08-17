using System.Collections.Generic;

namespace WebTesteAPI.Services
{
    public class ArquivoRepository
    {
        private readonly IConfiguration _configuration;
        public ArquivoRepository()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        public string CalculaLista(List<string> Lista)
        {
            string sResultado = string.Empty;

            try
            {
                string strNomeArquivo = _configuration.GetSection("ArquivoDestino").Value;

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

                sResultado = "Arquivo enviado com sucesso.";
            }
            catch (Exception ex)
            {
                sResultado = string.Format("Não foi possível enviar o arquivo.{0}{1}", Environment.NewLine, ex.Message);
            }
 
            return sResultado;
        }
    }
}
