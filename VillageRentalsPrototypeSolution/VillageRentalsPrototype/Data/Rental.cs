using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageRentalsPrototype.Data
{
    /// <summary>
    /// Rental
    /// This is a class for a Rental object. It takes in a customer, as well as a list of equipment.
    /// The format for an RentalID is "R0000", where 0000 is an incrementing number.
    /// </summary>
    public class Rental
    {
        // Public Variables
        public Customer customer { get; }
        public List<Equipment> EquipmentList { get; }

        public string RentalID { get; }
        public DateTime DateRented { get; }
        public string CustomerID { get; }
        public string CustomerLastName { get; }
        public decimal TotalFinalCost { get; }

        // Constructor
        public Rental (Customer customer, List<Equipment> equipmentList, string rentalID)
        {
            // should calculate total final cost based off of the equipment in the equipment list
            this.customer = customer;
            EquipmentList = equipmentList;
            RentalID = rentalID;
            DateRented = DateTime.Now; // sets date rented to current date
            CustomerID = customer.customerID;
            CustomerLastName = customer.lastName;
        }

        // Methods
    }
}
