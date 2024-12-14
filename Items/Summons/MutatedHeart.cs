using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Eventful.Invasions;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Eventful.Miscellaneous;

namespace Eventful.Items.Summons
{
    public class MutatedHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
            Item.noMelee = true;
            Item.consumable = true;
            Item.maxStack = 9999;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = Item.useAnimation = 45;
            Item.UseSound = SoundID.Roar;
        }

        public override bool CanUseItem(Player player)
        {
            if (BuriedBarrageInvasion.isActive == true)
            {
                return false;
            }

            if (player.ZoneNormalCaverns == false)
            {
                return false;
            }

            return true;
        }

        public override bool? UseItem(Player player)
        {
            BuriedBarrageInvasion.isActive = true;

            #region Chat Message
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            string key = "The Buried Barrage is invading the caverns!";
            Color messageColor = new Color(175, 75, 255);
            if (Main.netMode == NetmodeID.Server) // Server
            {
                Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
            }
            else if (Main.netMode == NetmodeID.SinglePlayer) // Single Player
            {
                Main.NewText(Language.GetTextValue(key), messageColor);
            }
            #endregion

            return true;
        }
    }
}