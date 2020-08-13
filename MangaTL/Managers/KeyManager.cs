using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Input;

namespace MangaTL.Managers
{
    public static class KeyManager
    {
        private static readonly HashSet<Key> keys = new HashSet<Key>();

        public static HashSet<Key> Keys => new HashSet<Key>(keys);

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
            keys.Remove(key);
            KeyUp?.Invoke(key);
        }

        public static void KeyPressed(Key key)
        {
            keys.Add(key);
            KeyDown?.Invoke(key);
        }

        public static bool IsPressed(Key key)
        {
            return keys.Contains(key);
        }

        public static event Action<Key> KeyDown;
        public static event Action<Key> KeyUp;
    }
}