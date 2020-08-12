using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace MangaTL.Managers
{
    public static class KeyManager
    {
        private static readonly HashSet<Key> Keys = new HashSet<Key>();

        private static Window window;
        public static void SetWindow(Window w)
        {
            window = w;
        }

        public static void ClearFocus()
        {
            window.Focus();
        }

        public static void KeyReleased(Key key)
        {
            Keys.Remove(key);
            KeyUp?.Invoke(key);
        }

        public static void KeyPressed(Key key)
        {
            Keys.Add(key);
            KeyDown?.Invoke(key);
        }

        public static bool IsPressed(Key key)
        {
            return Keys.Contains(key);
        }

        public static event Action<Key> KeyDown;
        public static event Action<Key> KeyUp;
    }
}