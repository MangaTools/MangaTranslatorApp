using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MangaTL.ViewModels;

namespace MangaTL.Managers
{
    public static class ToolManager
    {
        private static ToolControlVM activeTool;
        private static ActiveToolType type = ActiveToolType.Hotkey;

        private static ToolControlVM preferredFastTool;
        private static ToolControlVM preferredHotkeyTool;

        private static readonly HashSet<Key> PressedKeys = new HashSet<Key>();

        private static bool started;

        private static List<(List<Key>, ToolControlVM)> fastKeys;

        private static List<(List<Key>, ToolControlVM)> hotkeys;

        private static Comparer<(List<Key>, ToolControlVM)> comp;

        public static void AddTool(ToolControlVM vm, List<Key> shortcut, List<Key> fastKeysVm)
        {
            if (!started)
                return;

            if (fastKeysVm != null && fastKeysVm.Count != 0)
                fastKeys.Add((fastKeysVm, vm));
            if (shortcut != null && shortcut.Count != 0)
                hotkeys.Add((shortcut, vm));

            fastKeys.Sort(comp);
            hotkeys.Sort(comp);
        }

        public static void SetTool(ToolControlVM tool)
        {
            preferredHotkeyTool = tool;
            ChooseTool();
        }

        public static void Start()
        {
            if (started)
                return;

            // comparer by descending
            comp =
                Comparer<(List<Key>, ToolControlVM)>.Create((a, b) => b.Item1.Count.CompareTo(a.Item1.Count));

            fastKeys = new List<(List<Key>, ToolControlVM)>();
            hotkeys = new List<(List<Key>, ToolControlVM)>();

            started = true;
            KeyManager.KeyDown += KeyDown;
            KeyManager.KeyUp += KeyUp;
        }

        private static void RecalculateTools()
        {
            var choose = false;
            foreach (var toolControlVm in fastKeys)
            {
                var pressed = toolControlVm.Item1.All(key => PressedKeys.Contains(key));
                if (!pressed)
                    continue;

                preferredFastTool = toolControlVm.Item2;
                choose = true;
                break;
            }

            if (!choose)
                preferredFastTool = null;

            foreach (var toolControlVm in hotkeys)
            {
                var pressed = toolControlVm.Item1.All(key => PressedKeys.Contains(key));
                if (!pressed)
                    continue;

                preferredHotkeyTool = toolControlVm.Item2;
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

            if (activeTool != null)
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