namespace SpeechApp;

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
        components = new System.ComponentModel.Container();
        lblDurum = new Label();
        tblAna = new TableLayoutPanel();
        grpTts = new GroupBox();
        rtbMetin = new RichTextBox();
        prgOkuma = new ProgressBar();
        pnlTtsButonlar = new FlowLayoutPanel();
        btnOku = new Button();
        btnDuraklat = new Button();
        btnDurdur = new Button();
        btnTemizleTts = new Button();
        btnWavKaydet = new Button();
        pnlTtsAyar2 = new FlowLayoutPanel();
        lblSesSeviyesi = new Label();
        trkSesSeviyesi = new TrackBar();
        lblSesSeviyesiDeger = new Label();
        pnlTtsAyar = new FlowLayoutPanel();
        lblSes = new Label();
        cmbSes = new ComboBox();
        lblHiz = new Label();
        trkHiz = new TrackBar();
        lblHizDeger = new Label();
        grpStt = new GroupBox();
        rtbTaninan = new RichTextBox();
        lblAraSonuc = new Label();
        lstSonuclar = new ListBox();
        lblSonuclar = new Label();
        pnlSttButonlar = new FlowLayoutPanel();
        btnDinle = new Button();
        btnDinlemeDurdur = new Button();
        btnTemizleStt = new Button();
        btnTxtKaydet = new Button();
        btnOkumayaAktar = new Button();
        pnlSttAyar2 = new FlowLayoutPanel();
        lblMikrofon = new Label();
        cmbMikrofon = new ComboBox();
        prgSeviye = new ProgressBar();
        pnlSttAyar = new FlowLayoutPanel();
        lblMotor = new Label();
        cmbMotor = new ComboBox();
        btnModelIndir = new Button();
        tmrIlerleme = new System.Windows.Forms.Timer(components);
        tblAna.SuspendLayout();
        grpTts.SuspendLayout();
        pnlTtsButonlar.SuspendLayout();
        pnlTtsAyar2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)trkSesSeviyesi).BeginInit();
        pnlTtsAyar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)trkHiz).BeginInit();
        grpStt.SuspendLayout();
        pnlSttButonlar.SuspendLayout();
        pnlSttAyar2.SuspendLayout();
        pnlSttAyar.SuspendLayout();
        SuspendLayout();
        //
        // lblDurum
        //
        lblDurum.BackColor = Color.Gray;
        lblDurum.Dock = DockStyle.Top;
        lblDurum.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblDurum.ForeColor = Color.White;
        lblDurum.Name = "lblDurum";
        lblDurum.Size = new Size(1100, 44);
        lblDurum.Text = "●  HAZIR — aktif işlem yok";
        lblDurum.TextAlign = ContentAlignment.MiddleCenter;
        //
        // tblAna
        //
        tblAna.ColumnCount = 2;
        tblAna.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblAna.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tblAna.Controls.Add(grpTts, 0, 0);
        tblAna.Controls.Add(grpStt, 1, 0);
        tblAna.Dock = DockStyle.Fill;
        tblAna.Name = "tblAna";
        tblAna.Padding = new Padding(6);
        tblAna.RowCount = 1;
        tblAna.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tblAna.TabIndex = 0;
        //
        // grpTts
        //
        grpTts.Controls.Add(rtbMetin);
        grpTts.Controls.Add(prgOkuma);
        grpTts.Controls.Add(pnlTtsButonlar);
        grpTts.Controls.Add(pnlTtsAyar2);
        grpTts.Controls.Add(pnlTtsAyar);
        grpTts.Dock = DockStyle.Fill;
        grpTts.Font = new Font("Segoe UI", 9F);
        grpTts.Name = "grpTts";
        grpTts.Padding = new Padding(8);
        grpTts.TabIndex = 0;
        grpTts.TabStop = false;
        grpTts.Text = "Text-to-Speech  (Metni Sesli Oku)";
        //
        // rtbMetin
        //
        rtbMetin.Dock = DockStyle.Fill;
        rtbMetin.Font = new Font("Segoe UI", 11F);
        rtbMetin.Name = "rtbMetin";
        rtbMetin.TabIndex = 2;
        rtbMetin.Text = "";
        //
        // prgOkuma
        //
        prgOkuma.Dock = DockStyle.Bottom;
        prgOkuma.Name = "prgOkuma";
        prgOkuma.Size = new Size(520, 10);
        //
        // pnlTtsButonlar
        //
        pnlTtsButonlar.Controls.Add(btnOku);
        pnlTtsButonlar.Controls.Add(btnDuraklat);
        pnlTtsButonlar.Controls.Add(btnDurdur);
        pnlTtsButonlar.Controls.Add(btnTemizleTts);
        pnlTtsButonlar.Controls.Add(btnWavKaydet);
        pnlTtsButonlar.Dock = DockStyle.Bottom;
        pnlTtsButonlar.Name = "pnlTtsButonlar";
        pnlTtsButonlar.Padding = new Padding(0, 6, 0, 0);
        pnlTtsButonlar.Size = new Size(520, 44);
        pnlTtsButonlar.TabIndex = 3;
        pnlTtsButonlar.WrapContents = false;
        //
        // btnOku
        //
        btnOku.BackColor = Color.FromArgb(25, 118, 210);
        btnOku.FlatStyle = FlatStyle.Flat;
        btnOku.ForeColor = Color.White;
        btnOku.Name = "btnOku";
        btnOku.Size = new Size(95, 32);
        btnOku.TabIndex = 0;
        btnOku.Text = "▶ Oku";
        btnOku.UseVisualStyleBackColor = false;
        btnOku.Click += btnOku_Click;
        //
        // btnDuraklat
        //
        btnDuraklat.Enabled = false;
        btnDuraklat.Name = "btnDuraklat";
        btnDuraklat.Size = new Size(95, 32);
        btnDuraklat.TabIndex = 1;
        btnDuraklat.Text = "⏸ Duraklat";
        btnDuraklat.UseVisualStyleBackColor = true;
        btnDuraklat.Click += btnDuraklat_Click;
        //
        // btnDurdur
        //
        btnDurdur.Enabled = false;
        btnDurdur.Name = "btnDurdur";
        btnDurdur.Size = new Size(85, 32);
        btnDurdur.TabIndex = 2;
        btnDurdur.Text = "■ Durdur";
        btnDurdur.UseVisualStyleBackColor = true;
        btnDurdur.Click += btnDurdur_Click;
        //
        // btnTemizleTts
        //
        btnTemizleTts.Name = "btnTemizleTts";
        btnTemizleTts.Size = new Size(80, 32);
        btnTemizleTts.TabIndex = 3;
        btnTemizleTts.Text = "Temizle";
        btnTemizleTts.UseVisualStyleBackColor = true;
        btnTemizleTts.Click += btnTemizleTts_Click;
        //
        // btnWavKaydet
        //
        btnWavKaydet.Name = "btnWavKaydet";
        btnWavKaydet.Size = new Size(100, 32);
        btnWavKaydet.TabIndex = 4;
        btnWavKaydet.Text = ".wav Kaydet";
        btnWavKaydet.UseVisualStyleBackColor = true;
        btnWavKaydet.Click += btnWavKaydet_Click;
        //
        // pnlTtsAyar2
        //
        pnlTtsAyar2.Controls.Add(lblSesSeviyesi);
        pnlTtsAyar2.Controls.Add(trkSesSeviyesi);
        pnlTtsAyar2.Controls.Add(lblSesSeviyesiDeger);
        pnlTtsAyar2.Dock = DockStyle.Top;
        pnlTtsAyar2.Name = "pnlTtsAyar2";
        pnlTtsAyar2.Size = new Size(520, 36);
        pnlTtsAyar2.TabIndex = 1;
        pnlTtsAyar2.WrapContents = false;
        //
        // lblSesSeviyesi
        //
        lblSesSeviyesi.AutoSize = true;
        lblSesSeviyesi.Margin = new Padding(3, 8, 0, 0);
        lblSesSeviyesi.Name = "lblSesSeviyesi";
        lblSesSeviyesi.Text = "Ses düzeyi:";
        //
        // trkSesSeviyesi
        //
        trkSesSeviyesi.AutoSize = false;
        trkSesSeviyesi.Maximum = 100;
        trkSesSeviyesi.Name = "trkSesSeviyesi";
        trkSesSeviyesi.Size = new Size(160, 30);
        trkSesSeviyesi.TabIndex = 0;
        trkSesSeviyesi.TickFrequency = 10;
        trkSesSeviyesi.Value = 100;
        trkSesSeviyesi.ValueChanged += trkSesSeviyesi_ValueChanged;
        //
        // lblSesSeviyesiDeger
        //
        lblSesSeviyesiDeger.AutoSize = true;
        lblSesSeviyesiDeger.Margin = new Padding(0, 8, 0, 0);
        lblSesSeviyesiDeger.Name = "lblSesSeviyesiDeger";
        lblSesSeviyesiDeger.Text = "%100";
        //
        // pnlTtsAyar
        //
        pnlTtsAyar.Controls.Add(lblSes);
        pnlTtsAyar.Controls.Add(cmbSes);
        pnlTtsAyar.Controls.Add(lblHiz);
        pnlTtsAyar.Controls.Add(trkHiz);
        pnlTtsAyar.Controls.Add(lblHizDeger);
        pnlTtsAyar.Dock = DockStyle.Top;
        pnlTtsAyar.Name = "pnlTtsAyar";
        pnlTtsAyar.Size = new Size(520, 36);
        pnlTtsAyar.TabIndex = 0;
        pnlTtsAyar.WrapContents = false;
        //
        // lblSes
        //
        lblSes.AutoSize = true;
        lblSes.Margin = new Padding(3, 8, 0, 0);
        lblSes.Name = "lblSes";
        lblSes.Text = "Ses:";
        //
        // cmbSes
        //
        cmbSes.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSes.Margin = new Padding(3, 4, 3, 0);
        cmbSes.Name = "cmbSes";
        cmbSes.Size = new Size(210, 23);
        cmbSes.TabIndex = 0;
        //
        // lblHiz
        //
        lblHiz.AutoSize = true;
        lblHiz.Margin = new Padding(8, 8, 0, 0);
        lblHiz.Name = "lblHiz";
        lblHiz.Text = "Hız:";
        //
        // trkHiz
        //
        trkHiz.AutoSize = false;
        trkHiz.Maximum = 20;
        trkHiz.Minimum = 5;
        trkHiz.Name = "trkHiz";
        trkHiz.Size = new Size(120, 30);
        trkHiz.TabIndex = 1;
        trkHiz.TickFrequency = 5;
        trkHiz.Value = 10;
        trkHiz.ValueChanged += trkHiz_ValueChanged;
        //
        // lblHizDeger
        //
        lblHizDeger.AutoSize = true;
        lblHizDeger.Margin = new Padding(0, 8, 0, 0);
        lblHizDeger.Name = "lblHizDeger";
        lblHizDeger.Text = "1.0x";
        //
        // grpStt
        //
        grpStt.Controls.Add(rtbTaninan);
        grpStt.Controls.Add(lblAraSonuc);
        grpStt.Controls.Add(lblSonuclar);
        grpStt.Controls.Add(lstSonuclar);
        grpStt.Controls.Add(pnlSttButonlar);
        grpStt.Controls.Add(pnlSttAyar2);
        grpStt.Controls.Add(pnlSttAyar);
        grpStt.Dock = DockStyle.Fill;
        grpStt.Name = "grpStt";
        grpStt.Padding = new Padding(8);
        grpStt.TabIndex = 1;
        grpStt.TabStop = false;
        grpStt.Text = "Speech-to-Text  (Konuşmayı Metne Çevir)";
        //
        // rtbTaninan
        //
        rtbTaninan.Dock = DockStyle.Fill;
        rtbTaninan.Font = new Font("Segoe UI", 11F);
        rtbTaninan.Name = "rtbTaninan";
        rtbTaninan.TabIndex = 2;
        rtbTaninan.Text = "";
        //
        // lblAraSonuc
        //
        lblAraSonuc.Dock = DockStyle.Bottom;
        lblAraSonuc.Font = new Font("Segoe UI", 9.75F, FontStyle.Italic);
        lblAraSonuc.ForeColor = Color.DimGray;
        lblAraSonuc.Name = "lblAraSonuc";
        lblAraSonuc.Size = new Size(520, 24);
        lblAraSonuc.Text = "";
        lblAraSonuc.TextAlign = ContentAlignment.MiddleLeft;
        //
        // lstSonuclar
        //
        lstSonuclar.Dock = DockStyle.Bottom;
        lstSonuclar.Font = new Font("Consolas", 9.5F);
        lstSonuclar.HorizontalScrollbar = true;
        lstSonuclar.IntegralHeight = false;
        lstSonuclar.Name = "lstSonuclar";
        lstSonuclar.Size = new Size(520, 110);
        lstSonuclar.TabIndex = 3;
        //
        // lblSonuclar
        //
        lblSonuclar.Dock = DockStyle.Bottom;
        lblSonuclar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblSonuclar.Name = "lblSonuclar";
        lblSonuclar.Size = new Size(520, 22);
        lblSonuclar.Text = "Tanıma sonuçları (zaman damgalı):";
        lblSonuclar.TextAlign = ContentAlignment.BottomLeft;
        //
        // pnlSttButonlar
        //
        pnlSttButonlar.Controls.Add(btnDinle);
        pnlSttButonlar.Controls.Add(btnDinlemeDurdur);
        pnlSttButonlar.Controls.Add(btnTemizleStt);
        pnlSttButonlar.Controls.Add(btnTxtKaydet);
        pnlSttButonlar.Controls.Add(btnOkumayaAktar);
        pnlSttButonlar.Dock = DockStyle.Bottom;
        pnlSttButonlar.Name = "pnlSttButonlar";
        pnlSttButonlar.Padding = new Padding(0, 6, 0, 0);
        pnlSttButonlar.Size = new Size(520, 44);
        pnlSttButonlar.TabIndex = 4;
        pnlSttButonlar.WrapContents = false;
        //
        // btnDinle
        //
        btnDinle.BackColor = Color.FromArgb(198, 40, 40);
        btnDinle.FlatStyle = FlatStyle.Flat;
        btnDinle.ForeColor = Color.White;
        btnDinle.Name = "btnDinle";
        btnDinle.Size = new Size(110, 32);
        btnDinle.TabIndex = 0;
        btnDinle.Text = "🎤 Dinle";
        btnDinle.UseVisualStyleBackColor = false;
        btnDinle.Click += btnDinle_Click;
        //
        // btnDinlemeDurdur
        //
        btnDinlemeDurdur.Enabled = false;
        btnDinlemeDurdur.Name = "btnDinlemeDurdur";
        btnDinlemeDurdur.Size = new Size(85, 32);
        btnDinlemeDurdur.TabIndex = 1;
        btnDinlemeDurdur.Text = "■ Durdur";
        btnDinlemeDurdur.UseVisualStyleBackColor = true;
        btnDinlemeDurdur.Click += btnDinlemeDurdur_Click;
        //
        // btnTemizleStt
        //
        btnTemizleStt.Name = "btnTemizleStt";
        btnTemizleStt.Size = new Size(75, 32);
        btnTemizleStt.TabIndex = 2;
        btnTemizleStt.Text = "Temizle";
        btnTemizleStt.UseVisualStyleBackColor = true;
        btnTemizleStt.Click += btnTemizleStt_Click;
        //
        // btnTxtKaydet
        //
        btnTxtKaydet.Name = "btnTxtKaydet";
        btnTxtKaydet.Size = new Size(90, 32);
        btnTxtKaydet.TabIndex = 3;
        btnTxtKaydet.Text = ".txt Kaydet";
        btnTxtKaydet.UseVisualStyleBackColor = true;
        btnTxtKaydet.Click += btnTxtKaydet_Click;
        //
        // btnOkumayaAktar
        //
        btnOkumayaAktar.Name = "btnOkumayaAktar";
        btnOkumayaAktar.Size = new Size(110, 32);
        btnOkumayaAktar.TabIndex = 4;
        btnOkumayaAktar.Text = "◀ Sesli Okut";
        btnOkumayaAktar.UseVisualStyleBackColor = true;
        btnOkumayaAktar.Click += btnOkumayaAktar_Click;
        //
        // pnlSttAyar2
        //
        pnlSttAyar2.Controls.Add(lblMikrofon);
        pnlSttAyar2.Controls.Add(cmbMikrofon);
        pnlSttAyar2.Controls.Add(prgSeviye);
        pnlSttAyar2.Dock = DockStyle.Top;
        pnlSttAyar2.Name = "pnlSttAyar2";
        pnlSttAyar2.Size = new Size(520, 36);
        pnlSttAyar2.TabIndex = 1;
        pnlSttAyar2.WrapContents = false;
        //
        // lblMikrofon
        //
        lblMikrofon.AutoSize = true;
        lblMikrofon.Margin = new Padding(3, 8, 0, 0);
        lblMikrofon.Name = "lblMikrofon";
        lblMikrofon.Text = "Mikrofon:";
        //
        // cmbMikrofon
        //
        cmbMikrofon.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMikrofon.Margin = new Padding(3, 4, 3, 0);
        cmbMikrofon.Name = "cmbMikrofon";
        cmbMikrofon.Size = new Size(230, 23);
        cmbMikrofon.TabIndex = 0;
        //
        // prgSeviye
        //
        prgSeviye.Margin = new Padding(8, 8, 3, 0);
        prgSeviye.Name = "prgSeviye";
        prgSeviye.Size = new Size(120, 16);
        //
        // pnlSttAyar
        //
        pnlSttAyar.Controls.Add(lblMotor);
        pnlSttAyar.Controls.Add(cmbMotor);
        pnlSttAyar.Controls.Add(btnModelIndir);
        pnlSttAyar.Dock = DockStyle.Top;
        pnlSttAyar.Name = "pnlSttAyar";
        pnlSttAyar.Size = new Size(520, 36);
        pnlSttAyar.TabIndex = 0;
        pnlSttAyar.WrapContents = false;
        //
        // lblMotor
        //
        lblMotor.AutoSize = true;
        lblMotor.Margin = new Padding(3, 8, 0, 0);
        lblMotor.Name = "lblMotor";
        lblMotor.Text = "Tanıma motoru:";
        //
        // cmbMotor
        //
        cmbMotor.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMotor.Margin = new Padding(3, 4, 3, 0);
        cmbMotor.Name = "cmbMotor";
        cmbMotor.Size = new Size(285, 23);
        cmbMotor.TabIndex = 0;
        cmbMotor.SelectedIndexChanged += cmbMotor_SelectedIndexChanged;
        //
        // btnModelIndir
        //
        btnModelIndir.Margin = new Padding(3, 2, 3, 0);
        btnModelIndir.Name = "btnModelIndir";
        btnModelIndir.Size = new Size(100, 28);
        btnModelIndir.TabIndex = 1;
        btnModelIndir.Text = "Modeli İndir";
        btnModelIndir.UseVisualStyleBackColor = true;
        btnModelIndir.Click += btnModelIndir_Click;
        //
        // tmrIlerleme
        //
        tmrIlerleme.Interval = 200;
        tmrIlerleme.Tick += tmrIlerleme_Tick;
        //
        // AnaForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 640);
        Controls.Add(tblAna);
        Controls.Add(lblDurum);
        MinimumSize = new Size(1000, 520);
        Name = "AnaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Speech App - Text-to-Speech / Speech-to-Text - YMH219";
        FormClosing += AnaForm_FormClosing;
        Load += AnaForm_Load;
        tblAna.ResumeLayout(false);
        grpTts.ResumeLayout(false);
        pnlTtsButonlar.ResumeLayout(false);
        pnlTtsAyar2.ResumeLayout(false);
        pnlTtsAyar2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)trkSesSeviyesi).EndInit();
        pnlTtsAyar.ResumeLayout(false);
        pnlTtsAyar.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)trkHiz).EndInit();
        grpStt.ResumeLayout(false);
        pnlSttButonlar.ResumeLayout(false);
        pnlSttAyar2.ResumeLayout(false);
        pnlSttAyar2.PerformLayout();
        pnlSttAyar.ResumeLayout(false);
        pnlSttAyar.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private Label lblDurum;
    private TableLayoutPanel tblAna;
    private GroupBox grpTts;
    private RichTextBox rtbMetin;
    private ProgressBar prgOkuma;
    private FlowLayoutPanel pnlTtsButonlar;
    private Button btnOku;
    private Button btnDuraklat;
    private Button btnDurdur;
    private Button btnTemizleTts;
    private Button btnWavKaydet;
    private FlowLayoutPanel pnlTtsAyar2;
    private Label lblSesSeviyesi;
    private TrackBar trkSesSeviyesi;
    private Label lblSesSeviyesiDeger;
    private FlowLayoutPanel pnlTtsAyar;
    private Label lblSes;
    private ComboBox cmbSes;
    private Label lblHiz;
    private TrackBar trkHiz;
    private Label lblHizDeger;
    private GroupBox grpStt;
    private RichTextBox rtbTaninan;
    private Label lblAraSonuc;
    private ListBox lstSonuclar;
    private Label lblSonuclar;
    private FlowLayoutPanel pnlSttButonlar;
    private Button btnDinle;
    private Button btnDinlemeDurdur;
    private Button btnTemizleStt;
    private Button btnTxtKaydet;
    private Button btnOkumayaAktar;
    private FlowLayoutPanel pnlSttAyar2;
    private Label lblMikrofon;
    private ComboBox cmbMikrofon;
    private ProgressBar prgSeviye;
    private FlowLayoutPanel pnlSttAyar;
    private Label lblMotor;
    private ComboBox cmbMotor;
    private Button btnModelIndir;
    private System.Windows.Forms.Timer tmrIlerleme;
}
