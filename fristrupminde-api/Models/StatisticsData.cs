using System;
namespace fristrupminde_api.Models
{
    public class StatisticsData
    {
        public Guid ID { get; set; }
        public double MilkLiter { get; set; }
        public double FatPercentage { get; set; }
        public DateTime DateForData { get; set; }
        public string CreatedBy { get; set; }
    }
}
