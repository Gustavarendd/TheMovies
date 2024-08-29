using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using TheMovies.Models;

namespace TheMovies.ViewModels
{
    public class ReservationViewModel : INotifyPropertyChanged
    {
        private readonly ReservationRepo _reservationRepository;
        private readonly ShowRepository _showRepository;

        private ObservableCollection<Show> _shows;
        private ObservableCollection<Reservation> _reservations;
        private Show _selectedShow;
        private DateTime _showTime;
        private int _numberOfTickets;
        private string _availableSeats;
        private int _numberOfReservations;
        private string _customerEmail;
        private string _customerPhone;

        public ObservableCollection<Show> Shows
        {
            get => _shows;
            set
            {
                _shows = value;
                OnPropertyChanged(nameof(Shows));
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
        public Show SelectedShow
        {
            get => _selectedShow;
            set
            {
                _selectedShow = value;
                LoadReservationsForSelectedShow();
                OnPropertyChanged(nameof(SelectedShow));
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
        public string AvailableSeats
        {
            get => _availableSeats;
            set
            {
                _availableSeats = $"Number of Tickets ({value} available)";
                OnPropertyChanged(nameof(AvailableSeats));
            }
        }
        public int NumberOfReservations
        {
            get => _numberOfReservations;
            set
            {
                _numberOfReservations = value;
                OnPropertyChanged(nameof(NumberOfReservations));
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
            _showRepository = new ShowRepository();
            Shows = new ObservableCollection<Show>(_showRepository.LoadShows());
            Reservations = new ObservableCollection<Reservation>(_reservationRepository.LoadReservations());
            BookTicketCommand = new RelayCommand(BookTicket, CanBookTicket);

            NumberOfTickets = 0;
            AvailableSeats = GetAvailableSeats().ToString();
            NumberOfReservations = Reservations.Count;
            
            CustomerEmail = string.Empty;
            CustomerPhone = string.Empty;


        }

        public ICommand BookTicketCommand { get; }

        private void BookTicket()
        {
            if (SelectedShow == null || NumberOfTickets <= 0 || GetAvailableSeats() < NumberOfTickets)
            {
                Debug.WriteLine("Invalid reservation");
                return;
            }

            Reservation reservation = new Reservation
            ( 
                SelectedShow.Id,
                SelectedShow.MovieTitle,
                SelectedShow.ShowTime,
                NumberOfTickets,
                CustomerEmail,
                CustomerPhone
            );
            Show show = Shows.FirstOrDefault(s => s.Id == SelectedShow.Id)!;
  
            Reservations.Add(reservation);
           
            SaveReservations();
            ClearInputs();

        }

        private bool CanBookTicket()
        {
            return SelectedShow != null && NumberOfTickets > 0 && GetAvailableSeats() >= NumberOfTickets;
        }

        private int GetAvailableSeats()
        {
            int availableSeats = 100;
            foreach (Reservation reservation in Reservations)
            {
                availableSeats -= reservation.NumberOfTickets;
            }
            return availableSeats;
        }


        private void LoadReservationsForSelectedShow()
        {
            if (SelectedShow == null)
            {
                return;
            }

            Reservations = new ObservableCollection<Reservation>(_reservationRepository.LoadReservationsByShow(SelectedShow.Id));          

            NumberOfReservations = Reservations.Count;
            AvailableSeats = GetAvailableSeats().ToString();
        }


        private void SaveReservations()
        {
            var reservation = new Reservation
            (
                SelectedShow.Id,
                SelectedShow.MovieTitle,
                SelectedShow.ShowTime,
                NumberOfTickets,
                CustomerEmail,
                CustomerPhone
            );

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
