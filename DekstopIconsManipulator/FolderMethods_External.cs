using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DesktopIconsManipulator
{
    public partial class FolderMethods
    {
        const string DLL_NAME = "DekstopIcons.dll"; /*dynamic library's name*/
        [DllImport(DLL_NAME)]
        private static extern void FillPtr(IntPtr ptr);
        [DllImport(DLL_NAME)]
        private static extern IntPtr/* (void**) */ InitDesktop();
        [DllImport(DLL_NAME)]
        private static extern void Release(IntPtr init, IntPtr folderCOMPtr, IntPtr shellCOMPtr);
        [DllImport(DLL_NAME)]
        private static extern int Free(IntPtr ptr);

        [DllImport(DLL_NAME)]
        private static extern Point GetItemPosition(IntPtr folderView, IntPtr shellFolder, string fname);
        [DllImport(DLL_NAME)]
        private static extern Point GetItemPositionById(IntPtr folderView, IntPtr shellFolder, int index);
        
        [DllImport(DLL_NAME)]
        private static extern bool SetItemPosition(IntPtr folderView, IntPtr shellFolder, string fname, Point pt);
        [DllImport(DLL_NAME)]
        private static extern bool SetItemPositionById(IntPtr folderView, IntPtr shellFolder, int index, Point pt);

        [DllImport(DLL_NAME)]
        private static extern int GetItemsCount(IntPtr folderPtr);
        [DllImport(DLL_NAME)]
        private static extern void GetItemId(int index, IntPtr folderPtr, IntPtr shellPtr, StringBuilder itemName);
    }
}
