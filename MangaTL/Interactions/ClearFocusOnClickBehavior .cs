using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using MangaTL.Managers;

namespace MangaTL.Interactions
{
    public class ClearFocusOnClickBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            AssociatedObject.MouseDown += AssociatedObject_MouseDown;
            base.OnAttached();
        }

        private static void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
            FocusManager.SetFocusedElement(KeyManager.Window, KeyManager.Window);
            KeyManager.ClearFocus();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
        }
    }
}