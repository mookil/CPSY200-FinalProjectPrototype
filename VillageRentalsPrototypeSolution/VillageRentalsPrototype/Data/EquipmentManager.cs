using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillageRentalsPrototype.Data
{
    /// <summary>
    /// This class handles Equipment, and also Categories. 
    /// Equipment list will be handled in the equipment and rentals page.
    /// This functions this should have are:
    ///     - Add Equipment
    ///     - Add Category
    ///     - Edit Equipment
    ///     - Edit Category
    ///     - Remove Equipment
    ///     - Remove Category
    ///     
    /// It will also have access to the SystemManager for database purposes.
    /// </summary>
    public class EquipmentManager
    {
        // Variables
        private List<Equipment> equipmentList = new List<Equipment>();
        private List<Category> categoryList = new List<Category>();
        private SystemManager EquipmentSystemManager = new SystemManager();

        // Constructor
        public EquipmentManager() { }

        // Methods
        public List<Equipment> GetEquipments()
        {
            equipmentList = EquipmentSystemManager.GetEquipmentList();
            return equipmentList;
        }

        public Equipment GetEquipment(string equipmentID) 
        {
            return EquipmentSystemManager.GetEquipment(equipmentID);
        }


        public void AddEquipment(Equipment equipment) 
        { 
            EquipmentSystemManager.AddEquipment(equipment);
        }

        public void UpdateEquipment(Equipment equipment) 
        { 
            EquipmentSystemManager.UpdateEquipment(equipment);
        }

        public void RemoveEquipment(Equipment equipment)
        {
            EquipmentSystemManager.DeleteEquipment(equipment.equipmentID);
        }



        //public void AddCategory()
        //{

        //}

        //public void UpdateCategory()
        //{

        //}

        //public void RemoveCategory() { }

    }
}
