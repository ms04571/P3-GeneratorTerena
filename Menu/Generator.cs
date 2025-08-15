using System.Numerics;

namespace GeneratorTerena
{
    public class Generator
    {
        private const float MAX_INT_INVERZ = 1f / 4294967296f;
        private int velikostBlok = 64;
        private int seme = 0;
        private int oktave = 1;
        private float vztrajnost = 0.5f;
        private float lakunarnost = 2f;
        private float maxVisina = 50f;
        private float eksponent = 1f;
        private float razmerjeSuma = 0.5f;
        private float maxAmplitudaInverz; // hitrejše računanje

        public int Seme
        {
            get { return seme; }
            set { seme = value; }
        }
        public int Oktave
        {
            get { return oktave; }
            set
            {
                oktave = value;
                maxAmplitudaInverz = IzracunajMaxAmplitudaInverz();
            }
        }
        public int VelikostBlok
        {
            get { return velikostBlok; }
        }

        public float Vztrajnost
        {
            get { return vztrajnost; }
            set { vztrajnost = value; }
        }

        public float Lakunarnost
        {
            get { return lakunarnost; }
            set { lakunarnost = value; }
        }

        public float MaxVisina
        {
            get { return maxVisina; }
            set { maxVisina = value; }
        }

        public float Eksponent
        {
            get { return eksponent; }
            set { eksponent = value; }
        }

        public float MaxAmplitudaInverz
        {
            get { return maxAmplitudaInverz; }
        }

        public float RazmerjeZvoka
        {
            get { return razmerjeSuma; }
            set { razmerjeSuma = value; }
        }

        public Generator()
        {
            maxAmplitudaInverz = IzracunajMaxAmplitudaInverz();
        }


        /// <summary>
        /// iz koordinat in semena se ustvari deterministični hash
        /// in iz njega naredi enotni vektor v smeri nekje na intevalu [0, 2pi)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public Vector2 Gradient(int x, int y)
        {
            uint hash = Hash(x, y);
            float theta = hash * MAX_INT_INVERZ * 2f * (float)Math.PI;
            Vector2 g = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));

            return g;
        }

        /// <summary>
        /// ustvari deterministični hash tipa uint za uporabo pri gradientu
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        private uint Hash(int x, int y)
        {
            unchecked
            {
                uint h = (uint)x * 0x27d4eb2d + (uint)y * 0x165667b1 + (uint)seme * 0x85ebca6b;
                h ^= h >> 15;
                h *= 0x85ebca6b;
                h ^= h >> 13;
                h *= 0xc2b2ae35;
                h ^= h >> 16;
                return h;
            }
        }


        /// <summary>
        /// izračuna inverz največje amplitude pri generiranju blokov
        /// za bolj optimalno računanje
        /// </summary>
        /// <returns></returns>
        private float IzracunajMaxAmplitudaInverz()
        {
            float amplituda = 1f;
            float vsota = 0f;

            for (int i = 0; i < Oktave; i++)
            {
                vsota += amplituda;
                amplituda *= Vztrajnost;
            }

            return 1f / vsota;
        }

        /// <summary>
        /// algoritem za perlin noise
        /// vrne strukturo PerlinRezultat z višino in parcialnima odvoda po x in y
        /// odvodi se rabijo za izračun normal
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PerlinRezultat Perlin(float x, float y)
        {
            int gradX = (int)Math.Floor(x);
            int gradY = (int)Math.Floor(y);

            float relativenX = x - gradX;
            float relativenY = y - gradY;

            Vector2 gradient00 = Gradient(gradX, gradY);     //spodaj levo
            Vector2 gradient10 = Gradient(gradX + 1, gradY);     //spodaj desno
            Vector2 gradient01 = Gradient(gradX, gradY + 1); //zgoraj levo
            Vector2 gradient11 = Gradient(gradX + 1, gradY + 1); //zgoraj desno

            Vector2 vektor00 = new Vector2(relativenX, relativenY);
            Vector2 vektor10 = new Vector2(relativenX - 1, relativenY);
            Vector2 vektor01 = new Vector2(relativenX, relativenY - 1);
            Vector2 vektor11 = new Vector2(relativenX - 1, relativenY - 1);

            float skal00 = Vector2.Dot(vektor00, gradient00);
            float skal10 = Vector2.Dot(vektor10, gradient10);
            float skal01 = Vector2.Dot(vektor01, gradient01);
            float skal11 = Vector2.Dot(vektor11, gradient11);

            // uporabimo fade funkcijo za gladko interpoliranje
            float u = Fade(relativenX);
            float v = Fade(relativenY);

            // izračun višine in parcialnih odvodov se naredi analitično
            // ta postopek je iz članka https://iquilezles.org/articles/morenoise/
            float k0 = skal00;
            float k1 = skal10 - skal00;
            float k2 = skal01 - skal00;
            float k3 = skal00 - skal01 - skal10 + skal11;
            float du = FadeOdvod(relativenX);
            float dv = FadeOdvod(relativenY);

            // gladka nterpolacija med štirimi skalarnimi produkti (skrajšano seveda)
            float visina = k0 + k1 * u + k2 * v + k3 * u * v;
            // parcialna odvoda (uporabno za osnovno simuliranje erozije)
            float dx = du * (k1 + k3 * v);
            float dy = dv * (k2 + k3 * u);

            return new PerlinRezultat(visina, new Vector2(dx, dy));
        }


        /// <summary>
        /// fade funkcija
        /// t mora biti v intervalu 0, 1
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private float Fade(float t)
        {
            // Perlinova klasčna fade funkcija: 6t^5 - 15t^4 + 10t^3
            return t * t * t * (t * (t * 6f - 15f) + 10f);
        }

        /// <summary>
        /// odvod Fade funkcije za računanje parcialnih odvodov
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private float FadeOdvod(float t)
        {
            return 30 * t * t * (t * (t - 2f) + 1f);
        }

        /// <summary>
        /// tu se zgodi vso generiranje z Perlin noise algoritmom
        /// za vsak (x, z) v bloku vrne y koordinato (višino)
        /// </summary>
        /// <returns></returns>
        public float[,] VisinskaSlika(Blok blok)
        {

            float velikostInverz = 1f / (velikostBlok - 1); // (velikostBlok - 1) zato da se bloki zlepijo skupaj
            float[,] slika = new float[velikostBlok + 2, velikostBlok + 2];
            // velikostBlok + 2 za računaje normal robnih točk
            // malo škoda ker se bo rob večkrat izračunal ampak bo lepša osvetljava


            for (int x = -1; x < velikostBlok + 1; x++)
            {
                for (int z = -1; z < velikostBlok + 1; z++)
                {
                    float koncnaVisina = 0f;
                    float frekvenca = 1f;
                    float amplituda = 1f;
                    Vector2 gradient = Vector2.Zero;

                    // relativni koordinati
                    float globalX = blok.BlokX + x * velikostInverz;
                    float globalY = blok.BlokZ + z * velikostInverz;

                    for (int o = 0; o < Oktave; o++)
                    {
                        // frekvenca in razmerje se upoštevata tukaj
                        float X = globalX * frekvenca * RazmerjeZvoka;
                        float Z = globalY * frekvenca * RazmerjeZvoka;

                        PerlinRezultat rezultat = Perlin(X, Z);
                        gradient += rezultat.Gradient;
                        koncnaVisina += rezultat.Visina * amplituda / (1f + 0.25f * Vector2.Dot(gradient, gradient)); // 0.5f je lahko parameter

                        // amplituda in frekvenca za naslednjo oktavo
                        amplituda *= vztrajnost;
                        frekvenca *= lakunarnost;
                    }

                    // preoblikovanje višine
                    koncnaVisina *= MaxAmplitudaInverz;  // normalizira na [-1, 1]
                    koncnaVisina = (koncnaVisina + 1) * 0.5f;   // zamakne na  [0, 1]
                    koncnaVisina = MathF.Pow(koncnaVisina, Eksponent);
                    koncnaVisina *= MaxVisina;

                    slika[x + 1, z + 1] = koncnaVisina;
                }
            }

            return slika;

        }


    }

    /// <summary>
    /// struktura za rezultat od Perlin algoritma
    /// vsebuje višino in dva parcialna odvoda v tisti točki
    /// </summary>
    public readonly struct PerlinRezultat
    {
        public PerlinRezultat(float visina, Vector2 gradient)
        {
            Visina = visina;
            Gradient = gradient;
        }
        public float Visina { get; init; }
        public Vector2 Gradient { get; init; }
    }
}
