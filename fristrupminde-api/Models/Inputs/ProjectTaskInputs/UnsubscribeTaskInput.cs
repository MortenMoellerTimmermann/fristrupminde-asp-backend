using System;
using System.ComponentModel.DataAnnotations;

namespace fristrupminde_api.Models.Inputs.ProjectTaskInputs
{
    public class UnsubscribeTaskInput
    {
        [Required]
        public string TaskID { get; set; }
    }
}
