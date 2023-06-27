using SFML.Graphics;

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
