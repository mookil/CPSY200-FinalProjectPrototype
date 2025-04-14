using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageRentalsPrototype.Data
{
    public class RentalManager
    {
        private CustomerManager customerManager = new CustomerManager();
        private EquipmentManager equipmentManager = new EquipmentManager();
        private SystemManager systemManager = new SystemManager();

        public RentalManager() { }

        // Methods
        public List<Rental> GetRentals()
        {
            return systemManager.GetRentals();
        }

        public Rental GetRental(string rentalID)
        {
            return systemManager.GetRental(rentalID);
        }

        public void AddRental(Rental rental)
        {
            systemManager.AddRental(rental);
        }

        public void EditRental()
        {

        }
    }
}
