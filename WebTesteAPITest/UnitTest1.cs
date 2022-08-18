
using Newtonsoft.Json;
using System.Reflection;
using WebTesteAPI.Controllers;
using NLog;

namespace WebTesteAPITest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Logger _logger = LogManager.GetLogger("");

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