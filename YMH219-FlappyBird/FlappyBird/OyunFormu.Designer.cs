namespace FlappyBird;

partial class OyunFormu
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
        oyunZamanlayici = new System.Windows.Forms.Timer(components);
        pnlUst = new FlowLayoutPanel();
        lblSkor = new Label();
        lblEnYuksek = new Label();
        lblSeviye = new Label();
        lblZorluk = new Label();
        cmbZorluk = new ComboBox();
        btnYeniOyun = new Button();
        btnDuraklat = new Button();
        chkSes = new CheckBox();
        oyunPaneli = new OyunPaneli();
        lblYardim = new Label();
        pnlUst.SuspendLayout();
        SuspendLayout();
        //
        // oyunZamanlayici
        //
        oyunZamanlayici.Interval = 16;
        oyunZamanlayici.Tick += oyunZamanlayici_Tick;
        //
        // pnlUst
        //
        pnlUst.BackColor = Color.FromArgb(40, 44, 52);
        pnlUst.Controls.Add(lblSkor);
        pnlUst.Controls.Add(lblEnYuksek);
        pnlUst.Controls.Add(lblSeviye);
        pnlUst.Controls.Add(lblZorluk);
        pnlUst.Controls.Add(cmbZorluk);
        pnlUst.Controls.Add(btnYeniOyun);
        pnlUst.Controls.Add(btnDuraklat);
        pnlUst.Controls.Add(chkSes);
        pnlUst.Dock = DockStyle.Top;
        pnlUst.Name = "pnlUst";
        pnlUst.Padding = new Padding(6, 6, 6, 0);
        pnlUst.Size = new Size(880, 44);
        pnlUst.TabIndex = 0;
        pnlUst.WrapContents = false;
        //
        // lblSkor
        //
        lblSkor.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblSkor.ForeColor = Color.White;
        lblSkor.Margin = new Padding(3, 4, 3, 0);
        lblSkor.Name = "lblSkor";
        lblSkor.Size = new Size(95, 26);
        lblSkor.Text = "Skor: 0";
        //
        // lblEnYuksek
        //
        lblEnYuksek.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblEnYuksek.ForeColor = Color.Gold;
        lblEnYuksek.Margin = new Padding(3, 4, 3, 0);
        lblEnYuksek.Name = "lblEnYuksek";
        lblEnYuksek.Size = new Size(140, 26);
        lblEnYuksek.Text = "En Yüksek: 0";
        //
        // lblSeviye
        //
        lblSeviye.Font = new Font("Segoe UI", 10F);
        lblSeviye.ForeColor = Color.LightGray;
        lblSeviye.Margin = new Padding(3, 7, 3, 0);
        lblSeviye.Name = "lblSeviye";
        lblSeviye.Size = new Size(150, 23);
        lblSeviye.Text = "Seviye: 1  Hız: 4.0";
        //
        // lblZorluk
        //
        lblZorluk.AutoSize = true;
        lblZorluk.ForeColor = Color.White;
        lblZorluk.Margin = new Padding(3, 9, 0, 0);
        lblZorluk.Name = "lblZorluk";
        lblZorluk.Text = "Zorluk:";
        //
        // cmbZorluk
        //
        cmbZorluk.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbZorluk.Margin = new Padding(3, 5, 3, 0);
        cmbZorluk.Name = "cmbZorluk";
        cmbZorluk.Size = new Size(80, 23);
        cmbZorluk.TabIndex = 0;
        cmbZorluk.TabStop = false;
        cmbZorluk.SelectedIndexChanged += cmbZorluk_SelectedIndexChanged;
        //
        // btnYeniOyun
        //
        btnYeniOyun.BackColor = Color.FromArgb(76, 175, 80);
        btnYeniOyun.FlatStyle = FlatStyle.Flat;
        btnYeniOyun.ForeColor = Color.White;
        btnYeniOyun.Margin = new Padding(8, 2, 3, 0);
        btnYeniOyun.Name = "btnYeniOyun";
        btnYeniOyun.Size = new Size(110, 30);
        btnYeniOyun.TabIndex = 1;
        btnYeniOyun.TabStop = false;
        btnYeniOyun.Text = "Yeni Oyun (R)";
        btnYeniOyun.UseVisualStyleBackColor = false;
        btnYeniOyun.Click += btnYeniOyun_Click;
        //
        // btnDuraklat
        //
        btnDuraklat.BackColor = Color.FromArgb(255, 152, 0);
        btnDuraklat.FlatStyle = FlatStyle.Flat;
        btnDuraklat.ForeColor = Color.White;
        btnDuraklat.Margin = new Padding(3, 2, 3, 0);
        btnDuraklat.Name = "btnDuraklat";
        btnDuraklat.Size = new Size(100, 30);
        btnDuraklat.TabIndex = 2;
        btnDuraklat.TabStop = false;
        btnDuraklat.Text = "Duraklat (P)";
        btnDuraklat.UseVisualStyleBackColor = false;
        btnDuraklat.Click += btnDuraklat_Click;
        //
        // chkSes
        //
        chkSes.AutoSize = true;
        chkSes.Checked = true;
        chkSes.CheckState = CheckState.Checked;
        chkSes.ForeColor = Color.White;
        chkSes.Margin = new Padding(8, 8, 3, 0);
        chkSes.Name = "chkSes";
        chkSes.TabStop = false;
        chkSes.Text = "Ses";
        chkSes.UseVisualStyleBackColor = true;
        //
        // oyunPaneli
        //
        oyunPaneli.BackColor = Color.SkyBlue;
        oyunPaneli.Dock = DockStyle.Fill;
        oyunPaneli.Name = "oyunPaneli";
        oyunPaneli.Size = new Size(880, 520);
        oyunPaneli.TabIndex = 1;
        oyunPaneli.Paint += oyunPaneli_Paint;
        oyunPaneli.MouseDown += oyunPaneli_MouseDown;
        //
        // lblYardim
        //
        lblYardim.BackColor = Color.FromArgb(40, 44, 52);
        lblYardim.Dock = DockStyle.Bottom;
        lblYardim.ForeColor = Color.LightGray;
        lblYardim.Name = "lblYardim";
        lblYardim.Size = new Size(880, 24);
        lblYardim.Text = "  Kontroller:  SPACE / ↑ / Fare tıklaması = Zıpla     P = Duraklat     R = Yeni Oyun";
        lblYardim.TextAlign = ContentAlignment.MiddleLeft;
        //
        // OyunFormu
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(880, 588);
        Controls.Add(oyunPaneli);
        Controls.Add(lblYardim);
        Controls.Add(pnlUst);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        KeyPreview = true;
        MaximizeBox = false;
        Name = "OyunFormu";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Flappy Bird - YMH219";
        FormClosing += OyunFormu_FormClosing;
        Load += OyunFormu_Load;
        KeyDown += OyunFormu_KeyDown;
        KeyUp += OyunFormu_KeyUp;
        pnlUst.ResumeLayout(false);
        pnlUst.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Timer oyunZamanlayici;
    private FlowLayoutPanel pnlUst;
    private Label lblSkor;
    private Label lblEnYuksek;
    private Label lblSeviye;
    private Label lblZorluk;
    private ComboBox cmbZorluk;
    private Button btnYeniOyun;
    private Button btnDuraklat;
    private CheckBox chkSes;
    private OyunPaneli oyunPaneli;
    private Label lblYardim;
}
