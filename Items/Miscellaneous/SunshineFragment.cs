using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Items.Miscellaneous
{
    public class SunshineFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25;

            ItemID.Sets.ItemIconPulse[Type] = true;
            ItemID.Sets.ItemNoGravity[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(silver: 2);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 50);
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, new Color(255, 231, 0).ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }
    }
}
