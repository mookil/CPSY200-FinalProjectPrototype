using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageRentalsPrototype.Data
{
    /// <summary>
    /// [Customer]
    /// This class represents a Customer.
    /// The format for a customerID is "C0000" where 0000 is an incrementing number.
    /// </summary>
    public class Customer
    {
        // Variables
        public string customerID { get; }
        public string firstName { get; }
        public string lastName { get; }
        public string contactPhone { get; }
        public string Email { get; }
        public string customerNotes { get; }

        // Constructor
        public Customer(string customerId, string firstname, string lastname, string contactphone, string email, string customernotes)
        {
            customerID = customerId;
            firstName = firstname;
            lastName = lastname;
            contactPhone = contactphone;
            Email = email;
            customerNotes = customernotes;
        }

        // Methods
        public override string ToString()
        {
            return $"CustomerID = {customerID}\n" +
                $"firstName = {firstName}\n" +
                $"lastName = {lastName}\n" +
                $"contactPhone = {contactPhone}\n" +
                $"Email = {Email}\n" +
                $"customerNotes = {customerNotes}\n";
        }

    }
}
