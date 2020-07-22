using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
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

        private string curURL = "http://api.openweathermap.org/data/2.5/weather?";
        private string forecastURL = "http://api.openweathermap.org/data/2.5/forecast?";
        private const string AppID = "e02725c9ed5a4d87cc279b3dba21e6a6";
        private Location location = new Location();

        private const string cultPattern = "ru-RU";
        private System.Globalization.CultureInfo curCulture = new System.Globalization.CultureInfo(cultPattern);
        private string timePattern = "h'h 'mm'min'";

        private async void GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    var url = $"{curURL}lat={(int)location.Latitude}&lon={(int)location.Longitude}&appid={AppID}&units=metric";
                    GetWeather(url);
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
                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result);
                    currentTxt.Text = weatherInfo.weather[0].description.ToUpper();
                    mainIconImg.Source = $"w{weatherInfo.weather[0].icon}";
                    cityTxt.Text = weatherInfo.name.ToUpper();
                    temperatureTxt.Text = weatherInfo.main.temp.ToString("0");

                    DateTime dt = new DateTime().ToUniversalTime().AddSeconds(weatherInfo.dt);
                    dateTxt.Text = dt.ToString("dddd, MMM dd", curCulture).ToUpper();

                    double timestamp = weatherInfo.sys.sunset - weatherInfo.sys.sunrise;
                    DateTime dtt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    dtt = dtt.AddSeconds(timestamp);
                    dayLenTxt.Text = dtt.ToString(timePattern);
                }
            }
        }

        private string Temp(string full)
        {
            string novice = full.Substring(full.IndexOf("main"), full.IndexOf("description") - full.IndexOf("main"));
            return novice;
        }

        private async void GetForecast()
        {
            var url = $"{forecastURL}lat={(int)location.Latitude}&lon={(int)location.Longitude}&appid={AppID}&units=metric";

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(url, null);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var forcastInfo = JsonConvert.DeserializeObject<ForecastInfo>(result);

                    List<List> allList = new List<List>();

                    foreach (var list in forcastInfo.list)
                    {

                        var date = DateTime.Parse(list.dt_txt);

                        if (date > DateTime.Now && date.Hour == 0 && date.Minute == 0 && date.Second == 0)
                            allList.Add(list);
                    }
                    dayOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dddd", curCulture);
                    dateOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dd MMM", curCulture);
                    iconOne.Source = $"w{allList[0].weather[0].icon}";
                    maxTempOneTxt.Text = allList[0].main.temp_max.ToString("0");
                    minTempOneTxt.Text = allList[0].main.temp_min.ToString("0");

                    dayTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dddd", curCulture);
                    dateTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dd MMM", curCulture);
                    iconTwo.Source = $"w{allList[1].weather[0].icon}";
                    maxTempTwoTxt.Text = allList[1].main.temp_max.ToString("0");
                    minTempTwoTxt.Text = allList[1].main.temp_min.ToString("0");

                    dayThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dddd", curCulture);
                    dateThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dd MMM", curCulture);
                    iconThree.Source = $"w{allList[2].weather[0].icon}";
                    maxTempThreeTxt.Text = allList[2].main.temp_max.ToString("0");
                    minTempThreeTxt.Text = allList[2].main.temp_min.ToString("0");
                }
                else
                {
                    await DisplayAlert("Weather Info", "No forecast information found", "OK");
                }
            }
        }
    }
}