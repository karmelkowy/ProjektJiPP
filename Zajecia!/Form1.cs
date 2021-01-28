using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Projekt
{
    public partial class Form1 : Form
    {

        SimEngine sim;

        public Form1()
        {
            InitializeComponent();
            sim = new SimEngine(panel1);

            this.SetupGui();

        }

        void SetupGui()
        {
            chart1.Series["Series1"].Points.Clear();
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 5;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 5;
            chart1.ChartAreas[0].AxisX.Title = "Speed";
            chart1.ChartAreas[0].AxisY.Title = "Sense";
            Font font = new Font("Arial", 20, FontStyle.Bold);
            chart1.ChartAreas[0].AxisX.TitleFont = font;
            chart1.ChartAreas[0].AxisY.TitleFont = font;

            label1.Text = "Jedzenie: " + trackBar1.Value.ToString();
            label2.Text = "Ilość osobników: 0";
            label3.Text = "Epoka: 0";

        }

        void updateChart()
        {
            List<float[]> Data = sim.getPlotData();

            label2.Text = "Ilość osobników: " + Data.Count.ToString();
            label3.Text = "Epoka: " + this.sim.Epoh.ToString();

            chart1.Series["Series1"].Points.Clear();
            foreach (float[] data in Data)
            {
                chart1.Series["Series1"].Points.AddXY(data[0], data[1]);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            sim.NextEpoh();
            this.updateChart();

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = "Jedzenie: " + trackBar1.Value.ToString();
            sim.FoodAmount = trackBar1.Value;
        }

    }
}
