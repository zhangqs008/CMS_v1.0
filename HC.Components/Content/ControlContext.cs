namespace HC.Components.Content
{
    public class ControlContext
    {
        private readonly Control _control;

        public ControlContext(Control control)
        {
            _control = control;
        }

        public string Render(string val)
        {
            return _control.Render(val);
        }
    }
}