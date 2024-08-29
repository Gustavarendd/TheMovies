using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TheMovies.Models
{
    public class ShowRepository
    {
        private const string FilePath = "TheMoviesShowRepo.csv";
        private List<Show> _shows;

        public List<Show> LoadShows()
        {
            _shows = new List<Show>();
            
            if (File.Exists(FilePath))
            {
                var lines = File.ReadAllLines(FilePath);
                
                // Start from the second line (index 1) to skip the header
                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    var values = line.Split(';');

                    Show show = new Show
                    (
                        int.Parse(values[0]),
                        values[1],
                        DateTime.Parse(values[2]),
                        int.Parse(values[3]),
                        new CinemaHall(values[4], int.Parse(values[5]), new Cinema(values[6], values[7]))
                    );

                    _shows.Add(show);
                }
                
            }
            return _shows;
        }

        public void SaveShows(List<Show> shows)
        {

            using (var writer = new StreamWriter(FilePath))
            {
                
                foreach (var show in shows)
                {
                    string line = $"{show.Id};{show.MovieTitle};{show.ShowTime};{show.Duration};{show.Hall.Name};{show.Hall.Capacity};{show.Cinema.Name};{show.Cinema.City}";
                    writer.WriteLine(line);
                }
                                
            }
        }

        public void DeleteShow(Show show)
        {
            if (File.Exists(FilePath))
            {
                var lines = File.ReadAllLines(FilePath);
                var newLines = new List<string>();

                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    var values = line.Split(';');

                    if (values[0] != show.MovieTitle)
                    {
                        newLines.Add(line);
                    }
                }

                File.WriteAllLines(FilePath, newLines);
            }
        }

        public Show UpdateShow(Show show)
        {
            if (File.Exists(FilePath))
            {
                var lines = File.ReadAllLines(FilePath);
                var newLines = new List<string>();

                for (int i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    var values = line.Split(';');

                    if (int.Parse(values[0]) == show.Id)
                    {
                        values[1] = show.MovieTitle;
                        values[2] = show.ShowTime.ToString();
                        values[3] = show.Duration.ToString();
                        values[4] = show.Hall.Name;
                        values[5] = show.Hall.Capacity.ToString();
                        values[6] = show.Cinema.Name;
                        values[7] = show.Cinema.City;

                    }

                    newLines.Add(string.Join(";", values));
                }

                File.WriteAllLines(FilePath, newLines);
            }

            return show;
        }
    }
}
