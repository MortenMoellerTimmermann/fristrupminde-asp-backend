using System;
namespace fristrupminde_api.Models
{
    public class ProjectTask
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime Created { get; set; }
    }
}
