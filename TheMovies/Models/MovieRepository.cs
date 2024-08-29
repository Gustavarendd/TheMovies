using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Windows.Shapes;

namespace TheMovies.Models
{
    public class MovieRepository
    {
        private const string FilePathLoad = "TheMoviesRepo.csv";
        private const string FilePathSave = "TheMoviesRepoSave.csv";

        private List<Movie> movies = new List<Movie>();

        public List<Movie> LoadMovies(string? filepath = FilePathLoad)
        {
            if (File.Exists(filepath))
            {
                var lines = File.ReadAllLines(filepath);

                // Start from the second line (index 1) to skip the header
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] values = line.Split(';');
             
                    if (values.Length < 8)
                    {
                        Debug.WriteLine("Error: Invalid line");
                    }

                    try
                    {
                        int duration;
                        if (values[5].Contains(':'))
                        {
                            string[] durationString = values[5].Split(':');
                            duration = int.Parse(durationString[0]) * 60 + int.Parse(durationString[1]);
                        }
                        else
                        {
                            duration = int.Parse(values[5]);
                        }

                        Movie movie = new Movie
                        {
                            Id = i,
                            Title = values[3],
                            Duration = duration,
                            Genre = values[4],
                            Director = values[6],
                            PremiereDate = DateTime.Parse(values[7]),
                        };

                        if (!movies.Any(m => m.Title.Equals(movie.Title)))
                        {
                            movies.Add(movie);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error: {ex.Message}");
                        continue;
                    }

                }
            }

            return movies;
        }

        public void SaveMovies(List<Movie> movies, string? filepath = FilePathSave)
        {
            using (var writer = new StreamWriter(filepath!))
            {
                string header = "Id;Filmtitel;Filmgenre;Filmvarighed;Filminstruktør;Premieredato";
                writer.WriteLine(header);

                foreach (Movie movie in movies)
                {
                    var line = $"{movie.Id};{movie.Title};{movie.Genre};{movie.Duration};{movie.Director};{movie.PremiereDate:yyyy-MM-dd}";
                    writer.WriteLine(line);

                }

                
            }
        }
    }
}
