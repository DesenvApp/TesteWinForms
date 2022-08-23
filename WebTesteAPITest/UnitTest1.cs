
using Newtonsoft.Json;
using System.Reflection;
using WebTesteAPI.Controllers;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Microsoft.Extensions.Logging;

namespace WebTesteAPITest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly ILogger<ArquivoController> _logger;

        [TestMethod]
        public void TestMethod1()
        {         
            var lista = getLista();

            var result = JsonConvert.SerializeObject(lista);

            ArquivoController _ArquivoController = new ArquivoController(_logger);
            var retorno = _ArquivoController.GeraArquivoResultado(result);
            Assert.AreEqual("Arquivo enviado com sucesso.", retorno);
        }

        private List<string> getLista()
        {
            return new List<string>()
            {
                "0","1","2","3","4","5","6","7","8","9",
                "0","1","2","3","4","5","6","7","8","9"
            };
        }

    }
}