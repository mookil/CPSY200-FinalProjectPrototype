using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillageRentalsPrototype.Data;

namespace VillageRentalsPrototype.Components.Pages
{
    public partial class EditEquipment : ComponentBase
    {
        EquipmentManager equipmentManager = new EquipmentManager(); // equipment manager that deals with equipment stuff
        private Equipment equipment;
        public bool isSaved = false;

        [Inject] NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string equipmentID { get; set; }

        protected override void OnInitialized()
        {
            equipment = equipmentManager.GetEquipment(equipmentID);
        }

        private async Task UpdateEquipment()
        {
            equipmentManager.UpdateEquipment(equipment);
            isSaved = true;
            await Task.Delay(1000);
            NavigationManager.NavigateTo("/equipment?refresh=" + Guid.NewGuid(), forceLoad: true);
        }
    }
}
