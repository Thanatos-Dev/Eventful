using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace Eventful.Items.Accessories
{
    [AutoloadEquip(EquipType.HandsOn)]
    public class MosquitoBand : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 22;
            Item.accessory = true;
            Item.hasVanityEffects = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += (int)(player.statLifeMax2 * 0.05);
            player.lifeRegen = 2;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.AddIngredient(ItemID.BandofRegeneration);
            recipe.AddIngredient(ModContent.ItemType<MosquitoSack>());
            recipe.Register();
        }
    }
}