using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using VillageRentalsPrototype.Data;

namespace VillageRentalsPrototype.Components.Pages
{
    public partial class Home : ComponentBase
    {
        SystemManager systemManager = new SystemManager();
        protected override void OnInitialized() // overrides the OnInitializied method
        {
            systemManager.InitializeDatabase();
            systemManager.PopulateIfEmpty();
        }
    }
}
