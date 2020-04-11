using System;
using System.Collections.Generic;

namespace fristrupminde_api.Models
{
    public class Animal
    {
        public Guid ID { get; set; }
        public DateTime Born { get; set; }
        public DateTime? Death { get; set; }
    }
}
