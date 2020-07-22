using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sadovod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurePage : ContentPage
    {
        public string[] SymptomList { get; set; } = { "s1", "s2" };
        public bool[] IsSelectedItem { get; set; }
        public string PlantName { get; set; }
        public string PlantIllness { get; set; }

        private string symptURL = "https://sadovodazfunc.azurewebsites.net/api/GetSympthoms?";

        public CurePage()
        {
            InitializeComponent();
            
        }

        private async void SearchPlant(object sender, EventArgs e)
        {
            PlantName = PlantSearchBar.Text.ToString();
            GetSymptoms(PlantSearchBar.Text);
            if (SymptomList.Length > 0)
            {
                //OKbutton.IsVisible = true;

                //SymptomPicker.ItemsSource = SymptomList;
                //SymptomPicker.IsVisible = true;
                IsSelectedItem = new bool[SymptomList.Length];
                IsSelectedItem.SetValue(false, 0, SymptomList.Length - 1);

            }
            else
            {
                await DisplayAlert("Plant Search", "Unknown plant", "OK");
            }
        }

        private async void GetSymptoms(string plant)
        {
            var url = $"{symptURL}{plant}";
            using(var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var symptList = JsonConvert.DeserializeObject<SymptomsList>(result);
                    int count = 1;
                    foreach(string sympt in symptList.Symptoms)
                    {
                        SymptomListTxt.Text += $"{count++} {sympt}"
                    }
                }
                else
                {
                    await DisplayAlert("Plant search", "Unknown plant", "OK");
                }
            }

            var testList = new string[] { "sympt_1", "sympt_2", "sympt_3" };
            
            
        }

       

        private void OnButtonClicked(object sender, System.EventArgs e)
        {
            //SymptomPicker.IsVisible = false;
            
            PlantIllness = GetIllness(PlantName, IsSelectedItem);
            
        }

       

        private string GetIllness (string plant, bool[] symptMask)
        {
            /* */
            return "SomeIllness";
        }

        private string GetInfo(string illness)
        {
            /* */
            return "SomeText";
        }
    }
}