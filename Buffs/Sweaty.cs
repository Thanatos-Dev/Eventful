using Eventful.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Buffs
{
    public class Sweaty : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true; // The time remaining won't display on this buff
            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 10; // reset buff time

            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(player.position, player.width, player.height, DustID.Water, default, default, 150, default, 0.75f);
            }
        }
    }
}
