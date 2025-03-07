﻿using Terraria;
using Terraria.ModLoader;

namespace Eventful.Items.Miscellaneous
{
    public class MutatedFlesh : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 50;
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