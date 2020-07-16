using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Sadovod
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeatherPage : ContentPage
    {
        public WeatherPage()
        {
            InitializeComponent();

            GetLocation();
        }

        private async void GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {   
                    string URL = "https://api.openweathermap.org/data/2.5/weather?lat=" + (int)location.Latitude + "&lon=" + 
                        (int)location.Longitude + "&appid=e02725c9ed5a4d87cc279b3dba21e6a6";
                    GetWeather(URL);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                await DisplayAlert("Error!", "Your device does not support Geolocation", "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                await DisplayAlert("Error!", "Geolocation is not enabled", "OK");
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                await DisplayAlert("Error!", "There is some trouble with permission ", "OK");
            }
            catch (Exception ex)
            {
                // Unable to get location
                await DisplayAlert("Unindentified Error!", "Can't load your geolocation", "OK");
            }
        }

        async void GetWeather(string URL)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(URL, null);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var info = Temp(result);
                    await DisplayAlert("Hooray!", "Weather: " + info, "OK");
                }
            }
        }

        private string Temp(string full)
        {
            string novice = full.Substring(full.IndexOf("main\":")+6, full.IndexOf("\"description") - full.IndexOf("main\":") - 6);
            return novice;
        }
    }
}