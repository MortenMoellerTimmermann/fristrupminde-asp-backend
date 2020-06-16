using System;
namespace fristrupminde_api.Models.Inputs.Statistics
{
    public class CreateStatisticsData
    {
        public string DateForData { get; set; }
        public double MilkLiter { get; set; }
        public double FatPercentage { get; set; }
    }
}
