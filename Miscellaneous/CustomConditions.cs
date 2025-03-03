using Eventful.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Eventful.Miscellaneous
{
    public static class CustomConditions
    {
        public static Condition IsSunnyDay = new Condition("Mods.Eventful.Conditions.IsSunnyDay", () => SunnyDayEvent.isActive);
    }
}
