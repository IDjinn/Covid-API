using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyCsvParser.Mapping;

namespace CovidAPI.Models
{
    public class CovidData
    {
        public static readonly string DATASET_URL = "https://raw.githubusercontent.com/datasets/covid-19/master/data/time-series-19-covid-combined.csv";
        public DateTime Date { get; set; }
        public string Country { get; set; }
        public string? State { get; set; }
        public int? Confirmed { get; set; }
        public int? Recovered { get; set; }
        public int? Deaths { get; set; }
    }

    public class CovidDataMapping: CsvMapping<CovidData>
    {
        public CovidDataMapping() : base()
        {
            MapProperty(0, covidData => covidData.Date);
            MapProperty(1, covidData => covidData.Country);
            MapProperty(2, covidData => covidData.State);
            MapProperty(3, covidData => covidData.Confirmed);
            MapProperty(4, covidData => covidData.Recovered);
            MapProperty(5, covidData => covidData.Deaths);
        }
    }
}
