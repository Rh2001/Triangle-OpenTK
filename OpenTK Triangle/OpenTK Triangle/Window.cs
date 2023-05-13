//By Roham Harandi Fasih



using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace OpenTK_Triangle
{
    public class Window : GameWindow
    {
        float red = 0.0f, green = 1.0f, blue = 1.0f;
        string shape;
        //Manually write the shader for this program
        string vertexShaderSource = @"
                #version 400
                layout(location = 0) in vec3 aPosition;
               
                
                out vec4 vertexColor;
                void main()
                {
                    gl_Position = vec4(aPosition, 1.0);
                    vertexColor = vec4(0.0, 0.0, 0.0, 0.0);
                }
            ";

        string fragmentShaderSource = @"#version 400
                
                out vec4 fragColor;
                uniform vec4 currentColor;

                    void main()
                        {
                            fragColor = currentColor;
                        }
                    ";





        //Creates a set of vertices for a triangle
        //private readonly float[] vertices =
        //    { 0f , 0.5f, 0f,
        //      0.5f, -0.5f, 0f,
        //      -0.5f,-0.5f, 0f
        //    };


        //private float[] vertices;

        private float[] vertices;

        uint[] indices = {  // note that we start from 0!  these are the indices that tells us we need to use two triangles to make a square
            0, 1, 2,   // first triangle
            2, 3, 0    // second triangle
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


        //gets the vertices for both a square and a triangle depending on user input, if the input is null it instead draws a perpendicular triangle using the square vertices
        private void GetVertices()
        {
            if (Program.shape.ToLower() != null)
                vertices = Program.sendVertices(Program.shape);
            else
                vertices = new float[] {
                    0f , 0.5f, 0f,
                    0.5f, -0.5f, 0f,
                   -0.5f,-0.5f, 0f};

        }

        

        //override the OnLoad function of the GameWindow class
        protected override void OnLoad()
        {

            // Load the base functions OnLoad() first
            base.OnLoad();

            CompileShaders();

            GetVertices();

            // Set background color
            GL.ClearColor(0.5f, 0.4f, 0.8f, 1.0f);

            // Generate our VBO
            _vertexBufferObject = GL.GenBuffer();

            // Bind our VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Generate the VAO
            _vertexArrayObject = GL.GenVertexArray();

            // Bind it
            GL.BindVertexArray(_vertexArrayObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

         


            if (Program.shape.ToLower() == "square")
            {
                int _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            }




        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {

          
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(_shaderProgram);
            float timeValue = (float)GLFW.GetTime() / 5.0f;
            
            //define RGB colors to change according to time
            float red = (MathF.Sin(timeValue * 2.0f) + 1.0f) / 2.0f;
            float green = (MathF.Sin(timeValue * 3.0f) + 1.0f) / 2.0f;
            float blue = (MathF.Sin(timeValue * 4.0f) + 1.0f) / 2.0f;

            //get the uniform color from our shader
            int vertexColor = GL.GetUniformLocation(_shaderProgram,"currentColor");
            GL.Uniform4(vertexColor, red, green, blue, 1.0f);
            GL.BindVertexArray(_vertexArrayObject);

            //which shape to draw
            if (Program.shape.ToLower() == "square")
                GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            else
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

        public Window(GameWindowSettings gameSettings, NativeWindowSettings nativeSettings) : base(gameSettings, nativeSettings) {  }

    }
}
