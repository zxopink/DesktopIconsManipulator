using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace DesktopIconsManipulator
{
    public class IconItem : IEquatable<IconItem>
    {
        internal IconsManipulator Manager { get; private set; }

        /// <summary>Does the icon still exist (According to last refresh)</summary>
        public bool Exists => Manager.Icons.Contains(this);

        public string Name { get; internal set; }
        public int ID { get; internal set; }

        /// <summary>'string.Empty' if the icon is unknown</summary>
        public string FullPath { get; }
        public IconType IconType { get; }

        internal bool _locChanged;
        internal Point _location;
        /// <summary>Set location for next Apply call or get the location according to the last Apply call</summary>
        public Point Location
        {
            get => _location;
            set
            {
                _location = value;
                _locChanged = true;
            }
        }

        public Rectangle Rect => GetRectangle();

        public Size Size
        {
            get 
            {
                int size = Manager.IconsSize;
                return new Size(size, size);
            }
        }

        /// <summary>Center point of the icon</summary>
        public Point Center
        {
            get
            {
                Size size = Size;
                Size halfSize = new Size(size.Width / 2, size.Height / 2);
                return Location + halfSize;
            }
            set
            {
                Size size = Size;
                Size halfSize = new Size(size.Width / 2, size.Height / 2);
                Location = value - halfSize;
            }
        }

        public IconItem(IconsManipulator instance, string name, int id, string fullName, IconType type)
        {
            FullPath = fullName;
            IconType = type;
            Name = name;
            ID = id;
            Manager = instance;
            Location = GetItemPosition();
            _locChanged = false;
        }

        private Rectangle GetRectangle()
        {
            int size = Manager.IconsSize;
            return new Rectangle(Location, new Size(size, size));
        }

        public override string ToString()
        {
            return $"{nameof(FullPath)}:{FullPath}, {nameof(ID)}:{ID}";
        }

        /// <summary>
        /// Immediately set the icon's position, can get slow if moving more than one icon at a time.
        /// Use the `Location` property and `IconsManipulator` instead
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool SetItemPosition(Point point)
        {
            return Manager.SetItemPosition(ID, point);
        }

        /// <summary>
        /// Immediately get the icon's position.
        /// Use the `Location` property instead
        /// </summary>
        public Point GetItemPosition()
        {
            return Manager.GetItemPosition(ID);
        }

        public override bool Equals(object obj) =>
            obj is IconItem icon ? icon.Equals(this) : false;

        public bool Equals(IconItem other)
        {
            if (other is null)
                return false;

            return FullPath == other.FullPath;
        }

        public static bool operator ==(IconItem left, IconItem right) {
            if (left is null)
                return right is null;

            return left.Equals(right);
        }
        public static bool operator !=(IconItem left, IconItem right) => !(left == right);
    }
}
