namespace TheMovies.Models
{
    public class Show
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public DateTime ShowTime { get; set; }
        public int Duration {  get; set; }
        public Cinema Cinema { get; set; }
        public CinemaHall Hall { get; set; }
        public int Capacity {  get; set; }

        public List<Reservation> Reservations; 

        public Show(int id, string movieTitle, DateTime showtime, int duration, CinemaHall hall)
        {
            Id = id;
            MovieTitle = movieTitle;
            ShowTime = showtime;
            Duration = duration;
            Hall = hall;
            Cinema = hall.Cinema;
            Capacity = hall.Capacity;
            Reservations = new List<Reservation>();
        }
        
    }
}
