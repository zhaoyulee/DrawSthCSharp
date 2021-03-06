﻿//用来还原xml文件为绘图过程

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace DrawSomething
{
    class TraceToDraw
    {
        public Line drawingLine;
        public List<Line> lines;

        private Timer timer2;
        private PictureBox mainPictureBox;

        private XPathDocument doc;
        private XPathNavigator nav;
        private XPathNodeIterator ite;
        private XPathNodeIterator ite2;
        private int pointOfSectionTotalCount;
        private int pointOfSectionNowCount;

        public Color penColor;
        public int penWidth;
        public Point currentPoint;

        private string uid;
        private MainForm mainForm;

        public TraceToDraw(MainForm mainForm, Timer timer2, PictureBox mainPictureBox, string uid)
        {
            this.mainForm = mainForm;
            this.timer2 = timer2;
            this.mainPictureBox = mainPictureBox;
            this.uid = uid;
            lines = new List<Line>();
        }

        public void start()
        {
            string xmlbody = Util.getdrawxml(uid);
            File.WriteAllText(System.Environment.CurrentDirectory.ToString() + "\\Trace.xml", xmlbody, Encoding.UTF8);
            doc = new XPathDocument(System.Environment.CurrentDirectory.ToString() + "\\Trace.xml");
            nav = doc.CreateNavigator();
            ite = nav.Select("//Section");

            ite.MoveNext();
            this.penColor = Color.FromArgb(int.Parse(ite.Current.GetAttribute("penColor", "")));
            this.penWidth = int.Parse(ite.Current.GetAttribute("penWidth", ""));

            //读取第一个节点
            ite2 = ite.Current.Select("Point");
            ite2.MoveNext();
            pointOfSectionNowCount = 1;
            int x_point = int.Parse(ite2.Current.SelectSingleNode("X").Value);
            int y_point = int.Parse(ite2.Current.SelectSingleNode("Y").Value);
            currentPoint = new Point(x_point, y_point);
            drawingLine = new Line(currentPoint);
            drawingLine.penColor = this.penColor;
            drawingLine.penWidth = this.penWidth;
            lines.Add(drawingLine);

            timer2.Enabled = true;
        }

        //定时器2开启后，此函数每30毫秒会被调用一次
        public void drawit(string traceTime)
        {
            pointOfSectionTotalCount = ite2.Count+1;
            if (pointOfSectionNowCount.Equals(pointOfSectionTotalCount))
            {
                if (ite.MoveNext())
                {
                    this.penColor = Color.FromArgb(int.Parse(ite.Current.GetAttribute("penColor", "")));
                    this.penWidth = int.Parse(ite.Current.GetAttribute("penWidth", ""));

                    ite2 = ite.Current.Select("Point");
                    ite2.MoveNext();
                    pointOfSectionNowCount = 1;

                    int x_point = int.Parse(ite2.Current.SelectSingleNode("X").Value);
                    int y_point = int.Parse(ite2.Current.SelectSingleNode("Y").Value);
                    currentPoint = new Point(x_point, y_point);
                    drawingLine = new Line(currentPoint);
                    drawingLine.penColor = this.penColor;
                    drawingLine.penWidth = this.penWidth;
                    lines.Add(drawingLine);
                } 
                else
                {
                    timer2.Enabled = false;
                    MessageBox.Show("绘制完成。");
                    mainForm.traceToDrawFinish();
                }
            }
            
            string nodeTime = ite2.Current.GetAttribute("time", "");
            if (nodeTime == traceTime)
            {
                string x_point_str = ite2.Current.SelectSingleNode("X").Value.ToString();
                int x_point = int.Parse(x_point_str);
                int y_point = int.Parse(ite2.Current.SelectSingleNode("Y").Value);
                currentPoint = new Point();
                currentPoint.X = x_point;
                currentPoint.Y = y_point;
                drawingLine.Add(currentPoint);
                ite2.MoveNext();
                pointOfSectionNowCount++;
            }
            
        }
    }
}
