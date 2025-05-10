using Eventful.Invasions;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Eventful.Utilities
{
    internal class InvasionProgressUI : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));

            #region Buried Barrage
            if (BuriedBarrageInvasion.isActive)
            {
                int index = layers.FindIndex(layer => layer is not null && layer.Name.Equals("Vanilla: Inventory"));
                LegacyGameInterfaceLayer NewLayer = new LegacyGameInterfaceLayer("Eventful: Buried Barrage UI",
                    delegate
                    {
                        Eventful.Instance.DrawEventUI(Main.spriteBatch);
                        return true;
                    },
                    InterfaceScaleType.UI);
                layers.Insert(index, NewLayer);
            }
            #endregion
        }
    }
}