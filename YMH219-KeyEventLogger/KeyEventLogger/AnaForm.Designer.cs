namespace KeyEventLogger;

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
        pnlUst = new FlowLayoutPanel();
        lblDurum = new Label();
        btnBaslat = new Button();
        btnDurdur = new Button();
        btnTemizle = new Button();
        btnKaydet = new Button();
        lblOlaylar = new Label();
        chkKeyDown = new CheckBox();
        chkKeyUp = new CheckBox();
        chkKeyPress = new CheckBox();
        splitContainer1 = new SplitContainer();
        lstKayitlar = new ListBox();
        lblListeBaslik = new Label();
        grpDeneme = new GroupBox();
        txtDeneme = new TextBox();
        grpFiltre = new GroupBox();
        btnFiltreKaldir = new Button();
        btnFiltrele = new Button();
        dtpBitis = new DateTimePicker();
        dtpBaslangic = new DateTimePicker();
        lblBitis = new Label();
        lblBaslangic = new Label();
        grpIstatistik = new GroupBox();
        lstEnSik = new ListBox();
        lblEnSik = new Label();
        lblToplam = new Label();
        lblOdak = new Label();
        lblUyari = new Label();
        statusStrip1 = new StatusStrip();
        lblDurumCubugu = new ToolStripStatusLabel();
        tmrGosterge = new System.Windows.Forms.Timer(components);
        pnlUst.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        grpDeneme.SuspendLayout();
        grpFiltre.SuspendLayout();
        grpIstatistik.SuspendLayout();
        statusStrip1.SuspendLayout();
        SuspendLayout();
        //
        // pnlUst
        //
        pnlUst.Controls.Add(lblDurum);
        pnlUst.Controls.Add(btnBaslat);
        pnlUst.Controls.Add(btnDurdur);
        pnlUst.Controls.Add(btnTemizle);
        pnlUst.Controls.Add(btnKaydet);
        pnlUst.Controls.Add(lblOlaylar);
        pnlUst.Controls.Add(chkKeyDown);
        pnlUst.Controls.Add(chkKeyUp);
        pnlUst.Controls.Add(chkKeyPress);
        pnlUst.Dock = DockStyle.Top;
        pnlUst.Name = "pnlUst";
        pnlUst.Padding = new Padding(6, 8, 6, 4);
        pnlUst.Size = new Size(960, 52);
        pnlUst.TabIndex = 0;
        pnlUst.WrapContents = false;
        //
        // lblDurum
        //
        lblDurum.BackColor = Color.Gray;
        lblDurum.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblDurum.ForeColor = Color.White;
        lblDurum.Margin = new Padding(3, 3, 10, 3);
        lblDurum.Name = "lblDurum";
        lblDurum.Size = new Size(180, 30);
        lblDurum.Text = "■  KAYIT DURDURULDU";
        lblDurum.TextAlign = ContentAlignment.MiddleCenter;
        //
        // btnBaslat
        //
        btnBaslat.BackColor = Color.FromArgb(46, 125, 50);
        btnBaslat.FlatStyle = FlatStyle.Flat;
        btnBaslat.ForeColor = Color.White;
        btnBaslat.Name = "btnBaslat";
        btnBaslat.Size = new Size(90, 30);
        btnBaslat.TabIndex = 0;
        btnBaslat.Text = "▶ Başlat";
        btnBaslat.UseVisualStyleBackColor = false;
        btnBaslat.Click += btnBaslat_Click;
        //
        // btnDurdur
        //
        btnDurdur.BackColor = Color.FromArgb(198, 40, 40);
        btnDurdur.Enabled = false;
        btnDurdur.FlatStyle = FlatStyle.Flat;
        btnDurdur.ForeColor = Color.White;
        btnDurdur.Name = "btnDurdur";
        btnDurdur.Size = new Size(90, 30);
        btnDurdur.TabIndex = 1;
        btnDurdur.Text = "■ Durdur";
        btnDurdur.UseVisualStyleBackColor = false;
        btnDurdur.Click += btnDurdur_Click;
        //
        // btnTemizle
        //
        btnTemizle.Name = "btnTemizle";
        btnTemizle.Size = new Size(85, 30);
        btnTemizle.TabIndex = 2;
        btnTemizle.Text = "Temizle";
        btnTemizle.UseVisualStyleBackColor = true;
        btnTemizle.Click += btnTemizle_Click;
        //
        // btnKaydet
        //
        btnKaydet.Name = "btnKaydet";
        btnKaydet.Size = new Size(120, 30);
        btnKaydet.TabIndex = 3;
        btnKaydet.Text = ".txt Olarak Kaydet";
        btnKaydet.UseVisualStyleBackColor = true;
        btnKaydet.Click += btnKaydet_Click;
        //
        // lblOlaylar
        //
        lblOlaylar.AutoSize = true;
        lblOlaylar.Margin = new Padding(10, 10, 0, 0);
        lblOlaylar.Name = "lblOlaylar";
        lblOlaylar.Text = "Olaylar:";
        //
        // chkKeyDown
        //
        chkKeyDown.AutoSize = true;
        chkKeyDown.Checked = true;
        chkKeyDown.CheckState = CheckState.Checked;
        chkKeyDown.Margin = new Padding(3, 9, 3, 3);
        chkKeyDown.Name = "chkKeyDown";
        chkKeyDown.TabIndex = 4;
        chkKeyDown.Text = "KeyDown";
        chkKeyDown.UseVisualStyleBackColor = true;
        //
        // chkKeyUp
        //
        chkKeyUp.AutoSize = true;
        chkKeyUp.Margin = new Padding(3, 9, 3, 3);
        chkKeyUp.Name = "chkKeyUp";
        chkKeyUp.TabIndex = 5;
        chkKeyUp.Text = "KeyUp";
        chkKeyUp.UseVisualStyleBackColor = true;
        //
        // chkKeyPress
        //
        chkKeyPress.AutoSize = true;
        chkKeyPress.Margin = new Padding(3, 9, 3, 3);
        chkKeyPress.Name = "chkKeyPress";
        chkKeyPress.TabIndex = 6;
        chkKeyPress.Text = "KeyPress";
        chkKeyPress.UseVisualStyleBackColor = true;
        //
        // splitContainer1
        //
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Name = "splitContainer1";
        //
        // splitContainer1.Panel1
        //
        splitContainer1.Panel1.Controls.Add(lstKayitlar);
        splitContainer1.Panel1.Controls.Add(lblListeBaslik);
        splitContainer1.Panel1.Padding = new Padding(8, 4, 4, 8);
        //
        // splitContainer1.Panel2
        //
        splitContainer1.Panel2.Controls.Add(grpDeneme);
        splitContainer1.Panel2.Controls.Add(grpFiltre);
        splitContainer1.Panel2.Controls.Add(grpIstatistik);
        splitContainer1.Panel2.Padding = new Padding(4, 4, 8, 8);
        splitContainer1.Size = new Size(960, 520);
        splitContainer1.SplitterDistance = 580;
        splitContainer1.TabIndex = 1;
        splitContainer1.TabStop = false;
        //
        // lstKayitlar
        //
        lstKayitlar.Dock = DockStyle.Fill;
        lstKayitlar.Font = new Font("Consolas", 10F);
        lstKayitlar.HorizontalScrollbar = true;
        lstKayitlar.IntegralHeight = false;
        lstKayitlar.Name = "lstKayitlar";
        lstKayitlar.TabIndex = 0;
        //
        // lblListeBaslik
        //
        lblListeBaslik.Dock = DockStyle.Top;
        lblListeBaslik.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblListeBaslik.Name = "lblListeBaslik";
        lblListeBaslik.Size = new Size(568, 24);
        lblListeBaslik.Text = "Olay Kayıtları (Tarih / Saat  -  Olay  -  Tuş)";
        lblListeBaslik.TextAlign = ContentAlignment.MiddleLeft;
        //
        // grpDeneme
        //
        grpDeneme.Controls.Add(txtDeneme);
        grpDeneme.Dock = DockStyle.Fill;
        grpDeneme.Name = "grpDeneme";
        grpDeneme.Padding = new Padding(6);
        grpDeneme.TabIndex = 2;
        grpDeneme.TabStop = false;
        grpDeneme.Text = "Deneme Alanı (buraya yazarak tuş olaylarını izleyin)";
        //
        // txtDeneme
        //
        txtDeneme.Dock = DockStyle.Fill;
        txtDeneme.Font = new Font("Segoe UI", 11F);
        txtDeneme.Multiline = true;
        txtDeneme.Name = "txtDeneme";
        txtDeneme.ScrollBars = ScrollBars.Vertical;
        txtDeneme.TabIndex = 0;
        //
        // grpFiltre
        //
        grpFiltre.Controls.Add(btnFiltreKaldir);
        grpFiltre.Controls.Add(btnFiltrele);
        grpFiltre.Controls.Add(dtpBitis);
        grpFiltre.Controls.Add(dtpBaslangic);
        grpFiltre.Controls.Add(lblBitis);
        grpFiltre.Controls.Add(lblBaslangic);
        grpFiltre.Dock = DockStyle.Top;
        grpFiltre.Name = "grpFiltre";
        grpFiltre.Size = new Size(364, 120);
        grpFiltre.TabIndex = 1;
        grpFiltre.TabStop = false;
        grpFiltre.Text = "Tarih / Saat Aralığına Göre Filtrele";
        //
        // lblBaslangic
        //
        lblBaslangic.AutoSize = true;
        lblBaslangic.Location = new Point(10, 27);
        lblBaslangic.Name = "lblBaslangic";
        lblBaslangic.Text = "Başlangıç:";
        //
        // lblBitis
        //
        lblBitis.AutoSize = true;
        lblBitis.Location = new Point(10, 57);
        lblBitis.Name = "lblBitis";
        lblBitis.Text = "Bitiş:";
        //
        // dtpBaslangic
        //
        dtpBaslangic.CustomFormat = "dd.MM.yyyy HH:mm:ss";
        dtpBaslangic.Format = DateTimePickerFormat.Custom;
        dtpBaslangic.Location = new Point(80, 23);
        dtpBaslangic.Name = "dtpBaslangic";
        dtpBaslangic.ShowUpDown = true;
        dtpBaslangic.Size = new Size(170, 23);
        dtpBaslangic.TabIndex = 0;
        //
        // dtpBitis
        //
        dtpBitis.CustomFormat = "dd.MM.yyyy HH:mm:ss";
        dtpBitis.Format = DateTimePickerFormat.Custom;
        dtpBitis.Location = new Point(80, 53);
        dtpBitis.Name = "dtpBitis";
        dtpBitis.ShowUpDown = true;
        dtpBitis.Size = new Size(170, 23);
        dtpBitis.TabIndex = 1;
        //
        // btnFiltrele
        //
        btnFiltrele.Location = new Point(80, 84);
        btnFiltrele.Name = "btnFiltrele";
        btnFiltrele.Size = new Size(80, 27);
        btnFiltrele.TabIndex = 2;
        btnFiltrele.Text = "Filtrele";
        btnFiltrele.UseVisualStyleBackColor = true;
        btnFiltrele.Click += btnFiltrele_Click;
        //
        // btnFiltreKaldir
        //
        btnFiltreKaldir.Enabled = false;
        btnFiltreKaldir.Location = new Point(166, 84);
        btnFiltreKaldir.Name = "btnFiltreKaldir";
        btnFiltreKaldir.Size = new Size(110, 27);
        btnFiltreKaldir.TabIndex = 3;
        btnFiltreKaldir.Text = "Filtreyi Kaldır";
        btnFiltreKaldir.UseVisualStyleBackColor = true;
        btnFiltreKaldir.Click += btnFiltreKaldir_Click;
        //
        // grpIstatistik
        //
        grpIstatistik.Controls.Add(lstEnSik);
        grpIstatistik.Controls.Add(lblEnSik);
        grpIstatistik.Controls.Add(lblToplam);
        grpIstatistik.Dock = DockStyle.Top;
        grpIstatistik.Name = "grpIstatistik";
        grpIstatistik.Padding = new Padding(8, 4, 8, 8);
        grpIstatistik.Size = new Size(364, 165);
        grpIstatistik.TabIndex = 0;
        grpIstatistik.TabStop = false;
        grpIstatistik.Text = "İstatistik";
        //
        // lstEnSik
        //
        lstEnSik.Dock = DockStyle.Fill;
        lstEnSik.Font = new Font("Consolas", 9.75F);
        lstEnSik.IntegralHeight = false;
        lstEnSik.Name = "lstEnSik";
        lstEnSik.TabStop = false;
        //
        // lblEnSik
        //
        lblEnSik.Dock = DockStyle.Top;
        lblEnSik.Name = "lblEnSik";
        lblEnSik.Size = new Size(348, 20);
        lblEnSik.Text = "En sık kullanılan 5 tuş:";
        //
        // lblToplam
        //
        lblToplam.Dock = DockStyle.Top;
        lblToplam.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblToplam.Name = "lblToplam";
        lblToplam.Size = new Size(348, 24);
        lblToplam.Text = "Toplam tuş basma: 0";
        //
        // lblOdak
        //
        lblOdak.BackColor = Color.FromArgb(232, 245, 233);
        lblOdak.Dock = DockStyle.Bottom;
        lblOdak.ForeColor = Color.DarkGreen;
        lblOdak.Name = "lblOdak";
        lblOdak.Size = new Size(960, 22);
        lblOdak.Text = "  Pencere odakta: tuş olayları bu form üzerinden alınabilir.";
        lblOdak.TextAlign = ContentAlignment.MiddleLeft;
        //
        // lblUyari
        //
        lblUyari.BackColor = Color.FromArgb(255, 248, 225);
        lblUyari.Dock = DockStyle.Bottom;
        lblUyari.ForeColor = Color.FromArgb(120, 80, 0);
        lblUyari.Name = "lblUyari";
        lblUyari.Size = new Size(960, 36);
        lblUyari.Text = "  ⚠ Eğitsel uygulamadır: Yalnızca bu pencere odaktayken olay yakalar. Global hook, arka plan çalışması, otomatik başlatma,\r\n     parola yakalama, başka uygulamalardan veri toplama veya ağ üzerinden gönderme yapılmaz. Kayıtlar yalnızca kullanıcı isterse yerel dosyaya yazılır.";
        lblUyari.TextAlign = ContentAlignment.MiddleLeft;
        //
        // statusStrip1
        //
        statusStrip1.Items.AddRange(new ToolStripItem[] { lblDurumCubugu });
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new Size(960, 22);
        statusStrip1.TabIndex = 3;
        //
        // lblDurumCubugu
        //
        lblDurumCubugu.Name = "lblDurumCubugu";
        lblDurumCubugu.Text = "Hazır. Kayda başlamak için \"Başlat\" butonuna basın.";
        //
        // tmrGosterge
        //
        tmrGosterge.Interval = 500;
        tmrGosterge.Tick += tmrGosterge_Tick;
        //
        // AnaForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(960, 652);
        Controls.Add(splitContainer1);
        Controls.Add(lblOdak);
        Controls.Add(lblUyari);
        Controls.Add(pnlUst);
        Controls.Add(statusStrip1);
        KeyPreview = true;
        MinimumSize = new Size(900, 520);
        Name = "AnaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Key Event Logger (Eğitsel) - YMH219";
        Activated += AnaForm_Activated;
        Deactivate += AnaForm_Deactivate;
        KeyDown += AnaForm_KeyDown;
        KeyPress += AnaForm_KeyPress;
        KeyUp += AnaForm_KeyUp;
        pnlUst.ResumeLayout(false);
        pnlUst.PerformLayout();
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        grpDeneme.ResumeLayout(false);
        grpDeneme.PerformLayout();
        grpFiltre.ResumeLayout(false);
        grpFiltre.PerformLayout();
        grpIstatistik.ResumeLayout(false);
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private FlowLayoutPanel pnlUst;
    private Label lblDurum;
    private Button btnBaslat;
    private Button btnDurdur;
    private Button btnTemizle;
    private Button btnKaydet;
    private Label lblOlaylar;
    private CheckBox chkKeyDown;
    private CheckBox chkKeyUp;
    private CheckBox chkKeyPress;
    private SplitContainer splitContainer1;
    private ListBox lstKayitlar;
    private Label lblListeBaslik;
    private GroupBox grpDeneme;
    private TextBox txtDeneme;
    private GroupBox grpFiltre;
    private Button btnFiltreKaldir;
    private Button btnFiltrele;
    private DateTimePicker dtpBitis;
    private DateTimePicker dtpBaslangic;
    private Label lblBitis;
    private Label lblBaslangic;
    private GroupBox grpIstatistik;
    private ListBox lstEnSik;
    private Label lblEnSik;
    private Label lblToplam;
    private Label lblOdak;
    private Label lblUyari;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel lblDurumCubugu;
    private System.Windows.Forms.Timer tmrGosterge;
}
