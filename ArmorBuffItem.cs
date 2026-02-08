using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArmorBuffs;

public class ArmorBuffItem : GlobalItem
{
    public class ArmorBuffRecipeEdits : ModSystem 
    {
        public override void PostAddRecipes()
        {
            foreach (Recipe recipe in Main.recipe)
                if (recipe is not null && recipe.createItem.type == ItemID.DiamondRobe)
                    recipe.AddIngredient(ItemID.Bone, 70);
        }
    }

    public class ArmorBuffNecroBullets : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo { Player: Player plr, Item: Item item } && item.useAmmo == AmmoID.Bullet && plr.GetModPlayer<ArmorBuffPlayer>().Set == Necro)
            {
                projectile.velocity *= 1.35f;

                while (projectile.velocity.LengthSquared() > 16 * 16)
                {
                    projectile.velocity /= 2f;
                    projectile.extraUpdates++;
                }
            }
        }
    }

    public const string Copper = "Copper";
    public const string Tin = "Tin";
    public const string Iron = "Iron";
    public const string Lead = "Lead";
    public const string Silver = "Silver";
    public const string Tungsten = "Tungsten";
    public const string Gold = "Gold";
    public const string SnowSet = "Snow";
    public const string Archaeologist = "Archaeologist";
    public const string AnyNinja = "Ninja";
    public const string Rune = "Rune";
    public const string Necro = "Necro";
    public const string Fossil = "Fossil";
    public const string Crimson = "Crimson";

    internal readonly static Dictionary<string, LocalizedText> AutoloadedTips = [];

    public override void Load()
    {
        AutoloadedTips.Clear();

        AddTip(Copper);
        AddTip(Tin);
        AddTip(Iron);
        AddTip(Lead);
        AddTip(Silver);
        AddTip(Tungsten);
        AddTip(Gold);
        AddTip(SnowSet);
        AddTip(Archaeologist);
        AddTip(AnyNinja);
        AddTip(Rune);
        AddTip(Necro);
        AddTip(Fossil);
        AddTip(Crimson);

        AddTip("RoyalJelly");
        AddTip("AshPants");
        AddTip("RuneHat");
        AddTip("RuneRobe");
        AddTip("VikingHelmet");
        AddTip("MagicHat");
        AddTip("DiamondRobe");
        AddTip("WizardHat");

        AddTip("NecroDeath.0");
        AddTip("NecroDeath.1");
        AddTip("NecroDeath.2");

        static void AddTip(string name) => AutoloadedTips.Add(name, Language.GetOrRegister("Mods.ArmorBuffs.Sets." + name));
    }

    public override void SetDefaults(Item item)
    {
        if (item.type == ItemID.VikingHelmet)
            item.defense = 4;
        else if (item.type == ItemID.RuneHat)
            item.defense = 7;
        else if (item.type == ItemID.RuneRobe)
            item.defense = 4;
        
        if (item.type is ItemID.RuneHat or ItemID.RuneRobe or ItemID.ArchaeologistsHat or ItemID.ArchaeologistsJacket or ItemID.ArchaeologistsPants)
            item.vanity = false;
    }

    public override string IsArmorSet(Item head, Item body, Item legs)
    {
        if (IsSet(ItemID.CopperHelmet, ItemID.CopperChainmail, ItemID.CopperGreaves))
            return Copper;
        else if (IsSet(ItemID.TinHelmet, ItemID.TinChainmail, ItemID.TinGreaves))
            return Tin;
        else if (IsSet(ItemID.IronHelmet, ItemID.IronChainmail, ItemID.IronGreaves, ItemID.AncientIronHelmet))
            return Iron;
        else if (IsSet(ItemID.LeadHelmet, ItemID.LeadChainmail, ItemID.LeadGreaves))
            return Lead;
        else if (IsSet(ItemID.SilverHelmet, ItemID.SilverChainmail, ItemID.SilverGreaves))
            return Silver;
        else if (IsSet(ItemID.TungstenHelmet, ItemID.TungstenChainmail, ItemID.TungstenGreaves))
            return Tungsten;
        else if (IsSet(ItemID.GoldHelmet, ItemID.GoldChainmail, ItemID.GoldGreaves))
            return Gold;
        else if (IsSet(ItemID.EskimoHood, ItemID.EskimoCoat, ItemID.EskimoPants, ItemID.PinkEskimoHood, ItemID.PinkEskimoCoat, ItemID.PinkEskimoPants))
            return SnowSet;
        else if (IsSet(ItemID.ArchaeologistsHat, ItemID.ArchaeologistsJacket, ItemID.ArchaeologistsPants))
            return Archaeologist;
        else if (IsSet(ItemID.NinjaHood, ItemID.NinjaShirt, ItemID.NinjaPants, ItemID.CrystalNinjaHelmet, ItemID.CrystalNinjaChestplate, ItemID.CrystalNinjaLeggings))
            return AnyNinja;
        else if (head.type == ItemID.RuneHat && body.type == ItemID.RuneRobe)
            return Rune;
        else if (IsSet(ItemID.NecroHelmet, ItemID.NecroBreastplate, ItemID.NecroGreaves, ItemID.AncientNecroHelmet))
            return Necro;
        else if (IsSet(ItemID.FossilHelm, ItemID.FossilShirt, ItemID.FossilPants))
            return Fossil;
        else if (IsSet(ItemID.CrimsonHelmet, ItemID.CrimsonScalemail, ItemID.CrimsonGreaves))
            return Crimson;

        return string.Empty;

        bool IsSet(int headId, int bodyId, int legsId, int altHead = -1, int altBody = -1, int altLegs = -1)
        {
            bool isHead = head.type == headId || head.type == altHead;
            bool isBody = body.type == bodyId || body.type == altBody;
            bool isLegs = legs.type == legsId || legs.type == altLegs;
            return isHead && isBody && isLegs;
        }
    }

    public override void UpdateArmorSet(Player player, string set)
    {
        player.GetModPlayer<ArmorBuffPlayer>().Set = set;

        if (set is not "" && AutoloadedTips.TryGetValue(set, out LocalizedText text))
        {
            player.setBonus += (player.setBonus == "" ? "" : Environment.NewLine) + text.Value;

            if (set == Fossil)
                player.setBonus = text.Value;
        }

        switch (set)
        {
            case Copper:
                player.GetDamage(DamageClass.Generic).Flat += 3;
                break;

            case Tin:
                player.GetArmorPenetration(DamageClass.Generic) += 10;
                break;

            case Lead:
                player.noKnockback = true;
                break;

            case Silver:
                Player.jumpSpeed *= 1.5f;
                break;

            case Tungsten:
                player.GetKnockback(DamageClass.Generic) += 0.7f;
                break;

            case Rune:
                player.GetCritChance(DamageClass.Magic) += 25f;
                break;

            case Fossil:
                player.GetDamage(DamageClass.Ranged).Flat += 8;
                break;

            default:
                break;
        }
    }

    public override void UpdateEquip(Item item, Player player)
    {
        if (item.type == ItemID.AshWoodGreaves)
            player.GetModPlayer<ArmorBuffPlayer>().AshPants = true;
        else if (item.type == ItemID.VikingHelmet)
        {
            player.GetModPlayer<ArmorBuffPlayer>().VikingHelmet = true;
            player.endurance += 0.13f;
        }
        else if (item.type == ItemID.WizardHat)
            player.GetDamage(DamageClass.Magic) += 0.1f;
        else if (item.type == ItemID.MagicHat)
            player.GetCritChance(DamageClass.Magic) += 10f;
        else if (item.type == ItemID.RuneHat)
            player.GetDamage(DamageClass.Magic) += 0.15f;
        else if (item.type == ItemID.RuneRobe)
        {
            player.statManaMax2 += 120;
            player.manaCost *= 0.8f;
        }
        else if (item.type == ItemID.DiamondRobe)
        {
            player.GetDamage(DamageClass.Magic) += 0.15f;
            player.statManaMax2 += 20;
        }
        else if (item.type == ItemID.RoyalGel)
            player.GetModPlayer<ArmorBuffPlayer>().RoyalSlime = true;

        if (item.prefix == PrefixID.Lucky)
            player.luck += 0.1f;
        else if (item.prefix is PrefixID.Brisk or PrefixID.Fleeting or PrefixID.Hasty or PrefixID.Quick)
        {
            Player.jumpSpeed *= item.prefix switch
            {
                PrefixID.Brisk => 1.05f,
                PrefixID.Fleeting => 1.07f,
                PrefixID.Hasty => 1.10f,
                _ => 1.15f,
            };
        }
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        AddTip(ItemID.AshWoodGreaves, "AshPants");
        AddTip(ItemID.RoyalGel, "RoyalJelly");
        AddTip(ItemID.RuneRobe, "RuneRobe");
        AddTip(ItemID.RuneHat, "RuneHat");
        AddTip(ItemID.DiamondRobe, "DiamondRobe");
        AddTip(ItemID.WizardHat, "WizardHat");
        AddTip(ItemID.VikingHelmet, "VikingHelmet");

        if (item.type == ItemID.DiamondRobe && tooltips.Find(x => x.Name == "Tooltip0") is { } removeLine)
            tooltips.Remove(removeLine);
        else if (item.type == ItemID.WizardHat && tooltips.Find(x => x.Name == "Tooltip0") is { } wizardLine)
            tooltips.Remove(wizardLine);

        void AddTip(int id, string name)
        {
            if (item.type == id)
            {
                int index = tooltips.Count - 1;

                if (tooltips.FindIndex(x => x.Name == "SetBonus") is { } value and not -1)
                    index = value - 1;

                tooltips.Insert(index, new(Mod, name + "Bonus", AutoloadedTips[name].Value));
            }
        }
    }

    public override void ModifyItemScale(Item item, Player player, ref float scale)
    {
        if (player.GetModPlayer<ArmorBuffPlayer>().Set == Iron)
            scale *= 1.25f;
    }
}