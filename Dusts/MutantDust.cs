using Terraria;
using Terraria.ModLoader;

namespace Eventful.Dusts
{
    public class MutantDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.velocity *= 0.1f;
            dust.noGravity = false;
        }

        public override bool Update(Dust dust)
        { // Calls every frame the dust is active
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.15f;
            dust.scale -= 0.05f;

            if (dust.scale < 0.1f)
            {
                dust.active = false;
            }

            return false; // Return false to prevent vanilla behavior.
        }
    }
}