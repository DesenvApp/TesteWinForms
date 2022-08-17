using Teste.Interface;
using Teste.Repositories;

namespace Teste.Servico
{
    public class ServiceArquivo
    {
        private readonly IRepositoryArquivo _repositoryArquivo;

        public ServiceArquivo()
        {
            _repositoryArquivo = new RepositoryArquivo();
        }

        public async Task<List<string>> Execute()
        {
           return (await _repositoryArquivo.AsyncLerArquivo());
        }

    }
}
