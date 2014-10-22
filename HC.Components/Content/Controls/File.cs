using HC.Components.Model;

namespace HC.Components.Content.Controls
{
    public class File : Control
    {
        public File(ColumnField field, ColumnFieldOption option)
            : base(field, option)
        {
        }

        public override string Render(string val)
        {
            return string.Empty;
        }
    }
}