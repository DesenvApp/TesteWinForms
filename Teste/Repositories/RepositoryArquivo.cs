
using System.Collections.Generic;
using System.Configuration;
using Teste.Interface;
using Teste.Models;

namespace Teste.Repositories
{
    public class RepositoryArquivo : IRepositoryArquivo
    {
        private readonly string strNomeArquivo;
        ModelLine modelLine;
        List<string> lRetorno;

        public RepositoryArquivo()
        {
            strNomeArquivo = ConfigurationManager.AppSettings.Get("NomeArquivo");
            modelLine = new ModelLine();
        }

        public async Task<List<string>> AsyncLerArquivo()
        {
            if (!File.Exists(strNomeArquivo))
            {
                return null;            }

            
            modelLine.ListA = new List<string>();
            modelLine.ListB = new List<string>();

            try
            {
                using (var reader = new StreamReader(strNomeArquivo))
                {

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        if (values.Length > 0)
                        {
                            if (int.TryParse(values[0], out int a))
                            {
                                modelLine.ListA.Add(values[0]);
                            }
                            if (int.TryParse(values[1], out int b))
                            {
                                modelLine.ListB.Add(values[1]);
                            }
                        }
                    }

                    await Task.WhenAll(AsyncListA(), AsyncListB());                 
                }               

                return (await Task.FromResult(lRetorno));
            }
            catch (Exception)
            {
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
