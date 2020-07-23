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
        public Disease DiseaseInfo { get; set; }
        public SymptomsList symptList { get; set; }

        private string symptURL = "https://sadovodazfunc.azurewebsites.net/api/GetSympthoms?";
        private string disURL = "https://sadovodazfunc.azurewebsites.net/api/SadovodCure?";

        public CurePage()
        {
            InitializeComponent();
            
        }

        private void SearchPlant(object sender, EventArgs e)
        {
            PlantName = PlantSearchBar.Text.ToString();
            GetSymptoms(PlantSearchBar.Text);

            //IsSelectedItem = new bool[SymptomList.Length];
            //IsSelectedItem.SetValue(false, 0, SymptomList.Length - 1);

                       
        }

        private async void GetSymptoms(string plant)
        {
            var url = $"{symptURL}";
            var obj = new { name = plant };
            using(var client = new HttpClient())
            {
                var payload = JsonConvert.SerializeObject(obj);
                var content = new StringContent(payload, Encoding.UTF8, @"application/json");
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    symptList = Unwrap(result);
                    SymptomListTxt.Text = "";
                    int count = 1;
                    foreach(string sympt in symptList.Symptoms)
                    {
                        SymptomListTxt.Text += $"{count++} {sympt}\n"; // endline for each symptom
                    }
                }
                else
                {
                    await DisplayAlert("Plant search", "Unknown plant", "OK");
                }
            }

            
            
            
        }

        private SymptomsList Unwrap(string result)
        {
            result.Replace("[", String.Empty).Replace("]", String.Empty).Replace("{", String.Empty).Replace("}", String.Empty);
            SymptomsList list = new SymptomsList();
            list.Symptoms = result.Split(',');
            return list;
        }

        private async void OnButtonClicked(object sender, System.EventArgs e)
        {
            string[] selectedSympt = SymptChoise.Text.Split(' ');
            List<int> symptIndexes = new List<int>();
            foreach(string sympt in selectedSympt)
            {
                symptIndexes.Add(int.Parse(sympt));
            }
            var s1 = symptList.Symptoms[symptIndexes[0]];
            var s2 = symptIndexes.Count > 1 ? symptList.Symptoms[symptIndexes[1]] : null;
            var s3 = symptIndexes.Count > 2 ? symptList.Symptoms[symptIndexes[2]] : null;
            var diseaseInf = new
            {
                name = PlantName,
                sympt1 = s1,
                sympt2 = s2,
                sympt3 = s3
            };

            var url = $"{disURL}{diseaseInf}";
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    DiseaseInfo = JsonConvert.DeserializeObject<Disease>(result);
                    
                }
                else
                {
                    await DisplayAlert("Plant search", "Unknown plant", "OK");
                }
            }

            await Navigation.PushAsync(new DiseaseInfoPage(DiseaseInfo));
        }

       

        
    }
}
