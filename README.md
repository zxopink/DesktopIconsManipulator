# Desktop Icons Manipulator
[![NuGet Package][NuGet]][NuGet-url]

A simple way to maniplulate desktop icons in C# for Windows

## Get Instance
```cs
IconsManipulator instance = IconsManipulator.Instance;
```

## Get Icon Location
```cs
Point pt = instance.GetItemPosition("Recycle Bin");
```

## Set Icon Location
```cs
Point newPoint = new Point(x: 50, y: 50);
instance.SetItemPosition("Recycle Bin", newPoint);
```

## Get/Set Icon Size
```cs
int size = instance.IconsSize;
instance.IconsSize = 64; //is medium, 128 is large and 32 is small
```

## A Fun Tween Animation
```cs
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
```

### Result:
![](https://github.com/zxopink/DesktopIconsManipulator/blob/main/DesktopIconsManipulator/iconmanipulator.gif)

Credit me if you use this library

[NuGet]: https://img.shields.io/nuget/v/DesktopIconsManipulator?color=blue
[NuGet-url]: https://www.nuget.org/packages/DesktopIconsManipulator
