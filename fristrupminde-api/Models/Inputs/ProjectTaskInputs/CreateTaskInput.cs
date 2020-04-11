using System;
namespace fristrupminde_api.Models.Inputs.ProjectTaskInputs
{
    public class CreateTaskInput
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string DueDate { get; set; }
        public string AssignedTo { get; set; }
    }
}
