using System;
using System.Collections.Generic;

namespace fristrupminde_api.Models
{
    public class Animal
    {
        public int ID { get; set; }
        public DateTime Born { get; set; }
        public DateTime? Death { get; set; }
        public ICollection<Animal> Children { get; set; }
    }
}
