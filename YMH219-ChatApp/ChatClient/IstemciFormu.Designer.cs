namespace ChatClient;

partial class IstemciFormu
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
        lblSunucu = new Label();
        txtSunucu = new TextBox();
        lblPort = new Label();
        nudPort = new NumericUpDown();
        lblAd = new Label();
        txtKullaniciAdi = new TextBox();
        btnBaglan = new Button();
        btnKes = new Button();
        lblDurum = new Label();
        splitContainer1 = new SplitContainer();
        rtbMesajlar = new RichTextBox();
        lstKullanicilar = new ListBox();
        lblKullanicilar = new Label();
        lblOzelBilgi = new Label();
        pnlAlt = new TableLayoutPanel();
        lblAlici = new Label();
        txtMesaj = new TextBox();
        btnGonder = new Button();
        btnGecmisKaydet = new Button();
        pnlUst.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudPort).BeginInit();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        pnlAlt.SuspendLayout();
        SuspendLayout();
        //
        // pnlUst
        //
        pnlUst.Controls.Add(lblSunucu);
        pnlUst.Controls.Add(txtSunucu);
        pnlUst.Controls.Add(lblPort);
        pnlUst.Controls.Add(nudPort);
        pnlUst.Controls.Add(lblAd);
        pnlUst.Controls.Add(txtKullaniciAdi);
        pnlUst.Controls.Add(btnBaglan);
        pnlUst.Controls.Add(btnKes);
        pnlUst.Controls.Add(lblDurum);
        pnlUst.Dock = DockStyle.Top;
        pnlUst.Name = "pnlUst";
        pnlUst.Padding = new Padding(6, 8, 6, 0);
        pnlUst.Size = new Size(900, 46);
        pnlUst.TabIndex = 0;
        pnlUst.WrapContents = false;
        //
        // lblSunucu
        //
        lblSunucu.AutoSize = true;
        lblSunucu.Margin = new Padding(3, 8, 0, 0);
        lblSunucu.Name = "lblSunucu";
        lblSunucu.Text = "Sunucu:";
        //
        // txtSunucu
        //
        txtSunucu.Margin = new Padding(3, 4, 3, 0);
        txtSunucu.Name = "txtSunucu";
        txtSunucu.Size = new Size(110, 23);
        txtSunucu.TabIndex = 0;
        txtSunucu.Text = "127.0.0.1";
        //
        // lblPort
        //
        lblPort.AutoSize = true;
        lblPort.Margin = new Padding(6, 8, 0, 0);
        lblPort.Name = "lblPort";
        lblPort.Text = "Port:";
        //
        // nudPort
        //
        nudPort.Margin = new Padding(3, 4, 3, 0);
        nudPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        nudPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudPort.Name = "nudPort";
        nudPort.Size = new Size(65, 23);
        nudPort.TabIndex = 1;
        nudPort.Value = new decimal(new int[] { 5000, 0, 0, 0 });
        //
        // lblAd
        //
        lblAd.AutoSize = true;
        lblAd.Margin = new Padding(6, 8, 0, 0);
        lblAd.Name = "lblAd";
        lblAd.Text = "Kullanıcı adı:";
        //
        // txtKullaniciAdi
        //
        txtKullaniciAdi.Margin = new Padding(3, 4, 3, 0);
        txtKullaniciAdi.MaxLength = 20;
        txtKullaniciAdi.Name = "txtKullaniciAdi";
        txtKullaniciAdi.Size = new Size(110, 23);
        txtKullaniciAdi.TabIndex = 2;
        //
        // btnBaglan
        //
        btnBaglan.BackColor = Color.FromArgb(25, 118, 210);
        btnBaglan.FlatStyle = FlatStyle.Flat;
        btnBaglan.ForeColor = Color.White;
        btnBaglan.Margin = new Padding(8, 1, 3, 0);
        btnBaglan.Name = "btnBaglan";
        btnBaglan.Size = new Size(85, 30);
        btnBaglan.TabIndex = 3;
        btnBaglan.Text = "Bağlan";
        btnBaglan.UseVisualStyleBackColor = false;
        btnBaglan.Click += btnBaglan_Click;
        //
        // btnKes
        //
        btnKes.Enabled = false;
        btnKes.Margin = new Padding(3, 1, 3, 0);
        btnKes.Name = "btnKes";
        btnKes.Size = new Size(120, 30);
        btnKes.TabIndex = 4;
        btnKes.Text = "Bağlantıyı Kes";
        btnKes.UseVisualStyleBackColor = true;
        btnKes.Click += btnKes_Click;
        //
        // lblDurum
        //
        lblDurum.BackColor = Color.Gray;
        lblDurum.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDurum.ForeColor = Color.White;
        lblDurum.Margin = new Padding(8, 1, 3, 0);
        lblDurum.Name = "lblDurum";
        lblDurum.Size = new Size(150, 30);
        lblDurum.Text = "● Bağlı değil";
        lblDurum.TextAlign = ContentAlignment.MiddleCenter;
        //
        // splitContainer1
        //
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.FixedPanel = FixedPanel.Panel2;
        splitContainer1.Name = "splitContainer1";
        //
        // splitContainer1.Panel1
        //
        splitContainer1.Panel1.Controls.Add(rtbMesajlar);
        splitContainer1.Panel1.Padding = new Padding(8, 4, 4, 4);
        //
        // splitContainer1.Panel2
        //
        splitContainer1.Panel2.Controls.Add(lstKullanicilar);
        splitContainer1.Panel2.Controls.Add(lblOzelBilgi);
        splitContainer1.Panel2.Controls.Add(lblKullanicilar);
        splitContainer1.Panel2.Padding = new Padding(4, 4, 8, 4);
        splitContainer1.Size = new Size(900, 480);
        splitContainer1.SplitterDistance = 680;
        splitContainer1.TabIndex = 1;
        splitContainer1.TabStop = false;
        //
        // rtbMesajlar
        //
        rtbMesajlar.BackColor = Color.White;
        rtbMesajlar.Dock = DockStyle.Fill;
        rtbMesajlar.Font = new Font("Segoe UI", 10.5F);
        rtbMesajlar.Name = "rtbMesajlar";
        rtbMesajlar.ReadOnly = true;
        rtbMesajlar.TabIndex = 0;
        rtbMesajlar.TabStop = false;
        rtbMesajlar.Text = "";
        //
        // lstKullanicilar
        //
        lstKullanicilar.Dock = DockStyle.Fill;
        lstKullanicilar.Font = new Font("Segoe UI", 10F);
        lstKullanicilar.IntegralHeight = false;
        lstKullanicilar.Name = "lstKullanicilar";
        lstKullanicilar.TabIndex = 1;
        lstKullanicilar.SelectedIndexChanged += lstKullanicilar_SelectedIndexChanged;
        //
        // lblKullanicilar
        //
        lblKullanicilar.Dock = DockStyle.Top;
        lblKullanicilar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblKullanicilar.Name = "lblKullanicilar";
        lblKullanicilar.Size = new Size(200, 24);
        lblKullanicilar.Text = "Çevrimiçi Kullanıcılar";
        lblKullanicilar.TextAlign = ContentAlignment.MiddleLeft;
        //
        // lblOzelBilgi
        //
        lblOzelBilgi.Dock = DockStyle.Bottom;
        lblOzelBilgi.ForeColor = Color.DimGray;
        lblOzelBilgi.Name = "lblOzelBilgi";
        lblOzelBilgi.Size = new Size(200, 40);
        lblOzelBilgi.Text = "Özel mesaj için listeden kullanıcı seçin. \"(Herkes)\" = odaya mesaj.";
        //
        // pnlAlt
        //
        pnlAlt.ColumnCount = 4;
        pnlAlt.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pnlAlt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pnlAlt.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pnlAlt.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        pnlAlt.Controls.Add(lblAlici, 0, 0);
        pnlAlt.Controls.Add(txtMesaj, 1, 0);
        pnlAlt.Controls.Add(btnGonder, 2, 0);
        pnlAlt.Controls.Add(btnGecmisKaydet, 3, 0);
        pnlAlt.Dock = DockStyle.Bottom;
        pnlAlt.Name = "pnlAlt";
        pnlAlt.Padding = new Padding(6, 2, 6, 6);
        pnlAlt.RowCount = 1;
        pnlAlt.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        pnlAlt.Size = new Size(900, 46);
        pnlAlt.TabIndex = 2;
        //
        // lblAlici
        //
        lblAlici.Anchor = AnchorStyles.Left;
        lblAlici.AutoSize = true;
        lblAlici.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblAlici.Name = "lblAlici";
        lblAlici.Text = "Alıcı: Herkes";
        //
        // txtMesaj
        //
        txtMesaj.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtMesaj.Enabled = false;
        txtMesaj.Font = new Font("Segoe UI", 10.5F);
        txtMesaj.MaxLength = 1000;
        txtMesaj.Name = "txtMesaj";
        txtMesaj.PlaceholderText = "Mesajınızı yazın ve Enter'a basın...";
        txtMesaj.TabIndex = 0;
        //
        // btnGonder
        //
        btnGonder.Anchor = AnchorStyles.None;
        btnGonder.BackColor = Color.FromArgb(25, 118, 210);
        btnGonder.Enabled = false;
        btnGonder.FlatStyle = FlatStyle.Flat;
        btnGonder.ForeColor = Color.White;
        btnGonder.Name = "btnGonder";
        btnGonder.Size = new Size(110, 30);
        btnGonder.TabIndex = 1;
        btnGonder.Text = "Mesaj Gönder";
        btnGonder.UseVisualStyleBackColor = false;
        btnGonder.Click += btnGonder_Click;
        //
        // btnGecmisKaydet
        //
        btnGecmisKaydet.Anchor = AnchorStyles.None;
        btnGecmisKaydet.Name = "btnGecmisKaydet";
        btnGecmisKaydet.Size = new Size(110, 30);
        btnGecmisKaydet.TabIndex = 2;
        btnGecmisKaydet.Text = "Geçmişi Kaydet";
        btnGecmisKaydet.UseVisualStyleBackColor = true;
        btnGecmisKaydet.Click += btnGecmisKaydet_Click;
        //
        // IstemciFormu
        //
        AcceptButton = btnGonder;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 572);
        Controls.Add(splitContainer1);
        Controls.Add(pnlAlt);
        Controls.Add(pnlUst);
        MinimumSize = new Size(820, 400);
        Name = "IstemciFormu";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Sohbet İstemcisi - YMH219";
        FormClosing += IstemciFormu_FormClosing;
        pnlUst.ResumeLayout(false);
        pnlUst.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudPort).EndInit();
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        pnlAlt.ResumeLayout(false);
        pnlAlt.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private FlowLayoutPanel pnlUst;
    private Label lblSunucu;
    private TextBox txtSunucu;
    private Label lblPort;
    private NumericUpDown nudPort;
    private Label lblAd;
    private TextBox txtKullaniciAdi;
    private Button btnBaglan;
    private Button btnKes;
    private Label lblDurum;
    private SplitContainer splitContainer1;
    private RichTextBox rtbMesajlar;
    private ListBox lstKullanicilar;
    private Label lblKullanicilar;
    private Label lblOzelBilgi;
    private TableLayoutPanel pnlAlt;
    private Label lblAlici;
    private TextBox txtMesaj;
    private Button btnGonder;
    private Button btnGecmisKaydet;
}
