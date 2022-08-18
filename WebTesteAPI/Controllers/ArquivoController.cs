using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using WebTesteAPI.Services;
using NLog;

namespace WebTesteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArquivoController : ControllerBase
    {
        public ArquivoRepository _ArquivoRepository;

        private readonly Logger _logger;

        public ArquivoController(Logger logger)
        {            
            _logger = logger;
            _logger.Info("Inicializa a controller(ArquivoController)");
            _ArquivoRepository = new ArquivoRepository(_logger);
        }
         
        [HttpGet("GeraArquivoResultado")]      
        public string GeraArquivoResultado(string linhas)
        {
            _logger.Info("entrou no metodo GeraArquivoResultado");
            List<string> Lista = JsonConvert.DeserializeObject<List<string>>(linhas);

            return _ArquivoRepository.CalculaLista(Lista);
        }
       
    }
}
