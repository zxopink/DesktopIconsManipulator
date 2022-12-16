using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CsDesktop
{
    public partial class FolderMethods
    {
        [DllImport("DekstopIcons.dll")]
        private static extern void FillPtr(IntPtr ptr);
        [DllImport("DekstopIcons.dll")]
        private static extern IntPtr/* (void**) */ InitDesktop();
        [DllImport("DekstopIcons.dll")]
        private static extern void Release(IntPtr init, IntPtr folderCOMPtr, IntPtr shellCOMPtr);
        [DllImport("DekstopIcons.dll")]
        private static extern int Free(IntPtr ptr);

        [DllImport("DekstopIcons.dll")]
        private static extern Point GetItemPosition(IntPtr folderView, IntPtr shellFolder, string fname);
        [DllImport("DekstopIcons.dll")]
        private static extern Point GetItemPositionById(IntPtr folderView, IntPtr shellFolder, int index);
        
        [DllImport("DekstopIcons.dll")]
        private static extern bool SetItemPosition(IntPtr folderView, IntPtr shellFolder, string fname, Point pt);
        [DllImport("DekstopIcons.dll")]
        private static extern bool SetItemPositionById(IntPtr folderView, IntPtr shellFolder, int index, Point pt);

        [DllImport("DekstopIcons.dll")]
        private static extern int GetItemsCount(IntPtr folderPtr);
        [DllImport("DekstopIcons.dll")]
        private static extern void GetItemId(int index, IntPtr folderPtr, IntPtr shellPtr, StringBuilder itemName);
    }
}
