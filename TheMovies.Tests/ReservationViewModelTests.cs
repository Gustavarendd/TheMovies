using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheMovies.ViewModels;

namespace TheMovies.Tests
{
    [TestClass]
    public class ReservationViewModelTests
    {
        [TestMethod]
        public void CanBookTicketWithCapasity()
        {
            var viewModel = new ReservationViewModel();
            var show = viewModel.Shows.First();
            viewModel.SelectedShow = show;
            viewModel.NumberOfTickets = 5;
            viewModel.CustomerEmail = "test@example.com";
            viewModel.CustomerPhone = "12345678";

            viewModel.BookTicketCommand.Execute(null);

            Assert.AreEqual(5, viewModel.SelectedShow.Reservations.Count);
            Assert.AreEqual(1, viewModel.Reservations.Count);
        }

        [TestMethod]
        public void CannotBookTicketOverCapacity()
        {
            var viewModel = new ReservationViewModel();

          
            var show = viewModel.Shows.First();
            viewModel.SelectedShow = show;

           
            viewModel.NumberOfTickets = show.Capacity + 1; // Overbooking by 1
            viewModel.CustomerEmail = "test@example.com";
            viewModel.CustomerPhone = "12345678";

        
            viewModel.BookTicketCommand.Execute(null);

           
            Assert.AreEqual(0, viewModel.SelectedShow.Reservations.Count);
            Assert.AreEqual(0, viewModel.Reservations.Count);
        }

        [TestMethod]
        public void CannotBookZeroOrNegativeTickets()
        {
            var viewModel = new ReservationViewModel();
            var show = viewModel.Shows.First();
            viewModel.SelectedShow = show;
            viewModel.NumberOfTickets = 0;
            viewModel.CustomerEmail = "test@example.com";
            viewModel.CustomerPhone = "12345678";

            viewModel.BookTicketCommand.Execute(null);

            viewModel.SelectedShow = viewModel.Shows.Last();
            viewModel.SelectedShow = show;

            Assert.AreEqual("test@example.com", viewModel.Reservations.First().CustomerEmail);
            Assert.AreEqual(1, viewModel.Reservations.Count);
        }
    }
}
