using Eventful.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Weapons
{
    public class WaterBalloon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 99;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 18;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 16;
            Item.knockBack = 3;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(copper: 10);
            Item.maxStack = Item.CommonMaxStack;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<WaterBalloonProjectile>();
            Item.shootSpeed = 11;
            Item.UseSound = SoundID.Item1;
        }
    }
}