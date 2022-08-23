using System.Configuration;
using Teste.Servico;
using Microsoft.Extensions.Logging;
using Teste.Models;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Newtonsoft.Json;

namespace Teste
{
    public partial class Form1 : Form
    {
        private readonly ILogger _logger;
        ModelLine _ModelLine;

        public delegate void AddItemDelegate(ListViewItem item);        

        public Form1(ILogger<Form1> logger)
        {
            _logger = logger;
            InitializeComponent();            
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {          
            _logger.LogInformation("Disparando a thread Monitora(monitora a pasta).");          
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
                 listView1.Invoke(d, new object[] { item });
            }
        }

        public async void Monitora()
        {
            try
            {
                ServiceArquivo _ServiceArquivo = new ServiceArquivo(_logger);
                _ModelLine = _ServiceArquivo.Execute().Result;

                if (_ModelLine != null)
                {
                    _logger.LogInformation("disparando as threads de da coluna A e B.");
                    var task1 = Task.Run(() => AddItemDaLista(_ModelLine.ListA));
                    var task2 = Task.Run(() => AddItemDaLista(_ModelLine.ListB));

                    await Task.WhenAll(task1, task2);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        } 

        public void AddItemDaLista(List<string> lista)
        {
            if (lista != null && lista.Count > 0)
            {
                listView1.View = View.Details;
                _logger.LogInformation("Adicionando items na listview.");

                foreach (string s in lista)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = s; 
                    AddItem(item);
                }
            }
        }  

        private async void listView1_DoubleClick(object sender, EventArgs e)
        {
            List<string> lista;
            try
            {
                if (listView1.Items.Count > 0)
                {
                    lista = new List<string>();

                    foreach (ListViewItem item in listView1.Items)
                    {
                        lista.Add(item.Text);
                    }

                    _logger.LogInformation("envio do arquivo para a API.");

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
                                _logger.LogInformation("retorno de sucesso da API.");
                                MessageBox.Show(await response.Content.ReadAsStringAsync());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
 
        }
    }
}