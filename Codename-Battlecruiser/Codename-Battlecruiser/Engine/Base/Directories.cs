
namespace Codename_Battlecruiser.Engine.Base
{
    public static class Directories
    {
        public static readonly string pathToProject = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        public static readonly string pathToConfig = pathToProject + @"\Configs\";
        public static readonly string pathToTextures = pathToProject + @"\Textures\";
    }
}
