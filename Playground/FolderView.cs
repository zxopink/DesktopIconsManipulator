using DesktopIconsManipulator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Playground
{
    
    /// <summary>
    /// Singleton
    /// </summary>
    public class FolderView
    {
        private static IconsManipulator? Wrapper = null;
        private static FolderView? _instance = null;
        public static FolderView Instance { get { if (_instance == null) _instance = new(); return _instance; } }
        private FolderView()
        {
            Wrapper = IconsManipulator.Instance;
        }

        public async void LoopIt(string? fileName)
        {
            string deskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var files = Directory.GetFiles(deskPath);
            var rndFile = files[Random.Shared.Next(0, files.Length)];
            var file = fileName ?? Path.GetFileName(rndFile);
            await Task.Yield();
            Stopwatch s = Stopwatch.StartNew();
            while (true)
            {
                int offset = 500;
                int x = (int)(Math.Sin(s.Elapsed.TotalSeconds * Math.PI) * 250);
                //bool gotIt = Wrapper.SetItemPosition(file, new(x + offset, 150));
            }
        }

        private bool _disposed = false;
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            Wrapper?.Dispose();
        }

        ~FolderView()
        {
            Dispose();
        }
    }
}
