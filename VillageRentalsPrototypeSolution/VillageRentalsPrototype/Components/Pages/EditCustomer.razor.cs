using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillageRentalsPrototype.Data;

namespace VillageRentalsPrototype.Components.Pages
{
    public partial class EditCustomer : ComponentBase
    {
        CustomerManager customerManager = new CustomerManager();

        private Customer customer;
        private bool isSaved = false;

        [Parameter]
        public string customerId { get; set; }

        // used to navigate back to the Home page
        [Inject] NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            customer = customerManager.GetCustomer(customerId);
        }

        private async Task UpdateCustomer()
        {
            customerManager.UpdateCustomer(customer);

            isSaved = true;
            await Task.Delay(1000);
            NavigationManager.NavigateTo("/customer?refresh=" + Guid.NewGuid(), forceLoad: true);
        }
    }
}
