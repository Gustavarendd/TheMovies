using System;
using TheMovies;
using TheMovies.Models;

namespace TheMovies.Tests
{
    [TestClass]
    public class MovieRepositoryTests
    {
        private const string FilePath = "test_movies.csv";

        [TestMethod]
        public void CanSaveAndLoadMovies()
        {
            var repository = new MovieRepository();
            var movies = new List<Movie>
            {
                new Movie { Title = "Inception", Duration = 140, Genre = "Action", Director = "Christopher Nolan", PremiereDate= new System.DateTime(2016, 7, 16), TheaterHall = "Hall 1"},
                new Movie { Title = "The Matrix", Duration = 136, Genre = "Action", Director = "Wachowskis", PremiereDate = new System.DateTime(1999, 3, 31), TheaterHall = "Hall 2" },
            };

            repository.SaveMovies(movies, FilePath);
            var LoadedMovies = repository.LoadMovies(FilePath);

            Assert.IsNotNull(LoadedMovies);
            Assert.AreEqual(2, LoadedMovies.Count);
            Assert.AreEqual("Inception", LoadedMovies[0].Title);
            Assert.AreEqual("The Matrix", LoadedMovies[1].Title);
        }

        [TestMethod]
        public void Cleanup()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}