using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMovies.Models
{
    public class Cinema
    {
        public string Name {  get; set; }
        public string City { get; set; }

        public Cinema(string name, string city)
        {
            Name = name;
            City = city;
        }
    }
}
