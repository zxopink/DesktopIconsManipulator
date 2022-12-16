using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DesktopIconsManipulator
{
    public partial class IconsManipulator
    {
        public Point GetItemPosition(FileInfo file) =>
            GetItemPosition(file.Name);
        /// <param name="fname">Item's name (excluding path)</param>
        /// <returns>The item's position relative to screen</returns>
        public Point GetItemPosition(string fname)
        {
            return GetItemPosition(_FolderH, _ShellH, fname);
        }

        /// <param name="index">Item's id (excluding path)</param>
        /// <returns>The item's position relative to screen</returns>
        public Point GetItemPosition(int index)
        {
            return GetItemPositionById(_FolderH, _ShellH, index);
        }

        public bool SetItemPosition(FileInfo file, Point pt) =>
            SetItemPosition(file.Name, pt);

        /// <param name="fname">Item's name (excluding path)</param>
        /// <param name="pt">Point relative to screen</param>
        /// <returns>True if the Icon was found and changed, false otherwise</returns>
        public bool SetItemPosition(string fname, Point pt)
        {
            return SetItemPosition(_FolderH, _ShellH, fname, pt);
        }

        /// <param name="index">Item's id</param>
        /// <param name="pt">Point relative to screen</param>
        /// <returns>True if the Icon was found and changed, false otherwise</returns>
        public bool SetItemPosition(int index, Point pt)
        {
            return SetItemPositionById(_FolderH, _ShellH, index, pt);
        }
    }
}
