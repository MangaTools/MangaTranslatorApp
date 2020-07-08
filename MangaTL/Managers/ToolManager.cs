using MangaTL.ViewModels;

namespace MangaTL.Managers
{
    public static class ToolManager
    {

        //TODO : More stable working class
        private static ToolControlVM _currentTool;

        private static ToolControlVM _fastTool;

        public static ToolControlVM CurrentTool
        {
            get => _currentTool;
            set
            {
                if (_currentTool == value)
                    return;
                if (_currentTool != null)
                {
                    if (_currentTool.InAction)
                        _currentTool.StopAction();
                    _currentTool.Pressed = false;
                }

                _currentTool = value;
                _currentTool.Pressed = true;
            }
        }

        public static ToolControlVM FastTool
        {
            get => _fastTool;
            set
            {
                if (_fastTool == value)
                    return;
                if (_fastTool != null)
                {
                    if (_fastTool.InAction)
                        _fastTool.StopAction();
                    _fastTool.Pressed = false;
                }

                _fastTool = value;
                if (_fastTool == null)
                {
                    if (_currentTool != null)
                        _currentTool.Pressed = true;
                    return;
                }

                if (_fastTool != null)
                {
                    if (_currentTool != null)
                    {
                        if (_currentTool.InAction)
                            _currentTool.StopAction();
                        _currentTool.Pressed = false;
                    }
                }

                _fastTool.Pressed = true;
            }
        }
    }
}