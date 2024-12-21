using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Eventful.Items.WeatherToggles
{
    public class WindForecast : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Blue;
            Item.noMelee = true;
            Item.consumable = true;
            Item.maxStack = Item.CommonMaxStack;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = Item.useAnimation = 45;
            Item.UseSound = SoundID.Item4;
        }

        public override bool CanUseItem(Player player)
        {
            if (Main.windSpeedTarget <= 0.8f || Main.windSpeedTarget >= -0.8f)
            {
                return true;
            }

            return false;
        }

        public override bool? UseItem(Player player)
        {
            bool windDirection;
            
            windDirection = Main.rand.NextBool();

            if (windDirection == false)
            {
                Main.windSpeedTarget = Main.windSpeedCurrent = 0.8f;
            }
            else if (windDirection == true)
            {
                Main.windSpeedTarget = Main.windSpeedCurrent = -0.8f;
            }

            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }

            #region Chat Message
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
            string key = "The wind has picked up!";
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