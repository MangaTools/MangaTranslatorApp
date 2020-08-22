using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MangaTL.Core
{
    [Serializable]
    public class Chapter
    {
        public List<Page> Pages { get; private set; }

        public Chapter(string[] tlImagePath)
        {
            Pages = tlImagePath.Select(x => new Page(x)).ToList();
            foreach (var page in Pages)
                page.PageChanged += OnPageChanged;
        }

        public void Save(string path)
        {
            var formatter = new BinaryFormatter();

            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, this);
            }
        }

        public void ExportText(string path)
        {
            var result = new StringBuilder();
            foreach (var page in Pages)
            {
                foreach (var pageBubble in page.Bubbles)
                {
                    result.Append(pageBubble.TextContent);
                    result.Append("\n");
                }

                result.Append("\n");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var stream = File.CreateText(path);
            stream.Write(result.ToString());
            stream.Flush();
            stream.Close();
        }

        public static Chapter Load(string path)
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                return (Chapter) formatter.Deserialize(fs);
            }
        }

        private void OnPageChanged()
        {
            ChapterChanged?.Invoke();
        }

        [field: NonSerialized] public event Action ChapterChanged;
    }
}