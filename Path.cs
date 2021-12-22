using System.IO;

namespace DiagramCreater
{
    public static class Path
    {
        public static string path
        {
            get
            {
                var path = Directory.GetCurrentDirectory();
                path = path.Substring(0, path.Length - 9);
                return path;
            }
        }
    }
}