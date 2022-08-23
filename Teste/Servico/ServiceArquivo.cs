using Teste.Interface;
using Teste.Repositories;
using Teste.Models;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Teste.Servico
{
    public class ServiceArquivo
    {
        private readonly IRepositoryArquivo _repositoryArquivo;
        private ILogger _logger;

        public ServiceArquivo(ILogger logger)
        {            
            _logger = logger;
            _repositoryArquivo = new RepositoryArquivo(_logger);
        }   

        public Task<ModelLine> Execute()
        {
           return Task.FromResult(_repositoryArquivo.AsyncLerArquivo());
        }

    }
}
