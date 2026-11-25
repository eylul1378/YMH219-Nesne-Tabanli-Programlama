namespace NotepadApp;

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
        menuStrip1 = new MenuStrip();
        mnuDosya = new ToolStripMenuItem();
        mnuYeniNot = new ToolStripMenuItem();
        mnuKaydet = new ToolStripMenuItem();
        mnuYukle = new ToolStripMenuItem();
        mnuFarkliKaydet = new ToolStripMenuItem();
        mnuDisaAktar = new ToolStripMenuItem();
        mnuAyirici1 = new ToolStripSeparator();
        mnuCikis = new ToolStripMenuItem();
        mnuBicim = new ToolStripMenuItem();
        mnuKalin = new ToolStripMenuItem();
        mnuItalik = new ToolStripMenuItem();
        mnuAltiCizili = new ToolStripMenuItem();
        mnuYaziTipi = new ToolStripMenuItem();
        mnuYardim = new ToolStripMenuItem();
        mnuHakkinda = new ToolStripMenuItem();
        statusStrip1 = new StatusStrip();
        lblDurum = new ToolStripStatusLabel();
        lblDosya = new ToolStripStatusLabel();
        splitContainer1 = new SplitContainer();
        lstNotlar = new ListBox();
        lblNotSayisi = new Label();
        pnlArama = new Panel();
        txtAra = new TextBox();
        lblAra = new Label();
        rtbIcerik = new RichTextBox();
        lblTarihler = new Label();
        pnlUst = new TableLayoutPanel();
        lblBaslik = new Label();
        txtBaslik = new TextBox();
        lblKategori = new Label();
        cmbKategori = new ComboBox();
        pnlButonlar = new FlowLayoutPanel();
        btnYeni = new Button();
        btnKaydet = new Button();
        btnSil = new Button();
        btnDisaAktar = new Button();
        chkOtomatikKaydet = new CheckBox();
        fontDialog1 = new FontDialog();
        menuStrip1.SuspendLayout();
        statusStrip1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        pnlArama.SuspendLayout();
        pnlUst.SuspendLayout();
        pnlButonlar.SuspendLayout();
        SuspendLayout();
        //
        // menuStrip1
        //
        menuStrip1.Items.AddRange(new ToolStripItem[] { mnuDosya, mnuBicim, mnuYardim });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(984, 24);
        menuStrip1.TabIndex = 0;
        //
        // mnuDosya
        //
        mnuDosya.DropDownItems.AddRange(new ToolStripItem[] { mnuYeniNot, mnuKaydet, mnuYukle, mnuFarkliKaydet, mnuDisaAktar, mnuAyirici1, mnuCikis });
        mnuDosya.Name = "mnuDosya";
        mnuDosya.Text = "&Dosya";
        //
        // mnuYeniNot
        //
        mnuYeniNot.Name = "mnuYeniNot";
        mnuYeniNot.ShortcutKeys = Keys.Control | Keys.N;
        mnuYeniNot.Text = "&Yeni Not";
        mnuYeniNot.Click += btnYeni_Click;
        //
        // mnuKaydet
        //
        mnuKaydet.Name = "mnuKaydet";
        mnuKaydet.ShortcutKeys = Keys.Control | Keys.S;
        mnuKaydet.Text = "&Kaydet";
        mnuKaydet.Click += mnuKaydet_Click;
        //
        // mnuYukle
        //
        mnuYukle.Name = "mnuYukle";
        mnuYukle.ShortcutKeys = Keys.Control | Keys.O;
        mnuYukle.Text = "Dosyadan &Yükle...";
        mnuYukle.Click += mnuYukle_Click;
        //
        // mnuFarkliKaydet
        //
        mnuFarkliKaydet.Name = "mnuFarkliKaydet";
        mnuFarkliKaydet.Text = "&Farklı Kaydet...";
        mnuFarkliKaydet.Click += mnuFarkliKaydet_Click;
        //
        // mnuDisaAktar
        //
        mnuDisaAktar.Name = "mnuDisaAktar";
        mnuDisaAktar.ShortcutKeys = Keys.Control | Keys.E;
        mnuDisaAktar.Text = "Notu .txt Olarak &Dışa Aktar...";
        mnuDisaAktar.Click += btnDisaAktar_Click;
        //
        // mnuAyirici1
        //
        mnuAyirici1.Name = "mnuAyirici1";
        //
        // mnuCikis
        //
        mnuCikis.Name = "mnuCikis";
        mnuCikis.Text = "Çı&kış";
        mnuCikis.Click += mnuCikis_Click;
        //
        // mnuBicim
        //
        mnuBicim.DropDownItems.AddRange(new ToolStripItem[] { mnuKalin, mnuItalik, mnuAltiCizili, mnuYaziTipi });
        mnuBicim.Name = "mnuBicim";
        mnuBicim.Text = "&Biçim";
        //
        // mnuKalin
        //
        mnuKalin.Name = "mnuKalin";
        mnuKalin.ShortcutKeys = Keys.Control | Keys.B;
        mnuKalin.Text = "Kalın";
        mnuKalin.Click += mnuKalin_Click;
        //
        // mnuItalik
        //
        mnuItalik.Name = "mnuItalik";
        mnuItalik.ShortcutKeys = Keys.Control | Keys.I;
        mnuItalik.Text = "İtalik";
        mnuItalik.Click += mnuItalik_Click;
        //
        // mnuAltiCizili
        //
        mnuAltiCizili.Name = "mnuAltiCizili";
        mnuAltiCizili.ShortcutKeys = Keys.Control | Keys.U;
        mnuAltiCizili.Text = "Altı Çizili";
        mnuAltiCizili.Click += mnuAltiCizili_Click;
        //
        // mnuYaziTipi
        //
        mnuYaziTipi.Name = "mnuYaziTipi";
        mnuYaziTipi.Text = "Yazı Tipi...";
        mnuYaziTipi.Click += mnuYaziTipi_Click;
        //
        // mnuYardim
        //
        mnuYardim.DropDownItems.AddRange(new ToolStripItem[] { mnuHakkinda });
        mnuYardim.Name = "mnuYardim";
        mnuYardim.Text = "&Yardım";
        //
        // mnuHakkinda
        //
        mnuHakkinda.Name = "mnuHakkinda";
        mnuHakkinda.Text = "Hakkında";
        mnuHakkinda.Click += mnuHakkinda_Click;
        //
        // statusStrip1
        //
        statusStrip1.Items.AddRange(new ToolStripItem[] { lblDurum, lblDosya });
        statusStrip1.Location = new Point(0, 639);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new Size(984, 22);
        statusStrip1.TabIndex = 2;
        //
        // lblDurum
        //
        lblDurum.Name = "lblDurum";
        lblDurum.Spring = true;
        lblDurum.Text = "Hazır";
        lblDurum.TextAlign = ContentAlignment.MiddleLeft;
        //
        // lblDosya
        //
        lblDosya.ForeColor = Color.DimGray;
        lblDosya.Name = "lblDosya";
        lblDosya.Text = "";
        //
        // splitContainer1
        //
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(0, 24);
        splitContainer1.Name = "splitContainer1";
        //
        // splitContainer1.Panel1
        //
        splitContainer1.Panel1.Controls.Add(lstNotlar);
        splitContainer1.Panel1.Controls.Add(lblNotSayisi);
        splitContainer1.Panel1.Controls.Add(pnlArama);
        splitContainer1.Panel1.Padding = new Padding(8);
        //
        // splitContainer1.Panel2
        //
        splitContainer1.Panel2.Controls.Add(rtbIcerik);
        splitContainer1.Panel2.Controls.Add(lblTarihler);
        splitContainer1.Panel2.Controls.Add(pnlUst);
        splitContainer1.Panel2.Controls.Add(pnlButonlar);
        splitContainer1.Panel2.Padding = new Padding(8);
        splitContainer1.Size = new Size(984, 615);
        splitContainer1.SplitterDistance = 340;
        splitContainer1.TabIndex = 1;
        //
        // lstNotlar
        //
        lstNotlar.Dock = DockStyle.Fill;
        lstNotlar.Font = new Font("Segoe UI", 10F);
        lstNotlar.IntegralHeight = false;
        lstNotlar.Name = "lstNotlar";
        lstNotlar.TabIndex = 1;
        lstNotlar.SelectedIndexChanged += lstNotlar_SelectedIndexChanged;
        //
        // lblNotSayisi
        //
        lblNotSayisi.Dock = DockStyle.Bottom;
        lblNotSayisi.ForeColor = Color.DimGray;
        lblNotSayisi.Name = "lblNotSayisi";
        lblNotSayisi.Size = new Size(324, 22);
        lblNotSayisi.Text = "0 not";
        lblNotSayisi.TextAlign = ContentAlignment.MiddleLeft;
        //
        // pnlArama
        //
        pnlArama.Controls.Add(txtAra);
        pnlArama.Controls.Add(lblAra);
        pnlArama.Dock = DockStyle.Top;
        pnlArama.Name = "pnlArama";
        pnlArama.Size = new Size(324, 34);
        pnlArama.TabIndex = 0;
        //
        // txtAra
        //
        txtAra.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtAra.Location = new Point(40, 3);
        txtAra.Name = "txtAra";
        txtAra.PlaceholderText = "Başlık, içerik veya kategoriye göre ara...";
        txtAra.Size = new Size(284, 23);
        txtAra.TabIndex = 0;
        txtAra.TextChanged += txtAra_TextChanged;
        //
        // lblAra
        //
        lblAra.AutoSize = true;
        lblAra.Location = new Point(0, 6);
        lblAra.Name = "lblAra";
        lblAra.Text = "Ara:";
        //
        // rtbIcerik
        //
        rtbIcerik.Dock = DockStyle.Fill;
        rtbIcerik.Font = new Font("Segoe UI", 11F);
        rtbIcerik.Name = "rtbIcerik";
        rtbIcerik.TabIndex = 2;
        rtbIcerik.Text = "";
        rtbIcerik.TextChanged += Editor_TextChanged;
        //
        // lblTarihler
        //
        lblTarihler.Dock = DockStyle.Bottom;
        lblTarihler.ForeColor = Color.DimGray;
        lblTarihler.Name = "lblTarihler";
        lblTarihler.Size = new Size(624, 24);
        lblTarihler.Text = "Yeni not";
        lblTarihler.TextAlign = ContentAlignment.MiddleLeft;
        //
        // pnlUst
        //
        pnlUst.ColumnCount = 4;
        pnlUst.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pnlUst.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
        pnlUst.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pnlUst.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
        pnlUst.Controls.Add(lblBaslik, 0, 0);
        pnlUst.Controls.Add(txtBaslik, 1, 0);
        pnlUst.Controls.Add(lblKategori, 2, 0);
        pnlUst.Controls.Add(cmbKategori, 3, 0);
        pnlUst.Dock = DockStyle.Top;
        pnlUst.Name = "pnlUst";
        pnlUst.RowCount = 1;
        pnlUst.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        pnlUst.Size = new Size(624, 36);
        pnlUst.TabIndex = 0;
        //
        // lblBaslik
        //
        lblBaslik.Anchor = AnchorStyles.Left;
        lblBaslik.AutoSize = true;
        lblBaslik.Name = "lblBaslik";
        lblBaslik.Text = "Başlık:";
        //
        // txtBaslik
        //
        txtBaslik.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtBaslik.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        txtBaslik.MaxLength = 200;
        txtBaslik.Name = "txtBaslik";
        txtBaslik.TabIndex = 0;
        txtBaslik.TextChanged += Editor_TextChanged;
        //
        // lblKategori
        //
        lblKategori.Anchor = AnchorStyles.Left;
        lblKategori.AutoSize = true;
        lblKategori.Name = "lblKategori";
        lblKategori.Text = "Kategori:";
        //
        // cmbKategori
        //
        cmbKategori.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        cmbKategori.Name = "cmbKategori";
        cmbKategori.TabIndex = 1;
        cmbKategori.Text = "Genel";
        cmbKategori.TextChanged += Editor_TextChanged;
        //
        // pnlButonlar
        //
        pnlButonlar.Controls.Add(btnYeni);
        pnlButonlar.Controls.Add(btnKaydet);
        pnlButonlar.Controls.Add(btnSil);
        pnlButonlar.Controls.Add(btnDisaAktar);
        pnlButonlar.Controls.Add(chkOtomatikKaydet);
        pnlButonlar.Dock = DockStyle.Bottom;
        pnlButonlar.Name = "pnlButonlar";
        pnlButonlar.Padding = new Padding(0, 4, 0, 0);
        pnlButonlar.Size = new Size(624, 42);
        pnlButonlar.TabIndex = 3;
        //
        // btnYeni
        //
        btnYeni.Name = "btnYeni";
        btnYeni.Size = new Size(100, 30);
        btnYeni.TabIndex = 0;
        btnYeni.Text = "Yeni Not";
        btnYeni.UseVisualStyleBackColor = true;
        btnYeni.Click += btnYeni_Click;
        //
        // btnKaydet
        //
        btnKaydet.BackColor = Color.FromArgb(0, 120, 215);
        btnKaydet.FlatStyle = FlatStyle.Flat;
        btnKaydet.ForeColor = Color.White;
        btnKaydet.Name = "btnKaydet";
        btnKaydet.Size = new Size(110, 30);
        btnKaydet.TabIndex = 1;
        btnKaydet.Text = "Notu Kaydet";
        btnKaydet.UseVisualStyleBackColor = false;
        btnKaydet.Click += btnKaydet_Click;
        //
        // btnSil
        //
        btnSil.Name = "btnSil";
        btnSil.Size = new Size(100, 30);
        btnSil.TabIndex = 2;
        btnSil.Text = "Sil";
        btnSil.UseVisualStyleBackColor = true;
        btnSil.Click += btnSil_Click;
        //
        // btnDisaAktar
        //
        btnDisaAktar.Name = "btnDisaAktar";
        btnDisaAktar.Size = new Size(130, 30);
        btnDisaAktar.TabIndex = 3;
        btnDisaAktar.Text = ".txt Dışa Aktar";
        btnDisaAktar.UseVisualStyleBackColor = true;
        btnDisaAktar.Click += btnDisaAktar_Click;
        //
        // chkOtomatikKaydet
        //
        chkOtomatikKaydet.AutoSize = true;
        chkOtomatikKaydet.Checked = true;
        chkOtomatikKaydet.CheckState = CheckState.Checked;
        chkOtomatikKaydet.Margin = new Padding(12, 9, 3, 3);
        chkOtomatikKaydet.Name = "chkOtomatikKaydet";
        chkOtomatikKaydet.TabIndex = 4;
        chkOtomatikKaydet.Text = "Her işlemden sonra dosyaya otomatik kaydet";
        chkOtomatikKaydet.UseVisualStyleBackColor = true;
        //
        // AnaForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(984, 661);
        Controls.Add(splitContainer1);
        Controls.Add(statusStrip1);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        MinimumSize = new Size(800, 500);
        Name = "AnaForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Not Defteri - YMH219";
        FormClosing += AnaForm_FormClosing;
        Load += AnaForm_Load;
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        pnlArama.ResumeLayout(false);
        pnlArama.PerformLayout();
        pnlUst.ResumeLayout(false);
        pnlUst.PerformLayout();
        pnlButonlar.ResumeLayout(false);
        pnlButonlar.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip menuStrip1;
    private ToolStripMenuItem mnuDosya;
    private ToolStripMenuItem mnuYeniNot;
    private ToolStripMenuItem mnuKaydet;
    private ToolStripMenuItem mnuYukle;
    private ToolStripMenuItem mnuFarkliKaydet;
    private ToolStripMenuItem mnuDisaAktar;
    private ToolStripSeparator mnuAyirici1;
    private ToolStripMenuItem mnuCikis;
    private ToolStripMenuItem mnuBicim;
    private ToolStripMenuItem mnuKalin;
    private ToolStripMenuItem mnuItalik;
    private ToolStripMenuItem mnuAltiCizili;
    private ToolStripMenuItem mnuYaziTipi;
    private ToolStripMenuItem mnuYardim;
    private ToolStripMenuItem mnuHakkinda;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel lblDurum;
    private ToolStripStatusLabel lblDosya;
    private SplitContainer splitContainer1;
    private ListBox lstNotlar;
    private Label lblNotSayisi;
    private Panel pnlArama;
    private TextBox txtAra;
    private Label lblAra;
    private RichTextBox rtbIcerik;
    private Label lblTarihler;
    private TableLayoutPanel pnlUst;
    private Label lblBaslik;
    private TextBox txtBaslik;
    private Label lblKategori;
    private ComboBox cmbKategori;
    private FlowLayoutPanel pnlButonlar;
    private Button btnYeni;
    private Button btnKaydet;
    private Button btnSil;
    private Button btnDisaAktar;
    private CheckBox chkOtomatikKaydet;
    private FontDialog fontDialog1;
}
