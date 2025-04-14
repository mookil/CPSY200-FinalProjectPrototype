using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using VillageRentalsPrototype.Data;

namespace VillageRentalsPrototype.Components.Pages
{
    /// <summary>
    /// Rental page for making new rentals. Takes in all 3 managers, Customer, Equipment, and Rental.
    /// </summary>
    public partial class RentalPage : ComponentBase
    {
        public List<Rental> RentalsList { get; set; } = new List<Rental>();
        public RentalManager rentalManager = new RentalManager();

        [Inject] NavigationManager NavigationManager { get; set; }

        private Rental rental = new Rental();
        private bool isSaved = false;

        protected override void OnInitialized()
        {
            RentalsList = rentalManager.GetRentals();
            rental = new Rental
            {
                RentalID = GenerateNewRentalID()
            };
        }

        private string GenerateNewRentalID()
        {
            int id = 1000; // placeholder id
            if (RentalsList.Count == 0) // If the rental being added is the first rental, set ID to 1001.
            {
                id = 1001;
                return id.ToString();
            }
            else // Otherwise, grab the highest id code from the list and increment it
            {
                Rental temp = RentalsList.OrderByDescending(a => a.RentalID).First();
                int highestID = Convert.ToInt32(temp.RentalID);
                id = highestID + 1;

                return id.ToString();
            }
        }

        private async Task SaveRental()
        {
            rentalManager.AddRental(rental);
            isSaved = true;
            await Task.Delay(1000);
            NavigationManager.NavigateTo("/rental?refresh=" + Guid.NewGuid(), forceLoad: true);
        }
    }
}
