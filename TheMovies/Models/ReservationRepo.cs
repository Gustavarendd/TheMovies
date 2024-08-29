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
       
        private const string FilePath = "ReservationRepo.csv";
        private List<Reservation> reservations = new List<Reservation>();

        public List<Reservation> LoadReservations(string? filepath = FilePath)
        {
            if (File.Exists(filepath))
            {
                var lines = File.ReadAllLines(filepath);

                // Start from the second line (index 1) to skip the header
                for (int i = 1; i < lines.Length; i++)
                {
                    var line = lines[i];
                    var values = line.Split(';');

                    Reservation reservation = new Reservation
                    (
                        int.Parse(values[0]),
                        values[1],
                        DateTime.Parse(values[2]),
                        int.Parse(values[3]),
                        values[4],
                        values[5]
                    );

                    reservations.Add(reservation);
                }
            }

            return reservations;
        }

        public List<Reservation> LoadReservationsByShow(int showId, string? filepath = FilePath)
        {
            var reservations = LoadReservations(filepath);
            return reservations.Where(r => r.Id == showId).ToList();
        }

        public void SaveReservations(List<Reservation> reservations, string? filepath = FilePath)
        {
            if (File.Exists(filepath))
            {
                List<Reservation> oldReservations = LoadReservations();

                using (var writer = new StreamWriter(filepath))
                {
                    string header = "Id;Forestilling;Forestillingstidspunkt;Antal Biletter;Booking Email;Booking telefonnummer";
                    writer.WriteLine(header);

                    foreach (Reservation oldReservation in oldReservations)
                    {
                        string line = $"{oldReservation.Id};{oldReservation.ShowName};{oldReservation.ShowTime};{oldReservation.NumberOfTickets};" +
                                $"{oldReservation.CustomerEmail}; {oldReservation.CustomerPhone}";
                        writer.WriteLine(line);
                    }
                    foreach (var reservation in reservations)
                    {
                        string line = $"{reservation.Id};{reservation.ShowName};{reservation.ShowTime};{reservation.NumberOfTickets};" +
                                $"{reservation.CustomerEmail}; {reservation.CustomerPhone}";
                        writer.WriteLine(line);
                    }
                }
            } else
            {
                using (var writer = new StreamWriter(filepath))
                {
                    string header = "Id;Forestilling;Forestillingstidspunkt;Antal Biletter;Booking Email;Booking telefonnummer";
                    writer.WriteLine(header);

                    foreach (var reservation in reservations)
                    {
                        string line = $"{reservation.Id};{reservation.ShowName};{reservation.ShowTime};{reservation.NumberOfTickets};" +
                                $"{reservation.CustomerEmail}; {reservation.CustomerPhone}";
                        writer.WriteLine(line);
                    }
                }
            }
        }

        public void DeleteReservation(Reservation reservation)
        {
            if (File.Exists(FilePath))
            {
                var reservations = LoadReservations(FilePath);
                reservations.Remove(reservation);
                SaveReservations(reservations, FilePath);
            }
        }
    }
}
