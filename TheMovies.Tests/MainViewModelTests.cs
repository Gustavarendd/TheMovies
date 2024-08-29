
using TheMovies.Models;
using TheMovies.ViewModels;

namespace TheMovies.Tests
{
    [TestClass]
    public class MainViewModelTests
    {


        [TestMethod]
        public void CanCalculateTotalDuration()
        {
            var viewModel = new MainViewModel();
            viewModel.Title = "Inception";
            viewModel.Duration = 140;
            viewModel.Genre = "Action";
            viewModel.Director = "Christopher Nolan";
            viewModel.PremierDate = DateTime.Now;
          

            viewModel.SaveCommand.Execute(null);
            var savedMovie = viewModel.Movies.First();

            Assert.AreEqual(140 + 30, savedMovie.Duration + 30);
        }

        private const string TestFilePath = "movies_program.csv";


        [TestMethod]
        public void CanExportToExcel()
        {
            var viewModel = new MainViewModel
            {
                Title = "Inception",
                Duration = 148,
                Genre = "Action",
                Director = "Christopher Nolan",
                PremierDate = DateTime.Now,
               
                SelectedMovie = new Movie { Title = "Inception", Duration = 140, Genre = "Action", Director = "Christopher Nolan", PremiereDate = new System.DateTime(2016, 7, 16) },
            };

            viewModel.SaveCommand.Execute(null);
            viewModel.AddToExportListCommand.Execute(null);
            viewModel.ExportToExcelCommand.Execute(null);

            Assert.IsTrue(File.Exists(TestFilePath), "The Excel file was not created.");

            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }
    }
}
