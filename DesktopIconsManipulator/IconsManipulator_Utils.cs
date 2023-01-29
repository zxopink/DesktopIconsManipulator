using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopIconsManipulator
{
    public partial class IconsManipulator
    {

        /// <summary>Dictionary with desktop items and their corresponding id</summary>
        private List<IconItem> _icons;
        public ReadOnlyCollection<IconItem> Icons => _icons.AsReadOnly();

        /// <summary>Automatically refresh when an ID mismatch is detected</summary>
        public bool AutoRefresh = false;

        /// <param name="name">The icons' name (file or folder name)</param>
        /// <returns>The icon or null if not found</returns>
        public IconItem GetIcon(string name)
        {
            return Icons.Where(icon => icon.Name == name).FirstOrDefault();
        }

        private void RefreshItemsIds()
        {
            const int CPP_STR_LIMIT = 260;
            var icons = new List<IconItem>();
            StringBuilder strBuilder = new StringBuilder(CPP_STR_LIMIT /*Like Cpp's limit*/);
            int count = GetItemsCount(_FolderH);

            string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            string[] dirs = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            
            List<string> filesAndDirs = new List<string>(files.Length + dirs.Length);
            filesAndDirs.AddRange(files);
            filesAndDirs.AddRange(dirs);
            
            for (int i = 0; i < count; i++)
            {
                GetItemId(i, _FolderH, _ShellH, strBuilder);
                string fName = strBuilder.ToString();

                (string fullName, IconType type) = GetIconInfo(fName, filesAndDirs);
                if (!string.IsNullOrEmpty(fullName))
                    filesAndDirs.Remove(fullName);

                IconItem icon = GetIcon(i, fName, fullName, type);
                if (icon != null)
                    icons.Add(icon);

                strBuilder.Clear();
            }
            _icons = icons;
        }

        private (string fullName, IconType type) GetIconInfo(string icoName, IList<string> filesAndDirs)
        {
            //We have 2 options to check:
            //1. File extension is enabled and the icon's name is the files' name
            //2. File extension is disabled and we have to look for the extension in files
            //Option 1
            
            string iconFullPath = GetFullPath(icoName);
            if (File.Exists(iconFullPath))
                return (iconFullPath, IconType.File);
            if (Directory.Exists(iconFullPath))
                return (iconFullPath, IconType.Folder);

            //Option 2
            string fullName = filesAndDirs.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f) == icoName);
            if (string.IsNullOrEmpty(fullName))
                return (string.Empty, IconType.Unknown); //Shortcut

            if (Directory.Exists(fullName)) //(Path.GetExtension(fullName) == string.Empty)
                return (fullName, IconType.Folder);
            return (fullName, IconType.File);
        }

        private string GetFullPath(string fileName) =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);


        private IconItem GetIcon(int index, string fName, string fullName, IconType type)
        {
            IconItem icon = null;
            if (!string.IsNullOrWhiteSpace(fName))
            {
                icon = new IconItem(this, fName, index, fullName, type);
                if (_icons != null)
                {
                    IconItem prevIcon = _icons.Find(ic => ic == icon);
                    if (prevIcon != null)
                    {
                        if (icon.Location != prevIcon.Location)
                            prevIcon.Location = icon.Location;

                        prevIcon.ID = index;
                        icon = prevIcon; //Replace new icon
                    }
                }
            }
            return icon;
        }

        /// <summary>
        /// Mismatched
        /// </summary>
        /// <returns>True if removed an item</returns>
        private bool SafeCheck()
        {
            int count = ItemsCount;
            bool found = false;
            for (int i = 0; i < _icons.Count; i++)
            {
                if (_icons[i].ID >= count)
                {
                    _icons.RemoveAt(i--);
                    found = true;
                }
            }

            if (found && AutoRefresh)
                Refresh();

            return found;
        }

        /// <summary>Apply location changes</summary>
        public void Apply()
        {
            SafeCheck();

            IconItem[] icons = Icons.Where(ico => ico._locChanged).ToArray();
            foreach (var ico in icons)
                ico._locChanged = false;

            Point[] points = new Point[icons.Length];
            for (int i = 0; i < points.Length; i++)
                points[i] = icons[i].Location;

            SetItemsPosition(icons, points);
        }

        /// <summary>Asynchronous Apply as fast as possible, no interval</summary>
        public async Task AutoApply()
        {
            await Task.Yield();
            while (!_disposed)
            {
                Apply();
            }
        }
        /// <summary>Asynchronous Apply as fast as possible, no interval</summary>
        public async Task AutoApply(CancellationToken token)
        {
            await Task.Yield();
            while (!_disposed && !token.IsCancellationRequested)
            {
                Apply();
            }
        }

        /// <summary>Asynchronous Apply by the interval</summary>
        public async Task AutoApply(TimeSpan interval)
        {
            while (!_disposed)
            {
                Apply();
                await Task.Delay(interval);
            }
        }

        /// <summary>Asynchronous Apply by the interval</summary>
        public async Task AutoApply(TimeSpan interval, CancellationToken token)
        {
            while (!_disposed && !token.IsCancellationRequested)
            {
                Apply();
                await Task.Delay(interval, token);
            }
        }

        /// <summary>Asynchronous Apply by the interval</summary>
        public Task AutoApply(int millisecondsInterval) =>
            AutoApply(TimeSpan.FromMilliseconds(millisecondsInterval));
    }
}
