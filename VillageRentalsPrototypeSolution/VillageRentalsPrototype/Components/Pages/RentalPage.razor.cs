using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using VillageRentalsPrototype.Data;

namespace VillageRentalsPrototype.Components.Pages
{
    public partial class RentalPage : ComponentBase
    {
        public List<Equipment> RentalsList { get; set; } = new List<Equipment>();
        RentalManager rentalManager = new RentalManager();
    }
}
