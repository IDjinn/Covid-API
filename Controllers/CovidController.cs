using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidAPI.Models;
using CovidAPI.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidController : ControllerBase
    {
        private IEnumerable<CovidData> Data { get; set; }

        public CovidController()
        {
            Data = CSVParser.GetRecords<CovidData>("time-series-19-covid-combined.csv", new CovidDataMapping()).OrderByDescending(x => x.Date.Ticks);
        }

        [HttpGet]
        public ActionResult<CovidDataPage> Get(
            [FromQuery] int limit = 1, 
            [FromQuery] int after = 0, 
            [FromQuery] string? country = null,
            [FromQuery] string? state = null,
            [FromQuery] DateTime? afterDate = null,
            [FromQuery] DateTime? beforeDate = null)
        {
            limit = Math.Min(Math.Max(limit, 1), 100);
            after = Math.Min(Math.Max(after, 0), Data.Count() - 1);

            var page = new CovidDataPage();
            int skipperCounter = 0;
            foreach (var data in Data)
            {
                if (page.results.Count() >= limit)
                    break;

                if (!string.IsNullOrWhiteSpace(country) && !data.Country.Equals(country))
                    continue;

                if (!string.IsNullOrWhiteSpace(state) && !data.State.Equals(state))
                    continue;

                if (afterDate is DateTime && afterDate > data.Date)
                    continue;

                if (beforeDate is DateTime && data.Date > beforeDate)
                    continue;

                if (after > ++skipperCounter) // Need be here, we must be skip only filtred values, not all values
                    continue;

                page.results.Add(data);
            }
            
            return Ok(page);
        }
    }
}
