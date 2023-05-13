//By Roham Harandi Fasih


using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK_Triangle;
class Program
{
    public static string? shape;
    public float[] vertices;
    public static void Main()
    {

        Console.WriteLine("Enter the shape you would like to see: (Triangle or Rectangle)");
        shape = Console.ReadLine();

        


        var nativeSettings = new NativeWindowSettings()
        {
            Title = "Shape",
            Size = new Vector2i(800, 600)
        };

        using (var window = new Window(GameWindowSettings.Default, nativeSettings))
        {
            window.Run();
        }



    }

    public static float[] sendVertices(string shape) 
    {
        if (shape.ToLower() == "triangle")
        {
            return new float[] {
                    0f , 0.5f, 0f,
                    0.5f, -0.5f, 0f,
                   -0.5f,-0.5f, 0f};
        }
        else 
        {
            return new float[]{
                0.5f,  0.5f, 0.0f,  // top right
                0.5f, -0.5f, 0.0f,  // bottom right
               -0.5f, -0.5f, 0.0f,  // bottom left
               -0.5f,  0.5f, 0.0f   // top left
};
        }
        
    }
}