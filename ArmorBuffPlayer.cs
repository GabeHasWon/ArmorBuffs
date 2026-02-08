using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArmorBuffs;

public class ArmorBuffPlayer : ModPlayer
{
    public string OldSet { get; private set; }

    internal string Set = "";
    internal bool AshPants = false;
    internal bool RoyalSlime = false;
    internal bool VikingHelmet = false;
    internal ushort? NecroDeathTime = null;

    public override void Load() => On_Player.DoubleJumpVisuals += AddWingTime;

    private void AddWingTime(On_Player.orig_DoubleJumpVisuals orig, Player self)
    {
        orig(self);

        if (self.GetModPlayer<ArmorBuffPlayer>().OldSet == ArmorBuffItem.Tungsten)
            self.wingTimeMax = (int)(self.wingTimeMax * 1.25f);
    }

    public override void ResetEffects()
    {
        Player.rocketTimeMax = 7; // Vanilla default, awkward but there's no hooks for it

        if (Set == ArmorBuffItem.Tungsten)
        {
            if (Player.wingTimeMax <= 0)
                Player.rocketTimeMax = (int)(Player.rocketTimeMax * 1.25f);
        }

        OldSet = Set;
        Set = "";
        AshPants = RoyalSlime = VikingHelmet = false;

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
        if (Set == ArmorBuffItem.Crimson)
            Player.lifeRegen += 4;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (AshPants)
            target.AddBuff(BuffID.OnFire, 6 * 60);
        else if (Set == ArmorBuffItem.SnowSet)
            target.AddBuff(BuffID.Frostburn, 7 * 60);
        else if (Set == ArmorBuffItem.Archaeologist)
            target.AddBuff(BuffID.Cursed, 3 * 60);

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
        {
            Player.SetImmuneTimeForAllTypes(30);
            return Main.rand.NextFloat() < 0.1f;
        }

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

    public override void PostUpdateRunSpeeds()
    {
        if (Set == ArmorBuffItem.Lead || VikingHelmet)
        {
            const float Debuff = 0.85f;

            Player.accRunSpeed *= Debuff;
            Player.maxRunSpeed *= Debuff;
            Player.runAcceleration *= Debuff;
        }
    }
}