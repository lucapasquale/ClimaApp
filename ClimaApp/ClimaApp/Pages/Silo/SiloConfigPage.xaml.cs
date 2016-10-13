using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ClimaApp.Silo;

namespace ClimaApp.Pages.Silo
{
    public partial class SiloConfigPage : ContentPage
    {
        public Dictionary<string, SiloConfig> profiles { get; set; } = new Dictionary<string, SiloConfig>();
        public SiloDevice silo { get; set; } = new SiloDevice();

        public SiloConfigPage()
        {
            InitializeComponent();
            silo = DataResources.siloSelecionado;

            Title = silo.lora.comment;

            foreach (var config in ApplicationSilo.configs)
            {
                profiles.Add(config.configName, config);
                picker.Items.Add(config.configName);
            }

            picker.SelectedIndex = 0;
            labelGrao.Text = silo.latest.tempGrao.ToString("#.##");
            labelUmidInt.Text = silo.latest.umidInt.ToString("#.#");
            labelUmidExt.Text = silo.latest.umidExt.ToString("#.#");
        }

        void Picker_Changed(object sender, EventArgs e)
        {
            string pick = picker.Items[picker.SelectedIndex];
            labelDifUmid.Text = profiles[pick].difUmidade.ToString();
        }
    }
}