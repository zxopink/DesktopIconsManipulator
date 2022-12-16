using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopIconsManipulator
{
    public partial class FolderMethods
    {
        /// <summary>Path to the desktop's folder</summary>
        public string DesktopFolder => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        /// <summary>Array of paths to desktop files</summary>
        public string[] DesktopFiles => Directory.GetFiles(DesktopFolder);

        public int GetIdByName(string itemName) =>
            ItemsIds.First(pair => pair.Value == itemName).Key;

        /// <summary>Dictionary with desktop items and their corresponding id</summary>
        public Dictionary<int, string> ItemsIds { get; set; }

        private void RefreshItemsIds()
        {
            const int CPP_STR_LIMIT = 260;
            ItemsIds = new Dictionary<int, string>();
            StringBuilder strBuilder = new StringBuilder(CPP_STR_LIMIT /*Like Cpp's limit*/);
            int count = GetItemsCount(_FolderH);
            for (int i = 0; i < count; i++)
            {
                GetItemId(i, _FolderH, _ShellH, strBuilder);
                string fName = strBuilder.ToString();
                if(!string.IsNullOrWhiteSpace(fName))
                    ItemsIds.Add(i, fName);
                strBuilder.Clear();
            }
        }
    }
}
