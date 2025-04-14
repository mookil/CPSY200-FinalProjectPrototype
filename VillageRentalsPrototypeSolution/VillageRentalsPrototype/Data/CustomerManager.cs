using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageRentalsPrototype.Data
{
    /// <summary>
    /// This class manages reading in [Customer] objects and handles all necessary functions related to it.
    /// customerList will be accessed in the Customers and Rentals page.
    /// The functions this should have are:
    ///     - SaveCustomerList()
    ///     - LoadCustomerList()
    ///     - CreateNewCustomer()
    ///     - UpdateCustomerInfo()
    ///     
    /// It will also have access to the SystemManager in order to update and access Database records.
    /// </summary>
    public class CustomerManager
    {
        // Variables
        private List<Customer> customerList = new List<Customer>(); // represents the list of customers read in from data
        private SystemManager CustomerSystemManager = new SystemManager();

        // Constructor
        public CustomerManager()
        {
        }

        // Methods

        /// <summary>
        /// Gets the customer list stored in CustomerManager.
        /// </summary>
        /// <returns></returns>
        public List<Customer> GetCustomers()
        {
            LoadCustomerList();
            return customerList;
        }

        /// <summary>
        /// Saves all the changes made in app to the database.
        /// [obsolete], not needed since the page refreshes itself after adding a new customer
        /// </summary>
        //public void SaveCustomerList()
        //{

        //}

        public Customer GetCustomer(string customerId)
        {
            return CustomerSystemManager.GetCustomer(customerId);
        }

        /// <summary>
        /// Loads in all the customers from database into the list.
        /// </summary>
        public void LoadCustomerList()
        {
            customerList = CustomerSystemManager.GetCustomers();
        }

        /// <summary>
        /// Creates a new customer. Should save the customer list after successfully adding.
        /// </summary>
        public void CreateNewCustomer(Customer customer)
        {
            CustomerSystemManager.AddCustomer(customer);
        }

        /// <summary>
        /// Updates an existing customer in the list.
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer) 
        { 
            CustomerSystemManager.UpdateCustomer(customer);
        }

        public void DeleteCustomer(string customerId)
        {
            CustomerSystemManager.DeleteCustomer(customerId);
        }
    }
}
