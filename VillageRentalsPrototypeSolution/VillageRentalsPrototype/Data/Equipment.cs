using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageRentalsPrototype.Data
{
    /// <summary>
    /// An equipment object.
    /// The format for an equipmentID is "E0000", where 0000 is an incrementing number.
    /// </summary>
    public class Equipment
    {
        // Public Variables
        public string equipmentID { get; set; }
        public string CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal DailyRentalCost { get; set; }

        // [These two variables are for Rental equipment.]
        public string RentalDate { get; set; }

        public string RentalReturn { get; set; }

        // Constructor
        public Equipment()
        {
        }

        // Methods
        public override string ToString()
        {
            return $"{equipmentID} {Name}";
        }
    }
}
