using System.IO;
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
        private ICommand _keyDownCommand;

        private ICommand _keyUpCommand;

        private string _test;

        private Window _window;

        private Chapter chapter;
        private int currentPage;
        public ImageViewerVM Image { get; set; }
        public ToolsMenuVM Tools { get; set; }
        private Chapter loadedChapter { get; set; }

        public ICommand KeyDownCommand
        {
            get => _keyDownCommand;
            set => SetProperty(ref _keyDownCommand, value);
        }

        public ICommand KeyUpCommand
        {
            get => _keyUpCommand;
            set => SetProperty(ref _keyUpCommand, value);
        }

        public ICommand MouseDownCommand { get; }

        public ICommand MouseUpCommand { get; }

        public object MouseMoveCommand { get; }

        public string Test
        {
            get => _test;
            set => SetProperty(ref _test, value);
        }

        public ICommand NewChapterCommand { get; }

        public ICommand SaveCommand { get; }

        public ICommand ExportCommand { get; }

        public ICommand ExitCommand { get; }

        public ICommand OpenCommand { get; }


        public MainWindowVM(Window window)
        {
            _window = window;
            Image = new ImageViewerVM();
            Tools = new ToolsMenuVM(Image);


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
            ExitCommand = new DelegateCommand(() => Application.Current.Shutdown());
            ExportCommand = new DelegateCommand(() =>
            {
                if (chapter == null || chapter.Pages.Count == 0)
                    return;
                var exportFileDialog = new SaveFileDialog
                {
                    AddExtension = true,
                    DefaultExt = "json",
                    Title = "Export",
                    OverwritePrompt = true,
                    Filter = "json files (*.json) | *.json"
                };
                if (exportFileDialog.ShowDialog() == true)
                    File.WriteAllText(exportFileDialog.FileName, chapter.ConvertToJSON());
            });
            OpenCommand = new DelegateCommand(() =>
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
                {
                    chapter = Chapter.Load(openFileDialog.FileName);
                    currentPage = 0;
                    Image.LoadPage(chapter.Pages.FirstOrDefault());
                }
            });
            SaveCommand = new DelegateCommand(() =>
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
                    chapter.Save(saveFileDialog.FileName);
            });
            NewChapterCommand = new DelegateCommand(() =>
            {
                var newChapterDialog = new NewChapterDialog();
                if (newChapterDialog.ShowDialog() != true)
                    return;
                chapter = new Chapter(newChapterDialog.cleanedPath, newChapterDialog.tlPath);
                currentPage = 0;
                Image.LoadPage(chapter.Pages.FirstOrDefault());
            });
        }

        private void NextPage()
        {
            if (chapter != null && currentPage + 1 < chapter.Pages.Count)
            {
                currentPage++;
                Image.LoadPage(chapter.Pages[currentPage]);
            }
        }

        private void PreviousPage()
        {
            if (chapter != null && currentPage > 0)
            {
                currentPage--;
                Image.LoadPage(chapter.Pages[currentPage]);
            }
        }
    }
}