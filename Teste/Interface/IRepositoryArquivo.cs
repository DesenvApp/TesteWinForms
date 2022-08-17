using Teste.Models;

namespace Teste.Interface
{
    public interface IRepositoryArquivo
    {
        Task<List<string>> AsyncLerArquivo();
    }
}
