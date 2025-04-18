using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;



namespace Heating_Optimization.Models
{
    public static class SDM
    {
        public static Dictionary<DateTime, HourlyData> WinterPeriod { get; set; } = new Dictionary<DateTime, HourlyData>();
        public static Dictionary<DateTime, HourlyData> SummerPeriod { get; set; } = new Dictionary<DateTime, HourlyData>();
    }

}