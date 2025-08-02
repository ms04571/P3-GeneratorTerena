using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Drawing.Imaging;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;


namespace GeneratorTerena
{
    public class Okno : GameWindow
    {
        private Dictionary<string, int> shaderProgrami;
        private int vodaVAO;
        private Kamera kamera;
        private float hitrostKamerePocasi = 100f;
        private float hitrostKamereHitro = 200f;
        private float hitrostKamere;
        private Generator generator;
        private List<Blok> vidniBloki;
        private UpravljalnikBlokov upravljalnikBlokov;
        private float hitrostMiske;
        private Vector2 delitevBarve;

        // teksture
        private Dictionary<string, int> teksture;


        public Okno(Vector2i velikostOkna, Generator generator, Vector2 delitevBarve, float hitrostMiske, int vidnaRazadlja)
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            { 
                ClientSize = velikostOkna,
                Title = "Ogled terena"
            })
        {
            CenterWindow(ClientSize);
            CursorState = CursorState.Grabbed;            
            this.generator = generator;
            this.hitrostMiske = hitrostMiske;
            this.delitevBarve = delitevBarve;
            this.kamera = new Kamera(generator.MaxVisina * 0.9f);
            this.hitrostKamere = hitrostKamerePocasi;
            this.upravljalnikBlokov = new UpravljalnikBlokov(vidnaRazadlja, generator);
        }

        /// <summary>
        /// Se zgodi ko se premakne miška. Dobi razliko kotov iz parametra
        /// in jih prišteje k smernim kotom od kamere. Pazi tudi, da so koti 
        /// znotraj pravih intevalov: pitch = (-Pi/2, Pi/2), yaw = (-Pi, Pi).
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            Vector2 razlika = e.Delta;
            kamera.Yaw += razlika.X * hitrostMiske;
            kamera.Pitch -= razlika.Y * hitrostMiske;

            // meje
            if (kamera.Yaw > MathF.PI) kamera.Yaw -= MathF.Tau;
            if (kamera.Yaw < -MathF.PI) kamera.Yaw += MathF.Tau;

            kamera.Pitch = MathHelper.Clamp(
                kamera.Pitch,
                -(float)Math.PI * 0.5f + 0.001f,
                (float)Math.PI * 0.5f - 0.001f
            );
        }

        /// <summary>
        /// Posodobi trenutno stanje.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            // glavne smeri kamere
            kamera.PosodobiBazneVektorje();

            // preimkanje
            if (KeyboardState.IsKeyDown(Keys.LeftShift)) { hitrostKamere = hitrostKamereHitro; }
            if (KeyboardState.IsKeyReleased(Keys.LeftShift)) { hitrostKamere = hitrostKamerePocasi; }

            float pravaHitrost = hitrostKamere * (float)args.Time;

            // glavnih 6 smeri
            if (KeyboardState.IsKeyDown(Keys.W)) { kamera.Pozicija += kamera.Naprej * pravaHitrost; }
            if (KeyboardState.IsKeyDown(Keys.S)) { kamera.Pozicija -= kamera.Naprej * pravaHitrost; }
            if (KeyboardState.IsKeyDown(Keys.D)) { kamera.Pozicija += kamera.Desno * pravaHitrost; }
            if (KeyboardState.IsKeyDown(Keys.A)) { kamera.Pozicija -= kamera.Desno * pravaHitrost; }
            if (KeyboardState.IsKeyDown(Keys.Space)) { kamera.Pozicija += kamera.Gor * pravaHitrost; }
            if (KeyboardState.IsKeyDown(Keys.LeftControl)) { kamera.Pozicija -= kamera.Gor * pravaHitrost; }

            // zoom
            if (MouseState.IsButtonDown(MouseButton.Left)) { kamera.Fov = Math.Max(kamera.Fov - 0.01f, (float)(Math.PI * 0.125f)); }
            if (MouseState.IsButtonDown(MouseButton.Right)) { kamera.Fov = Math.Min(kamera.Fov + 0.01f, (float)(Math.PI * 0.5f)); }

            // zapri okno
            if (KeyboardState.IsKeyDown(Keys.Escape)){ Close(); }


            // generiranje in brisanje blokov glede na pozicijo kamere
            upravljalnikBlokov.PreveriPremikBlokov(kamera);

            // kateri bloki so vidni
            vidniBloki = upravljalnikBlokov.VidniBloki(kamera);


            base.OnUpdateFrame(args);
        }

        /// <summary>
        /// Nariše trenutno sliko.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // trenutno stanje kamere
            Matrix4 ogled = kamera.MatrikaOgleda;
            Matrix4 projekcija = kamera.ProjekcijskaMatrika;

            // teren
            GL.UseProgram(shaderProgrami["teren"]);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgrami["teren"], "uOgled"), false, ref ogled);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgrami["teren"], "uProjekcija"), false, ref projekcija);
            GL.Uniform3(GL.GetUniformLocation(shaderProgrami["teren"], "uSmerSvetlobe"), new Vector3(0f, 1f, 0f).Normalized());
            GL.Uniform2(GL.GetUniformLocation(shaderProgrami["teren"], "uIzbiraBarve"), delitevBarve * generator.MaxVisina);
            //GL.Uniform3(GL.GetUniformLocation(shaderProgrami["teren"], "viewPos"), kamera.Pozicija);

            // teksture
            PripraviTekstureZaRisanje();

            // voda
            GL.UseProgram(shaderProgrami["voda"]);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgrami["voda"], "uOgled"), false, ref ogled);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgrami["voda"], "uProjekcija"), false, ref projekcija);



            foreach (Blok blok in vidniBloki)
            {
                if (blok.JE_V_GPU)  // če je v pomnilniku se nariše
                    RisiBlok(blok);
                else
                {                                             
                    blok.NapolniPomnilnik();
                }
                    
            }

            foreach (Blok blok in vidniBloki)  // trenutno se za vsak blok nariše svoja voda (nujno po tem ko se narišejo bloki)
            {
                if (blok.JE_V_GPU)
                    RisiVoda(blok);
            }


            SwapBuffers();
            base.OnRenderFrame(args);
        }

        /// <summary>
        /// Ta funkcija se zažene takoj ko se izvede Okno.Run().
        /// Tu se pripravi vse kar je potrebno za program.
        /// </summary>
        protected override void OnLoad()
        {
            // okno je skrito dokler se ta funkcija ne konča (glej konec za IsVisible = true)
            IsVisible = false;

            // barva ozadja
            GL.ClearColor(Color.LightBlue);

            // pripravljanje vode
            vodaVAO = VodaVAO(generator.MaxVisina * delitevBarve[0] - 3f, generator.VelikostBlok);
            GL.Enable(EnableCap.Blend);  // pomembno da je voda prozorna
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // shader-ji
            shaderProgrami = ShaderCompiler.CompileShaders("shaders");

            // teksture
            teksture = new Dictionary<string, int>();
            NaloziVseTeksture();

            foreach (string tekstura in teksture.Keys)
            {
                Console.WriteLine("Naložena tekstura: " + tekstura);
            }

            Context.SwapInterval = 1; // vsync      
            // Stranski učinek je da se hitrost posodabljanja slik limitira na toliko kot je zmožen monitor



            GL.Enable(EnableCap.DepthTest);
            GL.FrontFace(FrontFaceDirection.Cw);
            base.OnLoad();


            IsVisible = true;
        }

        /// <summary>
        /// To se izvede ko se zapre okno. Trenutno se samo počisti pomnilnik od GPU-ja
        /// </summary>
        protected override void OnUnload()
        {
            upravljalnikBlokov.PocistiPomnilnik();
            base.OnUnload();
        }


        /// <summary>
        /// Na gpu pošlje vse podatke za 3D model vode. Trenutno sta to samo dva trikotnika (en kvadrat).
        /// </summary>
        /// <param name="visinaVode"></param>
        /// <param name="velikost"></param>
        /// <returns></returns>
        private static int VodaVAO(float visinaVode, float velikost)
        {
            Vector3[] vodaTocke = new Vector3[]
            {
                new Vector3(0f, visinaVode, 0f),
                new Vector3(velikost - 1f, visinaVode, 0f),
                new Vector3(velikost - 1f, visinaVode,  velikost - 1f),
                new Vector3(0f, visinaVode, velikost - 1f)
            };
            uint[] indeksi = new uint[] { 0, 1, 2, 2, 3, 0 }; // vrstni red točk

            int vao = GL.GenVertexArray();
            int vbo = GL.GenBuffer();
            int ebo = GL.GenBuffer();

            GL.BindVertexArray(vao);
             
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vodaTocke.Length * Vector3.SizeInBytes, vodaTocke, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indeksi.Length * sizeof(uint), indeksi, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);

            GL.BindVertexArray(0);

            return vao;
        }


        /// <summary>
        /// Pove GPU-ju da nariše blok
        /// </summary>
        /// <param name="blok"></param>
        private void RisiBlok(Blok blok)
        {
            // transliranje na pravo mesto v wolrd space, (VelikostBlok - 1) zato da se bloki stikajo
            Matrix4 model = Matrix4.CreateTranslation(blok.BlokX * (generator.VelikostBlok - 1), 0f, blok.BlokZ * (generator.VelikostBlok - 1));

            GL.UseProgram(shaderProgrami["teren"]);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgrami["teren"], "uModel"), false, ref model);
            GL.BindVertexArray(blok.VAO);
            int stTock = (generator.VelikostBlok - 1) * (generator.VelikostBlok - 1) * 6;
            GL.DrawElements(PrimitiveType.Triangles, stTock, DrawElementsType.UnsignedInt, 0);
        }

        /// <summary>
        /// Pove GPU-ju, da nariše vodo za ta blok
        /// </summary>
        /// <param name="blok"></param>
        private void RisiVoda(Blok blok)
        {
            Matrix4 model = Matrix4.CreateTranslation(blok.BlokX * (generator.VelikostBlok - 1), 0f, blok.BlokZ * (generator.VelikostBlok - 1));

            GL.UseProgram(shaderProgrami["voda"]);
            GL.UniformMatrix4(GL.GetUniformLocation(shaderProgrami["voda"], "uModel"), false, ref model);
            GL.BindVertexArray(vodaVAO);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            GL.DepthMask(true);
        }

        /// <summary>
        /// Vsako od tekstur iz imenika "teksture" pošlje v GPU. V slovar "teksture" pa shranjuje njihove
        /// "handlerje" (recimo nek kazalec) kot vrednosti in njihova imena kot ključe.
        /// </summary>
        private void NaloziVseTeksture()
        {
            string pot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Teksture");
            string[] datoteke = Directory.GetFiles(pot, "*.png");

            foreach (string datoteka in datoteke)
            {
                string imeDatoteke = Path.GetFileNameWithoutExtension(datoteka);

                int id = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, id);

                using (Bitmap bitmap = new Bitmap(datoteka))
                {

                    BitmapData podatki = bitmap.LockBits(
                        new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb
                    );

                    GL.TexImage2D(TextureTarget.Texture2D,
                        0,
                        PixelInternalFormat.Rgba,
                        podatki.Width,
                        podatki.Height,
                        0,
                        OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                        PixelType.UnsignedByte,
                        podatki.Scan0
                    );

                    bitmap.UnlockBits(podatki);
                }

                // Nastavitve teksture
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

                teksture[imeDatoteke] = id;
            }
        }



        /// <summary>
        /// Priprava tekstur pred vsakim klicom za risanje
        /// </summary>
        private void PripraviTekstureZaRisanje()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, teksture["pesek"]);
            GL.Uniform1(GL.GetUniformLocation(shaderProgrami["teren"], "uPesek"), 0);

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, teksture["trava"]);
            GL.Uniform1(GL.GetUniformLocation(shaderProgrami["teren"], "uTrava"), 1);

            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, teksture["kamen"]);
            GL.Uniform1(GL.GetUniformLocation(shaderProgrami["teren"], "uKamen"), 2);

            GL.ActiveTexture(TextureUnit.Texture3);
            GL.BindTexture(TextureTarget.Texture2D, teksture["sneg"]);
            GL.Uniform1(GL.GetUniformLocation(shaderProgrami["teren"], "uSneg"), 3);

            GL.ActiveTexture(TextureUnit.Texture4);
            GL.BindTexture(TextureTarget.Texture2D, teksture["kamenPesek"]);
            GL.Uniform1(GL.GetUniformLocation(shaderProgrami["teren"], "uKamenPesek"), 4);

            GL.ActiveTexture(TextureUnit.Texture5);
            GL.BindTexture(TextureTarget.Texture2D, teksture["kamenTrava"]);
            GL.Uniform1(GL.GetUniformLocation(shaderProgrami["teren"], "uKamenTrava"), 5);

            GL.ActiveTexture(TextureUnit.Texture6);
            GL.BindTexture(TextureTarget.Texture2D, teksture["kamenSneg"]);
            GL.Uniform1(GL.GetUniformLocation(shaderProgrami["teren"], "uKamenSneg"), 6);

            GL.ActiveTexture(TextureUnit.Texture7);
            GL.BindTexture(TextureTarget.Texture2D, teksture["travaSneg"]);
            GL.Uniform1(GL.GetUniformLocation(shaderProgrami["teren"], "uTravaSneg"), 7);

            GL.ActiveTexture(TextureUnit.Texture8);
            GL.BindTexture(TextureTarget.Texture2D, teksture["pesekTrava"]);
            GL.Uniform1(GL.GetUniformLocation(shaderProgrami["teren"], "uPesekTrava"), 8);
        }


    }
}
