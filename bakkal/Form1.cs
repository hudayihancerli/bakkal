using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bakkal
{
    public partial class Form1 : Form
    {
        MarketEntities marketEntities = new MarketEntities();
        PrintDialog printDialog = new PrintDialog();
        public Form1()
        {
            InitializeComponent();
        }

        //fonksiyon - start
        public void fisIptal()
        {
            try
            {
                if (marketEntities.sepetTbs.Count() > 0)
                {
                    foreach (var item in marketEntities.sepetTbs)
                    {
                        marketEntities.sepetTbs.Remove(item);
                    }
                    marketEntities.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Sepetiniz boş :D");
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Hata : " + e.Message);
            }
        }

        public void fiyat()
        {
            try
            {
                int sum = 0;
                if (marketEntities.sepetTbs.Count() > 0)
                {
                    foreach (var item in marketEntities.sepetTbs)
                    {
                        sum += Convert.ToInt32(item.sepetFiyat);
                    }
                }
                label4.Text = sum.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show("Hata : " + e.Message);
            }
        }

        public void sepetList()
        {
            try
            {
                var list = marketEntities.sepetTbs.ToList();
                dataGridView1.DataSource = list.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show("Hata : " + e.Message);
            }
        }

        public void sepetAdd()
        {
            try
            {
                sepetTb sepetTablosu = new sepetTb();
                string inputBarcode = barcode.Text;
                if (inputBarcode.Length == 13)
                {
                    var urun = marketEntities.urunler.Where(x => x.urunBarkod.Contains(inputBarcode)).FirstOrDefault();
                    if (urun == null)
                    {
                        MessageBox.Show("ürün bulunamadı");
                    }
                    else
                    {
                        sepetTablosu.sepetBarkod = urun.urunBarkod;
                        sepetTablosu.sepetAd = urun.urunAd;
                        sepetTablosu.sepetFiyat = urun.urunFiyat;
                        sepetTablosu.sepetStok = urun.urunStok;
                        marketEntities.sepetTbs.Add(sepetTablosu);
                        marketEntities.SaveChanges();
                    }
                    sepetList();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Hata : " + e.Message);
            }
        }

        public void satirIptal()
        {
            try
            {
                if (marketEntities.sepetTbs.Count() > 0)
                {
                    int secilenId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                    var secilen_urun = marketEntities.sepetTbs.First(x => x.sepetID == secilenId);
                    marketEntities.sepetTbs.Remove(secilen_urun);
                    marketEntities.SaveChanges();
                    sepetList();
                }
                else
                {
                    MessageBox.Show("Silinecek Öğeyi bulunamadı.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Hata : " + e.Message);
            }
        }

        public void fiyatla_add()
        {
            try
            {
                string barcodeLen = barcode.Text;

                if (barcodeLen.Length < 5 && barcodeLen.Length > 0)
                {
                    sepetTb sepetTablosu = new sepetTb();
                    sepetTablosu.sepetBarkod = "0000000000000";
                    sepetTablosu.sepetAd = "Barkodsuz ürün";
                    sepetTablosu.sepetFiyat = barcodeLen;
                    sepetTablosu.sepetStok = "1";
                    marketEntities.sepetTbs.Add(sepetTablosu);
                    marketEntities.SaveChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Hata : " + e.Message);
            }
        }
        public void add_num(string str, string process)
        {
            try
            {
                switch (process)
                {
                    case "add": barcode.Text += str; break;
                    case "delete":
                        if (barcode.Text.Length > 0)
                        {
                            int sonHarf = barcode.Text.Length - 1;
                            barcode.Text = barcode.Text.Substring(0, sonHarf);
                        }
                        break;
                    case "clear": barcode.Text = ""; break;
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
                Pen pencil2 = new Pen(Color.Black, 2);
                Font font = new Font("Arial", 12);
                Font font1 = new Font("Arial", 8);
                Font font2 = new Font("Arial", 28);
                sepetTb sepetTb = new sepetTb();
                SolidBrush pencil = new SolidBrush(Color.Black);

                int y = 0;
                int secilenId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                var secilen_urun = marketEntities.sepetTbs.First(x => x.sepetID == secilenId);
                var printObje = marketEntities.sepetTbs.FirstOrDefault(x => x.sepetID == secilenId);



                e.Graphics.DrawRectangle(pencil2, 4, y, 140, 100);
                y += 10;
                if (printObje.sepetAd.Length > 15)
                {
                    e.Graphics.DrawString(printObje.sepetAd, font1, pencil, 4, y);
                }
                else
                {
                    e.Graphics.DrawString($"{printObje.sepetAd}", font, pencil, 10, y);
                }
                e.Graphics.DrawString($"{printObje.sepetFiyat}₺", font2, pencil, 10, 20 + y);
                e.Graphics.DrawString($"{printObje.sepetBarkod}", font, pencil, 10, 70 + y);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata : " + ex.Message);
            }
        }


        //fonksiyon - end

        private void karakter_urunler_Click(object sender, EventArgs e)
        {
            tumurunler tumurunler = new tumurunler();
            tumurunler.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sepetList();
            fiyat();
        }

        private void barcode_TextChanged(object sender, EventArgs e)
        {
            sepetAdd();
            sepetList();
            fiyat();
        }

        private void btn_fis_iptal_Click(object sender, EventArgs e)
        {
            fisIptal();
            sepetList();
            fiyat();

            //print


        }

        private void btn_satir_iptal_Click(object sender, EventArgs e)
        {
            satirIptal();
            sepetList();
            fiyat();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            fiyatla_add();
            sepetList();
            fiyat();
        }

        private void karakter_1_Click(object sender, EventArgs e)
        {
            add_num("1", "add");
        }

        private void karakter_2_Click(object sender, EventArgs e)
        {
            add_num("2", "add");
        }

        private void karakter_3_Click(object sender, EventArgs e)
        {
            add_num("3", "add");
        }
        private void karakter_4_Click(object sender, EventArgs e)
        {
            add_num("4", "add");
        }

        private void karakter_5_Click(object sender, EventArgs e)
        {
            add_num("5", "add");
        }

        private void karakter_6_Click(object sender, EventArgs e)
        {
            add_num("6", "add");
        }

        private void karakter_7_Click(object sender, EventArgs e)
        {
            add_num("7", "add");
        }

        private void karakter_8_Click(object sender, EventArgs e)
        {
            add_num("8", "add");
        }

        private void karakter_9_Click(object sender, EventArgs e)
        {
            add_num("9", "add");
        }

        private void karakter_0_Click(object sender, EventArgs e)
        {
            add_num("0", "add");
        }

        private void karakter_clear_Click(object sender, EventArgs e)
        {
            add_num("0", "clear");
        }

        private void karakter_delete_Click(object sender, EventArgs e)
        {
            add_num("0", "delete");
        }

        private void karakter_fis_Click(object sender, EventArgs e)
        {

            print_fis();
        }
    }
}
