using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace coding
{
    public partial class Form1 : Form
    {
        class row
        {
            public double time;
            public double Velocity;
            public double acceleration;
            public double VelocityDerivative;
            public double AltitudeDerivative;
            public double Altitude;
        }

        List<row> table = new List<row>();
        private string imageFileName;

        void tableSort()
        {
            table = table.OrderBy(x => x.time).ToList();
        }


        void derivative()
        // this calculates velocity
        {
            for (int i = 1; i < table.Count; i++)

            {
                double dA = table[i].Altitude - table[i - 1].Altitude;
                double dt = table[i].time - table[i - 1].time;
                table[i].AltitudeDerivative = dA / dt;
                table[i].Velocity = table[i].AltitudeDerivative;
            }
        }

        void secondderivative()
        //this calculates acceleration
        {

            for (int i = 1; i < table.Count; i++)
            {
                double dA = table[i].Velocity - table[i - 1].Velocity;
                double dt = table[i].time - table[i - 1].time;
                table[i].VelocityDerivative = dA / dt;
            }

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        //This is open the file on menu strip
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "CSV Files|*.csv";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {


                        {
                            string line = sr.ReadLine();
                            while (!sr.EndOfStream)
                            {
                                table.Add(new row());
                                string[] l = sr.ReadLine().Split(',');
                                table.Last().time = double.Parse(l[0]);
                                table.Last().Altitude = double.Parse(l[1]);

                            }

                        }
                    }

                }

                catch (IOException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " Failed to open");

                }
                catch (FormatException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " is not in the requiered format");
                }

                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show(openFileDialog1.FileName + " is not in the required format");
                }
            }

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void AltitudeTimeToolStripMenuItem_Click(object sender, EventArgs e)
        //This works out altitude and puts it in a graph
        {
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series1 = new Series
            {
                Name = "Points",
                Color = Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2

            };

            chart1.Series.Add(series1);
            for (int i = 0; i < table.Count; i++)
            {
                series1.Points.AddXY(table[i].time, table[i].Altitude);
            }

            chart1.ChartAreas[0].AxisX.Title = "time / s";
            chart1.ChartAreas[0].AxisY.Title = "Altitude / V";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }


        private void VelocityTimeToolStripMenuItem_Click(object sender, EventArgs e)
        //This works out velocity and puts on a graph
        {
            derivative();

            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series1 = new Series
            {
                Name = "Points",
                Color = Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2

            };

            chart1.Series.Add(series1);
            for (int i = 0; i < table.Count; i++)
            {
                series1.Points.AddXY(table[i].time, table[i].AltitudeDerivative);
            }

            chart1.ChartAreas[0].AxisX.Title = "time / s";
            chart1.ChartAreas[0].AxisY.Title = "Velocity / V";
            chart1.ChartAreas[0].RecalculateAxesScale();
        }

        private void saveGraphToolStripMenuItem_Click(object sender, EventArgs e)
        //This saves CSV file
        {
            chart1.SaveImage(imageFileName, ChartImageFormat.Png);
        }

        private void accelarationToolStripMenuItem_Click(object sender, EventArgs e)
        ////This works out acceleration and puts on a graph
        {
            secondderivative();

            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            Series series1 = new Series
            {
                Name = "Points",
                Color = Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Spline,
                BorderWidth = 2

            };

            chart1.Series.Add(series1);
            for (int i = 0; i < table.Count; i++)
            {
                series1.Points.AddXY(table[i].time, table[i].VelocityDerivative);
            }
            chart1.ChartAreas[0].AxisX.Title = "time / s";
            chart1.ChartAreas[0].AxisY.Title = "Acceleration / V";
            chart1.ChartAreas[0].RecalculateAxesScale();
            
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        //This saves it to PNG file
        {

        }
    }
}


