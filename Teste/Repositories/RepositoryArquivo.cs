
using NLog;
using System.Configuration;
using Teste.Interface;
using Teste.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Microsoft.Extensions.Logging;

namespace Teste.Repositories
{
    public class RepositoryArquivo : IRepositoryArquivo
    {
        private readonly string strPath =string.Empty;
        ModelLine modelLine;
        private readonly ILogger _logger;

        public RepositoryArquivo(ILogger logger)
        {
            strPath = ConfigurationManager.AppSettings.Get("Path");
            modelLine = new ModelLine();  
            _logger = logger;
        }

        public ModelLine? AsyncLerArquivo()
        {
            try
            {
                string[] arrFiles = Directory.GetFiles(strPath, "*.csv");
                if (arrFiles.Length > 0)
                {
                    modelLine.ListA = new List<string>();
                    modelLine.ListB = new List<string>();

                    var csvconfig = new CsvConfiguration(CultureInfo.CurrentCulture)
                    {
                        HasHeaderRecord = false,
                        Delimiter = ";",
                    };

                    _logger.LogInformation("inicio da leitura do(s) arquivo(s).");

                    foreach (var sFile in arrFiles)
                    {
                        _logger.LogInformation(string.Format("leitura do arquivo: {0}", sFile));
                        using var streamReader = File.OpenText(sFile);
                        using var csvreader = new CsvReader(streamReader, csvconfig);

                        while (csvreader.Read())
                        {
                            var ColA = csvreader.GetField(0);
                            var ColB = csvreader.GetField(1);

                            if (!string.IsNullOrEmpty(ColA) && int.TryParse(ColA, out int a))
                            {
                                modelLine.ListA.Add(ColA);
                            }

                            if (!string.IsNullOrEmpty(ColB) && int.TryParse(ColB, out int b))
                            {
                                modelLine.ListA.Add(ColB);
                            }
                        } 
                    }

                    _logger.LogInformation("fim da leitura do(s) arquivo(s).");
                    _logger.LogInformation("disparando as threads.");                 

                    return modelLine;
                } 
                else
                {
                    _logger.LogInformation("não há arquivo .csv no diretório.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return null;
            }  
        }
    }
}
