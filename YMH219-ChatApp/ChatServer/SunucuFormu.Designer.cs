namespace ChatServer;

partial class SunucuFormu
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
        pnlUst = new FlowLayoutPanel();
        lblPort = new Label();
        nudPort = new NumericUpDown();
        chkSadeceYerel = new CheckBox();
        btnBaslat = new Button();
        btnDurdur = new Button();
        lblDurum = new Label();
        lblIpAdresleri = new Label();
        splitContainer1 = new SplitContainer();
        grpIstemciler = new GroupBox();
        lvIstemciler = new ListView();
        colAd = new ColumnHeader();
        colAdres = new ColumnHeader();
        colZaman = new ColumnHeader();
        colMesaj = new ColumnHeader();
        lblIstemciSayisi = new Label();
        grpGunluk = new GroupBox();
        lstGunluk = new ListBox();
        pnlDuyuru = new TableLayoutPanel();
        txtDuyuru = new TextBox();
        btnDuyuru = new Button();
        btnGunlukKaydet = new Button();
        pnlUst.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudPort).BeginInit();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        grpIstemciler.SuspendLayout();
        grpGunluk.SuspendLayout();
        pnlDuyuru.SuspendLayout();
        SuspendLayout();
        //
        // pnlUst
        //
        pnlUst.Controls.Add(lblPort);
        pnlUst.Controls.Add(nudPort);
        pnlUst.Controls.Add(chkSadeceYerel);
        pnlUst.Controls.Add(btnBaslat);
        pnlUst.Controls.Add(btnDurdur);
        pnlUst.Controls.Add(lblDurum);
        pnlUst.Dock = DockStyle.Top;
        pnlUst.Name = "pnlUst";
        pnlUst.Padding = new Padding(6, 8, 6, 0);
        pnlUst.Size = new Size(940, 48);
        pnlUst.TabIndex = 0;
        pnlUst.WrapContents = false;
        //
        // lblPort
        //
        lblPort.AutoSize = true;
        lblPort.Margin = new Padding(3, 8, 0, 0);
        lblPort.Name = "lblPort";
        lblPort.Text = "Port:";
        //
        // nudPort
        //
        nudPort.Margin = new Padding(3, 5, 3, 0);
        nudPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        nudPort.Minimum = new decimal(new int[] { 1024, 0, 0, 0 });
        nudPort.Name = "nudPort";
        nudPort.Size = new Size(75, 23);
        nudPort.TabIndex = 0;
        nudPort.Value = new decimal(new int[] { 5000, 0, 0, 0 });
        //
        // chkSadeceYerel
        //
        chkSadeceYerel.AutoSize = true;
        chkSadeceYerel.Margin = new Padding(10, 7, 3, 0);
        chkSadeceYerel.Name = "chkSadeceYerel";
        chkSadeceYerel.TabIndex = 1;
        chkSadeceYerel.Text = "Sadece bu bilgisayar (127.0.0.1)";
        chkSadeceYerel.UseVisualStyleBackColor = true;
        //
        // btnBaslat
        //
        btnBaslat.BackColor = Color.FromArgb(46, 125, 50);
        btnBaslat.FlatStyle = FlatStyle.Flat;
        btnBaslat.ForeColor = Color.White;
        btnBaslat.Margin = new Padding(10, 1, 3, 0);
        btnBaslat.Name = "btnBaslat";
        btnBaslat.Size = new Size(130, 30);
        btnBaslat.TabIndex = 2;
        btnBaslat.Text = "Sunucuyu Başlat";
        btnBaslat.UseVisualStyleBackColor = false;
        btnBaslat.Click += btnBaslat_Click;
        //
        // btnDurdur
        //
        btnDurdur.BackColor = Color.FromArgb(198, 40, 40);
        btnDurdur.Enabled = false;
        btnDurdur.FlatStyle = FlatStyle.Flat;
        btnDurdur.ForeColor = Color.White;
        btnDurdur.Margin = new Padding(3, 1, 3, 0);
        btnDurdur.Name = "btnDurdur";
        btnDurdur.Size = new Size(130, 30);
        btnDurdur.TabIndex = 3;
        btnDurdur.Text = "Sunucuyu Durdur";
        btnDurdur.UseVisualStyleBackColor = false;
        btnDurdur.Click += btnDurdur_Click;
        //
        // lblDurum
        //
        lblDurum.BackColor = Color.Gray;
        lblDurum.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
        lblDurum.ForeColor = Color.White;
        lblDurum.Margin = new Padding(12, 1, 3, 0);
        lblDurum.Name = "lblDurum";
        lblDurum.Size = new Size(210, 30);
        lblDurum.Text = "● Sunucu kapalı";
        lblDurum.TextAlign = ContentAlignment.MiddleCenter;
        //
        // lblIpAdresleri
        //
        lblIpAdresleri.Dock = DockStyle.Top;
        lblIpAdresleri.ForeColor = Color.DimGray;
        lblIpAdresleri.Name = "lblIpAdresleri";
        lblIpAdresleri.Padding = new Padding(8, 0, 0, 0);
        lblIpAdresleri.Size = new Size(940, 22);
        lblIpAdresleri.Text = "IP adresleri";
        lblIpAdresleri.TextAlign = ContentAlignment.MiddleLeft;
        //
        // splitContainer1
        //
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Name = "splitContainer1";
        //
        // splitContainer1.Panel1
        //
        splitContainer1.Panel1.Controls.Add(grpIstemciler);
        splitContainer1.Panel1.Padding = new Padding(8, 4, 4, 8);
        //
        // splitContainer1.Panel2
        //
        splitContainer1.Panel2.Controls.Add(grpGunluk);
        splitContainer1.Panel2.Padding = new Padding(4, 4, 8, 8);
        splitContainer1.Size = new Size(940, 510);
        splitContainer1.SplitterDistance = 450;
        splitContainer1.TabIndex = 1;
        //
        // grpIstemciler
        //
        grpIstemciler.Controls.Add(lvIstemciler);
        grpIstemciler.Controls.Add(lblIstemciSayisi);
        grpIstemciler.Dock = DockStyle.Fill;
        grpIstemciler.Name = "grpIstemciler";
        grpIstemciler.Padding = new Padding(8);
        grpIstemciler.TabIndex = 0;
        grpIstemciler.TabStop = false;
        grpIstemciler.Text = "Bağlı İstemciler";
        //
        // lvIstemciler
        //
        lvIstemciler.Columns.AddRange(new ColumnHeader[] { colAd, colAdres, colZaman, colMesaj });
        lvIstemciler.Dock = DockStyle.Fill;
        lvIstemciler.FullRowSelect = true;
        lvIstemciler.GridLines = true;
        lvIstemciler.Name = "lvIstemciler";
        lvIstemciler.TabIndex = 0;
        lvIstemciler.UseCompatibleStateImageBehavior = false;
        lvIstemciler.View = View.Details;
        //
        // colAd
        //
        colAd.Text = "Kullanıcı Adı";
        colAd.Width = 110;
        //
        // colAdres
        //
        colAdres.Text = "IP : Port";
        colAdres.Width = 130;
        //
        // colZaman
        //
        colZaman.Text = "Bağlanma";
        colZaman.Width = 80;
        //
        // colMesaj
        //
        colMesaj.Text = "Mesaj";
        colMesaj.Width = 60;
        //
        // lblIstemciSayisi
        //
        lblIstemciSayisi.Dock = DockStyle.Bottom;
        lblIstemciSayisi.Name = "lblIstemciSayisi";
        lblIstemciSayisi.Size = new Size(422, 24);
        lblIstemciSayisi.Text = "Bağlı istemci: 0";
        lblIstemciSayisi.TextAlign = ContentAlignment.MiddleLeft;
        //
        // grpGunluk
        //
        grpGunluk.Controls.Add(lstGunluk);
        grpGunluk.Controls.Add(pnlDuyuru);
        grpGunluk.Dock = DockStyle.Fill;
        grpGunluk.Name = "grpGunluk";
        grpGunluk.Padding = new Padding(8);
        grpGunluk.TabIndex = 0;
        grpGunluk.TabStop = false;
        grpGunluk.Text = "Sunucu Günlüğü ve Mesaj Trafiği";
        //
        // lstGunluk
        //
        lstGunluk.Dock = DockStyle.Fill;
        lstGunluk.Font = new Font("Consolas", 9.75F);
        lstGunluk.HorizontalScrollbar = true;
        lstGunluk.IntegralHeight = false;
        lstGunluk.Name = "lstGunluk";
        lstGunluk.TabIndex = 0;
        //
        // pnlDuyuru
        //
        pnlDuyuru.ColumnCount = 3;
        pnlDuyuru.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pnlDuyuru.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pnlDuyuru.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pnlDuyuru.Controls.Add(txtDuyuru, 0, 0);
        pnlDuyuru.Controls.Add(btnDuyuru, 1, 0);
        pnlDuyuru.Controls.Add(btnGunlukKaydet, 2, 0);
        pnlDuyuru.Dock = DockStyle.Bottom;
        pnlDuyuru.Name = "pnlDuyuru";
        pnlDuyuru.RowCount = 1;
        pnlDuyuru.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        pnlDuyuru.Size = new Size(458, 38);
        pnlDuyuru.TabIndex = 1;
        //
        // txtDuyuru
        //
        txtDuyuru.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtDuyuru.Name = "txtDuyuru";
        txtDuyuru.PlaceholderText = "Tüm kullanıcılara duyuru...";
        txtDuyuru.TabIndex = 0;
        txtDuyuru.KeyDown += txtDuyuru_KeyDown;
        //
        // btnDuyuru
        //
        btnDuyuru.Anchor = AnchorStyles.None;
        btnDuyuru.Name = "btnDuyuru";
        btnDuyuru.Size = new Size(110, 28);
        btnDuyuru.TabIndex = 1;
        btnDuyuru.Text = "Duyuru Gönder";
        btnDuyuru.UseVisualStyleBackColor = true;
        btnDuyuru.Click += btnDuyuru_Click;
        //
        // btnGunlukKaydet
        //
        btnGunlukKaydet.Anchor = AnchorStyles.None;
        btnGunlukKaydet.Name = "btnGunlukKaydet";
        btnGunlukKaydet.Size = new Size(110, 28);
        btnGunlukKaydet.TabIndex = 2;
        btnGunlukKaydet.Text = "Günlüğü Kaydet";
        btnGunlukKaydet.UseVisualStyleBackColor = true;
        btnGunlukKaydet.Click += btnGunlukKaydet_Click;
        //
        // SunucuFormu
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(940, 580);
        Controls.Add(splitContainer1);
        Controls.Add(lblIpAdresleri);
        Controls.Add(pnlUst);
        MinimumSize = new Size(800, 450);
        Name = "SunucuFormu";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Sohbet Sunucusu - YMH219";
        FormClosing += SunucuFormu_FormClosing;
        Load += SunucuFormu_Load;
        pnlUst.ResumeLayout(false);
        pnlUst.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudPort).EndInit();
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        grpIstemciler.ResumeLayout(false);
        grpGunluk.ResumeLayout(false);
        pnlDuyuru.ResumeLayout(false);
        pnlDuyuru.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private FlowLayoutPanel pnlUst;
    private Label lblPort;
    private NumericUpDown nudPort;
    private CheckBox chkSadeceYerel;
    private Button btnBaslat;
    private Button btnDurdur;
    private Label lblDurum;
    private Label lblIpAdresleri;
    private SplitContainer splitContainer1;
    private GroupBox grpIstemciler;
    private ListView lvIstemciler;
    private ColumnHeader colAd;
    private ColumnHeader colAdres;
    private ColumnHeader colZaman;
    private ColumnHeader colMesaj;
    private Label lblIstemciSayisi;
    private GroupBox grpGunluk;
    private ListBox lstGunluk;
    private TableLayoutPanel pnlDuyuru;
    private TextBox txtDuyuru;
    private Button btnDuyuru;
    private Button btnGunlukKaydet;
}
