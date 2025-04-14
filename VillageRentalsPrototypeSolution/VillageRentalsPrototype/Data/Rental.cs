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
        public Customer customer { get; set; }
        public List<Equipment> EquipmentList { get; set; }

        public string RentalID { get; set; }
        public string Date { get; set; }
        public string CustomerID { get; set; }
        public string CustomerLastName { get; set; }
        public string EquipmentId { get; set; }
        public decimal TotalFinalCost { get; set; }

        // Constructor
        public Rental ()
        {
        }

        // Methods
    }
}
