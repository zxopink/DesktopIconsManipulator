using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopIconsManipulator
{
    public partial class IconsManipulator
    {

        /// <summary>Dictionary with desktop items and their corresponding id</summary>
        private List<IconItem> icons { get; set; }
        public ReadOnlyCollection<IconItem> Icons => icons.AsReadOnly();

        /// <param name="name">The icons' name (file or folder name)</param>
        /// <returns>The icon or null if not found</returns>
        public IconItem GetIcon(string name)
        {
            return icons.Where(icon => icon.Name == name).FirstOrDefault();
        }

        private void RefreshItemsIds()
        {
            const int CPP_STR_LIMIT = 260;
            icons = new List<IconItem>();
            StringBuilder strBuilder = new StringBuilder(CPP_STR_LIMIT /*Like Cpp's limit*/);
            int count = GetItemsCount(_FolderH);
            for (int i = 0; i < count; i++)
            {
                GetItemId(i, _FolderH, _ShellH, strBuilder);
                string fName = strBuilder.ToString();
                if (!string.IsNullOrWhiteSpace(fName))
                {
                    IconItem icon = new IconItem(this, fName, i);
                    icons.Add(icon);
                }
                strBuilder.Clear();
            }
        }
    }
}
