using Eventful.Projectiles.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            Item.height = 16;

            Item.DamageType = DamageClass.Ranged;
            Item.damage = 16;
            Item.knockBack = 3;
            Item.rare = ItemRarityID.White;
            Item.value = Item.sellPrice(copper: 10);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.consumable = true;
            Item.maxStack = Item.CommonMaxStack;

            Item.shoot = ModContent.ProjectileType<WaterBalloonProjectile>();
            Item.shootSpeed = 11;
            Item.UseSound = SoundID.Item1;
        }
    }
}