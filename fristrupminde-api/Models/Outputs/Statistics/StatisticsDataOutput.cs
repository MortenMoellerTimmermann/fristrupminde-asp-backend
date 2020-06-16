using System;
using System.Collections.Generic;
namespace fristrupminde_api.Models.Outputs.Statistics
{
    public class StatisticsDataOutput
    {
        public StatisticsDataOutput()
        {
            /*
            IDs = new List<Guid>();
            Date = new List<DateTime>();
            MilkLiter = new List<double>();
            FatPercentage = new List<double>();*/
        }

        /*
        public List<Guid> IDs;
        public List<DateTime> Date;
        public List<double> MilkLiter;
        public List<double> FatPercentage; */

        public Guid ID { get; set; }
        public string Date { get; set; }
        public double Milk { get; set; }
        public double Fat { get; set; }
    }
}
