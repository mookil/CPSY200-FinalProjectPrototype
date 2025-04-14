using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageRentalsPrototype.Data
{
    /// <summary>
    /// [Category]
    /// This class represents a category of an [Equipment] object.
    /// It will be stored in a list in SystemManager.
    /// The format for a CategoryID is 10, 20, 30, 100, 110, etc.
    /// </summary>
    public class Category
    {
        // Public Variables
        public string CategoryID { get; set; }
        public string CategoryDesc { get; set; }

        // Constructor
        public Category ()
        {
        }

        // Methods
    }
}
