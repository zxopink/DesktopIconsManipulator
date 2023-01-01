

using DesktopIconsManipulator;
using Playground;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;


var val = Math.Cos(Math.PI + Math.pi);
//var inst = FolderView.Instance;
IconsManipulator instance = IconsManipulator.Instance;
ReadOnlyCollection<IconItem> icons = instance.Icons;
IconItem binIcon = instance.GetIcon("Recycle Bin");
var timer = Stopwatch.StartNew();


IconItem sel = instance.SelectedItem;
Console.WriteLine(sel);
Console.ReadLine();

instance.AutoApply();
while (true)
{
    foreach (var icon in icons)
    {
        Point pt = icon.Location;
        int speed = 250;
        int startX = 1000;


        int offsetX = (int)(Math.Sin(timer.Elapsed.TotalSeconds * Math.PI) * speed);
        offsetX += startX;
        pt.X = offsetX;
        icon.Location = pt;
    }
}


while (true) ;

//Point pt = icon.Location;
//int speed = 250;
//int startX = 1000;
//while (true)
//{
//    int offsetX = (int)(Math.Sin(timer.Elapsed.TotalSeconds * Math.PI) * speed);
//    offsetX += startX;
//    pt.X = offsetX;
//    icon.Location = pt;
//}

//FolderView.Instance.LoopIt("Counter-Strike Source");
//int count = 0;
//foreach (var pair in inst.ItemsIds)
//{
//    count++;
//    if (count == 5)
//        break;
//    Task.Run(() =>
//    {
//        while (true)
//        {
//            int offsetX = (int)(Math.Sin(timer.Elapsed.TotalSeconds * Math.PI) * 20);
//            int id = pair.Key;

//            Point p = inst.GetItemPosition(id);
//            p.X += offsetX;
//            inst.SetItemPosition(id, p);
//        }
//    });
//}

//while (true) ;

//while (true)
//{
//    int offsetX = (int)(Math.Sin(timer.Elapsed.TotalSeconds * Math.PI) * 20);
//    foreach (var pair in inst.ItemsIds)
//    {
//        Task.Run(() =>
//        {
//            int id = pair.Key;
//            Point p = inst.GetItemPosition(id);
//            p.X += offsetX;
//            inst.SetItemPosition(id, p);
//        });
//    }

//}