using Eventful.Projectiles.Weapons.SunBeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Weapons
{
    public class SunBeam : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;

            Item.mana = 6;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 22;
            Item.knockBack = 6.5f;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(gold: 1);

            Item.shoot = ModContent.ProjectileType<SunBeamProjectile>();
            Item.shootSpeed = 10f;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item45;
        }
    }
}