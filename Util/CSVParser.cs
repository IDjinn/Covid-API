using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace CovidAPI.Util
{
    public static class CSVParser
    {
        public static IEnumerable<T> GetRecords<T>(string fileName, ICsvMapping<T> Mapping)
        {
            string currentFolder = AppDomain.CurrentDomain.BaseDirectory;
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            var csvParser = new CsvParser<T>(csvParserOptions, Mapping);
            var records = csvParser.ReadFromFile(Path.Combine(currentFolder, fileName), Encoding.UTF8);
            return records.Select(x => x.Result).ToList();
        }
    }
}
