using OpenTK.Mathematics;
using System.Runtime.InteropServices;
using GeneratorTerena;


namespace Menu
{
    public partial class GlavniMenu : Form
    {
        private Generator generator;
        private Vector2 zacetnaDelitevBarve = new Vector2(0.45f, 0.6f);
        private Vector2 uporabljenaDelitevBarve = new Vector2(0f, 0f);
        private Vector2i velikostOkna;
        private Bitmap slikaZaPredogled = new Bitmap(640, 640);
        private Blok[,] bloki;
        private bool[,] jeBlokNarisan = new bool[9, 9];
        private UpravljalnikBlokov upravljalnikBlokov;
        private float hitrostMiske;
        Okno okno;



        public GlavniMenu()
        {
            InitializeComponent();
            PreberiVelikostOkna();
            generator = new Generator();
            upravljalnikBlokov = new UpravljalnikBlokov(4, generator);
            bloki = upravljalnikBlokov.UatvariBloke();
            NastaviZacetek();
            NastaviDodatneInformacije();
        }

        /// <summary>
        /// Prebere vse parametre iz drsnikov.
        /// Privzete vrednosti na drsnikih so zaèetne vrednosti parametrov za teren
        /// </summary>
        private void NastaviZacetek()
        {
            boxSeme_TextChanged(boxSeme, EventArgs.Empty);
            drsnikOktave_Scroll(drsnikOktave, EventArgs.Empty);
            drsnikVztrajnost_Scroll(drsnikVztrajnost, EventArgs.Empty);
            drsnikLakunarnost_Scroll(drsnikLakunarnost, EventArgs.Empty);
            drsnikEksponent_Scroll(drsnikEksponent, EventArgs.Empty);
            drsnikScale_Scroll(drsnikRazmerje, EventArgs.Empty);
            drsnikVisina_Scroll(drsnikVisina, EventArgs.Empty);
            drsnikHitrostMiske_Scroll(drsnikHitrostMiske, EventArgs.Empty);
        }

        // Spremembe parametrov za teren.
        // Pri vsakem se na koncu posodobi slika za predogled

        /// <summary>
        /// Novo seme se spremeni v neko celo število in nastavi v generator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void boxSeme_TextChanged(object sender, EventArgs e)
        {
            int novoSeme = 1;
            foreach (char c in boxSeme.Text)
            {
                novoSeme *= c;
            }
            generator.Seme = novoSeme;
            PonastaviBloke();
        }
        // vsi drsniki (dvomim, da lahko grejo vsi v eno funkcijo)
        private void drsnikOktave_Scroll(object sender, EventArgs e)
        {
            TrackBar drsnik = (TrackBar)sender;
            int vrednost = drsnik.Value;
            generator.Oktave = vrednost;
            tekstOktave.Text = vrednost.ToString();
            PonastaviBloke();
        }
        private void drsnikVztrajnost_Scroll(object sender, EventArgs e)
        {
            TrackBar drsnik = (TrackBar)sender;
            float vrednost = drsnik.Value * 0.001f;
            generator.Vztrajnost = vrednost;
            tekstVztrajnost.Text = $"{vrednost:F3}";
            PonastaviBloke();
        }
        private void drsnikLakunarnost_Scroll(object sender, EventArgs e)
        {
            TrackBar drsnik = (TrackBar)sender;
            float vrednost = drsnik.Value * 0.001f;
            generator.Lakunarnost = vrednost;
            tekstLakunarnost.Text = $"{vrednost:F3}";
            PonastaviBloke();
        }
        private void drsnikEksponent_Scroll(object sender, EventArgs e)
        {
            TrackBar drsnik = (TrackBar)sender;
            float vrednost = drsnik.Value * 0.001f;
            generator.Eksponent = vrednost;
            tekstEksponent.Text = $"{vrednost:F3}";
            PosodobiDelitevBarve();
            PonastaviBloke();
        }
        private void drsnikVisina_Scroll(object sender, EventArgs e)
        {
            TrackBar drsnik = (TrackBar)sender;
            float vrednost = drsnik.Value * 0.001f;
            generator.MaxVisina = vrednost;
            tekstVisina.Text = $"{vrednost:F3}";
            PonastaviBloke();
        }
        private void drsnikScale_Scroll(object sender, EventArgs e)
        {
            TrackBar drsnik = (TrackBar)sender;
            float vrednost = drsnik.Value * 0.001f;
            generator.RazmerjeZvoka = vrednost;
            tekstRazmerje.Text = $"{vrednost:F3}";
            PonastaviBloke();
        }

        private void drsnikHitrostMiske_Scroll(object sender, EventArgs e)
        {
            TrackBar drsnik = (TrackBar)sender;
            this.hitrostMiske = drsnik.Value * 0.0001f;
        }

        /// <summary>
        /// Zaène glavno okno za raziskovanje terena
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gumbZazeni_Click(object sender, EventArgs e)
        {
            if (gumbKonzola.Checked)
            {
                ConsoleManager.Show();
            }
            okno = new Okno(velikostOkna, generator, uporabljenaDelitevBarve, hitrostMiske, (int)vidnaRazdalja.Value);

            this.Visible = false;
            using (okno)
            {
                okno.Run();
            }
            this.Visible = true;
        }

        /// <summary>
        /// Zamenja izbrano rezolucijo okna
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void izborRezolucija_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreberiVelikostOkna();
        }


        /// <summary>
        /// Na sliki za predogled posodobi en blok in jo znova nariše
        /// </summary>
        /// <param name="blok"></param>
        private void PosodobiBlokNaSliki(Blok blok)
        {
            int velikostBloka = generator.VelikostBlok;
            float[,] visinskaSlika = blok.VisinjskaSlika;

            // koordinati so zamaknjeni ker so robovi odveè
            for (int x = 1; x < velikostBloka + 1; x++)
            {           
                for (int y = 1; y < velikostBloka + 1; y++)
                {
                    // barva glede na višino 
                    float visina = visinskaSlika[x, y] / generator.MaxVisina;
                    Color barva;
                    if (visina < uporabljenaDelitevBarve.X * 0.5f) barva = Color.MediumBlue;
                    else if (visina < uporabljenaDelitevBarve.X - (3f / generator.MaxVisina)) barva = Color.LightBlue;
                    else if (visina < uporabljenaDelitevBarve.X) barva = Color.Yellow;
                    else if (visina < uporabljenaDelitevBarve.Y) barva = Color.Green;
                    else barva = Color.White;

                    // senèitev (simulira sonce zgoraj levo)
                    float razlikaVisineDiagonala = visinskaSlika[x + 1, y + 1] - visinskaSlika[x, y];
                    float razlikaVisineDesno = visinskaSlika[x + 1, y] - visinskaSlika[x, y];
                    float razlikaVisineGor = visinskaSlika[x, y + 1] - visinskaSlika[x, y];
                    float vsotaRazlike = razlikaVisineDiagonala + razlikaVisineDesno + razlikaVisineGor;
                    vsotaRazlike = Math.Clamp(vsotaRazlike, -3, 3) / 3; // recimo neka normalizirana razlika višine

                    barva = Color.FromArgb(
                        255,
                        (int)Math.Clamp(barva.R + vsotaRazlike * 100, 0, 255),
                        (int)Math.Clamp(barva.G + vsotaRazlike * 100, 0, 255),
                        (int)Math.Clamp(barva.B + vsotaRazlike * 100, 0, 255)
                        );

                    int globalX = x + (blok.BlokX + 4) * velikostBloka;
                    int globalY = y + (blok.BlokZ + 4) * velikostBloka;

                    slikaZaPredogled.SetPixel(globalX, globalY, barva);
                }
            }
            mapa.Image = slikaZaPredogled;
        }

        /// <summary>
        /// Prebere rezolucijo iz ComboBox-a in jo shrani
        /// </summary>
        private void PreberiVelikostOkna()
        {
            string[] rezolucija = izborRezolucija.Text.Split(':');
            velikostOkna = new Vector2i(int.Parse(rezolucija[0]), int.Parse(rezolucija[1]));
        }

        /// <summary>
        /// Na novo zgenerira vse bloke za predogled.
        /// </summary>
        private void PonastaviBloke()
        {
            upravljalnikBlokov.SprazniVrstoZaGeneriranje(); // sprazni vrsto
            bloki = upravljalnikBlokov.UatvariBloke();
            jeBlokNarisan = new bool[9, 9];
        }

        /// <summary>
        /// Nariše vse bloke, ki so že zgenerirani.
        /// Èe jih druga nit še ni zgenerirala jih ignorira.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PosodobiSliko(object sender, EventArgs e)
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    if (bloki[x, y].JE_ZGENERIRAN_TEREN && !jeBlokNarisan[x, y])
                    {
                        PosodobiBlokNaSliki(bloki[x, y]);
                        jeBlokNarisan[x, y] = true;

                        mapa.Image = slikaZaPredogled;
                    }
                }
            }
        }


        /// <summary>
        /// Posodobi se delitev barve. Èe se spremeni eksponent se mora to tudi poznati na delitvi barve.
        /// Zaèetna delitev barve je pri eksponentu 1
        /// </summary>
        private void PosodobiDelitevBarve()
        {
            float novX = (float)Math.Pow(zacetnaDelitevBarve.X, generator.Eksponent);
            float novY = (float)Math.Pow(zacetnaDelitevBarve.Y, generator.Eksponent);
            uporabljenaDelitevBarve = new Vector2(novX, novY);
        }


        private void NastaviDodatneInformacije()
        {
            Control[] tabelaInformacijskihIkon =
            {
                infoSeme,
                infoOktave,
                infoVztrajnost,
                infoLakunarnost,
                infoEksponent,
                infoVisina,
                infoRazmerje,
                infoVidnaRazdalja,
                infoZazeni
            };


            using (StreamReader datoteka = File.OpenText("tekstZaInformacijo.txt"))
            {
                string celotenTekst = datoteka.ReadToEnd();
                string[] tabelaInformacij = celotenTekst.Split("###");

                for (int i = 0; i < tabelaInformacijskihIkon.Length; i++)
                {
                    dodatneInformacije.SetToolTip(tabelaInformacijskihIkon[i], tabelaInformacij[i].Trim());
                }

            }
        }

        private void MiskaInformacijaGor(object sender, EventArgs e)
        {
            Label tekst = (Label)sender;
            tekst.BackColor = Color.DeepSkyBlue;
        }

        private void MiskaInformacijaDol(object sender, EventArgs e)
        {
            Label tekst = (Label)sender;
            tekst.BackColor = Color.LightBlue;
        }
    }

    // da se vidi konzolo
    public static class ConsoleManager
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        public static void Show()
        {
            AllocConsole();
        }

        public static void Hide()
        {
            FreeConsole();
        }
    }
}
