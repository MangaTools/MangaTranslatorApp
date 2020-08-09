using System.Collections.Generic;
using System.Windows.Input;

namespace MangaTL.ViewModels.Tools
{
    public class TextTool : ToolControlVM
    {
        private readonly ImageViewerVM _image;

        public TextTool(ImageViewerVM image) : base(new List<Key> {Key.T}, new List<Key>())
        {
            _image = image;
        }

        protected override void DoAction()
        {
            base.DoAction();
            Clicked();
        }

        private void Clicked()
        {
            _image.AddBubble();
        }
    }
}