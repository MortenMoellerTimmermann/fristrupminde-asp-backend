using System;
namespace fristrupminde_api.Models.Inputs.ProjectTaskInputs
{
    public class CreateTaskInput
    {
        public String Title { get; set; }
        public String Description { get; set; }
        public String DueDate { get; set; }
        public String AssignedTo { get; set; }
    }
}
