using System.IO;
using System.Windows.Input;
using System.Windows.Shapes;

namespace TheMovies.Models
{
    public class MovieRepository
    {
        private const string FilePathLoad = "TheMoviesRepo.csv";
        private const string FilePathSave = "TheMoviesRepoSave.csv";
        public List<Movie> LoadMovies(string? filepath = FilePathLoad)
        {
            var movies = new List<Movie>();

            if (File.Exists(filepath))
            {
                var lines = File.ReadAllLines(filepath);

                // Start from the second line (index 1) to skip the header
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] values = line.Split(';');
                    int duration;

                    if (values[5].Contains(':'))
                    {
                        string[] durationString = values[5].Split(":");
                        duration = int.Parse(durationString[0]) * 60 + int.Parse(durationString[1]);
                    } else
                    {
                        duration = int.Parse(values[5]);
                    }

                    Movie movie = new Movie
                    {
                        TheaterHall = $"{values[0]}, {values[1]}",
                        Title = values[3],
                        Duration = duration,
                        Genre = values[4],
                        Director = values[6],
                        PremiereDate = DateTime.Parse(values[7]),
                    };

                    movies.Add(movie);
                }
            }

            return movies;
        }

        public void SaveMovies(List<Movie> movies, string? filepath = FilePathSave)
        {
            using (var writer = new StreamWriter(filepath))
            {
                string header = "Biograf;By;Forestillingstidspunkt;Filmtitel;Filmgenre;Filmvarighed;Filminstruktør;Premieredato;Bookingmail;Bookingtelefonnummer";
                writer.WriteLine(header);

                foreach (Movie movie in movies)
                {
                    var line = $"{movie.TheaterHall};;;{movie.Title};{movie.Genre};{movie.Duration};{movie.Director};{movie.PremiereDate:yyyy-MM-dd}";
                    writer.WriteLine(line);

                }

                
            }
        }
    }
}
