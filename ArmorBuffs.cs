using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArmorBuffs;

public class ArmorBuffs : Mod
{
    private static Asset<Texture2D> OldOiled = null;

    public override void Load()
    {
        OldOiled = TextureAssets.Buff[BuffID.Oiled];
        TextureAssets.Buff[BuffID.Oiled] = Assets.Request<Texture2D>("OiledDebuff");
    }

    public override void Unload() => TextureAssets.Buff[BuffID.Oiled] = OldOiled;
}
