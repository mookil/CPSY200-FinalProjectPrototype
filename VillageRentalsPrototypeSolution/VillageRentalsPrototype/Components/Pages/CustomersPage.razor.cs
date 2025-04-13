using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using VillageRentalsPrototype.Data;

namespace VillageRentalsPrototype.Components.Pages
{
    public partial class CustomersPage : ComponentBase
    {
        public List<Customer> Customers { get; set; } = new List<Customer>();
        CustomerManager customerManager = new CustomerManager();

        protected override void OnInitialized()
        {
            Customers = customerManager.GetCustomers();
        }
    }
}
