using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace MangaTL.Managers
{
    public static class ShortcutManager
    {
        private static bool started;
        private static Dictionary<List<Key>, Action> shortcuts = new Dictionary<List<Key>, Action>();

        public static void Start()
        {
            if(started)
                return;
            started = true;
            KeyManager.KeyDown += key => CalculateShortcuts();
        }

        public static void AddShortcut(List<Key> keys, Action action)
        {
            shortcuts.Add(keys, action);
        }

        private static void CalculateShortcuts()
        {
            var keys = KeyManager.Keys;

            foreach (var shortcut in shortcuts)
            {
                var passed = true;
                foreach (var key in shortcut.Key)
                {
                    if (!keys.Contains(key))
                    {
                        passed = false;
                        break;
                    }
                }

                if (!passed)
                    continue;

                shortcut.Value.Invoke();
                break;
            }
        }
    }
}