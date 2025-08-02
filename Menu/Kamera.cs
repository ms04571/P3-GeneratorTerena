using OpenTK.Mathematics;


namespace GeneratorTerena
{
    public class Kamera
    {
        private Vector3 pozicija;
        private float pitch = -0.5f;
        private float yaw = -MathF.PI * 0.5f;
        private float fov = (float)Math.PI * 0.5f;
        private float aspectRatio = 16f / 9f;
        private float blizu = 0.1f;
        private float dalec = 10000f;
        private Vector3 naprej;
        private Vector3 desno;
        private Vector3 gor;


        public Vector3 Pozicija { get { return pozicija; } set { pozicija = value; } }
        public float Pitch { get { return pitch; } set { pitch = value; } }
        public float Yaw { get { return yaw; } set { yaw = value; } }
        public float Fov { get { return fov; } set { fov = value; } }
        public float Razmerje { get { return aspectRatio; } set { aspectRatio = value; } }
        public float Blizu { get { return blizu; } set { blizu = value; } }
        public float Dalec { get { return dalec; } set { dalec = value; } }
        public Vector3 Naprej { get { return naprej; } set { naprej = value; } }
        public Vector3 Desno { get { return desno; } set { desno = value; } }
        public Vector3 Gor { get { return gor; } set { gor = value; } }
        public Matrix4 MatrikaOgleda
        {
            get { return Matrix4.LookAt(pozicija, pozicija + naprej, Vector3.UnitY); }
        }
        public Matrix4 ProjekcijskaMatrika
        {
            get {
                return Matrix4.CreatePerspectiveFieldOfView(
                Fov,
                Razmerje,
                Blizu,
                Dalec
            ); }
        }

        public Kamera(float visina)
        {
            Pozicija = new Vector3(0, visina, 0);
            PosodobiBazneVektorje();
        }




        /// <summary>
        /// Posodobi vektorje naprej,gor,desno glede na trenutno orientacijo kamere
        /// </summary>
        public void PosodobiBazneVektorje()
        {
            Naprej = new Vector3(
                MathF.Cos(Yaw) * MathF.Cos(Pitch),
                MathF.Sin(Pitch),
                MathF.Sin(Yaw) * MathF.Cos(Pitch)
            ).Normalized();
            Desno = Vector3.Cross(Naprej, Vector3.UnitY).Normalized();
            Gor = -Vector3.Cross(Naprej, Desno).Normalized();
        }

        /// <summary>
        /// Vrne tabelo ravnin, ki predstavljajo robove vidnega polja kamere
        /// </summary>
        /// <returns></returns>
        public Ravnina[] RavnineOdKamere()
        {
            Ravnina[] ravnine = new Ravnina[6];
            Matrix4 mat = MatrikaOgleda * ProjekcijskaMatrika;
            // leva
            ravnine[0] = new Ravnina(
                new Vector3(mat.M14 + mat.M11, mat.M24 + mat.M21, mat.M34 + mat.M31),
                mat.M44 + mat.M41
            );

            // desna
            ravnine[1] = new Ravnina(
                new Vector3(mat.M14 - mat.M11, mat.M24 - mat.M21, mat.M34 - mat.M31),
                mat.M44 - mat.M41
            );

            // spodaj
            ravnine[2] = new Ravnina(
                new Vector3(mat.M14 + mat.M12, mat.M24 + mat.M22, mat.M34 + mat.M32),
                mat.M44 + mat.M42
            );

            // zgoraj
            ravnine[3] = new Ravnina(
                new Vector3(mat.M14 - mat.M12, mat.M24 - mat.M22, mat.M34 - mat.M32),
                mat.M44 - mat.M42
            );

            // nazaj
            ravnine[4] = new Ravnina(
                new Vector3(mat.M13, mat.M23, mat.M33),
                mat.M43
            );

            // naprej
            ravnine[5] = new Ravnina(
                new Vector3(mat.M14 - mat.M13, mat.M24 - mat.M23, mat.M34 - mat.M33),
                mat.M44 - mat.M43
            );

            // normaliziranje
            for (int i = 0; i < 6; i++)
            {
                float length = ravnine[i].Normala.Length;
                ravnine[i].Normala /= length;
                ravnine[i].Razdalja /= length;
            }

            return ravnine;
        }
    }

    public struct Ravnina
    {
        public Vector3 Normala;
        public float Razdalja;

        public Ravnina(Vector3 normala, float razdalja)
        {
            Normala = normala;
            Razdalja = razdalja;
        }

        public float RazdaljaDoTocke(Vector3 point)
        {
            return Vector3.Dot(Normala, point) + Razdalja;
        }
    }
}
