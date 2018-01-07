using System.IO;

namespace SCPAK
{
    public struct PAKInfo
    {
        public Stream fileStream;
        public string fileName;
        public string typeName;
    }
    public struct ContentFileInfo
    {
        public string fileName;
        public string typeName;
    }
}
