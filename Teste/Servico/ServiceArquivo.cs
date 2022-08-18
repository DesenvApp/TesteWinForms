using Teste.Interface;
using Teste.Repositories;
using NLog;

namespace Teste.Servico
{
    public class ServiceArquivo
    {
        private readonly IRepositoryArquivo _repositoryArquivo;
        private readonly Logger _logger;

        public ServiceArquivo(Logger logger)
        {            
            _logger = logger;
            _repositoryArquivo = new RepositoryArquivo(_logger);
        }

        public async Task<List<string>> Execute()
        {
           return (await _repositoryArquivo.AsyncLerArquivo());
        }

    }
}
