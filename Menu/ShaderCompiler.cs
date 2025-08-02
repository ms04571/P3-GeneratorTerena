using OpenTK.Graphics.OpenGL4;
using System.Text;


// 100% GPT
// Prevajanje shader-jev je nekaj kar ni vgrajeno v eno preprosto funkcijo v openGL
// čeprav je postopek za prevajanje praktično enak za vsak shader. 



namespace GeneratorTerena
{
    public static class ShaderCompiler
    {
        /// <summary>
        /// Vrne slovar, ki ima za ključe imena shaderjev in za vrednosti njihove handler-je.
        /// Handler-je dobi tako, da prevede shader-je iz imenika "Shaders"
        /// </summary>
        /// <param name="imenik"></param>
        /// <returns></returns>
        public static Dictionary<string, int> CompileShaders(string imenik)
        {
            var shaderProgrami = new Dictionary<string, int>();

            string[] vertImena = Directory.GetFiles(imenik, "*.vert");

            foreach (string vertIme in vertImena)
            {
                string ime = Path.GetFileNameWithoutExtension(vertIme);
                string fragIme = Path.Combine(imenik, ime + ".frag");

                if (!File.Exists(fragIme))
                {
                    Console.WriteLine($"Missing fragment shader for {ime}, skipping.");
                    continue;
                }

                string vertString = File.ReadAllText(vertIme, Encoding.UTF8);
                string fragString = File.ReadAllText(fragIme, Encoding.UTF8);

                int program = CompileProgram(vertString, fragString);
                if (program != -1)
                {
                    shaderProgrami[ime] = program;
                    Console.WriteLine($"Compiled shader program: {ime}");
                }
            }

            return shaderProgrami;
        }

        /// <summary>
        /// Vrne celo število, ki predstavlja "handler" za shader program (vertex in fragment)
        /// za vnešeno kodo v obliki niza.
        /// </summary>
        /// <param name="vertString"></param>
        /// <param name="fragString"></param>
        /// <returns></returns>
        private static int CompileProgram(string vertString, string fragString)
        {
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertString);
            GL.CompileShader(vertexShader);
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int vStatus);
            if (vStatus != (int)All.True) // če je prišlo do napake pri prevajanju
            {
                string info = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine("Vertex shader compilation failed!");
                Console.WriteLine("--- Source ---");
                Console.WriteLine(vertString);
                Console.WriteLine("--- Error Log ---");
                Console.WriteLine(info);
                return -1;
            }

            // isto samo za "fragment"
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragString);
            GL.CompileShader(fragmentShader);
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int fStatus);
            if (fStatus != (int)All.True)
            {
                string info = GL.GetShaderInfoLog(fragmentShader);
                Console.WriteLine("Fragment shader compilation failed!");
                Console.WriteLine("--- Source ---");
                Console.WriteLine(fragString);
                Console.WriteLine("--- Error Log ---");
                Console.WriteLine(info);
                return -1;
            }

            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int linkStatus);
            if (linkStatus != (int)All.True) // napaka pri povezovanju
            {
                string info = GL.GetProgramInfoLog(program);
                Console.WriteLine("Shader linking error:\n" + info);
                return -1;
            }

            // niso več potrebni
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
            return program;
        }
    }
}
