using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using TheMovies.Models;

namespace TheMovies.ViewModels
{
    public class ReservationViewModel : INotifyPropertyChanged
    {
        private readonly ReservationRepo _reservationRepository;
        private ObservableCollection<TheaterShow> _theaterShows;
        private ObservableCollection<Reservation> _reservations;
        private TheaterShow _selectedShow;
        private DateTime _showTime;
        private int _numberOfTickets;
        private string _customerEmail;
        private string _customerPhone;

        public ObservableCollection<TheaterShow> TheaterShows
        {
            get => _theaterShows;
            set
            {
                _theaterShows = value;
                OnPropertyChanged(nameof(TheaterShows));
            }
        }

        public ObservableCollection<Reservation> Reservations
        {
            get => _reservations;
            set
            {
                _reservations = value;
                OnPropertyChanged(nameof(Reservations));
            }
        }

        public TheaterShow SelectedShow
        {
            get => _selectedShow;
            set
            {
                _selectedShow = value;
                OnPropertyChanged(nameof(SelectedShow));
                LoadReservationsForSelectedShow();
            }
        }

        public DateTime Showtime
        {
            get { return _showTime; }
            set { _showTime = value; }
        }

        public int NumberOfTickets
        {
            get => _numberOfTickets;
            set
            {
                _numberOfTickets = value;
                OnPropertyChanged(nameof(NumberOfTickets));
            }
        }

        public string CustomerEmail
        {
            get => _customerEmail;
            set
            {
                _customerEmail = value;
                OnPropertyChanged(nameof(CustomerEmail));
            }
        }

        public string CustomerPhone
        {
            get => (string)_customerPhone;
            set
            {
                _customerPhone = value;
                OnPropertyChanged(nameof(CustomerPhone));
            }
        }

        public ReservationViewModel()
        {
            _reservationRepository = new ReservationRepo();
            TheaterShows = new ObservableCollection<TheaterShow>();
            Reservations = new ObservableCollection<Reservation>();
            BookTicketCommand = new RelayCommand(BookTicket);
            Reservations = new ObservableCollection<Reservation>(_reservationRepository.LoadReservations());
        }

      

        public ICommand BookTicketCommand { get; }

        private void BookTicket()
        {
            if (SelectedShow == null || NumberOfTickets <= 0 || SelectedShow.ReservedSeats + NumberOfTickets > SelectedShow.TotalSeats)
            {
                return;
            }

            var reservation = new Reservation
            {
                MovieTitle = SelectedShow.MovieTitle,
                ShowTime = SelectedShow.ShowTime,
                TheaterHall = SelectedShow.TheaterHall,
                NumberOfTickets = NumberOfTickets,
                CustomerEmail = CustomerEmail,
                CustomerPhone = CustomerPhone,
            };
            
            Reservations.Add(reservation);
            SelectedShow.ReservedSeats += NumberOfTickets;
            SaveReservations();
            ClearInputs();

        }

        //private void LoadData()
        //{
        //    // Todo
        //    TheaterShows.Add(new TheaterShow { MovieTitle = "Inception", ShowTime = DateTime.Now.AddHours(1), TheaterHall = "Hall 1", TotalSeats = 100, ReservedSeats = 0 });
        //    TheaterShows.Add(new TheaterShow { MovieTitle = "The Matrix", ShowTime = DateTime.Now.AddHours(2), TheaterHall = "Hall 2", TotalSeats = 100, ReservedSeats = 0 });
        //}

        private void LoadReservationsForSelectedShow()
        {
            // Todo
        }

        private void SaveReservations()
        {
            var reservation = new Reservation
            {
                MovieTitle = SelectedShow.MovieTitle,
                ShowTime = SelectedShow.ShowTime,
                TheaterHall = SelectedShow.TheaterHall,
                NumberOfTickets = NumberOfTickets,
                CustomerEmail = CustomerEmail,
                CustomerPhone = CustomerPhone,
            };
            Reservations.Add(reservation);
            _reservationRepository.SaveReservations(new List<Reservation>(Reservations));
            ClearInputs();


        }

        private void ClearInputs()
        {
            NumberOfTickets = 0;
            CustomerEmail = string.Empty;
            CustomerPhone = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
