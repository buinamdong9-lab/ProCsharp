namespace FrmProject.Models
{
    internal sealed class LookupItem
    {
        public int Id { get; }
        public string Text { get; }

        public LookupItem(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}
