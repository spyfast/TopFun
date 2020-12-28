namespace TopFun.Targets.Flooder
{
    public struct FlooderTarget
    {
        public string Name { get; set; }
        public string Target { get; set; }
        public string NamePlace { get; set; }
        public string Content { get; set; }

        public FlooderTarget(string name, string target, string namePlace, string content)
        {
            Name = name;
            Target = target;
            NamePlace = namePlace;
            Content = content;
        }
    }
}
