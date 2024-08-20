using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMovies.Models
{
    public class Reservation
    {
        public string MovieTitle { get; set; }
        public DateTime ShowTime { get; set; }
        public string TheaterHall { get; set; }
        public int NumberOfTickets { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }
}
