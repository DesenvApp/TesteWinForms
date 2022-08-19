using Newtonsoft.Json;
using NLog;
using System.Configuration;
using Teste.Servico;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Teste
{
    public partial class Form1 : Form
    {
        readonly Logger _logger;
        List<string> lista = null;

        public delegate void AddItemDelegate(ListViewItem item);        

        public Form1()
        {
            InitializeComponent();
            _logger = LogManager.GetLogger("");
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {          
            _logger.Info("Disparando a thread");          
            var threadMonitora = new Thread(Monitora);
            threadMonitora.Start();
        }

        public void AddItem(ListViewItem item)
        {
            
            if (!listView1.InvokeRequired)
            {
                listView1.Items.Add(item);
            }
            else
            {
                var d = new AddItemDelegate(AddItem);
                 listView1.Invoke(d);
            }
        }

        public void Monitora()
        {
            try
            {
                ServiceArquivo _ServiceArquivo = new ServiceArquivo(_logger);
                lista = _ServiceArquivo.Execute().Result;

                if (lista !=null && lista.Count > 0)
                {
                    listView1.View = View.Details;
                    _logger.Info("Adicionando items na listview.");

                    foreach (string s in lista)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = s;
                        //listView1.Items.Add(item);
                        AddItem(item);
                    }   
                }
                else
                {
                    _logger.Info("Lista vazia.");
                }
 
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }
        } 

        private async void listView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (lista != null && lista.Count > 0)
                {
                    _logger.Info("envio do arquivo para a API.");

                    string UrlAPI = ConfigurationManager.AppSettings.Get("UrlAPI");

                    var result = JsonConvert.SerializeObject(lista);

                    string sParametro = string.Format("linhas={0}", result);

                    string sUrl = String.Format("{0}?{1}", UrlAPI, sParametro);

                    using (var client = new HttpClient())
                    {
                        using (var response = await client.GetAsync(sUrl))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                _logger.Info("retorno de sucesso da API.");
                                MessageBox.Show(await response.Content.ReadAsStringAsync());
                            }    
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Info(ex.Message);
            }
 
        }
    }
}