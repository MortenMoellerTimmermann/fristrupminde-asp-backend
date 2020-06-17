using System;
namespace fristrupminde_api.Models
{
    public class ProjectTask
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime Created { get; set; }

        //Nullable attributes
        public DateTime? CompletedDate { get; set; }
        //String is always nullable
        public string CompletedBy { get; set; }
    }
}
