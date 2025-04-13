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
    }
}
