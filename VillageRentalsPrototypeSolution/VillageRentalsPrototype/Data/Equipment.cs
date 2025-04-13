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
        public string equipmentID { get; }
        public Category Category { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal DailyRentalCost { get; }

        // [These two variables are for Rental equipment.]
        public DateTime RentalDate { get; }

        public DateTime RentalReturn { get; }

        // Constructor
        public Equipment(string id, Category category, string name, string desc, decimal dailyrentalcost)
        {
            equipmentID = id;
            Category = category;
            Name = name;
            Description = desc;
            DailyRentalCost = dailyrentalcost;
        }

        // Methods
    }
}
