using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheMovies.Models
{
    public class ReservationRepo
    {
        private const string FilePathLoad = "TheMoviesRepo.csv";
        private const string FilePathSave = "TheMoviesRepoResSave.csv";

        public List<Reservation> LoadReservations(string? filepath = FilePathLoad)
        {
            var reservations = new List<Reservation>();

            if (File.Exists(filepath))
            {
                var lines = File.ReadAllLines(filepath);

                // Start from the second line (index 1) to skip the header
                for (int i = 1; i < lines.Length; i++)
                {
                    var line = lines[i];
                    var values = line.Split(';');

                    Reservation reservation = new Reservation
                    {
                        MovieTitle = values[3],
                        ShowTime = DateTime.Parse(values[2]),
                        TheaterHall = values[0],
                        NumberOfTickets = int.Parse(values[10]),
                        CustomerEmail = values[8],
                        CustomerPhone = values[9],
                    };

                    reservations.Add(reservation);
                }
            }

            return reservations;
        }

        public void SaveReservations(List<Reservation> reservations, string? filepath = FilePathSave)
        {
            using (var writer = new StreamWriter(filepath))
            {
                string header = "Filmtitel;Forestillingstidspunkt;Biograf;Antal Biletter;Booking Email;Booking telefonnummer";
                writer.WriteLine(header);

                foreach (var reservation in reservations)
                {
                    string line = $"{reservation.MovieTitle};{reservation.ShowTime};{reservation.TheaterHall};{reservation.NumberOfTickets};" +
                        $"{reservation.CustomerEmail}; {reservation.CustomerPhone}";
                    writer.WriteLine(line);
                }
            }
        }
    }
}
