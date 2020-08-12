using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using MangaTL.Managers;

namespace MangaTL.Interactions
{
    public class TextBoxBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            if (AssociatedObject == null)
                return;
            base.OnAttached();
            AssociatedObject.KeyDown += AssociatedObject_KeyDown;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject == null)
                return;
            AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
            base.OnDetaching();
        }

        private static void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox box))
                return;
            if (e.Key == Key.Enter)
            {
                // Kill logical focus
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(box), null);
// Kill keyboard focus
                Keyboard.ClearFocus();
                KeyManager.ClearFocus();
            }
        }
    }
}