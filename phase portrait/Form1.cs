using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace third
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        double w = 10;
        private void Form1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f); //цвет очистки экрана
            GL.MatrixMode(MatrixMode.Projection);   //матрица проекции
            GL.LoadIdentity();                      // вместо текущей матрицы загружается единичная матриц
            GL.Ortho(-w, 3*w, -w, w, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);    //модельновидовая матрица (текущая). Отвечает за геом.преобразования. Сначала все точки (vector) умножаются на модельновидовую матрицу слева (на единичную), затем умножается на матрицу проекции слева. Т.о. происходит отображение точек так как они есть.
                                                    //Существуют функции которые изменяют проекцию вектора. GL.Rotate() - поворот на угол вокруг вектора. GL.Translate() - параллельный перенос на вектор (ко всем координатам будет прибавлен вектор). GL.Scale() - масштабирование. Все эти преобразования являются матрицами преобразований, на которые умножается модельновидовая матрица справа.
                                                    //Преобразования выполняются в обратном порядке вызова, т.к. домножаюся на текущую матрицу справа. Преобразования накапливаются.
                                                    //Все эти матрицы 4х4. Можно самим создавать (определять матрицы)
                                                    //Если нужно сохранить результат, чтобы далее по-разному его преобразовывать, есть команды GL.PushMatrix() - сохранить текущую матрицу, GL.PopMatrix() - удалить текущую матрицу.
            GL.LoadIdentity();
        }

        void Coord()
        {
            double h1 = w / 2; // малые деления
            double h2 = h1 / 5; // средние деления

            GL.LineWidth(1.0f);

            GL.Begin(PrimitiveType.Lines);

            // малые разделения
            GL.Color3(0.2f, 0.2f, 0.2f);

            for (double x = 0; x <= w; x += h2)
            {
                // по 4 линии за шаг
                GL.Vertex2(x, -w);
                GL.Vertex2(x, w);
                GL.Vertex2(-x, -w);
                GL.Vertex2(-x, w);
                GL.Vertex2(-w, x);
                GL.Vertex2(w, x);
                GL.Vertex2(-w, -x);
                GL.Vertex2(w, -x);
            }

            // средние разделения
            GL.Color3(0.45f, 0.45f, 0.45f);

            for (double x = 0; x <= w; x += h1)
            {
                // по 4 линии за шаг
                GL.Vertex2(x, -w);
                GL.Vertex2(x, w);
                GL.Vertex2(-x, -w);
                GL.Vertex2(-x, w);
                GL.Vertex2(-w, x);
                GL.Vertex2(w, x);
                GL.Vertex2(-w, -x);
                GL.Vertex2(w, -x);
            }

            // оси
            GL.Color3(1.0f, 1.0f, 1.0f);

            GL.Vertex2(-w, 0);
            GL.Vertex2(w, 0);
            GL.Vertex2(0, -w);
            GL.Vertex2(0, w);

            GL.End();

            // стрелочки (вытянуты и их длина зависит от области видимости)
            GL.Begin(PrimitiveType.Triangles);
            // по оси х
            GL.Vertex2(w, 0);
            GL.Vertex2(w - w / 20, w / 40);
            GL.Vertex2(w - w / 20, -w / 40);
            // по оси у
            GL.Vertex2(0, w);
            GL.Vertex2(w / 40, w - w / 20);
            GL.Vertex2(-w / 40, w - w / 20);

            GL.End();
        }

        


        int N = 400;

        double x0 = 1;
        double y0 = 1;


        double alpha = 0;
        double beta = 40;
        double T;
        double C1(double x0, double y0)
        {
            return 4 * y0 - 2 * x0;
        }

        double C2(double x0, double y0)
        {
            return 2 * x0 - 3 * y0;
        }

        double x_common(double t)
        {
            return C1(x0, y0) * Math.Exp(-t) * 3 / 2 + C2(x0, y0) * Math.Exp(-t / 2) * 2;
        }

        double y_common(double t)
        {
            return C1(x0, y0) * Math.Exp(-t) + C2(x0, y0) * Math.Exp(-t / 2);
        }
        double a_11, a_12, a_21, a_22;
        double func_1(double x, double y)
        {
            ///return x - 3 * y;
            return a_11 * x*x -  a_12 * y;
        }

        double func_2(double x, double y)
        {
            ///return x - 5 * y / 2;
            return a_21 * x - a_22 * y*y;

        }

        /// y1 = x; y2 = y; y3 = x_; y4 = y_;

        /// y1_ = y3; y2_ = y4; y3_ =  a_11 * x -  a_12 * y; y4_ = a_21 * x - a_22 * ;



        double g1(double y1, double y2, double y3, double y4)
        {
            return y3;
        }
        double g2(double y1, double y2, double y3, double y4)
        {
            return y4;
        }

        
         double g3(double y1, double y2, double y3, double y4)
        {
            return a_11 * y1*y1 - a_12 * y2;
        }
        double g4(double y1, double y2, double y3, double y4)
        {
            return a_21 * y1 - a_22 * y2*y2;
        }

        double y1_0 = 1;
        double y2_0 = 1;
        double y3_0 = 0;
        double y4_0 = 0;

        
        double[,] GetAproximateSolution1(double y1_0, double y2_0, double y3_0, double y4_0)
        {
            ///double T = beta - alpha;
            double[,] AprS = new double[N + 1, 4];
            double dt = T / N;
            AprS[0, 0] = y1_0;
            AprS[0, 1] = y2_0;
            AprS[0, 2] = y3_0;
            AprS[0, 3] = y4_0;

            for (int i = 0; i<N; i++)
            {
                AprS[i + 1, 0] = AprS[i, 0] + dt * g1(AprS[i, 0], AprS[i, 1], AprS[i, 2], AprS[i, 3]);
                AprS[i + 1, 1] = AprS[i, 1] + dt * g2(AprS[i, 0], AprS[i, 1], AprS[i, 2], AprS[i, 3]);
                AprS[i + 1, 2] = AprS[i, 2] + dt * g3(AprS[i, 0], AprS[i, 1], AprS[i, 2], AprS[i, 3]);
                AprS[i + 1, 3] = AprS[i, 3] + dt * g4(AprS[i, 0], AprS[i, 1], AprS[i, 2], AprS[i, 3]);
            }

            return AprS;
        }
        List<double[,]> Fase_Portrait1;
        List<double[,]> Fase_Portrait0;
        double x_0 = 1;
        double y_0 = 0;
        double[,] GetAproximateSolution0(double x_0, double y_0)
        {
            ///double T = beta - alpha;
            double[,] AprS = new double[N + 1, 2];
            double dt = T / N;
            AprS[0, 0] = x_0;
            AprS[0, 1] = y_0;
            

            for (int i = 0; i < N; i++)
            {
                AprS[i + 1, 0] = AprS[i, 0] + dt * func_1(AprS[i, 0], AprS[i, 1]);
                AprS[i + 1, 1] = AprS[i, 1] + dt * func_2(AprS[i, 0], AprS[i, 1]);
            }

            return AprS;
        }

        void Calc_Fase_Portrait1()
        {
            Fase_Portrait1 = new List<double[,]>();

            for (int y1 = -10; y1 <= 10; y1++)
            {
                for (int y2 = -10; y2 <= 10; y2++)
                {
                    
                    Fase_Portrait1.Add(GetAproximateSolution1(y1, y2, y3_0, y4_0));
                }
            }
        }
        void Calc_Fase_Portrait0()
        {
            Fase_Portrait0 = new List<double[,]>();

            for(int x = -10; x<=10; x++)
            {
                for(int y=-10; y<=10; y++)
                {
                    Fase_Portrait0.Add(GetAproximateSolution0(x, y));
                }
            }
        }

        void Fase_Portrait_Graphic1()
        {

            for(int i = 0; i<Fase_Portrait1.Count; i++)
            {
                GL.Begin(PrimitiveType.LineStrip);

                for (int j = 0; j < N; j++)
                {
                    if (Fase_Portrait1[i][j, 2] >= -w)
                    {
                        GL.Vertex2(Fase_Portrait1[i][j, 2], Fase_Portrait1[i][j, 3]);
                    }
                                        
                }

                GL.End();
            }
        }
        void Fase_Portrait_Graphic0()
        {

            for (int i = 0; i < Fase_Portrait0.Count; i++)
            {
                GL.Begin(PrimitiveType.LineStrip);

                for (int j = 0; j < N; j++)

                {
                    if(Fase_Portrait0[i][j, 0] < w)
                    {
                        GL.Vertex2(Fase_Portrait0[i][j, 0], Fase_Portrait0[i][j, 1]);
                    }
                    


                }

                GL.End();
            }
        }
        /*void AprSGraphic()
        {
            double[,] x = GetAproximateSolution(x0, y0);

            GL.Color3(1f, 0.0f, 0.5f);
            GL.LineWidth(2);

            GL.Begin(PrimitiveType.LineStrip);

            for (int i = 0; i < N; i++)
            {
                GL.Vertex2(x[i, 0], x[i, 1]);
            }

            GL.End();
        }*/
        int N1 = 200;
        void RealSGraphic()
        {
            ///double T = beta - alpha;
            double dt = T / N1;

            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.LineWidth(2);

            GL.Begin(PrimitiveType.LineStrip);

            for (double t = 0; t < T; t += dt)
            {
                GL.Vertex2(x_common(t), y_common(t));
            }

            GL.End();
        }

        double[,] AprS;
        

        private void button1_Click(object sender, EventArgs e)
        {
            ///Draw();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {

        }

        void Draw()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit); // очистка буфера цвета

            Coord();

            GL.PushMatrix();

            GL.Scale(2, 2, 1);

            ///AprSGraphic();
            ///RealSGraphic();
            GL.PopMatrix();
            Fase_Portrait_Graphic0();

            GL.PushMatrix();
            GL.Translate(2 * w, 0, 0);
            Coord();
            Fase_Portrait_Graphic1();
            GL.PopMatrix();

            glControl1.SwapBuffers(); //перемена буфера местами

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            x0 = trackBar1.Value;
            y0 = trackBar2.Value;
            N = trackBar3.Value;
            T = trackBar4.Value;
            a_11 = trackBar5.Value / 100.0;
            label3.Text = "a_11: " + a_11.ToString();
            a_12 = trackBar6.Value / 100.0;
            label4.Text = "a_12: " + a_12.ToString();
            a_21 = trackBar7.Value / 100.0;
            label5.Text = "a_21: " + a_21.ToString();
            a_22 = trackBar8.Value / 100.0;
            label6.Text = "a_22: " + a_22.ToString();

            Calc_Fase_Portrait1();
            Calc_Fase_Portrait0();

            Draw();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
