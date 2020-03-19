using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Suffix_Tree_CS_Forms
{
    class DrawTree
    {
        Bitmap bitmap;
        public Pen blackPen;
        Pen redPen;
        //Pen darkGoldPen;
        public Graphics gr;
        Font font;
        Brush brush;
        PointF point;
        public int R = 20; //радиус окружности вершины
        string input;


        public DrawTree(int width, int height, string input)
        {
            bitmap = new Bitmap(width, height);
            gr = Graphics.FromImage(bitmap);
            clearSheet();
            blackPen = new Pen(Color.Black);
            blackPen.Width = 2;
            redPen = new Pen(Color.Red);
            redPen.Width = 2;
            //darkGoldPen = new Pen(Color.DarkGoldenrod);
            //darkGoldPen.Width = 2;
            font = new Font("Arial", 15);
            brush = Brushes.Black;
            this.input = input;
        }

        public void clearSheet()
        {
            gr.Clear(Color.White);
        }

        public Bitmap GetBitmap()
        {
            return bitmap;
        }

        public void DrawNode(int x, int y)
        {
            gr.FillEllipse(Brushes.White, (x - R), (y - R), 2 * R, 2 * R);
            gr.DrawEllipse(blackPen, (x - R), (y - R), 2 * R, 2 * R);
            //point = new PointF(x - 9, y - 9);
            //gr.DrawString(number, fo, br, point);
        }

        public void DrawNode(PointF center)
        {
            gr.FillEllipse(Brushes.White, (center.X - R), (center.Y - R), 2 * R, 2 * R);
            gr.DrawEllipse(blackPen, (center.X - R), (center.Y - R), 2 * R, 2 * R);
            //point = new PointF(x - 9, y - 9);
            //gr.DrawString(number, fo, br, point);
        }


        public void DrawList(int x, int y, int listNum)
        {
            gr.FillEllipse(Brushes.White, (x - R), (y - R), 2 * R, 2 * R);
            gr.DrawEllipse(blackPen, (x - R), (y - R), 2 * R, 2 * R);
            point = new PointF(x - 9, y - 9);
            gr.DrawString(listNum.ToString(), font, brush, point);
        }

        public void DrawList(PointF center, int listNum)
        {
            gr.FillEllipse(Brushes.White, (center.X - R), (center.Y - R), 2 * R, 2 * R);
            gr.DrawEllipse(blackPen, (center.X - R), (center.Y - R), 2 * R, 2 * R);
            point = new PointF(center.X - 9, center.Y - 9);
            gr.DrawString(listNum.ToString(), font, brush, point);
        }


        public void DrawEdge(SuffixNode node1, SuffixNode node2)
        {
            gr.DrawLine(blackPen, node1.center, node2.center);
            if (node1.listNum != -1)
            {
                //drawTree.DrawList(x, y, currentNode.listNum);
                DrawList(node1.center, node1.listNum);
            }
            else
            {
                DrawNode(node1.center);
            }

            if (node2.listNum != -1)
            {
                //drawTree.DrawList(x, y, currentNode.listNum);
                DrawList(node2.center, node2.listNum);
            }
            else
            {
                DrawNode(node2.center);
            }

            if (node2.length != -1)
            {
                double length = Math.Sqrt(Math.Pow(node2.center.X - node1.center.X, 2) + Math.Pow(node2.center.Y - node1.center.Y, 2));


                double x = node1.center.X;
                double y = node1.center.Y;

                double dX = node2.center.X - node1.center.X;
                double dY = node2.center.Y - node1.center.Y;

                double delta = length / node2.length;

                double deltaX = dX / (node2.length + 2);
                double deltaY = dY / (node2.length + 2);

                x += deltaX;
                y += deltaY;

                for (int i = node2.position; i < node2.position + node2.length; i++)
                {

                    gr.DrawString(input[i].ToString(), font, brush, Convert.ToSingle(x - 9), Convert.ToSingle(y - 9));
                    x += deltaX;
                    y += deltaY;
                }
            }
            
        }

        public void DrawSelectedLeaf(SuffixNode node)
        {

            gr.DrawEllipse(redPen, (node.center.X - R), (node.center.Y - R), 2 * R, 2 * R);
        }
    }
}
