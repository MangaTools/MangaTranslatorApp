using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

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

        public event Action ChapterChanged;
    }
}