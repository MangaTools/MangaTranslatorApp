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

        private static Window _mainWindow;

        public static void SetMainWindow(Window control)
        {
            _mainWindow = control;
        }

        public static void SetMousePressed(MouseButtonEventArgs args)
        {
            MousePressed?.Invoke(args);
            _mainWindow.CaptureMouse();
        }

        public static void SetMouseReleased(MouseButtonEventArgs args)
        {
            MouseReleased?.Invoke(args);
            _mainWindow.ReleaseMouseCapture();
        }

        public static void MoveMouse(MouseEventArgs args)
        {
            MousePosition = args.GetPosition(_mainWindow);
        }

        public static event Action<MouseButtonEventArgs> MousePressed;
        public static event Action<MouseButtonEventArgs> MouseReleased;
        public static event Action<Point> MouseMove;

        public static event Action<ControlType> MouseOverChanged;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(ref Win32Point pt);

        private static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Win32Point
        {
            public readonly int X;
            public readonly int Y;
        }
    }
}