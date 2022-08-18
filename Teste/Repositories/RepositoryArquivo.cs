
using NLog;
using System.Configuration;
using Teste.Interface;
using Teste.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Teste.Repositories
{
    public class RepositoryArquivo : IRepositoryArquivo
    {
        private readonly string strPath =string.Empty;
        ModelLine modelLine;
        List<string> lRetorno;
        private readonly Logger _logger;

        public RepositoryArquivo(Logger logger)
        {
            strPath = ConfigurationManager.AppSettings.Get("Path");
            modelLine = new ModelLine();  
            _logger = logger;
        }

        public async Task<List<string>> AsyncLerArquivo()
        {
            try
            {
                modelLine.ListA = new List<string>();
                modelLine.ListB = new List<string>();

                string[] arrFiles = Directory.GetFiles(strPath, "*.csv");
                if (arrFiles.Length > 0)
                {
                    var csvconfig = new CsvConfiguration(CultureInfo.CurrentCulture)
                    {
                        HasHeaderRecord = false,
                        Delimiter = ";",
                    };

                    _logger.Info("inicio da leitura do(s) arquivo(s).");

                    foreach (var sFile in arrFiles)
                    {
                        _logger.Info(string.Format("leitura do arquivo: {0}", sFile));
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

                    _logger.Info("fim da leitura do(s) arquivo(s).");
                    _logger.Info("disparando as threads.");

                    await Task.WhenAll(AsyncListA(), AsyncListB());

                    return (await Task.FromResult(lRetorno));
                } 
                else
                {
                    _logger.Info("não há arquivo .csv no diretório.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
                return null;
            }  
        }

        public Task AsyncListA()
        {
            if (modelLine.ListA.Count > 0)
            {
                if (lRetorno == null)
                    lRetorno = new List<string>();

                foreach (string s in modelLine.ListA)
                {
                    lRetorno.Add(s);
                }
            }

            return Task.CompletedTask;
        }

        public Task AsyncListB()
        {
            if (modelLine.ListB.Count > 0)
            {
                if (lRetorno == null)
                    lRetorno = new List<string>();

                foreach (string s in modelLine.ListB)
                {
                    lRetorno.Add(s);
                }
            }

            return Task.CompletedTask;
        }
    }
}
