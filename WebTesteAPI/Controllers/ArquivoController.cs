using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using WebTesteAPI.Services;

namespace WebTesteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArquivoController : ControllerBase
    {
        public ArquivoRepository _ArquivoRepository;

        public ArquivoController()
        {
            _ArquivoRepository = new ArquivoRepository();
        }

        [HttpGet("GeraArquivoResultado")]      
        public string GeraArquivoResultado(string linhas)
        {
            List<string> Lista = JsonConvert.DeserializeObject<List<string>>(linhas);

            return _ArquivoRepository.CalculaLista(Lista);
        }
       
    }
}
