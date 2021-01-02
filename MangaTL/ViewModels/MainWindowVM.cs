using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows;
using System.Windows.Input;

using MangaTL.Controls;
using MangaTL.Core;
using MangaTL.Managers;

using Microsoft.Win32;

using Prism.Commands;
using Prism.Mvvm;

namespace MangaTL.ViewModels
{
    public class MainWindowVM : BindableBase
    {
        private readonly Window window;
        private bool canUndo;
        private Chapter chapter;
        private string currentChapterSavePath;
        private int currentPage;
        private bool isChapterDirty;
        private ICommand keyDownCommand;

        private ICommand keyUpCommand;

        private int pages;

        private string title;
        public ImageViewerVM Image { get; set; }
        public ToolsMenuVM Tools { get; set; }

        public ICommand KeyDownCommand
        {
            get => keyDownCommand;
            set => SetProperty(ref keyDownCommand, value);
        }

        public ICommand KeyUpCommand
        {
            get => keyUpCommand;
            set => SetProperty(ref keyUpCommand, value);
        }

        public ICommand MouseDownCommand { get; }

        public ICommand MouseUpCommand { get; }

        public object MouseMoveCommand { get; }

        public ICommand NewChapterCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand ExitCommand { get; }

        public ICommand OpenCommand { get; }

        public StyleControlVM Style { get; set; }

        public ICommand SaveAsCommand { get; }

        public ICommand UndoCommand { get; }

        public bool CanUndo
        {
            get => canUndo;
            private set => SetProperty(ref canUndo, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public int CurrentPage
        {
            get => currentPage;
            set
            {
                if (chapter == null || value <= 0 || value > Pages)
                    return;
                SetProperty(ref currentPage, value);

                Image.LoadPage(chapter.Pages[value - 1]);
            }
        }

        public int Pages
        {
            get => pages;
            set => SetProperty(ref pages, value);
        }

        public ICommand HelpScreenCommand { get; }

        public ICommand AboutCommand { get; }

        public ICommand ImportCommand { get; }

        public ICommand CalculateCommand { get; }


        public MainWindowVM(Window window)
        {
            this.window = window;
            Title = "MangaTL";

            Image = new ImageViewerVM();
            Style = new StyleControlVM();
            Tools = new ToolsMenuVM(Image, Style);

            KeyManager.SetWindow(window);
            ShortcutManager.Start();


            KeyUpCommand = new DelegateCommand<KeyEventArgs>(x => KeyManager.KeyReleased(x.Key));
            KeyDownCommand = new DelegateCommand<KeyEventArgs>(x => KeyManager.KeyPressed(x.Key));
            MouseDownCommand = new DelegateCommand<MouseButtonEventArgs>(MouseManager.SetMousePressed);
            MouseUpCommand = new DelegateCommand<MouseButtonEventArgs>(MouseManager.SetMouseReleased);
            MouseMoveCommand = new DelegateCommand<MouseEventArgs>(MouseManager.MoveMouse);
            KeyManager.KeyDown += x =>
            {
                switch (x)
                {
                    case Key.Left:
                        PreviousPage();
                        break;
                    case Key.Right:
                        NextPage();
                        break;
                }
            };

            UndoManager.CountChanged += val =>
            {
                CanUndo = UndoManager.CanUndo;
            };


            ExitCommand = new DelegateCommand(Exit);
            OpenCommand = new DelegateCommand(OpenDialog);
            SaveAsCommand = new DelegateCommand(() => SaveAsDialog());
            SaveCommand = new DelegateCommand(() => SaveDialog());
            NewChapterCommand = new DelegateCommand(NewChapterDialog);
            UndoCommand = new DelegateCommand(UndoManager.Undo);
            AboutCommand = new DelegateCommand(ShowAboutWindow);
            HelpScreenCommand = new DelegateCommand(ShowHelpWindow);
            ImportCommand = new DelegateCommand(ImportDialog);
            CalculateCommand = new DelegateCommand(CalculateDialog);

            ShortcutManager.AddShortcut(new List<Key> { Key.LeftCtrl, Key.S }, () => SaveDialog());
            ShortcutManager.AddShortcut(new List<Key> { Key.LeftCtrl, Key.LeftShift, Key.S }, () => SaveAsDialog());
            ShortcutManager.AddShortcut(new List<Key> { Key.LeftCtrl, Key.O }, OpenDialog);
            ShortcutManager.AddShortcut(new List<Key> { Key.LeftCtrl, Key.Q }, Exit);
            ShortcutManager.AddShortcut(new List<Key> { Key.LeftCtrl, Key.N }, NewChapterDialog);
            ShortcutManager.AddShortcut(new List<Key> { Key.LeftCtrl, Key.Z }, UndoManager.Undo);
        }

        private void CalculateDialog()
        {
            var text = $"Bubbles: {chapter.Pages.Sum(x => x.Bubbles.Count)} " +
                        $"Words: {chapter.Pages.Sum(x => x.Bubbles.Sum(y => y.TextContent.Split(' ').Count()))} " +
                        $"Characters: {chapter.Pages.Sum(x => x.Bubbles.Sum(y => y.TextContent.Length))}";
            MessageBox.Show(text);
        }

        private void ImportDialog()
        {
            var importFileDialog = new OpenFileDialog
            {
                DefaultExt = "txt",
                Title = "Import",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                Filter = "TXT files (*.txt) | *.txt"
            };
            if (importFileDialog.ShowDialog() == true)
            {
                chapter.ImportText(importFileDialog.FileName);
                UpdatePage();
            }
        }

        private void ShowHelpWindow()
        {
            var screen = new HelpScreen();
            screen.ShowDialog();
        }

        private void ShowAboutWindow()
        {
            var screen = new AboutScreen();
            screen.ShowDialog();
        }

        private void Exit()
        {
            window.Close();
        }

        public void OnExitWindow(CancelEventArgs args)
        {
            if (!isChapterDirty)
                return;

            var result = MessageBox.Show("Save changes in chapter before exiting?", "MangaTL",
                                         MessageBoxButton.YesNoCancel,
                                         MessageBoxImage.Exclamation, MessageBoxResult.Cancel);
            switch (result)
            {
                case MessageBoxResult.Cancel:
                    args.Cancel = true;
                    break;
                case MessageBoxResult.Yes:
                    if (!SaveDialog())
                        args.Cancel = true;
                    break;
            }
        }

        private bool SaveDialog()
        {
            if (string.IsNullOrWhiteSpace(currentChapterSavePath))
                return SaveAsDialog();

            SaveTlmFile(currentChapterSavePath);
            return true;
        }

        private void NewChapterDialog()
        {
            var newChapterDialog = new NewChapterDialog();
            if (newChapterDialog.ShowDialog() != true)
                return;
            currentChapterSavePath = null;
            Title = "New File";
            UnsubscribeChapter();

            chapter = new Chapter(newChapterDialog.tlPath);

            chapter.ChapterChanged += OnChapterChanged;
            isChapterDirty = true;
            Pages = chapter.Pages.Count;
            CurrentPage = 1;

            UndoManager.ClearManager();
        }

        private void OpenDialog()
        {
            var openFileDialog = new OpenFileDialog
            {
                DefaultExt = "tlm",
                Title = "Open",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                Filter = "TLM files (*.tlm) | *.tlm"
            };
            if (openFileDialog.ShowDialog() == true)
                OpenFile(openFileDialog.FileName);
        }

        private bool SaveAsDialog()
        {
            if (chapter == null || chapter.Pages.Count == 0)
                return false;
            var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = "tlm",
                Title = "Save",
                OverwritePrompt = true,
                Filter = "TLM files (*.tlm) | *.tlm|txt files(*.txt)| *.txt"
            };

            if (saveFileDialog.ShowDialog() != true)
                return false;

            switch (Path.GetExtension(saveFileDialog.FileName).ToLower())
            {
                case ".tlm":
                    SaveTlmFile(saveFileDialog.FileName);
                    break;
                case ".txt":
                    chapter.ExportText(saveFileDialog.FileName);
                    break;
                default:
                    throw new ArgumentException();
            }

            return true;
        }

        private void SaveTlmFile(string path)
        {
            currentChapterSavePath = path;
            Title = Path.GetFileNameWithoutExtension(path);
            isChapterDirty = false;
            chapter.Save(path);
        }

        public void OpenFile(string path)
        {
            Title = Path.GetFileNameWithoutExtension(path);
            UnsubscribeChapter();

            chapter = Chapter.Load(path);

            isChapterDirty = false;
            chapter.ChapterChanged += OnChapterChanged;
            currentChapterSavePath = path;
            Pages = chapter.Pages.Count;
            CurrentPage = 1;

            UndoManager.ClearManager();
        }

        private void UnsubscribeChapter()
        {
            if (chapter != null)
                chapter.ChapterChanged -= OnChapterChanged;
        }

        private void OnChapterChanged()
        {
            isChapterDirty = true;
            if (!Title.EndsWith("*"))
                Title += "*";
        }

        private void NextPage()
        {
            CurrentPage++;
        }

        private void PreviousPage()
        {
            CurrentPage--;
        }

        private void UpdatePage()
        {
            CurrentPage = currentPage;
        }
    }
}