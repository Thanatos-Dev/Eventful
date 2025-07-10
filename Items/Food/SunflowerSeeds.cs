using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Eventful.Items.Miscellaneous;

namespace Eventful.Items.Food
{
    public class SunflowerSeeds : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;

            // This is to show the correct frame in the inventory
            // The MaxValue argument is for the animation speed, we want it to be stuck on frame 1
            // Setting it to max value will cause it to take 414 days to reach the next frame
            // No one is going to have game open that long so this is fine
            // The second argument is the number of frames, which is 3
            // The first frame is the inventory texture, the second frame is the holding texture,
            // and the third frame is the placed texture
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

            // This allows you to change the color of the crumbs that are created when you eat.
            // The numbers are RGB (Red, Green, and Blue) values which range from 0 to 255.
            // Most foods have 3 crumb colors, but you can use more or less if you desire.
            // Depending on if you are making solid or liquid food switch out FoodParticleColors
            // with DrinkParticleColors. The difference is that food particles fly outwards
            // whereas drink particles fall straight down and are slightly transparent
            ItemID.Sets.FoodParticleColors[Item.type] = [
                new Color(214, 189, 168),
                new Color(100, 83, 72),
                new Color(53, 37, 32)
            ];

            ItemID.Sets.IsFood[Type] = true; //This allows it to be placed on a plate and held correctly
        }

        public override void SetDefaults()
        {
            int buffDuration = 10; // In minutes

            // DefaultToFood sets all of the food related item defaults such as the buff type, buff duration, use sound, and animation time.
            Item.DefaultToFood(22, 22, BuffID.WellFed2, 60 * 60 * buffDuration);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<SunshineFragment>(2)
                .AddTile(TileID.CookingPots)
                .Register();
        }
    }
}