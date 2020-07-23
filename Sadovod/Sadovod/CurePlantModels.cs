using System;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sadovod
{
    public class Disease
    {
        public string Name { get; set; }
        public string Info { get; set; }
        public string Solution { get; set; }
    }

    public class SymptomsList
    {
        public string[] Symptoms { get; set; }
    }

    public class Symptom
    {
        public string SymptName { get; set; }
    }
}