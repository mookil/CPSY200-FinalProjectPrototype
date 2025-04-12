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
    /// </summary>
    public class Category
    {
        // Public Variables
        public string CategoryID { get; }
        public string CategoryDesc { get; }

        // Constructor
        public Category (string categoryID, string desc)
        {
            CategoryID = categoryID;
            CategoryDesc = desc;
        }

        // Methods
    }
}
