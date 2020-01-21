using System;
using System.Collections.Generic;
using fristrupminde_api_backend.Models;

namespace fristrupminde_api_backend.Models
{
    public class Animal
    {
        public int ID { get; set; }
        public DateTime Born { get; set; }
        public DateTime? Death { get; set; }
        public ICollection<Animal> Children { get; set; }
    }
}
