using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DesktopIconsManipulator
{
    public partial class IconsManipulator
    {
        internal int ItemsCount =>
            GetItemsCount(_FolderH);

        internal Point GetItemPosition(FileInfo file) =>
            GetItemPosition(file.Name);
        /// <param name="fname">Item's name (excluding path)</param>
        /// <returns>The item's position relative to screen</returns>
        internal Point GetItemPosition(string fname)
        {
            return GetItemPosition(_FolderH, _ShellH, fname);
        }

        /// <param name="index">Item's id (excluding path)</param>
        /// <returns>The item's position relative to screen</returns>
        internal Point GetItemPosition(int index)
        {
            return GetItemPositionById(_FolderH, _ShellH, index);
        }

        internal bool SetItemPosition(FileInfo file, Point pt) =>
            SetItemPosition(file.Name, pt);

        /// <param name="fname">Item's name (excluding path)</param>
        /// <param name="pt">Point relative to screen</param>
        /// <returns>True if the Icon was found and changed, false otherwise</returns>
        internal bool SetItemPosition(string fname, Point pt)
        {
            return SetItemPosition(_FolderH, _ShellH, fname, pt);
        }

        /// <param name="index">Item's id</param>
        /// <param name="pt">Point relative to screen</param>
        /// <returns>True if the Icon was found and changed, false otherwise</returns>
        internal bool SetItemPosition(int index, Point pt)
        {
            return SetItemPositionById(_FolderH, _ShellH, index, pt);
        }


        public unsafe bool SetItemsPosition(IList<IconItem> icons, IList<Point> points)
        {
            if (icons.Count != points.Count)
                throw new ArgumentException($"{nameof(icons)} and {nameof(points)} must be same length");

            int[] indexes = icons.Select(ic => ic.ID).ToArray();
            Point[] pointsArr = points.ToArray();
            bool flag = false;
            fixed (Point* pointPtr = pointsArr)
            {
                fixed (int* indexesPtr = indexes)
                {
                    IntPtr indPtr = new IntPtr(indexesPtr);
                    IntPtr pPtr = new IntPtr(pointPtr);
                    flag = SetItemsPositionById(_FolderH, _ShellH, indPtr, pPtr, indexes.Length);
                }
            }
            return flag;
        }

        /// <summary>The icons' size</summary>
        public int IconsSize 
        {
            get => GetIconsSize(_FolderH);
            set => SetIconsSize(value);
        }

        /// <returns>The icons' size</returns>
        internal int GetIconsSize()
        {
            return GetIconsSize(_FolderH);
        }

        /// <param name="size">The new icon's size</param>
        /// <returns>True if the icons' size changed, false otherwise</returns>
        internal bool SetIconsSize(int size)
        {
            return SetIconsSize(_FolderH, size);
        }

        /// <summary>Item selected by User</summary>
        public IconItem SelectedItem { get
            {
                int id = GetSelectedIcon(_FolderH);
                return Icons.FirstOrDefault(icon => icon.ID == id);
            } }
    }
}
