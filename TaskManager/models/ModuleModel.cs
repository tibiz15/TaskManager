namespace TaskManager.models
{
    class ModuleModel
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public ModuleModel(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}
