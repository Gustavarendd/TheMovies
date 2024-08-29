using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TheMovies.Models;

namespace TheMovies.ViewModels
{
    public class CreateShowViewModel : INotifyPropertyChanged
    {
   
        private ObservableCollection<Movie> _movies;
        private ObservableCollection<Show> _shows;


        private DateTime _showTime;
        private string _hallName;
        private int _capacity;
        private string _cinemaName;
        private string _city;
        private Movie _selectedMovie;
        private Show _selectedShow;

        public ObservableCollection<Movie> Movies
        {
            get => _movies;
            set
            {
                _movies = value;
                OnPropertyChanged(nameof(Movies));
            }
        }
        public ObservableCollection<Show> Shows
        {
            get => _shows;
            set
            {
                _shows = value;
                OnPropertyChanged(nameof(Shows));
            }
        }
        public DateTime ShowTime 
        { 
            get => _showTime;
            set { 
                _showTime = value;
                OnPropertyChanged(nameof(ShowTime)); 
            }
        }
        public string HallName
        {
            get => _hallName;
            set
            {
                _hallName = value;
                OnPropertyChanged(nameof(HallName));
            }
        }
        public int Capacity
        {
            get => _capacity;
            set
            {
                _capacity = value;
                OnPropertyChanged(nameof(Capacity));
            }
        }
        public string CinemaName
        {
            get => _cinemaName;
            set
            {
                _cinemaName = value;
                OnPropertyChanged(nameof(CinemaName));
            }
        }
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                _selectedMovie = value;
                OnPropertyChanged(nameof(SelectedMovie));
            }
        }

        public Show SelectedShow
        {
            get => _selectedShow;
            set
            {
                _selectedShow = value;
                OnPropertyChanged(nameof(SelectedShow));
            }
        }

        public CreateShowViewModel()
        {
            
            Movies = new ObservableCollection<Movie>();
            Shows = new ObservableCollection<Show>();

            ShowTime = DateTime.Now;
            HallName = "";
            Capacity = 100;
            CinemaName = "";
            City = "";
            
            SaveCommand = new RelayCommand(SaveShow, CanSave);
            DeleteCommand = new RelayCommand(DeleteShow, CanDelete);

            LoadMovies();
            LoadShows();
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

       
        private void LoadMovies()
        {
            MovieRepository movieRepository = new MovieRepository();
            Movies = new ObservableCollection<Movie>(movieRepository.LoadMovies());
        }

        private void LoadShows()
        {
            ShowRepository showRepository = new ShowRepository();
            Shows = new ObservableCollection<Show>(showRepository.LoadShows());
        }

        public void SaveShow()
        { 
            ShowRepository showRepository = new ShowRepository();
            Show show = new Show(Shows.Count + 1,SelectedMovie.Title, ShowTime, SelectedMovie.Duration, 
                new CinemaHall(HallName, Capacity, new Cinema(CinemaName, City)));
            Shows.Add(show);

            showRepository.SaveShows(new List<Show>(Shows));

            ClearInputs();
        }

        private bool CanSave()
        {
            return SelectedMovie != null && !string.IsNullOrEmpty(HallName) && !string.IsNullOrEmpty(CinemaName) && !string.IsNullOrEmpty(City);
        }

        public void DeleteShow() 
        {
            ShowRepository showRepository = new ShowRepository();
            showRepository.DeleteShow(SelectedShow);
            Shows.Remove(SelectedShow);

            ClearInputs();
        }

        private bool CanDelete()
        {
            return SelectedShow != null;
        }

        public void ClearInputs()
        {
            ShowTime = DateTime.Now;
            HallName = "";
            Capacity = 100;
            CinemaName = "";
            City = "";
            SelectedMovie = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
