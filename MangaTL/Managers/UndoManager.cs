using System;
using MangaTL.Managers.Helpers;

namespace MangaTL.Managers
{
    public static class UndoManager
    {
        private const int UndoActions = 100;
        private static readonly DropoutStack<Action> ActionStack = new DropoutStack<Action>(UndoActions);

        public static bool CanUndo => ActionStack.Count > 0;

        public static void CountAction(Action undoAction)
        {
            ActionStack.Push(undoAction);
            if (ActionStack.Count != UndoActions)
                CountChanged?.Invoke(ActionStack.Count);
        }

        public static void Undo()
        {
            if (ActionStack.Count <= 0)
                return;

            var action = ActionStack.Pop();
            action.Invoke();
            CountChanged?.Invoke(ActionStack.Count);
        }

        public static void ClearManager()
        {
            ActionStack.Clear();
        }

        public static void RemoveLast(int count)
        {
            for (var i = 0; i < count; i++)
                ActionStack.Pop();
        }

        public static event Action<int> CountChanged;
    }
}