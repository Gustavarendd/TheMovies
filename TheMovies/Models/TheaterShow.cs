namespace TheMovies.Models
{
    public class TheaterShow
    {
        public string MovieTitle { get; set; }
        public DateTime ShowTime { get; set; }
        public string TheaterHall { get; set; }
        public int TotalSeats { get; set; }
        public int ReservedSeats { get; set; }
    }
}
