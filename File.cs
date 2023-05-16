namespace Rule30
{
    internal class File
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public File() { }
        public File(string name, string path)
        {
            Name = name;
            Path = path;

        }
    }
}
