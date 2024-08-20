using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using TheMovies.Models;
using OfficeOpenXml;

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
        private string _theaterHall;
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
                TheaterHall = value.TheaterHall;
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

        public string TheaterHall
        {
            get => _theaterHall;
            set
            {
                _theaterHall = value;
                OnPropertyChanged(nameof(TheaterHall));
            }
        }

        public MainViewModel()
        {
            _movieRepository = new MovieRepository();
            Movies = new ObservableCollection<Movie>(_movieRepository.LoadMovies());
            ExportList = new ObservableCollection<Movie>();
            SaveCommand = new RelayCommand(SaveMovie);
            ExportToExcelCommand = new RelayCommand(ExportToExcel);
            AddToExportListCommand = new RelayCommand(AddToExportList);
            OpenReservationWindowCommand = new RelayCommand(OpenReservationWindow);
        }

        public ICommand SaveCommand { get; }

        public ICommand AddToExportListCommand { get; }

        public ICommand ExportToExcelCommand { get; }

        public ICommand OpenReservationWindowCommand { get; }

        private void OpenReservationWindow()
        {
            ReservationView reservationView = new ReservationView();
            reservationView.ShowDialog();
        }

        private void SaveMovie() 
        {
            var movie = new Movie
            {
                Title = Title,
                Duration = Duration,
                Genre = Genre,
                Director = Director,
                PremiereDate = PremierDate,
                TheaterHall = TheaterHall
            };
            Movies.Add(movie);
            _movieRepository.SaveMovies(new List<Movie>(Movies));
            ClearInputs();
        }

        private void AddToExportList()
        {

            if (SelectedMovie == null)
                return;

            if (!string.IsNullOrEmpty(SelectedMovie.Title) &&
                !string.IsNullOrEmpty(SelectedMovie.Duration.ToString()) &&
                !string.IsNullOrEmpty(SelectedMovie.PremiereDate.ToString()) &&
                !string.IsNullOrEmpty(SelectedMovie.Director) &&
                !string.IsNullOrEmpty(SelectedMovie.TheaterHall) &&
                !string.IsNullOrEmpty(SelectedMovie.Genre))
            {
                
                if (!ExportList.Contains(SelectedMovie))
                {
                    ExportList.Add(SelectedMovie);
                }
            }
        }

        private void ExportToExcel()
        {
            if(ExportList.Count <= 0)
            {
                return;
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Movies");
                worksheet.Cells[1, 1].Value = "Title";
                worksheet.Cells[1, 2].Value = "Duration";
                worksheet.Cells[1, 3].Value = "Genre";
                worksheet.Cells[1, 4].Value = "Director";
                worksheet.Cells[1, 5].Value = "Premiere Date";
                worksheet.Cells[1, 6].Value = "Theater Hall";
                worksheet.Cells[1, 7].Value = "Total Duration";


                for (int i = 0; i < ExportList.Count; i++)
                {
                    var movie = ExportList[i];
                    worksheet.Cells[i + 2, 1].Value = movie.Title;
                    worksheet.Cells[i + 2, 2].Value = movie.Duration;
                    worksheet.Cells[i + 2, 3].Value = movie.Genre.ToString();
                    worksheet.Cells[i + 2, 4].Value = movie.Director;
                    worksheet.Cells[i + 2, 5].Value = movie.PremiereDate.ToString("yyyy-MM-dd"); // Format date
                    worksheet.Cells[i + 2, 6].Value = movie.TheaterHall;
                    worksheet.Cells[i + 2, 7].Value = movie.Duration + 30; // Total Duration = Duration + 30
                }

                var filePath = "movies_program.xlsx";
                package.SaveAs(new FileInfo(filePath));
            }
        }

        public void ClearInputs() 
        {
            Title = string.Empty;
            Duration = 0; 
            Genre = string.Empty;
            Director = string.Empty;
            TheaterHall = string.Empty;
            PremierDate = new DateTime();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
