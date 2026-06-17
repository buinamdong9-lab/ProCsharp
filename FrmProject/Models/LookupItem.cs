namespace FrmProject.Models
{
    public sealed class LookupItem(int id, string text)
    {
        public int Id { get; } = id;
        public string Text { get; } = text;
    }
}
