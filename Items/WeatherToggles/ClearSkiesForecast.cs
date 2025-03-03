using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Eventful.Items.WeatherToggles
{
    public class ClearSkiesForecast : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 42;
            Item.scale = 0.75f;
            Item.rare = ItemRarityID.Blue;
            Item.noMelee = true;
            Item.consumable = true;
            Item.maxStack = Item.CommonMaxStack;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.holdStyle = ItemHoldStyleID.HoldFront;
            Item.useTime = Item.useAnimation = 30;
            Item.UseSound = SoundID.Item92;
            Item.value = Item.buyPrice(gold: 1);
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation += new Vector2(0, -7.5f);
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation += new Vector2(-15 * player.direction, 5);
        }

        public override bool CanUseItem(Player player)
        {
            return Main.IsItRaining || Main.IsItStorming;
        }

        public override bool? UseItem(Player player)
        {
            Main.raining = false;
            Main.maxRaining = Main.cloudAlpha = 0;

            if (Main.IsItStorming == true)
            {
                Main.windSpeedTarget = Main.windSpeedCurrent = 0;
            }

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
                Main.SyncRain();
            }

            #region Chat Message
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            }

            string key = "It has stopped raining!";


            if (Main.IsItStorming == true)
            {
                key = "It has stopped thunderstorming!";
            }
            else if (Main.IsItStorming == false)
            {
                key = "It has stopped raining!";
            }

            Color messageColor = new Color(50, 255, 130);
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