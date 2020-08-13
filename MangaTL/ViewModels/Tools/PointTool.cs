using System.Collections.Generic;
using System.Windows.Input;
using MangaTL.Managers;

namespace MangaTL.ViewModels.Tools
{
    public class PointTool : ToolControlVM
    {
        private readonly ImageViewerVM _imageVM;
        private readonly StyleControlVM _styleVM;
        private BubbleVM _selectedBubble;

        public PointTool(ImageViewerVM imageVM, StyleControlVM styleVM) : base(new List<Key> {Key.P},
                                                                               new List<Key> {Key.LeftCtrl})
        {
            _imageVM = imageVM;
            _styleVM = styleVM;
        }

        protected override void DoAction()
        {
            base.DoAction();
            Click();
        }

        private void Click()
        {
            var mousePos = MouseManager.MousePosition;
            _selectedBubble = _imageVM.GetBubbleFromPoint(mousePos);
            if (_selectedBubble == null)
                return;
            _styleVM.SetBubble(_selectedBubble);
        }

        protected override void Activated()
        {
            KeyManager.KeyDown += KeyPressed;
        }

        protected override void Deactivated()
        {
            KeyManager.KeyDown -= KeyPressed;
        }

        private void KeyPressed(Key key)
        {
            if (key == Key.Delete && _selectedBubble != null)
            {
                _imageVM.RemoveBubble(_selectedBubble);
                _selectedBubble = null;
                _styleVM.SetBubble(null);
            }
        }
    }
}