namespace Menu
{
    partial class GlavniMenu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            mapa = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            drsnikOktave = new TrackBar();
            drsnikVztrajnost = new TrackBar();
            drsnikLakunarnost = new TrackBar();
            drsnikEksponent = new TrackBar();
            boxSeme = new TextBox();
            label5 = new Label();
            label6 = new Label();
            drsnikVisina = new TrackBar();
            gumbZazeni = new Button();
            label7 = new Label();
            drsnikRazmerje = new TrackBar();
            izborRezolucija = new ComboBox();
            casZaPosodobitiSliko = new System.Windows.Forms.Timer(components);
            gumbKonzola = new CheckBox();
            groupBox1 = new GroupBox();
            infoRazmerje = new Label();
            infoVisina = new Label();
            infoEksponent = new Label();
            infoLakunarnost = new Label();
            infoVztrajnost = new Label();
            infoSeme = new Label();
            infoOktave = new Label();
            tekstRazmerje = new Label();
            tekstVisina = new Label();
            tekstEksponent = new Label();
            tekstLakunarnost = new Label();
            tekstVztrajnost = new Label();
            tekstOktave = new Label();
            groupBox2 = new GroupBox();
            vidnaRazdalja = new NumericUpDown();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            infoVidnaRazdalja = new Label();
            drsnikHitrostMiske = new TrackBar();
            groupBox3 = new GroupBox();
            infoZazeni = new Label();
            dodatneInformacije = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)mapa).BeginInit();
            ((System.ComponentModel.ISupportInitialize)drsnikOktave).BeginInit();
            ((System.ComponentModel.ISupportInitialize)drsnikVztrajnost).BeginInit();
            ((System.ComponentModel.ISupportInitialize)drsnikLakunarnost).BeginInit();
            ((System.ComponentModel.ISupportInitialize)drsnikEksponent).BeginInit();
            ((System.ComponentModel.ISupportInitialize)drsnikVisina).BeginInit();
            ((System.ComponentModel.ISupportInitialize)drsnikRazmerje).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)vidnaRazdalja).BeginInit();
            ((System.ComponentModel.ISupportInitialize)drsnikHitrostMiske).BeginInit();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // mapa
            // 
            mapa.Location = new Point(6, 17);
            mapa.Name = "mapa";
            mapa.Size = new Size(576, 576);
            mapa.TabIndex = 0;
            mapa.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 85);
            label1.Name = "label1";
            label1.Size = new Size(100, 15);
            label1.TabIndex = 1;
            label1.Text = "Nivo podrobnosti";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(26, 150);
            label2.Name = "label2";
            label2.Size = new Size(59, 15);
            label2.TabIndex = 1;
            label2.Text = "Vztrajnost";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(26, 212);
            label3.Name = "label3";
            label3.Size = new Size(72, 15);
            label3.TabIndex = 1;
            label3.Text = "Lakunarnost";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(26, 276);
            label4.Name = "label4";
            label4.Size = new Size(62, 15);
            label4.TabIndex = 1;
            label4.Text = "Eksponent";
            // 
            // drsnikOktave
            // 
            drsnikOktave.Location = new Point(26, 103);
            drsnikOktave.Maximum = 15;
            drsnikOktave.Minimum = 1;
            drsnikOktave.Name = "drsnikOktave";
            drsnikOktave.Size = new Size(260, 45);
            drsnikOktave.TabIndex = 2;
            drsnikOktave.Value = 6;
            drsnikOktave.Scroll += drsnikOktave_Scroll;
            // 
            // drsnikVztrajnost
            // 
            drsnikVztrajnost.Location = new Point(26, 168);
            drsnikVztrajnost.Maximum = 750;
            drsnikVztrajnost.Minimum = 250;
            drsnikVztrajnost.Name = "drsnikVztrajnost";
            drsnikVztrajnost.Size = new Size(260, 45);
            drsnikVztrajnost.TabIndex = 2;
            drsnikVztrajnost.Value = 500;
            drsnikVztrajnost.Scroll += drsnikVztrajnost_Scroll;
            // 
            // drsnikLakunarnost
            // 
            drsnikLakunarnost.Location = new Point(26, 230);
            drsnikLakunarnost.Maximum = 2500;
            drsnikLakunarnost.Minimum = 1500;
            drsnikLakunarnost.Name = "drsnikLakunarnost";
            drsnikLakunarnost.Size = new Size(260, 45);
            drsnikLakunarnost.TabIndex = 2;
            drsnikLakunarnost.Value = 2000;
            drsnikLakunarnost.Scroll += drsnikLakunarnost_Scroll;
            // 
            // drsnikEksponent
            // 
            drsnikEksponent.Location = new Point(26, 299);
            drsnikEksponent.Maximum = 5000;
            drsnikEksponent.Minimum = 500;
            drsnikEksponent.Name = "drsnikEksponent";
            drsnikEksponent.Size = new Size(260, 45);
            drsnikEksponent.TabIndex = 2;
            drsnikEksponent.Value = 4000;
            drsnikEksponent.Scroll += drsnikEksponent_Scroll;
            // 
            // boxSeme
            // 
            boxSeme.Location = new Point(26, 46);
            boxSeme.Name = "boxSeme";
            boxSeme.Size = new Size(260, 23);
            boxSeme.TabIndex = 4;
            boxSeme.TextChanged += boxSeme_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(26, 28);
            label5.Name = "label5";
            label5.Size = new Size(36, 15);
            label5.TabIndex = 1;
            label5.Text = "Seme";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(26, 351);
            label6.Name = "label6";
            label6.Size = new Size(38, 15);
            label6.TabIndex = 1;
            label6.Text = "Višina";
            // 
            // drsnikVisina
            // 
            drsnikVisina.Location = new Point(26, 369);
            drsnikVisina.Maximum = 1000000;
            drsnikVisina.Minimum = 10000;
            drsnikVisina.Name = "drsnikVisina";
            drsnikVisina.Size = new Size(260, 45);
            drsnikVisina.TabIndex = 2;
            drsnikVisina.Value = 300000;
            drsnikVisina.Scroll += drsnikVisina_Scroll;
            // 
            // gumbZazeni
            // 
            gumbZazeni.Location = new Point(235, 602);
            gumbZazeni.Name = "gumbZazeni";
            gumbZazeni.Size = new Size(139, 53);
            gumbZazeni.TabIndex = 5;
            gumbZazeni.Text = "zaženi";
            gumbZazeni.UseVisualStyleBackColor = true;
            gumbZazeni.Click += gumbZazeni_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(26, 419);
            label7.Name = "label7";
            label7.Size = new Size(55, 15);
            label7.TabIndex = 1;
            label7.Text = "Razmerje";
            // 
            // drsnikRazmerje
            // 
            drsnikRazmerje.Location = new Point(26, 437);
            drsnikRazmerje.Maximum = 1000;
            drsnikRazmerje.Minimum = 100;
            drsnikRazmerje.Name = "drsnikRazmerje";
            drsnikRazmerje.Size = new Size(260, 45);
            drsnikRazmerje.TabIndex = 2;
            drsnikRazmerje.Value = 500;
            drsnikRazmerje.Scroll += drsnikScale_Scroll;
            // 
            // izborRezolucija
            // 
            izborRezolucija.FormattingEnabled = true;
            izborRezolucija.Items.AddRange(new object[] { "1920:1080", "1280:720", "800:600" });
            izborRezolucija.Location = new Point(246, 39);
            izborRezolucija.Name = "izborRezolucija";
            izborRezolucija.Size = new Size(83, 23);
            izborRezolucija.TabIndex = 7;
            izborRezolucija.Text = "1280:720";
            izborRezolucija.SelectedIndexChanged += izborRezolucija_SelectedIndexChanged;
            // 
            // casZaPosodobitiSliko
            // 
            casZaPosodobitiSliko.Enabled = true;
            casZaPosodobitiSliko.Interval = 10;
            casZaPosodobitiSliko.Tick += PosodobiSliko;
            // 
            // gumbKonzola
            // 
            gumbKonzola.AutoSize = true;
            gumbKonzola.Checked = true;
            gumbKonzola.CheckState = CheckState.Checked;
            gumbKonzola.Location = new Point(21, 39);
            gumbKonzola.Name = "gumbKonzola";
            gumbKonzola.Size = new Size(105, 19);
            gumbKonzola.TabIndex = 8;
            gumbKonzola.Text = "Prikaži konzolo";
            gumbKonzola.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(infoRazmerje);
            groupBox1.Controls.Add(infoVisina);
            groupBox1.Controls.Add(infoEksponent);
            groupBox1.Controls.Add(infoLakunarnost);
            groupBox1.Controls.Add(infoVztrajnost);
            groupBox1.Controls.Add(infoSeme);
            groupBox1.Controls.Add(infoOktave);
            groupBox1.Controls.Add(tekstRazmerje);
            groupBox1.Controls.Add(tekstVisina);
            groupBox1.Controls.Add(tekstEksponent);
            groupBox1.Controls.Add(tekstLakunarnost);
            groupBox1.Controls.Add(tekstVztrajnost);
            groupBox1.Controls.Add(tekstOktave);
            groupBox1.Controls.Add(boxSeme);
            groupBox1.Controls.Add(drsnikRazmerje);
            groupBox1.Controls.Add(drsnikVisina);
            groupBox1.Controls.Add(drsnikEksponent);
            groupBox1.Controls.Add(drsnikLakunarnost);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(drsnikVztrajnost);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(drsnikOktave);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 11);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(353, 505);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = "Parametri za teren";
            // 
            // infoRazmerje
            // 
            infoRazmerje.AutoSize = true;
            infoRazmerje.BackColor = Color.LightBlue;
            infoRazmerje.Font = new Font("Segoe UI", 6.75F);
            infoRazmerje.Location = new Point(274, 422);
            infoRazmerje.Name = "infoRazmerje";
            infoRazmerje.Size = new Size(9, 12);
            infoRazmerje.TabIndex = 6;
            infoRazmerje.Text = "?";
            infoRazmerje.MouseLeave += MiskaInformacijaDol;
            infoRazmerje.MouseHover += MiskaInformacijaGor;
            // 
            // infoVisina
            // 
            infoVisina.AutoSize = true;
            infoVisina.BackColor = Color.LightBlue;
            infoVisina.Font = new Font("Segoe UI", 6.75F);
            infoVisina.Location = new Point(274, 354);
            infoVisina.Name = "infoVisina";
            infoVisina.Size = new Size(9, 12);
            infoVisina.TabIndex = 6;
            infoVisina.Text = "?";
            infoVisina.MouseLeave += MiskaInformacijaDol;
            infoVisina.MouseHover += MiskaInformacijaGor;
            // 
            // infoEksponent
            // 
            infoEksponent.AutoSize = true;
            infoEksponent.BackColor = Color.LightBlue;
            infoEksponent.Font = new Font("Segoe UI", 6.75F);
            infoEksponent.Location = new Point(274, 284);
            infoEksponent.Name = "infoEksponent";
            infoEksponent.Size = new Size(9, 12);
            infoEksponent.TabIndex = 6;
            infoEksponent.Text = "?";
            infoEksponent.MouseLeave += MiskaInformacijaDol;
            infoEksponent.MouseHover += MiskaInformacijaGor;
            // 
            // infoLakunarnost
            // 
            infoLakunarnost.AutoSize = true;
            infoLakunarnost.BackColor = Color.LightBlue;
            infoLakunarnost.Font = new Font("Segoe UI", 6.75F);
            infoLakunarnost.Location = new Point(274, 212);
            infoLakunarnost.Name = "infoLakunarnost";
            infoLakunarnost.Size = new Size(9, 12);
            infoLakunarnost.TabIndex = 6;
            infoLakunarnost.Text = "?";
            infoLakunarnost.MouseLeave += MiskaInformacijaDol;
            infoLakunarnost.MouseHover += MiskaInformacijaGor;
            // 
            // infoVztrajnost
            // 
            infoVztrajnost.AutoSize = true;
            infoVztrajnost.BackColor = Color.LightBlue;
            infoVztrajnost.Font = new Font("Segoe UI", 6.75F);
            infoVztrajnost.Location = new Point(274, 153);
            infoVztrajnost.Name = "infoVztrajnost";
            infoVztrajnost.Size = new Size(9, 12);
            infoVztrajnost.TabIndex = 6;
            infoVztrajnost.Text = "?";
            infoVztrajnost.MouseLeave += MiskaInformacijaDol;
            infoVztrajnost.MouseHover += MiskaInformacijaGor;
            // 
            // infoSeme
            // 
            infoSeme.AutoSize = true;
            infoSeme.BackColor = Color.LightBlue;
            infoSeme.Font = new Font("Segoe UI", 6.75F);
            infoSeme.Location = new Point(274, 31);
            infoSeme.Name = "infoSeme";
            infoSeme.Size = new Size(9, 12);
            infoSeme.TabIndex = 6;
            infoSeme.Text = "?";
            infoSeme.MouseLeave += MiskaInformacijaDol;
            infoSeme.MouseHover += MiskaInformacijaGor;
            // 
            // infoOktave
            // 
            infoOktave.AutoSize = true;
            infoOktave.BackColor = Color.LightBlue;
            infoOktave.Font = new Font("Segoe UI", 6.75F);
            infoOktave.Location = new Point(274, 88);
            infoOktave.Name = "infoOktave";
            infoOktave.Size = new Size(9, 12);
            infoOktave.TabIndex = 6;
            infoOktave.Text = "?";
            infoOktave.MouseLeave += MiskaInformacijaDol;
            infoOktave.MouseHover += MiskaInformacijaGor;
            // 
            // tekstRazmerje
            // 
            tekstRazmerje.AutoSize = true;
            tekstRazmerje.Location = new Point(292, 437);
            tekstRazmerje.Name = "tekstRazmerje";
            tekstRazmerje.Size = new Size(13, 15);
            tekstRazmerje.TabIndex = 5;
            tekstRazmerje.Text = "0";
            // 
            // tekstVisina
            // 
            tekstVisina.AutoSize = true;
            tekstVisina.Location = new Point(292, 372);
            tekstVisina.Name = "tekstVisina";
            tekstVisina.Size = new Size(13, 15);
            tekstVisina.TabIndex = 5;
            tekstVisina.Text = "0";
            // 
            // tekstEksponent
            // 
            tekstEksponent.AutoSize = true;
            tekstEksponent.Location = new Point(292, 299);
            tekstEksponent.Name = "tekstEksponent";
            tekstEksponent.Size = new Size(13, 15);
            tekstEksponent.TabIndex = 5;
            tekstEksponent.Text = "0";
            // 
            // tekstLakunarnost
            // 
            tekstLakunarnost.AutoSize = true;
            tekstLakunarnost.Location = new Point(292, 234);
            tekstLakunarnost.Name = "tekstLakunarnost";
            tekstLakunarnost.Size = new Size(13, 15);
            tekstLakunarnost.TabIndex = 5;
            tekstLakunarnost.Text = "0";
            // 
            // tekstVztrajnost
            // 
            tekstVztrajnost.AutoSize = true;
            tekstVztrajnost.Location = new Point(292, 168);
            tekstVztrajnost.Name = "tekstVztrajnost";
            tekstVztrajnost.Size = new Size(13, 15);
            tekstVztrajnost.TabIndex = 5;
            tekstVztrajnost.Text = "0";
            // 
            // tekstOktave
            // 
            tekstOktave.AutoSize = true;
            tekstOktave.Location = new Point(292, 103);
            tekstOktave.Name = "tekstOktave";
            tekstOktave.Size = new Size(13, 15);
            tekstOktave.TabIndex = 5;
            tekstOktave.Text = "0";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(vidnaRazdalja);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(gumbKonzola);
            groupBox2.Controls.Add(infoVidnaRazdalja);
            groupBox2.Controls.Add(drsnikHitrostMiske);
            groupBox2.Controls.Add(izborRezolucija);
            groupBox2.Location = new Point(12, 522);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(353, 150);
            groupBox2.TabIndex = 10;
            groupBox2.TabStop = false;
            groupBox2.Text = "Nastavitve";
            // 
            // vidnaRazdalja
            // 
            vidnaRazdalja.Location = new Point(283, 96);
            vidnaRazdalja.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            vidnaRazdalja.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            vidnaRazdalja.Name = "vidnaRazdalja";
            vidnaRazdalja.Size = new Size(46, 23);
            vidnaRazdalja.TabIndex = 10;
            vidnaRazdalja.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(196, 101);
            label10.Name = "label10";
            label10.Size = new Size(83, 15);
            label10.TabIndex = 9;
            label10.Text = "Vidna razdalja:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(177, 44);
            label9.Name = "label9";
            label9.Size = new Size(63, 15);
            label9.TabIndex = 9;
            label9.Text = "Rezolucija:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(11, 78);
            label8.Name = "label8";
            label8.Size = new Size(77, 15);
            label8.TabIndex = 9;
            label8.Text = "Hitrost miške";
            // 
            // infoVidnaRazdalja
            // 
            infoVidnaRazdalja.AutoSize = true;
            infoVidnaRazdalja.BackColor = Color.LightBlue;
            infoVidnaRazdalja.Font = new Font("Segoe UI", 6.75F);
            infoVidnaRazdalja.Location = new Point(335, 91);
            infoVidnaRazdalja.Name = "infoVidnaRazdalja";
            infoVidnaRazdalja.Size = new Size(9, 12);
            infoVidnaRazdalja.TabIndex = 6;
            infoVidnaRazdalja.Text = "?";
            infoVidnaRazdalja.MouseLeave += MiskaInformacijaDol;
            infoVidnaRazdalja.MouseHover += MiskaInformacijaGor;
            // 
            // drsnikHitrostMiske
            // 
            drsnikHitrostMiske.Location = new Point(11, 96);
            drsnikHitrostMiske.Maximum = 20;
            drsnikHitrostMiske.Minimum = 1;
            drsnikHitrostMiske.Name = "drsnikHitrostMiske";
            drsnikHitrostMiske.Size = new Size(150, 45);
            drsnikHitrostMiske.TabIndex = 2;
            drsnikHitrostMiske.Value = 10;
            drsnikHitrostMiske.Scroll += drsnikHitrostMiske_Scroll;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(infoZazeni);
            groupBox3.Controls.Add(gumbZazeni);
            groupBox3.Controls.Add(mapa);
            groupBox3.Location = new Point(371, 11);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(590, 661);
            groupBox3.TabIndex = 11;
            groupBox3.TabStop = false;
            groupBox3.Text = "Predogled";
            // 
            // infoZazeni
            // 
            infoZazeni.AutoSize = true;
            infoZazeni.BackColor = Color.LightBlue;
            infoZazeni.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 238);
            infoZazeni.Location = new Point(380, 609);
            infoZazeni.Name = "infoZazeni";
            infoZazeni.Size = new Size(30, 40);
            infoZazeni.TabIndex = 6;
            infoZazeni.Text = "?";
            infoZazeni.MouseLeave += MiskaInformacijaDol;
            infoZazeni.MouseHover += MiskaInformacijaGor;
            // 
            // GlavniMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(972, 681);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "GlavniMenu";
            Text = "Menu";
            ((System.ComponentModel.ISupportInitialize)mapa).EndInit();
            ((System.ComponentModel.ISupportInitialize)drsnikOktave).EndInit();
            ((System.ComponentModel.ISupportInitialize)drsnikVztrajnost).EndInit();
            ((System.ComponentModel.ISupportInitialize)drsnikLakunarnost).EndInit();
            ((System.ComponentModel.ISupportInitialize)drsnikEksponent).EndInit();
            ((System.ComponentModel.ISupportInitialize)drsnikVisina).EndInit();
            ((System.ComponentModel.ISupportInitialize)drsnikRazmerje).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)vidnaRazdalja).EndInit();
            ((System.ComponentModel.ISupportInitialize)drsnikHitrostMiske).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox mapa;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TrackBar drsnikOktave;
        private TrackBar drsnikVztrajnost;
        private TrackBar drsnikLakunarnost;
        private TrackBar drsnikEksponent;
        private TextBox boxSeme;
        private Label label5;
        private Label label6;
        private TrackBar drsnikVisina;
        private Button gumbZazeni;
        private Label label7;
        private TrackBar drsnikRazmerje;
        private ComboBox izborRezolucija;
        private System.Windows.Forms.Timer casZaPosodobitiSliko;
        private CheckBox gumbKonzola;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private TrackBar drsnikHitrostMiske;
        private Label label8;
        private Label label9;
        private GroupBox groupBox3;
        private Label tekstRazmerje;
        private Label tekstVisina;
        private Label tekstEksponent;
        private Label tekstLakunarnost;
        private Label tekstVztrajnost;
        private Label tekstOktave;
        private ToolTip dodatneInformacije;
        private Label infoRazmerje;
        private Label infoVisina;
        private Label infoEksponent;
        private Label infoLakunarnost;
        private Label infoVztrajnost;
        private Label infoOktave;
        private Label infoSeme;
        private NumericUpDown vidnaRazdalja;
        private Label label10;
        private Label infoZazeni;
        private Label infoVidnaRazdalja;
    }
}
