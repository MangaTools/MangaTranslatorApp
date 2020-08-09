using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MangaTL.ViewModels;

namespace MangaTL.Managers
{
    public static class ToolManager
    {
        private static ToolControlVM activeTool;
        private static ActiveToolType type;

        private static ToolControlVM preferredFastTool;
        private static ToolControlVM preferredHotkeyTool;

        private static readonly HashSet<Key> PressedKeys = new HashSet<Key>();

        private static bool started;

        private static SortedList<List<Key>, ToolControlVM> fastKeys;

        private static SortedList<List<Key>, ToolControlVM> hotkeys;

        public static void AddTool(ToolControlVM vm, List<Key> shortcut, List<Key> fastKeysVm)
        {
            if (!started)
                return;

            if (fastKeysVm != null && fastKeysVm.Count != 0)
                fastKeys[fastKeysVm] = vm;
            if (shortcut != null && shortcut.Count != 0)
                hotkeys[shortcut]= vm;
        }

        public static void Start()
        {
            if (started)
                return;

            // comparer by descending
            var defComparer = Comparer<List<Key>>.Create((a, b) => b.Count.CompareTo(a.Count));

            fastKeys = new SortedList<List<Key>, ToolControlVM>(defComparer);
            hotkeys = new SortedList<List<Key>, ToolControlVM>(defComparer);

            started = true;
            KeyManager.KeyDown += KeyDown;
            KeyManager.KeyUp += KeyUp;
        }

        private static void RecalculateTools()
        {
            foreach (var toolControlVm in fastKeys)
            {
                var pressed = toolControlVm.Key.All(key => PressedKeys.Contains(key));
                if (!pressed)
                    continue;

                preferredFastTool = toolControlVm.Value;
                break;
            }

            foreach (var toolControlVm in hotkeys)
            {
                var pressed = toolControlVm.Key.All(key => PressedKeys.Contains(key));
                if (!pressed)
                    continue;

                preferredHotkeyTool = toolControlVm.Value;
                break;
            }

            ChooseTool();
        }

        private static void ChooseTool()
        {
            // If current tool is fast
            if (type == ActiveToolType.FastKey)
            {
                // If same tool is preferred
                if (activeTool == preferredFastTool)
                    return;
                // If new fast tool is more preferred
                if (preferredFastTool != null)
                {
                    ApplyTool(preferredFastTool, ActiveToolType.FastKey);
                    return;
                }

                // If no fast tool is preferred
                ApplyTool(preferredHotkeyTool, ActiveToolType.Hotkey);
            }
            // If current tool is Hotkey tool
            else
            {
                // If now we have fast tool preferred
                if (preferredFastTool != null)
                {
                    ApplyTool(preferredFastTool, ActiveToolType.FastKey);
                    return;
                }

                // If now we have new hotkey tool
                if (activeTool != preferredHotkeyTool && preferredHotkeyTool != null)
                    ApplyTool(preferredHotkeyTool, ActiveToolType.Hotkey);
            }
        }

        private static void ApplyTool(ToolControlVM vm, ActiveToolType newType)
        {
            if (activeTool != null)
                activeTool.Pressed = false;
            activeTool = vm;
            type = newType;

            activeTool.Pressed = true;
        }

        private static void KeyUp(Key key)
        {
            PressedKeys.Remove(key);
            RecalculateTools();
        }

        private static void KeyDown(Key key)
        {
            PressedKeys.Add(key);
            RecalculateTools();
        }

        private enum ActiveToolType
        {
            FastKey,
            Hotkey
        }
    }
}