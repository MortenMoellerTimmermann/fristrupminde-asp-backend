using System;
namespace fristrupminde_api.Models
{
    public class ProjectTaskUser
    {
        public Guid ProjectTaskUserID { get; set; }
        public Guid ProjectTaskID { get; set; }
        public Guid UserID { get; set; }
    }
}
