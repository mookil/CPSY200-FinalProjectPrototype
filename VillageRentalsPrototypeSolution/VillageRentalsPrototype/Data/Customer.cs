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
        public string customerID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string contactPhone { get; set; }
        public string Email { get; set; }
        public string customerNotes { get; set; }

        // Constructor
        public Customer()
        {
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
