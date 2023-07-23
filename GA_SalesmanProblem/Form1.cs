using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace GA_SalesmanProblem
{
    public partial class Form1 : Form
    {
        bool b = false;
        Comisvoiajor cv;
        Graphics gr;
        SolidBrush Mb;
        SolidBrush Mb1;
        Point[] p;
        Pen Mp;
           
        public Form1()
        {
            InitializeComponent();
            gr = pictureBox1.CreateGraphics();//crearea graficii
            Mb1 = new SolidBrush(System.Drawing.Color.White);
            Mb = new SolidBrush(System.Drawing.Color.Red);
            Mp = new Pen(Color.Green, 1);       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int x1, x2;
            double x3, x4;
            x1 = Convert.ToInt32(textBox1.Text);
            x2 = Convert.ToInt32(textBox2.Text);
            x3 = Convert.ToDouble(textBox3.Text);
            x4 = Convert.ToDouble(textBox4.Text);
            cv = new Comisvoiajor(x1,x2,x3,x4);//construim clasa cu parametrii indicati
            p = new Point[cv.Norase];
            for (int j = 0; j < cv.Norase; j++)
                p[j] = new Point(cv.OrCoordonate[cv.Pop[0, j], 0] + 3, cv.OrCoordonate[cv.Pop[0, j], 1] + 3);
            b = true;
            desenare();
            label9.Text = "";
            label11.Text = Convert.ToString(cv.PopFitness[0]);
            button2.Enabled = true;//button2 enabled
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            button2.Focus();
            progressBar1.Value = 0;
        }

        private void desenare()
        {
            gr.FillRectangle(Mb1, 0, 0, 310, 310);
            for (int i = 0; i < p.Length; i++)//desenarea oraselor
              gr.FillEllipse(Mb, p[i].X-3, p[i].Y-3, 7, 7); 
            //desenarea drumului
            gr.DrawPolygon(Mp, p);
            //afisarea inf           
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Comisvoiajor cv1;
            int x2, gen;
            double x3, x4;
            p = new Point[cv.Norase];
            x2 = Convert.ToInt32(textBox2.Text);
            x3 = Convert.ToDouble(textBox3.Text);
            x4 = Convert.ToDouble(textBox4.Text);
            cv1 = new Comisvoiajor(cv, x2, x3, x4);
            gen = Convert.ToInt32(textBox5.Text);//cite generatii
            
            for (int i = 0; i < gen; i++)
            {
                cv1.crosingover();
                cv1.mutatie();
                cv1.ordonare();
                progressBar1.Value=i*100/gen;
            }
            progressBar1.Value = 100;
            
            for (int j = 0; j < cv1.Norase; j++)
                p[j] = new Point(cv1.OrCoordonate[cv1.Pop[0, j], 0] + 3, cv1.OrCoordonate[cv1.Pop[0, j], 1] + 3);
            desenare();
            label9.Text = Convert.ToString(cv1.PopFitness[0]);

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (b == true)
            {  //desenare();
                e.Graphics.FillRectangle(Mb1, 0, 0, 310, 310);
                for (int i = 0; i < p.Length; i++)//desenarea oraselor
                    e.Graphics.FillEllipse(Mb, p[i].X - 3, p[i].Y - 3, 7, 7);
                //desenarea drumului
                e.Graphics.DrawPolygon(Mp, p);
                //afisarea inf      
            }
        }


    }
}
