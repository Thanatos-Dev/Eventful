using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Dusts
{
    public class SunBeamExplosionDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.scale = Main.rand.NextFloat(4f, 8f);
            dust.alpha = 255;
            dust.color = new Color(255, 255, 255, 50);
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.25f;
            dust.scale *= 0.9f;
            dust.velocity *= 0.9f;

            Lighting.AddLight(dust.position, new Color(255, 191, 0).ToVector3() * 0.35f);

            if (dust.scale < 1.25f)
            {
                dust.alpha = 1;
            }

            if (dust.scale < 0.05f)
            {
                dust.active = false;
            }

            return false;
        }
    }
}