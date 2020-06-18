using System;
using System.Collections.Generic;
using fristrupminde_api.Models.Outputs.Authentication;

namespace fristrupminde_api.Models.Outputs.ProjectTaskOutputs
{
    public class DetailsTaskOutput
    {
        public List<UserOutput> UsersOnTask { get; set; }
        public List<Remark> Remarks { get; set; }
    }

}
