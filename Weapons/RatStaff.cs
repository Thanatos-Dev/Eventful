using Eventful.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Weapons
{
    public class RatStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 46;

            Item.mana = 30;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 8;
            Item.knockBack = 0;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 1, silver: 50);

            Item.shoot = ModContent.ProjectileType<RatStaffProjectile>();
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.noMelee = true;

            #region Use sound
            Item.UseSound = SoundID.Zombie15 with
            {
                Volume = 0.25f,
                PitchVariance = 0.25f,
            };
            #endregion
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[Item.shoot] == 0 && !Collision.IsWorldPointSolid(Main.MouseWorld, true))
            {
                RatStaffProjectile.spawnTimer = RatStaffProjectile.spawnTimerMax;

                Projectile.NewProjectile(source, Main.MouseWorld + new Vector2(0, -10), velocity, type, damage, knockback, Main.myPlayer);
            }
            else
            {
                foreach (var projectile in Main.ActiveProjectiles)
                {
                    if (projectile.type == ModContent.ProjectileType<RatStaffProjectile>())
                    {
                        projectile.Kill();
                    }
                }

                if (!Collision.IsWorldPointSolid(Main.MouseWorld, true))
                {
                    RatStaffProjectile.spawnTimer = RatStaffProjectile.spawnTimerMax;

                    Projectile.NewProjectile(source, Main.MouseWorld + new Vector2(0, -10), velocity, type, damage, knockback, Main.myPlayer);
                }
            }

            return false;
        }
    }
}