using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MangaTL.Managers;

namespace MangaTL.ViewModels.Tools
{
    public class PointTool : ToolControlVM
    {
        private readonly ImageViewerVM imageVm;
        private readonly StyleControlVM styleVm;
        private BubbleVM selectedBubble;

        public PointTool(ImageViewerVM imageVM, StyleControlVM styleVM) : base(new List<Key> {Key.P},
                                                                               new List<Key> {Key.LeftCtrl})
        {
            imageVm = imageVM;
            styleVm = styleVM;
            ToolTip = "Select bubble(P)";
            ImageSource =
                new BitmapImage(new Uri("pack://application:,,,/MangaTL.Core;component/Resources/PointIcon.png"));
        }

        protected override void DoAction()
        {
            base.DoAction();
            Click();
        }

        private void Click()
        {
            var mousePos = MouseManager.MousePosition;
            selectedBubble?.Deselect();
            selectedBubble = imageVm.GetBubbleFromPoint(mousePos);
            if (selectedBubble == null)
            {
                styleVm.Clear();
                return;
            }
            selectedBubble.Select();

            styleVm.SetBubble(selectedBubble);
        }

        protected override void Activated()
        {
            imageVm.Cursor = "Cross";

            KeyManager.KeyDown += KeyPressed;
        }

        protected override void Deactivated()
        {
            KeyManager.KeyDown -= KeyPressed;
            imageVm.Cursor = "Arrow";
        }

        private void KeyPressed(Key key)
        {
            if (selectedBubble == null)
                return;
            var data = KeyManager.Keys;

            if (key == Key.Delete)
            {
                imageVm.RemoveBubble(selectedBubble);
                selectedBubble = null;
                styleVm.SetBubble(null);
            }
            else if (data.Contains(Key.LeftCtrl) && data.Contains(Key.C))
            {
                Clipboard.SetText(selectedBubble.Text);
            }
        }
    }
}