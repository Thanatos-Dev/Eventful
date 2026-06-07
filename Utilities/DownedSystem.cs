using System.Collections;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Eventful.Utilities
{
    public class DownedSystem : ModSystem
    {
        public static bool downedBuriedBarrage = false;

        public override void ClearWorld()
        {
            downedBuriedBarrage = false;
        }

        // We save our data sets using TagCompounds.
        // NOTE: The tag instance provided here is always empty by default.
        public override void SaveWorldData(TagCompound tag)
        {
            if (downedBuriedBarrage)
            {
                tag["downedBuriedBarrage"] = true;
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            downedBuriedBarrage = tag.ContainsKey("downedBuriedBarrage");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WriteFlags(downedBuriedBarrage);
        }

        public override void NetReceive(BinaryReader reader)
        {
            reader.ReadFlags(out downedBuriedBarrage);
        }
    }
}