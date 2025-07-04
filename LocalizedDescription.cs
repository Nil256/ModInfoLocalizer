namespace ModInfoLocalizer
{
    internal readonly struct LocalizedDescription
    {
        internal readonly int Priority { get; init; }
        internal readonly string Text { get; init; }

        internal LocalizedDescription(string text, int priority = 0)
        {
            Text = text;
            Priority = priority;
        }

        public static LocalizedDescription operator +(LocalizedDescription a, LocalizedDescription b)
        {
            return (a.Priority <= b.Priority) ? b : a;
        }
    }
}
