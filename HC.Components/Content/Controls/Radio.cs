using HC.Components.Model;

namespace HC.Components.Content.Controls
{
    public class Radio : Control
    {
        public Radio(ColumnField field, ColumnFieldOption option)
            : base(field, option)
        {
        }

        public override string Render(string val)
        {
            return string.Empty;
        }
    }
}