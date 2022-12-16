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
        public Point GetItemPosition(string fname)
        {
            return GetItemPosition(_FolderH, _ShellH, fname);
        }

        public bool SetItemPosition(string fname, Point pt)
        {
            return SetItemPosition(_FolderH, _ShellH, fname, pt);
        }

        public Point GetItemPosition(int index)
        {
            return GetItemPositionById(_FolderH, _ShellH, index);
        }

        public bool SetItemPosition(int index, Point pt)
        {
            return SetItemPositionById(_FolderH, _ShellH, index, pt);
        }
    }
}
