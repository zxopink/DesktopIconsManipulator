using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CsDesktop
{
    /// <summary>Singleton, use `FolderMethods.Instance`</summary>
    public unsafe sealed partial class FolderMethods : IDisposable
    {
        //private (IntPtr _MainH, IntPtr _FolderH, IntPtr _ShellH) ptrs { get; set; }
        private IntPtr _MainCOM { get; set; }
        private IntPtr _FolderCOMPtr { get; set; }
        private IntPtr _ShellCOMPtr { get; set; }
        private IntPtr _FolderH { get; set; }
        private IntPtr _ShellH { get; set; }

        private static FolderMethods _instance;
        public static FolderMethods Instance { get { if (_instance == null) _instance = new FolderMethods(); return _instance; } }

        private FolderMethods()
        {
            Init();
            Refresh();
        }

        /// <summary>Refresh catches data to keep up with new item changes</summary>
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
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            Release(_MainCOM, _FolderCOMPtr, _ShellCOMPtr);
        }

        ~FolderMethods()
        {
            Dispose();
        }

        public void TestMeth()
        {
            var ptr = Marshal.AllocHGlobal(4);
            FillPtr(ptr);
            int num = Marshal.PtrToStructure<int>(ptr);
            Console.WriteLine("Num is: " + num);
        }
    }
}
