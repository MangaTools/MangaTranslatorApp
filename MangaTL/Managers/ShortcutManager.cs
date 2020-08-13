using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace MangaTL.Managers
{
    public static class ShortcutManager
    {
        private static bool started;
        private static readonly List<(List<Key> keys, Action action)> shortcuts = new List<(List<Key>, Action)>();

        public static void Start()
        {
            if(started)
                return;
            started = true;
            KeyManager.KeyDown += key => CalculateShortcuts();
        }

        public static void AddShortcut(List<Key> keys, Action action)
        {
            shortcuts.Add((keys, action));
            shortcuts.Sort((a,b) => b.keys.Count.CompareTo(a.keys.Count));
        }

        private static void CalculateShortcuts()
        {
            var pressedKeys = KeyManager.Keys;

            foreach (var (shortcutKeys, action) in shortcuts)
            {
                var passed = shortcutKeys.All(key => pressedKeys.Contains(key));

                if (!passed)
                    continue;

                action.Invoke();
                break;
            }
        }
    }
}