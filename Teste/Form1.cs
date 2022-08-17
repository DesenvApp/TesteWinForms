using Newtonsoft.Json;
using System.Configuration;
using Teste.Models;
using Teste.Servico;
using static System.Net.WebRequestMethods;

namespace Teste
{
    public partial class Form1 : Form
    {
        List<string> lista = null;

        public Form1()
        {
            InitializeComponent();            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            try
            {
                ServiceArquivo _ServiceArquivo = new ServiceArquivo();
                lista = _ServiceArquivo.Execute().Result;

                foreach (string s in lista)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = s;
                    listView1.Items.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
  
        }

        private async void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (lista != null && lista.Count > 0)
            {
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
                            MessageBox.Show(await response.Content.ReadAsStringAsync());
                        }
                    }
                }
            }
        }
    }
}