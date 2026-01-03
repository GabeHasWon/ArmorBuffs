using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArmorBuffs;

public class ArmorBuffPlayer : ModPlayer
{
    internal string Set = "";
    internal bool AshPants = false;
    internal bool RoyalSlime = false;
    internal ushort? NecroDeathTime = null;

    public override void ResetEffects()
    {
        Set = "";
        AshPants = RoyalSlime = false;

        if (NecroDeathTime.HasValue)
        {
            NecroDeathTime--;

            if (NecroDeathTime % 3 == 0)
                Dust.NewDustPerfect(Player.Center, Main.rand.NextBool() ? DustID.Bone : DustID.Ash, Main.rand.NextVector2Circular(6, 6), Main.rand.Next(0, 135));

            if (NecroDeathTime == 0)
            {
                Player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromKey(ArmorBuffItem.AutoloadedTips["NecroDeath." + Main.rand.Next(3)].Key, Player.name)), 9999, 0);
                NecroDeathTime = null;

                for (int i = 0; i < 15; ++i)
                {
                    short type = Main.rand.NextBool() ? DustID.Bone : DustID.Ash;
                    Dust.NewDustPerfect(Player.Center, type, Main.rand.NextVector2Circular(6, 6), Main.rand.Next(0, 135), Scale: Main.rand.NextFloat(1, 1.5f));
                }
            }
        }
    }

    public override void UpdateLifeRegen()
    {
        if (Set == ArmorBuffItem.Silver)
            Player.lifeRegen += 2;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (AshPants)
            target.AddBuff(BuffID.OnFire, 6 * 60);
        else if (Set == ArmorBuffItem.SnowSet && Main.rand.NextBool(3))
            target.AddBuff(BuffID.Frostburn, 3 * 60);

        if (RoyalSlime && Main.rand.NextFloat() <= 0.15f)
            target.AddBuff(BuffID.Oiled, 4 * 60);
    }

    public override void OnHitAnything(float x, float y, Entity victim)
    {
        if (victim is Player plr && RoyalSlime && Main.rand.NextFloat() <= 0.15f)
            plr.AddBuff(BuffID.Oiled, 4 * 60);
    }

    public override bool FreeDodge(Player.HurtInfo info)
    {
        if (Set == ArmorBuffItem.AnyNinja)
            return Main.rand.NextFloat() < 0.1f;

        if (Set == ArmorBuffItem.Necro && NecroDeathTime is > 0)
            return true;

        return false;
    }

    public override void UpdateBadLifeRegen()
    {
        if (Player.HasBuff(BuffID.Oiled))
        {
            if (Player.HasBuff(BuffID.OnFire) || Player.HasBuff(BuffID.OnFire3) || Player.HasBuff(BuffID.ShadowFlame) || Player.HasBuff(BuffID.CursedInferno)
                || Player.HasBuff(BuffID.Frostburn) || Player.HasBuff(BuffID.Frostburn2))
                Player.lifeRegen -= 50;
        }
    }

    public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
    {
        if (Set == ArmorBuffItem.Necro && NecroDeathTime is null)
        {
            NecroDeathTime = 7 * 60;
            return false;
        }

        return true;
    }

    public override bool CanConsumeAmmo(Item weapon, Item ammo) => Set != ArmorBuffItem.Fossil || Main.rand.NextBool();
}