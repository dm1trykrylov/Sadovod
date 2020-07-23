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
	public partial class DiseaseInfoPage : ContentPage
	{
		public DiseaseInfoPage (Disease CurDiseaseInfo)
		{
			InitializeComponent ();

            DiseaseNameTxt.Text = CurDiseaseInfo.Name;
            DiseaseInfoTxt.Text = CurDiseaseInfo.Info;
            DiseaseSolutionTxt.Text = CurDiseaseInfo.Solution;
		}

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CurePage());
        }
	}
}