using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArmorBuffs;

internal class ArmorRecipeModifications : ModSystem
{
    public static Dictionary<int, int> ArmorIdToMaterial = new()
    {
        { ItemID.CopperHelmet, ItemID.CopperBar }, { ItemID.CopperChainmail, ItemID.CopperBar }, { ItemID.CopperGreaves, ItemID.CopperBar },
        { ItemID.TinHelmet, ItemID.TinBar }, { ItemID.TinChainmail, ItemID.TinBar }, { ItemID.TinGreaves, ItemID.TinBar },
        { ItemID.IronHelmet, ItemID.IronBar }, { ItemID.IronChainmail, ItemID.IronBar  }, { ItemID.IronGreaves, ItemID.IronBar },
        { ItemID.LeadHelmet, ItemID.LeadBar }, { ItemID.LeadChainmail, ItemID.LeadBar  }, { ItemID.LeadGreaves, ItemID.LeadBar },
        { ItemID.SilverHelmet, ItemID.SilverBar }, { ItemID.SilverChainmail, ItemID.SilverBar  }, { ItemID.SilverGreaves, ItemID.SilverBar },
        { ItemID.TungstenHelmet, ItemID.TungstenBar }, { ItemID.TungstenChainmail, ItemID.TungstenBar  }, { ItemID.TungstenGreaves, ItemID.TungstenBar },
    };

    public override void PostAddRecipes()
    {
        for (int i = 0; i < Main.recipe.Length; ++i)
        {
            Recipe recipe = Main.recipe[i];

            if (recipe != null && ArmorIdToMaterial.TryGetValue(recipe.createItem.type, out int material))
            {
                foreach (Item ingredient in recipe.requiredItem)
                {
                    if (ingredient.type == material)
                        ingredient.stack /= 2;
                }
            }
        }
    }
}
