using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidAPI.Models
{
    public class CovidDataPage
    {
        public int count { get => results.Count(); }
        public List<CovidData> results { get; internal set; } = new List<CovidData>();
    }
}
