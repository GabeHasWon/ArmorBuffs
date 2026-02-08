using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArmorBuffs;

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

    private bool _spaceGunAttack = false;

    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.GreenLaser;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (source is EntitySource_ItemUse_WithAmmo { Item: Item item } && item.type == ItemID.SpaceGun)
            _spaceGunAttack = true;
    }

    public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (_spaceGunAttack)
            target.AddBuff(BuffID.Electrified, 3 * 60);
    }

    public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
    {
        if (_spaceGunAttack)
            target.AddBuff(BuffID.Electrified, 3 * 60);
    }
}
