using Eventful.Invasions;
using Eventful.Items.Miscellaneous;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Eventful.Items.Summons
{
    public class MutatedHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
            Item.noMelee = true;
            Item.consumable = true;
            Item.maxStack = Item.CommonMaxStack;
            Item.autoReuse = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = Item.useAnimation = 45;
            Item.UseSound = SoundID.Roar;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossSpawners;
        }

        public override bool CanUseItem(Player player)
        {
            if (BuriedBarrageInvasion.isActive)
            {
                return false;
            }

            return true;
        }

        public override bool? UseItem(Player player)
        {
            string key = "The Buried Barrage is invading the caverns!";
            Color messageColor = new Color(175, 75, 255);
            Main.NewText(Language.GetTextValue(key), messageColor);

            BuriedBarrageInvasion.isActive = true;
            BuriedBarrageInvasion.killCount = 0;

            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<MutatedFlesh>(15)
                .AddTile(TileID.DemonAltar)
                .Register();
        }
    }
}