using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace DesktopIconsManipulator
{
    public class IconItem
    {
        internal IconsManipulator Manager { get; private set; }

        /// <summary>Does the icon still exist (According to last refresh)</summary>
        public bool Exists => Manager.Icons.Contains(this);

        internal readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public string FullPath => Path.Combine(DesktopPath, Name);
        public string Name { get; private set; }
        public int ID { get; private set; }
        public Point Location
        {
            get => Manager.GetItemPosition(ID);
            set => Manager.SetItemPosition(ID, value);
        }
        public Rectangle Rect => GetRectangle();
        public IconItem(IconsManipulator instance, string name, int id)
        {
            Name = name;
            ID = id;
            Manager = instance;
        }

        private Rectangle GetRectangle()
        {
            int size = Manager.IconsSize;
            return new Rectangle(Location, new Size(size, size));
        }
    }
}
