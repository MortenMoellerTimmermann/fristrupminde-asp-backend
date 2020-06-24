using System;
using System.ComponentModel.DataAnnotations;

namespace fristrupminde_api.Models.Inputs.ProjectTaskInputs
{
    public class FinishTaskInput
    {
        [Required]
        public string TaskID { get; set; }
    }
}
