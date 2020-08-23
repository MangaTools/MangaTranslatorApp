using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using MangaTL.Managers;

namespace MangaTL.ViewModels.Tools
{
    public class OrderTool : ToolControlVM
    {
        private readonly ImageViewerVM imageVm;

        public OrderTool(ImageViewerVM vm) : base(new List<Key> {Key.O}, new List<Key>())
        {
            imageVm = vm;
        }

        protected override void Activated()
        {
            imageVm.SetOrderText();
            imageVm.PageLoaded += SetText;
            base.Activated();
        }

        protected override void Deactivated()
        {
            imageVm.PageLoaded -= SetText;
            imageVm.SetContentText();
            base.Deactivated();
        }

        private void SetText()
        {
            imageVm.SetOrderText();
        }

        protected override void DoAction(MouseButton pressedButton)
        {
            if (pressedButton != MouseButton.Left && pressedButton != MouseButton.Right)
                return;
            base.DoAction(pressedButton);
            ChangeOrder(MouseManager.MousePosition, pressedButton != MouseButton.Left);
        }

        private void ChangeOrder(Point mousePosition, bool up)
        {
            var bubble = imageVm.GetBubbleFromPoint(mousePosition);
            if (bubble == null)
                return;
            imageVm.ChangeBubbleOrder(bubble, up);
        }
    }
}