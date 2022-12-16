using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsDesktop
{
    public partial class FolderMethods
    {
        public string DesktopFolder => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public string[] DesktopFiles => Directory.GetFiles(DesktopFolder);

        public int GetIdByName(string itemName) =>
            ItemsIds.First(pair => pair.Value == itemName).Key;

        public Dictionary<int, string> ItemsIds { get; set; }

        private void RefreshItemsIds()
        {
            const int CPP_STR_LIMIT = 260;
            ItemsIds = new();
            StringBuilder strBuilder = new(CPP_STR_LIMIT /*Like Cpp's limit*/);
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
