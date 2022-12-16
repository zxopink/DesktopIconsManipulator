﻿

using DesktopIconsManipulator;
using Playground;
using System.Diagnostics;
using System.Drawing;

//var inst = FolderView.Instance;

IconsManipulator instance = IconsManipulator.Instance;
var timer = Stopwatch.StartNew();


Point pt = instance.GetItemPosition("Recycle Bin");
int speed = 250;
int startX = 1000;
while (true)
{
    int offsetX = (int)(Math.Sin(timer.Elapsed.TotalSeconds * Math.PI) * speed);
    offsetX += startX;
    pt.X = offsetX;
    instance.SetItemPosition("Recycle Bin", pt);
}

//FolderView.Instance.LoopIt("Counter-Strike Source");
while (true) ;
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