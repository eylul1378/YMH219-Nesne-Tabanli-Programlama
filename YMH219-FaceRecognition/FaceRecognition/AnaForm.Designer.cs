namespace FaceRecognition;

partial class AnaForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        lblDurum = new Label();
        pbGoruntu = new PictureBox();
        pnlSag = new Panel();
        grpSecenekler = new GroupBox();
        nudEsik = new NumericUpDown();
        lblEsik = new Label();
        chkAlgilananKaydet = new CheckBox();
        grpEgitim = new GroupBox();
        lblModelDurum = new Label();
        btnKlasorAc = new Button();
        btnEtiketSil = new Button();
        btnEgit = new Button();
        lstEtiketler = new ListBox();
        btnOrnekEkle = new Button();
        txtEtiket = new TextBox();
        lblEtiket = new Label();
        grpSonuc = new GroupBox();
        lstYuzler = new ListBox();
        lblSonuc = new Label();
        grpKaynak = new GroupBox();
        btnGorselSec = new Button();
        btnKameraDurdur = new Button();
        btnKameraBaslat = new Button();
        nudKamera = new NumericUpDown();
        lblKamera = new Label();
        lblUyari = new Label();
        ((System.ComponentModel.ISupportInitialize)pbGoruntu).BeginInit();
        pnlSag.SuspendLayout();
        grpSecenekler.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudEsik).BeginInit();
        grpEgitim.SuspendLayout();
        grpSonuc.SuspendLayout();
        grpKaynak.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudKamera).BeginInit();
        SuspendLayout();
        //
        // lblDurum
        //
        lblDurum.BackColor = Color.Gray;
        lblDurum.Dock = DockStyle.Top;
        lblDurum.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblDurum.ForeColor = Color.White;
        lblDurum.Name = "lblDurum";
        lblDurum.Size = new Size(1160, 36);
        lblDurum.Text = "Kamerayı başlatın veya bir görüntü seçin";
        lblDurum.TextAlign = ContentAlignment.MiddleCenter;
        //
        // pbGoruntu
        //
        pbGoruntu.BackColor = Color.FromArgb(30, 30, 30);
        pbGoruntu.Dock = DockStyle.Fill;
        pbGoruntu.Name = "pbGoruntu";
        pbGoruntu.SizeMode = PictureBoxSizeMode.Zoom;
        pbGoruntu.TabStop = false;
        //
        // pnlSag
        //
        pnlSag.AutoScroll = true;
        pnlSag.Controls.Add(grpSecenekler);
        pnlSag.Controls.Add(grpEgitim);
        pnlSag.Controls.Add(grpSonuc);
        pnlSag.Controls.Add(grpKaynak);
        pnlSag.Dock = DockStyle.Right;
        pnlSag.Name = "pnlSag";
        pnlSag.Padding = new Padding(6);
        pnlSag.Size = new Size(350, 600);
        pnlSag.TabIndex = 0;
        //
        // grpKaynak
        //
        grpKaynak.Controls.Add(btnGorselSec);
        grpKaynak.Controls.Add(btnKameraDurdur);
        grpKaynak.Controls.Add(btnKameraBaslat);
        grpKaynak.Controls.Add(nudKamera);
        grpKaynak.Controls.Add(lblKamera);
        grpKaynak.Dock = DockStyle.Top;
        grpKaynak.Name = "grpKaynak";
        grpKaynak.Size = new Size(338, 96);
        grpKaynak.TabIndex = 0;
        grpKaynak.TabStop = false;
        grpKaynak.Text = "Görüntü Kaynağı";
        //
        // lblKamera
        //
        lblKamera.AutoSize = true;
        lblKamera.Location = new Point(10, 28);
        lblKamera.Name = "lblKamera";
        lblKamera.Text = "Kamera no:";
        //
        // nudKamera
        //
        nudKamera.Location = new Point(85, 24);
        nudKamera.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
        nudKamera.Name = "nudKamera";
        nudKamera.Size = new Size(45, 23);
        nudKamera.TabIndex = 0;
        //
        // btnKameraBaslat
        //
        btnKameraBaslat.BackColor = Color.FromArgb(46, 125, 50);
        btnKameraBaslat.FlatStyle = FlatStyle.Flat;
        btnKameraBaslat.ForeColor = Color.White;
        btnKameraBaslat.Location = new Point(140, 21);
        btnKameraBaslat.Name = "btnKameraBaslat";
        btnKameraBaslat.Size = new Size(90, 28);
        btnKameraBaslat.TabIndex = 1;
        btnKameraBaslat.Text = "▶ &Kamera";
        btnKameraBaslat.UseVisualStyleBackColor = false;
        btnKameraBaslat.Click += btnKameraBaslat_Click;
        //
        // btnKameraDurdur
        //
        btnKameraDurdur.Enabled = false;
        btnKameraDurdur.Location = new Point(236, 21);
        btnKameraDurdur.Name = "btnKameraDurdur";
        btnKameraDurdur.Size = new Size(90, 28);
        btnKameraDurdur.TabIndex = 2;
        btnKameraDurdur.Text = "■ Durdur";
        btnKameraDurdur.UseVisualStyleBackColor = true;
        btnKameraDurdur.Click += btnKameraDurdur_Click;
        //
        // btnGorselSec
        //
        btnGorselSec.Location = new Point(10, 56);
        btnGorselSec.Name = "btnGorselSec";
        btnGorselSec.Size = new Size(316, 28);
        btnGorselSec.TabIndex = 3;
        btnGorselSec.Text = "🖼  Görüntü &Seç (dosyadan)";
        btnGorselSec.UseVisualStyleBackColor = true;
        btnGorselSec.Click += btnGorselSec_Click;
        //
        // grpSonuc
        //
        grpSonuc.Controls.Add(lstYuzler);
        grpSonuc.Controls.Add(lblSonuc);
        grpSonuc.Dock = DockStyle.Top;
        grpSonuc.Name = "grpSonuc";
        grpSonuc.Padding = new Padding(8);
        grpSonuc.Size = new Size(338, 150);
        grpSonuc.TabIndex = 1;
        grpSonuc.TabStop = false;
        grpSonuc.Text = "Algılama / Tanıma Sonucu";
        //
        // lblSonuc
        //
        lblSonuc.Dock = DockStyle.Top;
        lblSonuc.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblSonuc.Name = "lblSonuc";
        lblSonuc.Size = new Size(322, 30);
        lblSonuc.Text = "Yüz algılanmadı";
        lblSonuc.TextAlign = ContentAlignment.MiddleLeft;
        //
        // lstYuzler
        //
        lstYuzler.Dock = DockStyle.Fill;
        lstYuzler.IntegralHeight = false;
        lstYuzler.Name = "lstYuzler";
        lstYuzler.TabStop = false;
        //
        // grpEgitim
        //
        grpEgitim.Controls.Add(lblModelDurum);
        grpEgitim.Controls.Add(btnKlasorAc);
        grpEgitim.Controls.Add(btnEtiketSil);
        grpEgitim.Controls.Add(btnEgit);
        grpEgitim.Controls.Add(lstEtiketler);
        grpEgitim.Controls.Add(btnOrnekEkle);
        grpEgitim.Controls.Add(txtEtiket);
        grpEgitim.Controls.Add(lblEtiket);
        grpEgitim.Dock = DockStyle.Top;
        grpEgitim.Name = "grpEgitim";
        grpEgitim.Size = new Size(338, 250);
        grpEgitim.TabIndex = 2;
        grpEgitim.TabStop = false;
        grpEgitim.Text = "Eğitim (gönüllü yerel örnekler)";
        //
        // lblEtiket
        //
        lblEtiket.AutoSize = true;
        lblEtiket.Location = new Point(10, 28);
        lblEtiket.Name = "lblEtiket";
        lblEtiket.Text = "Etiket / kişi:";
        //
        // txtEtiket
        //
        txtEtiket.Location = new Point(90, 24);
        txtEtiket.MaxLength = 30;
        txtEtiket.Name = "txtEtiket";
        txtEtiket.PlaceholderText = "ör. Eylul";
        txtEtiket.Size = new Size(110, 23);
        txtEtiket.TabIndex = 0;
        //
        // btnOrnekEkle
        //
        btnOrnekEkle.Location = new Point(206, 22);
        btnOrnekEkle.Name = "btnOrnekEkle";
        btnOrnekEkle.Size = new Size(120, 27);
        btnOrnekEkle.TabIndex = 1;
        btnOrnekEkle.Text = "+ Örnek &Ekle";
        btnOrnekEkle.UseVisualStyleBackColor = true;
        btnOrnekEkle.Click += btnOrnekEkle_Click;
        //
        // lstEtiketler
        //
        lstEtiketler.IntegralHeight = false;
        lstEtiketler.Location = new Point(10, 56);
        lstEtiketler.Name = "lstEtiketler";
        lstEtiketler.Size = new Size(316, 96);
        lstEtiketler.TabIndex = 2;
        //
        // btnEgit
        //
        btnEgit.BackColor = Color.FromArgb(25, 118, 210);
        btnEgit.FlatStyle = FlatStyle.Flat;
        btnEgit.ForeColor = Color.White;
        btnEgit.Location = new Point(10, 158);
        btnEgit.Name = "btnEgit";
        btnEgit.Size = new Size(120, 28);
        btnEgit.TabIndex = 3;
        btnEgit.Text = "&Modeli Eğit";
        btnEgit.UseVisualStyleBackColor = false;
        btnEgit.Click += btnEgit_Click;
        //
        // btnEtiketSil
        //
        btnEtiketSil.Location = new Point(136, 158);
        btnEtiketSil.Name = "btnEtiketSil";
        btnEtiketSil.Size = new Size(80, 28);
        btnEtiketSil.TabIndex = 4;
        btnEtiketSil.Text = "Etiketi Sil";
        btnEtiketSil.UseVisualStyleBackColor = true;
        btnEtiketSil.Click += btnEtiketSil_Click;
        //
        // btnKlasorAc
        //
        btnKlasorAc.Location = new Point(222, 158);
        btnKlasorAc.Name = "btnKlasorAc";
        btnKlasorAc.Size = new Size(104, 28);
        btnKlasorAc.TabIndex = 5;
        btnKlasorAc.Text = "Klasörü Aç";
        btnKlasorAc.UseVisualStyleBackColor = true;
        btnKlasorAc.Click += btnKlasorAc_Click;
        //
        // lblModelDurum
        //
        lblModelDurum.ForeColor = Color.DimGray;
        lblModelDurum.Location = new Point(10, 192);
        lblModelDurum.Name = "lblModelDurum";
        lblModelDurum.Size = new Size(316, 50);
        lblModelDurum.Text = "Model eğitilmedi.";
        //
        // grpSecenekler
        //
        grpSecenekler.Controls.Add(nudEsik);
        grpSecenekler.Controls.Add(lblEsik);
        grpSecenekler.Controls.Add(chkAlgilananKaydet);
        grpSecenekler.Dock = DockStyle.Top;
        grpSecenekler.Name = "grpSecenekler";
        grpSecenekler.Size = new Size(338, 80);
        grpSecenekler.TabIndex = 3;
        grpSecenekler.TabStop = false;
        grpSecenekler.Text = "Seçenekler";
        //
        // chkAlgilananKaydet
        //
        chkAlgilananKaydet.AutoSize = true;
        chkAlgilananKaydet.Location = new Point(10, 24);
        chkAlgilananKaydet.Name = "chkAlgilananKaydet";
        chkAlgilananKaydet.TabIndex = 0;
        chkAlgilananKaydet.Text = "Algılanan yüzleri ayrı klasöre kaydet";
        chkAlgilananKaydet.UseVisualStyleBackColor = true;
        //
        // lblEsik
        //
        lblEsik.AutoSize = true;
        lblEsik.Location = new Point(10, 51);
        lblEsik.Name = "lblEsik";
        lblEsik.Text = "Tanıma eşiği (LBPH mesafe):";
        //
        // nudEsik
        //
        nudEsik.Location = new Point(190, 48);
        nudEsik.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
        nudEsik.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
        nudEsik.Name = "nudEsik";
        nudEsik.Size = new Size(60, 23);
        nudEsik.TabIndex = 1;
        nudEsik.Value = new decimal(new int[] { 85, 0, 0, 0 });
        nudEsik.ValueChanged += nudEsik_ValueChanged;
        //
        // lblUyari
        //
        lblUyari.BackColor = Color.FromArgb(255, 248, 225);
        lblUyari.Dock = DockStyle.Bottom;
        lblUyari.ForeColor = Color.FromArgb(120, 80, 0);
        lblUyari.Name = "lblUyari";
        lblUyari.Size = new Size(1160, 26);
        lblUyari.Text = "  ⚠ Yalnızca ders kapsamında, yerel ve gönüllü örneklerle kullanılmalıdır. İzinsiz kimlik takibi, gizli görüntü toplama veya uzaktan izleme yapılmamalıdır.";
        lblUyari.TextAlign = ContentAlignment.MiddleLeft;
        //
        // AnaForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1160, 662);
        Controls.Add(pbGoruntu);
        Controls.Add(pnlSag);
        Controls.Add(lblUyari);
        Controls.Add(lblDurum);
        MinimumSize = new Size(900, 600);
        Name = "AnaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "OpenCV ile Yüz Algılama ve Tanıma - YMH219";
        FormClosing += AnaForm_FormClosing;
        Load += AnaForm_Load;
        ((System.ComponentModel.ISupportInitialize)pbGoruntu).EndInit();
        pnlSag.ResumeLayout(false);
        grpSecenekler.ResumeLayout(false);
        grpSecenekler.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudEsik).EndInit();
        grpEgitim.ResumeLayout(false);
        grpEgitim.PerformLayout();
        grpSonuc.ResumeLayout(false);
        grpKaynak.ResumeLayout(false);
        grpKaynak.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudKamera).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Label lblDurum;
    private PictureBox pbGoruntu;
    private Panel pnlSag;
    private GroupBox grpKaynak;
    private Label lblKamera;
    private NumericUpDown nudKamera;
    private Button btnKameraBaslat;
    private Button btnKameraDurdur;
    private Button btnGorselSec;
    private GroupBox grpSonuc;
    private Label lblSonuc;
    private ListBox lstYuzler;
    private GroupBox grpEgitim;
    private Label lblEtiket;
    private TextBox txtEtiket;
    private Button btnOrnekEkle;
    private ListBox lstEtiketler;
    private Button btnEgit;
    private Button btnEtiketSil;
    private Button btnKlasorAc;
    private Label lblModelDurum;
    private GroupBox grpSecenekler;
    private CheckBox chkAlgilananKaydet;
    private Label lblEsik;
    private NumericUpDown nudEsik;
    private Label lblUyari;
}
