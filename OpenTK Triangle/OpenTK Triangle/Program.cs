//By Roham Harandi Fasih


using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK_Triangle;
class Program
{
    public static void Main()
    {
       
        var nativeSettings = new NativeWindowSettings()
        {
            Title = "Triangle",
            Size = new Vector2i(800, 600)
        };
        
        using (var window = new Window(GameWindowSettings.Default, nativeSettings))
        {
            window.Run();
        }



    }
}