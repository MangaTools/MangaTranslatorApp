using System.Linq;
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
        private Chapter chapter;
        private string currentChapterSavePath;
        private int currentPage;
        private ICommand keyDownCommand;

        private ICommand keyUpCommand;

        private string test;

        private Window window;
        public ImageViewerVM Image { get; set; }
        public ToolsMenuVM Tools { get; set; }
        private Chapter LoadedChapter { get; set; }

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

        public string Test
        {
            get => test;
            set => SetProperty(ref test, value);
        }

        public ICommand NewChapterCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand ExitCommand { get; }

        public ICommand OpenCommand { get; }

        public StyleControlVM Style { get; set; }

        public ICommand SaveAsCommand { get; }


        public MainWindowVM(Window window)
        {
            this.window = window;
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
            MouseManager.MouseMove += x => Test = $"{MouseManager.MousePosition.X} {MouseManager.MousePosition.Y}";
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


            ExitCommand = new DelegateCommand(Application.Current.Shutdown);
            OpenCommand = new DelegateCommand(OpenFileDialog);
            SaveAsCommand = new DelegateCommand(SaveFileDialog);
            SaveCommand = new DelegateCommand(() =>
            {
                if (string.IsNullOrWhiteSpace(currentChapterSavePath))
                    SaveFileDialog();
                else
                    SaveFile(currentChapterSavePath);
            });
            NewChapterCommand = new DelegateCommand(NewChapterDialog);
        }

        private void NewChapterDialog()
        {
            var newChapterDialog = new NewChapterDialog();
            if (newChapterDialog.ShowDialog() != true)
                return;
            currentChapterSavePath = null;
            chapter = new Chapter(newChapterDialog.tlPath);
            currentPage = 0;
            Image.LoadPage(chapter.Pages.FirstOrDefault());
        }

        private void OpenFileDialog()
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

        private void SaveFileDialog()
        {
            if (chapter == null || chapter.Pages.Count == 0)
                return;
            var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = "tlm",
                Title = "Save",
                OverwritePrompt = true,
                Filter = "TLM files (*.tlm) | *.tlm"
            };
            if (saveFileDialog.ShowDialog() == true)
                SaveFile(saveFileDialog.FileName);
        }

        private void SaveFile(string path)
        {
            currentChapterSavePath = path;
            chapter.Save(path);
        }

        public void OpenFile(string path)
        {
            chapter = Chapter.Load(path);
            currentChapterSavePath = path;
            currentPage = 0;
            Image.LoadPage(chapter.Pages.FirstOrDefault());
        }

        private void NextPage()
        {
            if (chapter == null || currentPage + 1 >= chapter.Pages.Count)
                return;

            currentPage++;
            Image.LoadPage(chapter.Pages[currentPage]);
        }

        private void PreviousPage()
        {
            if (chapter == null || currentPage <= 0)
                return;
            currentPage--;
            Image.LoadPage(chapter.Pages[currentPage]);
        }
    }
}