using System;
namespace fristrupminde_api.Models
{
    public class Remark
    {
        public Guid ID { get; set; }
        public Guid ProjectTaskID { get; set; }
        public string Description { get; set; }
    }
}
