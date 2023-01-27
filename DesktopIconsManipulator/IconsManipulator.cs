using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
///Made by Yoav Haik
///Big thanks to Raymond Chen and his developer blog post:
///https://devblogs.microsoft.com/oldnewthing/20130318-00/?p=4933

namespace DesktopIconsManipulator
{
    /// <summary>Singleton class to manipulate desktop icons</summary>
    public unsafe sealed partial class IconsManipulator : IDisposable
    {
        private IntPtr _MainCOM { get; set; }
        private IntPtr _FolderCOMPtr { get; set; }
        private IntPtr _ShellCOMPtr { get; set; }
        private IntPtr _FolderH { get; set; }
        private IntPtr _ShellH { get; set; }

        private static IconsManipulator _instance;
        public static IconsManipulator Instance { get { if (_instance == null) _instance = new IconsManipulator(); return _instance; } }

        public Rectangle ScreenSize { get; }

        private IconsManipulator()
        {
            Init();
            Refresh();
            ScreenSize = _GetScrenSize();
        }

        private Rectangle _GetScrenSize()
        {
            RECT rect = GetDesktopSize();
            return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }

        /// <summary>Refresh catched data to keep up with new item changes</summary>
        public void Refresh()
        {
            RefreshItemsIds();
        }

        private unsafe void Init()
        {
            void** ptrArr = (void**)InitDesktop().ToPointer();
            int size = sizeof(IntPtr);

            _MainCOM = new IntPtr(ptrArr[0]);
            _FolderH = new IntPtr(ptrArr[1]);
            _ShellH = new IntPtr(ptrArr[2]);
            _FolderCOMPtr = new IntPtr(ptrArr[3]);
            _ShellCOMPtr = new IntPtr(ptrArr[4]);

            Free(new IntPtr(ptrArr));
        }

        private bool _disposed = false;
        /// <summary>Dispose all COM Objects, should be used when Application closes</summary>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            Release(_MainCOM, _FolderCOMPtr, _ShellCOMPtr);
            _instance = null;
        }

        ~IconsManipulator()
        {
            Dispose();
        }
    }
}
