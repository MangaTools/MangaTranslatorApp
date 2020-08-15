using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MangaTL.Managers
{
    public static class KeyManager
    {
        private static readonly HashSet<Key> keys = new HashSet<Key>();

        public static Window Window { get; private set; }

        public static HashSet<Key> Keys => new HashSet<Key>(keys);

        public static bool IsCatchingKeys { get; private set; } = true;


        public static void SetWindow(Window w)
        {
            Window = w;
        }

        public static void ClearFocus()
        {
            Window.Focus();
        }

        public static void KeyReleased(Key key)
        {
            if (!IsCatchingKeys)
                return;
            keys.Remove(key);
            KeyUp?.Invoke(key);
        }

        public static void KeyPressed(Key key)
        {
            if (!IsCatchingKeys)
                return;
            foreach (var k in keys.Where(k => !Keyboard.IsKeyDown(k)).ToList())
            {
                keys.Remove(k);
                KeyReleased(k);
            }

            keys.Add(key);
            KeyDown?.Invoke(key);
        }

        public static void StopCatchingKeys()
        {
            IsCatchingKeys = false;
        }

        public static void ResumeCatchingKeys()
        {
            IsCatchingKeys = true;
        }

        public static bool IsPressed(Key key)
        {
            return keys.Contains(key);
        }

        public static event Action<Key> KeyDown;
        public static event Action<Key> KeyUp;
    }
}