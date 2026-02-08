using Terraria;
using Terraria.ModLoader;

namespace ArmorBuffs;

internal class PvPCritFunctionality : ModPlayer
{
    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (modifiers.PvP)
        {
            if (modifiers.DamageSource is { SourcePlayerIndex: >-1 and int attackingWho })
            {
                Player attacker = Main.player[attackingWho];

                if (modifiers.DamageSource is { SourceItem: Item item } && attacker.GetWeaponCrit(item) > Main.rand.NextFloat())
                {
                    modifiers.FinalDamage += 2f;
                    modifiers.Knockback *= attacker.GetTotalKnockback(item.DamageType).ApplyTo(1.4f);
                }

                if (modifiers.DamageSource is { SourceProjectileLocalIndex: int projectileWho } && Main.projectile[projectileWho].CritChance > Main.rand.NextFloat())
                {
                    modifiers.FinalDamage += 2f;
                    modifiers.Knockback *= attacker.GetTotalKnockback(Main.projectile[projectileWho].DamageType).ApplyTo(1.4f);
                }
            }
        }
    }
}
