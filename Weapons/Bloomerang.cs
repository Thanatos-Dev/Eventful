using Eventful.Projectiles.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Weapons
{
    public class Bloomerang : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;

            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.damage = 22;
            Item.knockBack = 8;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 1);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = false;

            Item.shoot = ModContent.ProjectileType<BloomerangProjectile>();
            Item.shootSpeed = 11.5f;
            Item.UseSound = SoundID.Item1;
        }

        public override bool CanShoot(Player player)
        {
            if (player.ownedProjectileCounts[Item.shoot] == 0)
            {
                return base.CanShoot(player);
            }

            return false;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[Item.shoot] == 0)
            {
                return base.CanUseItem(player);
            }

            return false;
        }
    }
}