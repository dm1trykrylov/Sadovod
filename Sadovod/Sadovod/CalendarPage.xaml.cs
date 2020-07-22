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
    public partial class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            InitializeComponent();

            XamForms.Controls.Calendar LunarCalendar = new XamForms.Controls.Calendar();

            GetAndSetSpecialDates(DateTime.Now.ToString("MM yyyy"));
        }

        

        private void GetAndSetSpecialDates(string monthYear)
        {
            /* send request */
            var notRecommendedDates = new int[] { 1, 2, 5, 7, 9, 23};
            var bannedDates = new int[] { 13, 30 };
            DateTime monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var specialList = new List<XamForms.Controls.SpecialDate>();
            foreach (int date in notRecommendedDates)
            {
                specialList.Add(new XamForms.Controls.SpecialDate(monthStart.AddDays(date))
                {
                    BackgroundColor = Color.Yellow
                });
            }

            foreach (int date in bannedDates)
            {
                specialList.Add(new XamForms.Controls.SpecialDate(monthStart.AddDays(date))
                {
                    BackgroundColor = Color.Red
                });
            }

            LunarCalendar.SpecialDates = specialList;
        }

        private void OnDateChosen (object sender, EventArgs e)
        {
            var dateSelect = LunarCalendar.SelectedDate;
        }
    }
}