using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private TextBox txtEkran;
        private double sayi1 = 0;
        private string islem = "";
        private bool yeniSayi = true;

        public Form1()
        {
            InitializeComponent();
            HesapMakinesiArayuzunuOlustur();
        }

        private void HesapMakinesiArayuzunuOlustur()
        {
            // Form Temel Ayarları
            this.Text = "Hesap Makinesi";
            this.Size = new Size(340, 470);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(32, 32, 32);

            // Ekran (TextBox)
            txtEkran = new TextBox();
            txtEkran.Text = "0";
            txtEkran.Location = new Point(15, 20);
            txtEkran.Size = new Size(295, 50);
            txtEkran.Font = new Font("Segoe UI", 26, FontStyle.Bold);
            txtEkran.TextAlign = HorizontalAlignment.Right;
            txtEkran.ReadOnly = true;
            txtEkran.BackColor = Color.FromArgb(40, 40, 40);
            txtEkran.ForeColor = Color.White;
            txtEkran.BorderStyle = BorderStyle.None;
            this.Controls.Add(txtEkran);

            // Buton Düzeni
            string[,] tuslar = {
                { "C", "←", "/", "*" },
                { "7", "8", "9", "-" },
                { "4", "5", "6", "+" },
                { "1", "2", "3", "=" },
                { "0", ",", "", "" }
            };

            int baslangicX = 15;
            int baslangicY = 90;
            int butonGenislik = 68;
            int butonYukseklik = 55;
            int bosluk = 8;

            for (int satir = 0; satir < 5; satir++)
            {
                for (int sutun = 0; sutun < 4; sutun++)
                {
                    string tusMetni = tuslar[satir, sutun];
                    if (string.IsNullOrEmpty(tusMetni)) continue;

                    Button btn = new Button();
                    btn.Text = tusMetni;
                    btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Cursor = Cursors.Hand;

                    int x = baslangicX + sutun * (butonGenislik + bosluk);
                    int y = baslangicY + satir * (butonYukseklik + bosluk);

                    if (tusMetni == "0")
                    {
                        btn.Size = new Size(butonGenislik * 2 + bosluk, butonYukseklik);
                        btn.Location = new Point(x, y);
                    }
                    else if (satir == 4 && tusMetni == ",")
                    {
                        btn.Size = new Size(butonGenislik, butonYukseklik);
                        btn.Location = new Point(baslangicX + 2 * (butonGenislik + bosluk), y);
                    }
                    else
                    {
                        btn.Size = new Size(butonGenislik, butonYukseklik);
                        btn.Location = new Point(x, y);
                    }

                    // Renk Teması ve Olay Atamaları
                    if (tusMetni == "=")
                    {
                        btn.BackColor = Color.FromArgb(0, 120, 215);
                        btn.ForeColor = Color.White;
                        btn.Click += Esittir_Click;
                    }
                    else if (tusMetni == "+" || tusMetni == "-" || tusMetni == "*" || tusMetni == "/")
                    {
                        btn.BackColor = Color.FromArgb(60, 60, 60);
                        btn.ForeColor = Color.FromArgb(255, 165, 0);
                        btn.Click += Islem_Click;
                    }
                    else if (tusMetni == "C" || tusMetni == "←")
                    {
                        btn.BackColor = Color.FromArgb(70, 70, 70);
                        btn.ForeColor = Color.White;
                        if (tusMetni == "C") btn.Click += Temizle_Click;
                        else btn.Click += GeriAl_Click;
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(50, 50, 50);
                        btn.ForeColor = Color.White;
                        btn.Click += Rakam_Click;
                    }

                    this.Controls.Add(btn);
                }
            }
        }

        // --- İŞLEM MANTIKLARI ---

        private void Rakam_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Text == ",")
            {
                if (!txtEkran.Text.Contains(","))
                    txtEkran.Text += ",";
                return;
            }

            if (txtEkran.Text == "0" || yeniSayi)
            {
                txtEkran.Text = "";
                yeniSayi = false;
            }

            txtEkran.Text += btn.Text;
        }

        private void Islem_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (double.TryParse(txtEkran.Text, out double deger))
            {
                sayi1 = deger;
            }
            islem = btn.Text;
            yeniSayi = true;
        }

        private void Esittir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(islem)) return;

            double.TryParse(txtEkran.Text, out double sayi2);
            double sonuc = 0;

            switch (islem)
            {
                case "+": sonuc = sayi1 + sayi2; break;
                case "-": sonuc = sayi1 - sayi2; break;
                case "*": sonuc = sayi1 * sayi2; break;
                case "/":
                    if (sayi2 != 0)
                        sonuc = sayi1 / sayi2;
                    else
                    {
                        MessageBox.Show("Sıfıra bölünemez!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    break;
            }

            txtEkran.Text = sonuc.ToString();
            islem = "";
            yeniSayi = true;
        }

        private void Temizle_Click(object sender, EventArgs e)
        {
            txtEkran.Text = "0";
            sayi1 = 0;
            islem = "";
            yeniSayi = true;
        }

        private void GeriAl_Click(object sender, EventArgs e)
        {
            if (txtEkran.Text.Length > 1)
                txtEkran.Text = txtEkran.Text.Substring(0, txtEkran.Text.Length - 1);
            else
                txtEkran.Text = "0";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Yüklenme anı
        }
    }
}