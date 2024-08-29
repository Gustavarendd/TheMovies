namespace TheMovies.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public string Genre {  get; set; } 
        public string Director { get; set; }
        public DateTime PremiereDate { get; set; }
     
    }
}
