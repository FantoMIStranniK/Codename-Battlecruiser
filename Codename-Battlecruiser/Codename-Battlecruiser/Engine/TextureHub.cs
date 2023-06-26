using Codename_Battlecruiser.Engine.Base;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum TextureName
{
    BattleCruiser,
    DestroyedCell,
    EmptyCell,
}
namespace Codename_Battlecruiser.Engine
{
    public static class TextureHub
    {
        public static Dictionary<TextureName, Texture> Textures = new Dictionary<TextureName, Texture>();

        public static void LoadTextures()
        {

        }
    }
}
