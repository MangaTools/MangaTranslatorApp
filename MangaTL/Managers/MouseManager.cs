using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using MangaTL.Core;

namespace MangaTL.Managers
{
    public static class MouseManager
    {
        private static ControlType _currentMouseOver = ControlType.Null;

        private static Point _mousePosition;

        public static ControlType CurrentMouseOver
        {
            get => _currentMouseOver;
            set
            {
                if (CurrentMouseOver == value)
                    return;
                _currentMouseOver = value;
                MouseOverChanged?.Invoke(value);
            }
        }

        public static Point MousePosition
        {
            get => _mousePosition;
            private set
            {
                if (MousePosition == value)
                    return;
                var delta = new Point(value.X - _mousePosition.X, value.Y - _mousePosition.Y);
                _mousePosition = value;
                MouseMove?.Invoke(delta);
            }
        }

        private static IInputElement _frame;

        public static void SetMainWindow(IInputElement control)
        {
            _frame = control;
        }

        public static void SetMousePressed(MouseButtonEventArgs args)
        {
            MousePressed?.Invoke(args);
            _frame.CaptureMouse();
        }

        public static void SetMouseReleased(MouseButtonEventArgs args)
        {
            MouseReleased?.Invoke(args);
            _frame.ReleaseMouseCapture();
        }

        public static void MoveMouse(MouseEventArgs args)
        {
            MousePosition = args.GetPosition(_frame);
        }

        public static event Action<MouseButtonEventArgs> MousePressed;
        public static event Action<MouseButtonEventArgs> MouseReleased;
        public static event Action<Point> MouseMove;

        public static event Action<ControlType> MouseOverChanged;
    }
}