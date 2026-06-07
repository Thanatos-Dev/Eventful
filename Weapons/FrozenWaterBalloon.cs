using Eventful.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Weapons
{
    public class FrozenWaterBalloon : ModItem
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
            Item.damage = 18;
            Item.knockBack = 3;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(copper: 10);
            Item.maxStack = Item.CommonMaxStack;

            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.consumable = true;

            Item.shoot = ModContent.ProjectileType<FrozenWaterBalloonProjectile>();
            Item.shootSpeed = 11;
            Item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            CreateRecipe(10)
                .AddIngredient(ModContent.ItemType<WaterBalloon>(), 10)
                .AddIngredient(ItemID.IceTorch)
                .Register();
        }
    }
}