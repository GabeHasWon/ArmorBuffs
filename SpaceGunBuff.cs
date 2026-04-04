using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArmorBuffs;

[Autoload(false)]
internal class SpaceGunBuff : GlobalProjectile
{
    internal class ElectrifiedFunctionalityForNPCs : GlobalNPC
    {
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (npc.HasBuff(BuffID.Electrified))
            {
                damage = Math.Max(damage, 5);
                npc.lifeRegen = 0;

                if (npc.velocity.LengthSquared() < 0.01f)
                    npc.lifeRegen -= 40;
                else
                    npc.lifeRegen -= 8;
            }
        }
    }

    public override bool InstancePerEntity => true;

    internal bool spaceGunAttack = false;

    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.GreenLaser;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (source is EntitySource_ItemUse_WithAmmo { Item: Item item, Player: Player plr } && item.type == ItemID.SpaceGun 
            && plr.GetModPlayer<ArmorBuffPlayer>().Set == ArmorBuffItem.Meteorite)
            spaceGunAttack = true;
    }

    public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (spaceGunAttack)
            target.AddBuff(BuffID.Electrified, 3 * 60);
    }

    public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
    {
        if (spaceGunAttack)
            target.AddBuff(BuffID.Electrified, 3 * 60, false);
    }

    public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter) => bitWriter.WriteBit(spaceGunAttack);
    public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader) => spaceGunAttack = bitReader.ReadBit();
}
