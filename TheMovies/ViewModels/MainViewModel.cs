using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using TheMovies.Models;
using TheMovies.Views;

namespace TheMovies.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
       
        private readonly MovieRepository _movieRepository;
        private ObservableCollection<Movie> _movies;
        private ObservableCollection<Movie> _exportList;

        private string _title;
        private int _duration;
        private string _genre;
        private string _director;
        private DateTime _premierDate = DateTime.Now;
        private Movie _selectedMovie;

        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set 
            { 
                _selectedMovie = value;
                Title = value.Title;
                Duration = value.Duration;
                Genre = value.Genre;
                PremierDate = value.PremiereDate;
                Director = value.Director;
                OnPropertyChanged(nameof(SelectedMovie));
            }
        }

        public ObservableCollection<Movie> Movies
        {
            get => _movies;
            set
            {
                _movies = value;
                OnPropertyChanged(nameof(Movies));
            }
        }

        public ObservableCollection<Movie> ExportList
        {
            get => _exportList;
            set
            {
                if (_exportList != value)
                {
                    _exportList = value;
                    OnPropertyChanged(nameof(ExportList));
                }
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public int Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        public string Genre
        {
            get => _genre;
            set
            {
                _genre = (string)value;
                OnPropertyChanged(nameof(Genre));
            }
        }

        public string Director
        {
            get => _director;
            set
            {
                _director = value;
                OnPropertyChanged(nameof(Director));
            }
        }

        public DateTime PremierDate
        {
            get => _premierDate;
            set
            {
                _premierDate = value;
                OnPropertyChanged(nameof(PremierDate));
            }
        }

       

        public MainViewModel()
        {
            _movieRepository = new MovieRepository();
            Movies = new ObservableCollection<Movie>(_movieRepository.LoadMovies());
            ExportList = new ObservableCollection<Movie>();
            SaveCommand = new RelayCommand(SaveMovie, CanSave);
            ExportToExcelCommand = new RelayCommand(ExportToExcel, CanExportToExcel);
            AddToExportListCommand = new RelayCommand(AddToExportList, CanAddToExportList);
            OpenReservationWindowCommand = new RelayCommand(OpenReservationWindow);
            OpenCreateShowWindowCommand = new RelayCommand(OpenCreateShowWindow);
        }

        public ICommand SaveCommand { get; }

        public ICommand AddToExportListCommand { get; }

        public ICommand ExportToExcelCommand { get; }

        public ICommand OpenReservationWindowCommand { get; }

        public ICommand OpenCreateShowWindowCommand { get; }

        private void OpenReservationWindow()
        {
            ReservationView reservationView = new ReservationView();
            reservationView.ShowDialog();
        }

        private void OpenCreateShowWindow()
        {
            CreateShowView createShowView = new CreateShowView();
            createShowView.ShowDialog();
        }

        private void SaveMovie() 
        {
            if (!IsMovieValid(new Movie { Title = Title, Duration = Duration, Genre = Genre, Director = Director, PremiereDate = PremierDate }))
                return;
            var movie = new Movie
            {
                Id = Movies.Count + 1,
                Title = Title,
                Duration = Duration,
                Genre = Genre,
                Director = Director,
                PremiereDate = PremierDate,
                
            };
            Movies.Add(movie);
            _movieRepository.SaveMovies(new List<Movie>(Movies));
            ClearInputs();
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Title) && Duration > 0 && !string.IsNullOrEmpty(Genre) && !string.IsNullOrEmpty(Director) && PremierDate != default;
        }

        private void AddToExportList()
        {
            if (SelectedMovie == null || !IsMovieValid(SelectedMovie))
                return;

            if (!ExportList.Contains(SelectedMovie))
            {
                ExportList.Add(SelectedMovie);
            }
        }

        private bool CanAddToExportList()
        {
            return SelectedMovie != null && !ExportList.Contains(SelectedMovie);
        }

        private bool IsMovieValid(Movie movie)
        {
            return !string.IsNullOrEmpty(movie.Title) &&
                   movie.Duration > 0 &&
                   !string.IsNullOrEmpty(movie.Genre) &&
                   !string.IsNullOrEmpty(movie.Director) &&
                   movie.PremiereDate != default;
        }

        public void ExportToExcel()
        {

            string filePath = "movies_program.csv";
            using (var writer = new StreamWriter(filePath))
            {
                string header = "Title;Duration;Genre;Director;Premiere Date;Theater Hall;Total Duration";
                writer.WriteLine(header);

                foreach (Movie movie in ExportList)
                {
                    var line = $"{movie.Title};{movie.Duration};{movie.Genre};{movie.Director};{movie.PremiereDate:yyyy-MM-dd}";
                    writer.WriteLine(line);

                }


            }
        }

       private bool CanExportToExcel()
        {
            return ExportList.Count > 0;
        }

        public void ClearInputs() 
        {
            Title = string.Empty;
            Duration = 0; 
            Genre = string.Empty;
            Director = string.Empty;
            PremierDate = new DateTime();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
