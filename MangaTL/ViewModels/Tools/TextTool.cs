using System.Collections.Generic;
using System.Windows.Input;
using MangaTL.Managers;

namespace MangaTL.ViewModels.Tools
{
    public class TextTool : ToolControlVM
    {
        private readonly ImageViewerVM _image;

        public TextTool(ImageViewerVM image) : base(Key.T, new List<Key>())
        {
            _image = image;
        }

        public override void DoAction()
        {
            InAction = true;
            Clicked();
        }

        public override void StopAction()
        {
            InAction = false;
        }

        private void Clicked()
        {
            _image.AddBubble();
        }
    }
}