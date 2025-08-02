
using OpenTK.Mathematics;
using System.Collections.Concurrent;

namespace GeneratorTerena
{
    public class UpravljalnikBlokov
    {
        private Blok[,] bloki;
        private int velikostTabele;
        private Generator generator;
        private ConcurrentQueue<Blok> vrstaZaGeneriranjeTerena = new ConcurrentQueue<Blok>();
        private int steviloNiti = 2; // zaenkrat se to lahko spremeni samo tukaj

        public UpravljalnikBlokov(int vidnaRazdaljaBlokov, Generator generator)
        {
            this.generator = generator;
            this.velikostTabele = vidnaRazdaljaBlokov * 2 + 1;
            this.bloki = UatvariBloke();

            for (int i = 0; i < steviloNiti; i++)
            {
                ZacniNovoNit();
            }
        }

        /// <summary>
        /// Ustvari 2D tabelo blokov in pošlje delo generiranja terena na druge niti.
        /// </summary>
        /// <returns></returns>
        public Blok[,] UatvariBloke()
        {
            Blok[,] bloki = new Blok[velikostTabele, velikostTabele];
            for (int x = 0; x < velikostTabele; x++)
            {
                for (int y = 0; y < velikostTabele; y++)
                {
                    Blok novBlok = new Blok(x - (velikostTabele / 2), y - (velikostTabele / 2));
                    vrstaZaGeneriranjeTerena.Enqueue(novBlok); // delo za druge niti
                    bloki[x, y] = novBlok;
                }
            }
            return bloki;
        }

        /// <summary>
        /// Vrne seznam vseh blokov, ki so trenutno v vidnem polju kamere.
        /// </summary>
        /// <param name="kamera"></param>
        /// <returns></returns>
        public List<Blok> VidniBloki(Kamera kamera)
        {
            List<Blok> vidniBloki = new List<Blok>();
            Ravnina[] ravnine = kamera.RavnineOdKamere();
            int velikostBloka = generator.VelikostBlok - 1;

            for (int x = 0; x < velikostTabele; x++)
            {
                for (int z = 0; z < velikostTabele; z++)
                {
                    Blok blok = bloki[x, z];

                    // Bounding box podatki za ta blok
                    Vector3 min = new Vector3(blok.BlokX * velikostBloka, 0, blok.BlokZ * velikostBloka);
                    Vector3 max = new Vector3((blok.BlokX + 1) * velikostBloka, generator.MaxVisina, (blok.BlokZ + 1) * velikostBloka);

                    if (AliBlokVVidnemmPolju(max, min, ravnine))
                    {
                        vidniBloki.Add(blok);
                    }
                }
            }
            // izpis števila vidnih blokov
            Console.WriteLine($"vidni bloki: {vidniBloki.Count()} / {velikostTabele * velikostTabele}");

            return vidniBloki;
        }

        /// <summary>
        /// Vrne true če je blok znotraj vidnega polja kamere podanega z ravninami.
        /// </summary>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="ravnine"></param>
        /// <returns></returns>
        private bool AliBlokVVidnemmPolju(Vector3 max, Vector3 min, Ravnina[] ravnine)
        {
            foreach (Ravnina ravnina in ravnine)
            {
                Vector3 najblizjaTocka = new Vector3(
                    ravnina.Normala.X >= 0 ? max.X : min.X,
                    ravnina.Normala.Y >= 0 ? max.Y : min.Y,
                    ravnina.Normala.Z >= 0 ? max.Z : min.Z
                );

                if (Vector3.Dot(ravnina.Normala, najblizjaTocka) + ravnina.Razdalja < 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Preveri če je kamera izven srednjega bloka. Če je, sproži funkcijo za premik blokov.
        /// </summary>
        /// <param name="kamera"></param>
        public void PreveriPremikBlokov(Kamera kamera)
        {
            int velikostBloka = generator.VelikostBlok;
            Vector2 kamera2DPozicija = new Vector2(kamera.Pozicija.X, kamera.Pozicija.Z);  // y se ne rabi
            Vector2 kameraPozicijaBlok = (kamera2DPozicija / velikostBloka).Floor(); // pozicija v blokih (v katerem se nahaja)

            Blok opazovalniBlok = bloki[0, 0]; // spodnji levi 
            int dx = (int)kameraPozicijaBlok.X - opazovalniBlok.BlokX - (velikostTabele / 2);
            int dz = (int)kameraPozicijaBlok.Y - opazovalniBlok.BlokZ - (velikostTabele / 2);

            if (dx != 0 || dz != 0)
            {
                PremikBlokov(dx, dz);
            }
        }

        /// <summary>
        /// Zamakne vse bloke v tabeli blokov za nek dx, dz.
        /// Blokom izven nove tabele se zbrišejo podatki v GPU pomnilniku.
        /// V smeri premika pa se zgenerirajo novi bloki.
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dz"></param>
        private void PremikBlokov(int dx, int dz)
        {
            Blok[,] premaknjeniBloki = new Blok[velikostTabele, velikostTabele];
            int spodnjiLeviBlokX = bloki[0, 0].BlokX + dx;
            int spodnjiLeviBlokZ = bloki[0, 0].BlokZ + dz;

            for (int x = 0; x < velikostTabele; x++)
            {
                for (int z = 0; z < velikostTabele; z++)
                {
                    int stariX = x + dx;
                    int stariZ = z + dz;

                    // če se bo na tem mestu pojavil blok iz prejsnje tabele po tem, ko bi bil zamaknjen
                    bool aliZamik = stariX >= 0 && stariX < velikostTabele && stariZ >= 0 && stariZ < velikostTabele;

                    if (aliZamik)
                    {
                        premaknjeniBloki[x, z] = bloki[stariX, stariZ];

                        // če bo prejsnji blok izven tabele
                        bool jeZunaj = (stariX < 0 || stariX >= velikostTabele ||
                                        stariZ < 0 || stariZ >= velikostTabele);
                        if (jeZunaj) { bloki[x, z].SprazniPomnilnik(); }
                    }
                    else
                    {
                        // generiranje novega
                        int praviX = spodnjiLeviBlokX + x;
                        int praviZ = spodnjiLeviBlokZ + z;
                        Blok novBlok = new Blok(praviX, praviZ);
                        vrstaZaGeneriranjeTerena.Enqueue(novBlok); // delo za druge niti
                        premaknjeniBloki[x, z] = novBlok;
                    }
                }
            }
            bloki = premaknjeniBloki;
        }

        /// <summary>
        /// Za vse bloke pobriše vse podatke na pomnilniku od GPU-ja.
        /// </summary>
        public void PocistiPomnilnik()
        {
            foreach (Blok blok in bloki)
            {
                blok.SprazniPomnilnik();
            }
        }


        /// <summary>
        /// Ustvari novo nit, ki bo generirala teren.
        /// Nit konstantno preverja če ima delo v vrsti. Če ima, ga opravi, drugače pa spi.
        /// </summary>
        private void ZacniNovoNit()
        {
            new Thread(() =>
            {
                while (true)
                {
                    if (vrstaZaGeneriranjeTerena.TryDequeue(out Blok blok))
                    {
                        blok.GenerirajTeren(generator);
                    }
                    else
                    {
                        Thread.Sleep(1);
                    }
                }
            })
            {
                IsBackground = true
            }.Start();
        }

        /// <summary>
        /// Vrsta v kateri se nahaja delo za druge niti se spraznejo vsa napovedana dela.
        /// </summary>
        public void SprazniVrstoZaGeneriranje()
        {
            while (vrstaZaGeneriranjeTerena.TryDequeue(out _)) { }
        }

    }

}
