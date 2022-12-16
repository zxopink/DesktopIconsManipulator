# Dekstop Icons Manipulator

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

## A Fun little tween animation
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

Result:
