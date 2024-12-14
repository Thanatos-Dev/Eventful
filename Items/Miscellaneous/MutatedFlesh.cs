using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Eventful.Items.Miscellaneous
{
    public class MutatedFlesh : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(copper: 2);
        }
    }
}