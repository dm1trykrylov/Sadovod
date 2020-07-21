using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CurePage()
        {
            InitializeComponent();
        }

        private async void SearchPlantButtonPressed(object sender, EventArgs e)
        {
            PlantName = searchPlant.Text.ToString();
            SymptomList = GetSymptoms(searchPlant.Text);
            if(SymptomList.Length > 0)
            {
                OKbutton.IsVisible = true;

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

        private string[] GetSymptoms(string plant)
        {
            var testList = new string[] { "sympt_1", "sympt_2", "sympt_3" };
            
            return testList;
        }

        private void SymptomSelected (object sender, ItemTappedEventArgs e)
        {
            if(e.Item != null)
            {
                if(IsSelectedItem[e.ItemIndex] == true)
                {
                    ((ListView)sender).SelectedItem = null;
                    IsSelectedItem[e.ItemIndex] = false;
                }
                else
                {
                    IsSelectedItem[e.ItemIndex] = true;
                }
            }
        }

        private void OnButtonClicked (object sender, System.EventArgs e)
        {
            //SymptomPicker.IsVisible = false;
            OKbutton.IsVisible = false;
            PlantIllness = GetIllness(PlantName, IsSelectedItem);
            IName.Text = PlantIllness;
            IName.IsVisible = true;

            IArticle.Text = GetInfo(PlantIllness);
            IArticle.IsVisible = true;
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