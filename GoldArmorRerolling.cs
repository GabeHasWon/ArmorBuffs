using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace ArmorBuffs;

#nullable enable

internal class GoldArmorRerolling : ILoadable
{
    private static Player? DroppingPlayer = null;

    public void Load(Mod mod)
    {
        On_NPC.NPCLoot_DropItems += OnDropItems;
        On_ItemDropResolver.ResolveRule += ModifyDropRate;
    }

    private ItemDropAttemptResult ModifyDropRate(On_ItemDropResolver.orig_ResolveRule orig, ItemDropResolver self, IItemDropRule rule, DropAttemptInfo info)
    {
        if (DroppingPlayer is not null && DroppingPlayer.TryGetModPlayer(out ArmorBuffPlayer tracker) && tracker.Set == ArmorBuffItem.Gold && Main.rand.NextBool())
            orig(self, rule, info);

        return orig(self, rule, info);
    }

    private void OnDropItems(On_NPC.orig_NPCLoot_DropItems orig, NPC self, Player closestPlayer)
    {
        DroppingPlayer = closestPlayer;
        orig(self, closestPlayer);
        DroppingPlayer = null;
    }

    public void Unload()
    {
    }
}
