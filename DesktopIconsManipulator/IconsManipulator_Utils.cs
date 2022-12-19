using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
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
            for (int i = 0; i < count; i++)
            {
                GetItemId(i, _FolderH, _ShellH, strBuilder);
                string fName = strBuilder.ToString();
                if (!string.IsNullOrWhiteSpace(fName))
                {
                    IconItem icon = new IconItem(this, fName, i);
                    if (_icons != null)
                    {
                        IconItem prevIcon = _icons.Find(ic => ic == icon);
                        if (prevIcon != null)
                        {
                            prevIcon.ID = i;
                            icon = prevIcon;
                        }
                    }    
                    icons.Add(icon);
                }
                strBuilder.Clear();
            }
            _icons = icons;
        }

        /// <summary>Apply location changes</summary>
        public void Apply()
        {
            IconItem[] icons = Icons.Where(ico => ico._locChanged && ico.PathExist).ToArray();
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
