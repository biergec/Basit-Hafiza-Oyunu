using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HafızaOyunu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer1_Baslama.Interval = 1000;
            timer1_Secim.Interval = 1000;
        }

        public void ResimGosterme()
        {
            //resimleri ekrana bastırdık, başlangıçta açılması için
            foreach (PictureBox a in panel1_AltKısım.Controls)
                a.Image = ımageList1.Images[(int)a.Tag];
        }

        public void ResimGizleme()
        {
            //resimleri ekrana bastırdık, başlangıçta açılması için
            //image 0 soru işareti
            foreach (PictureBox a in panel1_AltKısım.Controls)
                a.Image = ımageList1.Images[0];
        }

        public void ResimTagDoldurma()
        {
            ArrayList arrayListTag = new ArrayList();
            //(imageList1.Images.Count-1)*2 her resimden 2 tane koyduk
            for (int i = 0; i < CiftSayisi * 2; i++)
            {   //0.Elemandan dolayı +1
                arrayListTag.Add((i % CiftSayisi) + 1);
            }

            Random rd = new Random();
            //removeat o indextekini siler, remove değeri siler ama nizde 2 tane aynı var
            foreach (PictureBox a in panel1_AltKısım.Controls)
            {
                int AtamaDegeri = rd.Next(arrayListTag.Count);
                a.Tag = arrayListTag[AtamaDegeri];
                a.Show();
                arrayListTag.RemoveAt(AtamaDegeri);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {   
            //İlk başlama anıyla aynı işlemler silsilesi
            YenidenBalat();
        }

        int OyuncuSirasi = 2;
        public void OyuncuSirasiBelirleme()
        {
            if (OyuncuSirasi % 2 == 0)
            {
                label2_OyunSisrasi.Text = "1";
            }
            if (OyuncuSirasi % 2 != 0)
            {
                label2_OyunSisrasi.Text = "2";
            }
        }

        void puanlamasistemi()
        {
            int Oyuncubirskor = Convert.ToInt32(label9_OyuncuBir.Text);
            int Oyuncuikiskor = Convert.ToInt32(label10_Oyuncuİki.Text);

            if (label2_OyunSisrasi.Text=="1")
            {
                label9_OyuncuBir.Text = (Oyuncubirskor + 10).ToString();
            }
            if (label2_OyunSisrasi.Text == "2")
            {
                label10_Oyuncuİki.Text = (Oyuncuikiskor + 10).ToString();
            }
            if (label9_OyuncuBir.Text=="110")
            {
                MessageBox.Show("Tebrikler Oyuncu Bir Oyunu Kazandı, Yeniden Başlamak İçin Tıklayınız.");
                YenidenBalat();
            }
            if (label10_Oyuncuİki.Text == "110")
            {
                MessageBox.Show("Tebrikler Oyuncu İki Oyunu Kazandı, Yeniden Başlamak İçin Tıklayınız.");
                YenidenBalat();
            }
        }

        PictureBox OncekiResim = new PictureBox();
        int TiklamaSayisi = 0;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox SimdikiResim = (sender as PictureBox);
            SimdikiResim.Image = ımageList1.Images[(int)SimdikiResim.Tag];
            panel1_AltKısım.Refresh();
            panel1_AltKısım.Refresh();
            Thread.Sleep(1000);

            //ikinci tıklama
            if (TiklamaSayisi == 1)
            {   
                if (OncekiResim == SimdikiResim)
                {   
                    MessageBox.Show("Olmadı. Tekrar farklı resim dene, önceki resim ile aynı olamaz...");
                    return;
                }

                //2. tıklamada sayacı dırdurduk...
                label9_SecimSuresi.Text = "5";
                timer1_Secim.Stop();

                Thread.Sleep(2000);
                if (OncekiResim.Tag.ToString() == SimdikiResim.Tag.ToString())
                {   

                    //Doğru ise puanlama yapılıyor
                    puanlamasistemi();

                    //Resimler gizlendi
                    SimdikiResim.Hide();
                    OncekiResim.Hide();

                    //Kalan kart sayısı güncellendi
                    label6_KalanKART.Text = (--Kalan).ToString();
                    if (Kalan == 0)
                    {
                        MessageBox.Show("Kart Kalmadı. Kazanan : " + "" + " Başa Dönülüyor!..");
                        YenidenBalat();
                    }
                }
                else
                {
                    TiklamaSayisi = 0;
                    ResimGizleme();
                    OyuncuSirasi = OyuncuSirasi + 1;
                    OyuncuSirasiBelirleme();
                    return;
                }
                TiklamaSayisi = 0;
                OncekiResim = null;
                return;
            }

            if (TiklamaSayisi == 0)
            {
                OyuncuSirasiBelirleme();
                timer1_Secim.Start();
                OncekiResim = null;
                OncekiResim = SimdikiResim;
                TiklamaSayisi = 1;
            }


        }

        int CiftSayisi, Kalan;
        public void YenidenBalat()
        {
            CiftSayisi = ımageList1.Images.Count-1;
            Kalan = CiftSayisi;
            label6_KalanKART.Text = Kalan.ToString();
            label3.Show();
            label4.Show();
            label4.Text = "5";
            ResimTagDoldurma();
            OncekiResim = null;
            ResimGosterme();
            timer1_Baslama.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            YenidenBalat();
        }
        private void timer1_Baslama_Tick(object sender, EventArgs e)
        {
            int sayac = Convert.ToInt32(label4.Text);
            sayac--;
            label4.Text = sayac.ToString();


            if (sayac==0)
            {
                //Ekrana soru işaretini koyduk ilk
                ResimGizleme();
                label3.Hide();
                label4.Hide();
                timer1_Baslama.Stop();
            }
        }

        private void label6_KalanKART_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Süreniz Seçim Yaptığınızda Başlar");
        }

        private void timer1_Secim_Tick(object sender, EventArgs e)
        {
            int SecimSure = Convert.ToInt32(label9_SecimSuresi.Text);
            SecimSure=--SecimSure;
            label9_SecimSuresi.Text = SecimSure.ToString();

            if (SecimSure==0)
            {   
                label9_SecimSuresi.Text = "0";
                timer1_Secim.Stop();
                OncekiResim.Image = ımageList1.Images[0];
                label9_SecimSuresi.Text = "5";
                int d1 = 1, d2 = 2;
                OyuncuSirasi = OyuncuSirasi + 1;
                OyuncuSirasiBelirleme();
                TiklamaSayisi = 0;
            }


        }
    }
}
