using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMovies.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string ShowName { get; set; }
        public DateTime ShowTime { get; set; }
        public int NumberOfTickets { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        public Reservation(int id, string showName, DateTime showTime, int numberOfTickets, string customerEmail, string customerPhone)
        {
            Id = id;
            ShowName = showName;
            ShowTime = showTime;
            NumberOfTickets = numberOfTickets;
            CustomerEmail = customerEmail;
            CustomerPhone = customerPhone;
        }
    }
}
