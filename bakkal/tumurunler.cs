using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Printing;

namespace bakkal
{
    public partial class tumurunler : Form
    {
        MarketEntities marketEntities = new MarketEntities();
        PrintDialog printDialog = new PrintDialog();
        public tumurunler()
        {
            InitializeComponent();
        }

        //fonksiyon - start
        public void list()
        {
            try
            {
                var liste = marketEntities.urunler.ToList();
                dataGridView1.DataSource = liste.ToList();
            }
            catch (Exception e)
            {

                MessageBox.Show("Hata : " + e.Message);
            }

        }

        public void add_urun()
        {



            try
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                {
                    MessageBox.Show("Lütfen kutucukları boş bırakmayınız.");
                }
                else
                {
                    urunTablosu urunTablosu = new urunTablosu();
                    urunTablosu.urunBarkod = Convert.ToString(textBox1.Text);
                    urunTablosu.urunAd = Convert.ToString(textBox2.Text);
                    urunTablosu.urunFiyat = Convert.ToString(textBox3.Text);
                    urunTablosu.urunStok = Convert.ToString(textBox4.Text);
                    marketEntities.urunler.Add(urunTablosu);
                    marketEntities.SaveChanges();
                    MessageBox.Show("Kayıt başarılı :) ");
                    clearTextbox();

                }

            }
            catch (Exception e)
            {

                MessageBox.Show("Hata : " + e.Message);
            }

        }

        public void clearTextbox()
        {
            try
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
            }
            catch (Exception e)
            {

                MessageBox.Show("Hata : " + e.Message);
            }


        }

        public void update_urun()
        {

            try
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                {
                    MessageBox.Show("Lütfen kutucukları boş bırakmayınız.");
                }
                else
                {
                    int secilenId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                    var guncellenecek_urun = marketEntities.urunler.First(x => x.urunID == secilenId);
                    guncellenecek_urun.urunBarkod = Convert.ToString(textBox1.Text);
                    guncellenecek_urun.urunAd = Convert.ToString(textBox2.Text);
                    guncellenecek_urun.urunFiyat = Convert.ToString(textBox3.Text);
                    guncellenecek_urun.urunStok = Convert.ToString(textBox4.Text);
                    marketEntities.SaveChanges();
                    list();
                }

            }
            catch (Exception e)
            {

                MessageBox.Show("Hata : " + e.Message);
            }
        }

        public void sec_urun()
        {
            try
            {
                int secilenId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                var secilen_urun = marketEntities.urunler.First(x => x.urunID == secilenId);
                textBox1.Text = secilen_urun.urunBarkod;
                textBox2.Text = secilen_urun.urunAd;
                textBox3.Text = secilen_urun.urunFiyat;
                textBox4.Text = secilen_urun.urunStok;
            }
            catch (Exception e)
            {

                MessageBox.Show("Hata : " + e.Message);
            }

        }

        public void delete_urun()
        {
            try
            {
                int secilenId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                var silinen_urun = marketEntities.urunler.First(x => x.urunID == secilenId);
                marketEntities.urunler.Remove(silinen_urun);
                marketEntities.SaveChanges();
                list();
                MessageBox.Show("Ürün silindi :(");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        public void search()
        {
            try
            {
                string inputBarcode = barcode.Text;
                if (inputBarcode.Length <= 13)
                {
                    var urun = marketEntities.urunler.Where(x => x.urunBarkod.Contains(inputBarcode)).ToList();
                    dataGridView1.DataSource = urun.ToList();
                }
                else
                {
                    MessageBox.Show("Lütfen 13 karakterden kısa bir barkod giriniz.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Hata : " + e.Message);
            }

        }

        public void print_fis()
        {
            try
            {
                PrintDocument paper = new PrintDocument();
                PaperSize boyut = new PaperSize("pprnm", 285, 600);
                paper.DefaultPageSettings.PaperSize = boyut;
                DialogResult yazdir;
                yazdir = printDialog.ShowDialog();
                paper.PrintPage += Paper_PrintPage;
                if (yazdir == DialogResult.OK)
                {
                    paper.Print();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Hata : " + e.Message);
            }

        }

        private void Paper_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Font font = new Font("Arial", 12);
                Font font1 = new Font("Arial", 8);
                Font font2 = new Font("Arial", 28);
                SolidBrush pencil = new SolidBrush(Color.Black);
                Pen pencil2 = new Pen(Color.Black, 2);
                int y = 4;

                foreach (var item in marketEntities.urunler)
                {
                    e.Graphics.DrawRectangle(pencil2, 4, y, 140, 100);
                    y += 10;
                    if (item.urunAd.Length > 15)
                    {
                        e.Graphics.DrawString(item.urunAd, font1, pencil, 4, y);
                    }
                    else
                    {
                        e.Graphics.DrawString($"{item.urunAd}", font, pencil, 10, y);
                    }
                    e.Graphics.DrawString($"{item.urunFiyat}₺", font2, pencil, 10, 20 + y);
                    e.Graphics.DrawString($"{item.urunBarkod}", font, pencil, 10, 70 + y);
                    y += 100;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata : " + ex.Message);
            }}

        

        //fonksiyon - end

        private void tumurunler_Load(object sender, EventArgs e)
        {
            list();
        }

        private void barcode_TextChanged(object sender, EventArgs e)
        {
            search();
        }

        private void urun_sil_Click(object sender, EventArgs e)
        {
            delete_urun();
        }

        private void urun_etiket_Click(object sender, EventArgs e)
        {
            print_fis();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yedek eklenmedi :)");
        }

        private void button47_Click(object sender, EventArgs e)
        {
            add_urun();
            list();
        }

        private void urun_sec_Click(object sender, EventArgs e)
        {
            sec_urun();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            update_urun();
        }

        private void button34_Click(object sender, EventArgs e)
        {
            clearTextbox();
        }
    }
}
