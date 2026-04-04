using Terraria;
using Terraria.ModLoader;

namespace ArmorBuffs;

internal class PvPCritFunctionality : ModPlayer
{
    public override bool IsLoadingEnabled(Mod mod) => false;

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (modifiers.PvP)
        {
            if (modifiers.DamageSource is { SourcePlayerIndex: >-1 and int attackingWho })
            {
                Player attacker = Main.player[attackingWho];

                if (modifiers.DamageSource is { SourceItem: Item item } && attacker.GetWeaponCrit(item) > Main.rand.NextFloat())
                    modifiers.FinalDamage += 1f;

                if (modifiers.DamageSource is { SourceProjectileLocalIndex: int projectileWho } && Main.projectile[projectileWho].CritChance / 100f > Main.rand.NextFloat())
                    modifiers.FinalDamage += 1f;
            }
        }
    }
}
