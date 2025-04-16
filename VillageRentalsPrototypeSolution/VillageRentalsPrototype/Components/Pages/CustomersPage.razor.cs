using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebView.Maui;
using VillageRentalsPrototype.Data;

namespace VillageRentalsPrototype.Components.Pages
{
    public partial class CustomersPage : ComponentBase
    {
        public List<Customer> Customers { get; set; } = new List<Customer>(); // list of customers, read in from database
        CustomerManager customerManager = new CustomerManager(); // CustomerManager class, handles dealing stuff with customers and getting from SystemManager

        private Customer customer; // temporary customer class to be added into the list
        private bool isSaved = false; // if a customer has been saved
        private bool isDeleted = false; // if a customer has been deleted

        // used to navigate back to the Home page
        [Inject] NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            Customers = customerManager.GetCustomers();
            customer = new Customer
            {
                customerID = GenerateNewCustomerID()
            };
        }

        /// <summary>
        /// Creates a new customer ID based on the most recent (current highest ID in list) customer ID.
        /// </summary>
        /// <returns></returns>
        //private string GenerateNewCustomerID() // commented out id generator, generates based off of highest id
        //{
        //    string id = "C0000"; // placeholder id
        //    if (Customers.Count == 0) // If the customer being added is the first customer, set ID to 0001.
        //    {
        //        id = "C0001";
        //    }
        //    else // Otherwise, grab the highest id code from the list and increment it
        //    {
        //        Customer temp = Customers.OrderByDescending(a => a.customerID).First();
        //        string highestCode = temp.customerID;
        //        char letter = 'C';
        //        int number = Int32.Parse(highestCode.Substring(1)); // converts rest of the code after the letter to int which should be digits
        //        number++; // Increment the number
        //        id = $"{letter}{number:D4}"; // create the new id
        //    }
        //        return id;
        //}

        /// <summary>
        /// Creates a new customer ID. Looks for gaps that arent filled (ex. fills in c0002 if c0001 and c0003 exist)
        /// Otherwise, creates new id based on number after highest.
        /// [obsolete] doesn't match data sample format :( i didn't read it before making my id generator
        /// </summary>
        /// <returns></returns>
        //private string GenerateNewCustomerID()
        //{
        //    string id = "C0000"; // placeholder id
        //    if (Customers.Count == 0) // If the customer being added is the first customer, set ID to 0001.
        //    {
        //        id = "C0001";
        //    }
        //    else // Otherwise, grab the highest id code from the list and increment it
        //    {
        //        // Get all existing numbers sorted
        //        List<int> existingNumbers = Customers
        //            .Select(c => int.Parse(c.customerID.Substring(1)))
        //            .OrderBy(n => n)
        //            .ToList();

        //        // loop that checks for gaps starting from 1
        //        int expectedNumber = 1;
        //        foreach (int existingNumber in existingNumbers)
        //        {
        //            if (existingNumber > expectedNumber)
        //            {
        //                // if theres a gap, use the expected number
        //                id = $"C{expectedNumber:D4}";
        //                return id;
        //            }
        //            expectedNumber = existingNumber + 1;
        //        }
        //        id = $"C{existingNumbers.Last() + 1:D4}"; // set id to the number after the greatest
        //    }
        //    return id;
        //}

        private string GenerateNewCustomerID()
        {
            int id = 1000; // placeholder id
            if (Customers.Count == 0) // If the customer being added is the first customer, set ID to 1001.
            {
                id = 1001;
                
                return Convert.ToString(id);
            }
            else // Otherwise, grab the highest id code from the list and increment it
            {
                Customer temp = Customers.OrderByDescending(a => a.customerID).First();
                int highestID = Convert.ToInt32(temp.customerID);
                id = highestID + 1;

                return Convert.ToString(id);
            }
        }

        /// <summary>
        /// Saves a customer to database using the customer manager and refreshes the page.
        /// </summary>
        /// <returns></returns>
        private async Task SaveCustomer()
        {
            customerManager.CreateNewCustomer(customer);
            isSaved = true;
            await Task.Delay(1000);
            //NavigationManager.NavigateTo("/customer");
            NavigationManager.NavigateTo("/customer?refresh=" + Guid.NewGuid(), forceLoad: true); // force refreshes the page by introducing a random parameter
        }

        /// <summary>
        /// Moves to the EditCustomer page.
        /// </summary>
        /// <param name="chosenCustomer"></param>
        private void EditCustomer(Customer chosenCustomer)
        {
            NavigationManager.NavigateTo($"/customeredit/{chosenCustomer.customerID}");
        }

        private async Task DeleteCustomer(Customer chosenCustomer)
        {
            customerManager.DeleteCustomer(chosenCustomer.customerID);
            isDeleted = true;
            await Task.Delay(1000);

            NavigationManager.NavigateTo("/customer?refresh=" + Guid.NewGuid(), forceLoad: true);
        }
    }
}
