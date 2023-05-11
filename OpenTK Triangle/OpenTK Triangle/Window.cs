//By Roham Harandi Fasih



using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTK_Triangle
{
    public class Window : GameWindow
    {
        //Manually write the shader for this program
        string vertexShaderSource = @"
                #version 400
                layout(location = 0) in vec3 aPosition;
                void main()
                {
                    gl_Position = vec4(aPosition, 1.0);
                }
            ";

        string fragmentShaderSource = @"#version 400
                out vec4 fragColor;
                void main()
                {
                    fragColor = vec4(1, 0.5, 0, 1.0);
                }
";



        //Creates a set of vertices for a triangle
        private readonly float[] vertices =
            { 0f , 0.5f, 0f,
              0.5f, -0.5f, 0f,
              -0.5f,-0.5f, 0f
            };
        //VBO and VAO are declared here for ease
        private int _vertexBufferObject;

        private int _vertexArrayObject;

        //Shader program details
        private int _shaderProgram;


        //Compiler our C code shaders from the string above 
        //Note: you can do this as an external file
        public void CompileShaders()
        {
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);
            _shaderProgram = GL.CreateProgram();
            GL.AttachShader(_shaderProgram, vertexShader);
            GL.AttachShader(_shaderProgram, fragmentShader);
            GL.LinkProgram(_shaderProgram);
            GL.DetachShader(_shaderProgram, vertexShader);
            GL.DetachShader(_shaderProgram, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        //override the OnLoad function of the GameWindow class
        protected override void OnLoad()
        {

            //Load the base functoins OnLoad() first
            base.OnLoad();

            CompileShaders();

            //Set background color
            GL.ClearColor(0.5f, 0.4f, 0.8f, 1.0f);

            //Generate our VBO
            _vertexBufferObject = GL.GenBuffer();

            //Bind our VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            //Upload the data to the GPU buffer
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);   //Note: StaticDraw means that our buffer will remain static


            //Generate the VAO
            _vertexArrayObject = GL.GenVertexArray();

            //Bind it
            GL.BindVertexArray(_vertexArrayObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);





        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.UseProgram(_shaderProgram);
            GL.BindVertexArray(_vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            SwapBuffers();
            Console.WriteLine("Render Time: {0}", this.RenderTime);
        }


        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }


        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(Keys.Escape))
                Close();

           
        }

        public Window(GameWindowSettings gameSettings, NativeWindowSettings nativeSettings) : base(gameSettings, nativeSettings) { }

    }
}
