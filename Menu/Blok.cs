using OpenTK.Graphics.OpenGL4;
using System.Numerics;

namespace GeneratorTerena
{

    public class Blok
    {
        private bool je_v_gpu = false;
        private bool je_zgeneriran_teren = false;
        private float[,] visinjskaSlika;
        // "kazalci"/hadler-ji na spomin v GPU
        private int vao; // vertex array object
        private int vbo; // vertex buffer object
        private int ebo; // element buffer object
        private object zaklenjenObjekt = new();


        public int BlokX { get; init; }
        public int BlokZ { get; init; }
        public bool JE_V_GPU { get { return je_v_gpu; } }
        public bool JE_ZGENERIRAN_TEREN { get { return je_zgeneriran_teren; } }
        public float[,] VisinjskaSlika {
            get
            {
                if (JE_ZGENERIRAN_TEREN)
                {
                    return this.visinjskaSlika;
                }
                else
                {
                    throw new Exception("Teren še ni zgeneriran");
                }
            }
        }

        public int VAO {
            get
            {
                if (JE_V_GPU)
                {
                    return this.vao;
                }
                else
                {
                    throw new Exception("VAO še ni narejen");
                }
            }
        }

        public Blok(int X, int Z)
        {
            BlokX = X;
            BlokZ = Z;
        }

        /// <summary>
        /// Ustvari višinjsko sliko za ta blok
        /// </summary>
        /// <param name="generator"></param>
        public void GenerirajTeren(Generator generator)
        {
            lock (zaklenjenObjekt)
            {
                if (je_zgeneriran_teren) return;

                visinjskaSlika = generator.VisinskaSlika(this);
                je_zgeneriran_teren = true;
            }
        }

        /// <summary>
        /// V GPU pomnilnik pošlje vse podatke o 3D mreži terena.
        /// Pošlje se tabela podatkov o 3D mreži
        /// in tabela indeksov za to tabelo, ki predstavlja vrstni red točk za sestavljanje trikotnikov.
        /// </summary>
        public void NapolniPomnilnik()
        {
            if (je_v_gpu) return; // če je že v GPU
            if (!je_zgeneriran_teren) return; // če podatki še niso zgenerirani

            float[] podatki = PodatkiZaVBO();
            int[] indeksi = Indeksi();

            // predstavljaj si kot kazalci na spomin v GPU
            int vao = GL.GenVertexArray();
            int vbo = GL.GenBuffer();
            int ebo = GL.GenBuffer();

            GL.BindVertexArray(vao);

            int zamik = 6 * sizeof(float); // 6 float-ov za eno točko

            // Vertex buffer object
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * podatki.Length, podatki, BufferUsageHint.StaticDraw);

            // Element buffer object (indeksi)
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * indeksi.Length, indeksi, BufferUsageHint.StaticDraw);

            // pozicija
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, zamik, 0);
            GL.EnableVertexAttribArray(0);

            // normala
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, zamik, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);


            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);


            je_v_gpu = true;
            this.vao = vao;
            this.vbo = vbo;
            this.ebo = ebo;
        }

        /// <summary>
        /// Za vsako točko sešteje vse normale tistih trikotnikov, ki vsebujejo to točko.
        /// Višinjska slika je za dve enoti večja zato, da se tudi pri robnih točkah upoštevajo
        /// normale trikotnikov, ki niso iz tega bloka.
        /// </summary>
        /// <returns></returns>
        private Vector3[,] SestejNormale()
        {
            float[,] visine = VisinjskaSlika;
            int dolzina = visine.GetLength(0) - 2;
            Vector3[,] normale = new Vector3[(dolzina + 2), (dolzina + 2)];
            // v funkciji "PodatkiZaVAO()" se na koncu robovi ne upoštevajo

            for (int z = 0; z < dolzina + 1; z++)
            {
                for (int x = 0; x < dolzina + 1; x++)
                {
                    Vector3 p00 = new Vector3(x, visine[x, z], z);
                    Vector3 p10 = new Vector3(x + 1, visine[x + 1, z], z);
                    Vector3 p01 = new Vector3(x, visine[x, z + 1], z + 1);
                    Vector3 p11 = new Vector3(x + 1, visine[x + 1, z + 1], z + 1);

                    Vector3 normala1 = Vector3.Normalize(Vector3.Cross(p01 - p00, p10 - p00));
                    Vector3 normala2 = Vector3.Normalize(Vector3.Cross(p11 - p01, p10 - p01));

                    normale[x, z] += normala1;
                    normale[x + 1, z] += normala1 + normala2;
                    normale[x, z + 1] += normala1 + normala2;
                    normale[x + 1, z + 1] += normala2;
                }
            }
            return normale;
        }

        /// <summary>
        /// Vrne tabelo indeksov, ki predstavlja vrstni red točk za risnje trikotnikov
        /// </summary>
        /// <returns></returns>
        private int[] Indeksi()
        {
            int dolzina = VisinjskaSlika.GetLength(0) - 2;
            int[] indeksi = new int[6 * (dolzina - 1) * (dolzina - 1)]; // 3 točke na trikotnik in 2 trikotnika na 2x2 kvadratu

            // vrstni red indeksov
            int i = 0;
            for (int z = 0; z < dolzina - 1; z++)
            {
                for (int x = 0; x < dolzina - 1; x++)
                {
                    int i0 = x + z * dolzina;
                    int i1 = x + (z + 1) * dolzina;
                    int i2 = x + 1 + z * dolzina;
                    int i3 = x + 1 + (z + 1) * dolzina;

                    // Trikotnik 1: i0, i1, i2
                    indeksi[i++] = i0;
                    indeksi[i++] = i1;
                    indeksi[i++] = i2;

                    // Trikotnik 2: i2, i1, i3
                    indeksi[i++] = i2;
                    indeksi[i++] = i1;
                    indeksi[i++] = i3;
                }
            }
            return indeksi;
        }


        /// <summary>
        /// Vrne tabelo dolžine 8 * št. točk na mreži
        /// 3 za koordinate točke
        /// 3 za normalo te točke
        /// 2 za koordinate teksture
        /// </summary>
        /// <returns></returns>
        private float[] PodatkiZaVBO()
        {
            float[,] visine = VisinjskaSlika;
            int dolzina = visine.GetLength(0) - 2;
            float[] podatki = new float[6 * dolzina * dolzina]; // 8 vrednosti na točko (3 za pizicijo, 3 za normalo)
            Vector3[,] normale = SestejNormale();

            // vstavljanje točk, normal in koordinat v VAO
            int i = 0;
            for (int z = 0; z < dolzina; z++)
            {
                for (int x = 0; x < dolzina; x++)
                {
                    Vector3 pozicija = new Vector3(x, visine[x + 1, z + 1], z); // ta tabela je za 2 prevelika zato "+ 1" zamik
                    Vector3 normala = Vector3.Normalize(normale[x + 1, z + 1]); // tu isti razlog

                    podatki[i++] = pozicija.X;
                    podatki[i++] = pozicija.Y;
                    podatki[i++] = pozicija.Z;
                    podatki[i++] = normala.X;
                    podatki[i++] = normala.Y;
                    podatki[i++] = normala.Z;
                }
            }
            return podatki;
        }

        /// <summary>
        /// izbriše podatke o tem bloku, ki so bili poslani na GPU z funkcijo NapolniPomnilnik
        /// če se blok ne bo več risal je pametno izbrisati pomnilnik
        /// </summary>
        public void SprazniPomnilnik()
        {
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(vbo);
            GL.DeleteBuffer(ebo);
            je_v_gpu = false;
        }

    }
}
