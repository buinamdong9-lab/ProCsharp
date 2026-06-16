namespace FrmProject.Models
{
    public sealed class LookupItem
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
