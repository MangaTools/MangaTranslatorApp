using System.Collections.Generic;
using System.Windows.Input;
using MangaTL.Managers;

namespace MangaTL.ViewModels.Tools
{
    public class PointTool : ToolControlVM
    {
        private readonly ImageViewerVM _imageVM;
        private readonly StyleControlVM _styleVM;

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
            var bubble = _imageVM.GetBubbleFromPoint(mousePos);
            if (bubble == null)
                return;
            _styleVM.SetBubble(bubble);
        }
    }
}