using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using VillageRentalsPrototype.Data;

namespace VillageRentalsPrototype.Components.Pages
{
    public partial class EquipmentPage : ComponentBase
    {
        public List<Equipment> EquipmentList { get; set; } = new List<Equipment>();

        EquipmentManager equipManager = new EquipmentManager();

        [Inject] NavigationManager NavigationManager { get; set; }

        private Equipment equipment;
        private bool isSaved = false;
        private bool isDeleted = false;

        // Methods
        protected override void OnInitialized()
        {
            EquipmentList = equipManager.GetEquipments();
            equipment = new Equipment();
            
        }

        //private string GenerateNewEquipmentID()
        //{
        //    string id = "E0000"; // placeholder id
        //    if (EquipmentList.Count == 0) // If the customer being added is the first customer, set ID to 0001.
        //    {
        //        id = "E0001";
        //    }
        //    else // Otherwise, grab the highest id code from the list and increment it
        //    {
        //        // Get all existing numbers sorted
        //        List<int> existingNumbers = EquipmentList
        //            .Select(c => int.Parse(c.equipmentID.Substring(1)))
        //            .OrderBy(n => n)
        //            .ToList();

        //        // loop that checks for gaps starting from 1
        //        int expectedNumber = 1;
        //        foreach (int existingNumber in existingNumbers)
        //        {
        //            if (existingNumber > expectedNumber)
        //            {
        //                // if theres a gap, use the expected number
        //                id = $"E{expectedNumber:D4}";
        //                return id;
        //            }
        //            expectedNumber = existingNumber + 1;
        //        }
        //        id = $"E{existingNumbers.Last() + 1:D4}"; // set id to the number after the greatest
        //    }
        //    return id;
        //}

        //private string GenerateNewEquipmentID()
        //{
        //    int id = 1;
        //    if (EquipmentList.Count == 0)
        //    {
        //        id = 1;
        //    }
        //    else
        //    {
        //        Equipment temp = EquipmentList.OrderByDescending(a => a.equipmentID).First();
        //        int highestId = Convert.ToInt32(temp.equipmentID);
        //        id = highestId + 1;
        //    }
        //    string categoryId = equipment.CategoryID;
        //    string equipmentId = $"{categoryId}{id}";
        //    return equipmentId;
            
        //}

        private string GenerateNewEquipmentID()
        {
            // Get the numeric portion of all existing IDs for this category
            var existingNumbers = EquipmentList
                .Where(e => e.equipmentID.StartsWith(equipment.CategoryID))
                .Select(e => int.Parse(e.equipmentID.Substring(equipment.CategoryID.Length)))
                .ToList();

            // Calculate next available number (starts at 1 if no existing equipment)
            int nextNumber = existingNumbers.Count > 0 ? existingNumbers.Max() + 1 : 1;

            return $"{equipment.CategoryID}{nextNumber}";
        }

        private async Task SaveEquipment() 
        { 
            equipment.equipmentID = GenerateNewEquipmentID();
            equipManager.AddEquipment(equipment);
            isSaved = true;
            await Task.Delay(1000);
            NavigationManager.NavigateTo("/equipment?refresh=" + Guid.NewGuid(), forceLoad: true);
        }

        private void EditEquipment(Equipment equipment)
        {
            NavigationManager.NavigateTo($"/equipmentedit/{equipment.equipmentID}");
        }

        private async Task DeleteEquipment(Equipment equipment) 
        { 
            equipManager.RemoveEquipment(equipment);
            isDeleted = true;
            await Task.Delay(1000);
            NavigationManager.NavigateTo("/equipment?refresh=" + Guid.NewGuid(), forceLoad: true);
        }

    }
}
