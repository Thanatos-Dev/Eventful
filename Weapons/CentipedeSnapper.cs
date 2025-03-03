using Eventful.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Eventful.Weapons
{
    public class CentipedeSnapper : ModItem
    {
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(CentipedeSnapperProjectile.summonTagDamage);

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 32;

            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 15;
            Item.knockBack = 1;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 1, silver: 50);

            Item.shoot = ModContent.ProjectileType<CentipedeSnapperProjectile>();
            Item.shootSpeed = 8;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item152;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        //Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}