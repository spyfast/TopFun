namespace TopFun.Targets.Autoans
{
    public struct AutoansTarget
    {
        public string Name { get; set; }
        public string Target { get; set; }
        public string Ids { get; set; }
        public string Content { get; set; }

        public AutoansTarget(string name, string target, string ids, string content)
        {
            Name = name;
            Target = target;
            Ids = ids;
            Content = content;
        }
    }
}
