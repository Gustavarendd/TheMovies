using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMovies.Models
{
    public class CinemaHall
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public Cinema Cinema { get; set; }

        public CinemaHall(string name, int capacity, Cinema cinema) 
        { 
            Name = name;
            Capacity = capacity;
            Cinema = cinema;
        }
    }
}
