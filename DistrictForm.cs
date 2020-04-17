using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xxjjyx.UCControl;

namespace xxjjyx
{
    public partial class DistrictForm : Form
    {
        private static DistrictForm frm = null;
        private DistrictForm()
        {
            InitializeComponent();
            this.TopMost = true;
        }
        /// <summary>
        /// District单例模式
        /// </summary>
        /// <returns></returns>
        public static DistrictForm CreateInstrance()
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new DistrictForm();
            }
            return frm;
        }
        /// <summary>
        /// 绘制的画板
        /// </summary>
        Bitmap bmp;
        /// <summary>
        /// 绘制工具
        /// </summary>
        Graphics g;
        /// <summary>
        /// 当前选择的place
        /// </summary>
        Place p;

        private void District_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 展示place
        /// </summary>
        public void ShowDistrictInfo(Place p)
        {
            this.p = p;
            if (p.TerrainType == -1)
            {
                //List<Place> nearPlaces=  Draw.GetAroundPlaceList(Globle.AllPlaceList, Globle.SelectPlace);
                this.Close();
                return;
            }
            button1.Enabled = Globle.AllHumansList.Exists(o => o.SetPlace == p) == false ? false : true;
            //p = Globle.SelectPlace;
            //string temp = "";
            //foreach (var item in p.DistrictList)
            //{
            //    temp += "(" + item.CrossingTime + "|" + Globle.DistFeature[item.TypeIndex] + ")";
            //}
            label1.Text = p.PlaceName + "|人口合计(" + p.Population + "/" + p.MaxPoplation + ")|统治力度(" + p.OrderValue + "/100)";
            if (p.IsExplore == false)
            {
                label2.Text = "地域未探索。";
                label2.ForeColor = Color.Blue;
            }
            else
            {
                label2.Text = "地域已探索。";
                label2.ForeColor = Color.Black;
            }
            DrawDistrict(p);
            ShowReiki();
        }
        /// <summary>
        /// 绘制方法
        /// </summary>
        /// <param name="p"></param>
        private void DrawDistrict(Place p)
        {
            int width = pictureBox1.Width / 3;
            int height = pictureBox1.Height / 3;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);

            pictureBox1.BackgroundImage = bmp;
            int ii = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //int ii = (i + 1) * (j + 1) - 1;
                    District dist = p.DistrictList[ii];
                    dist.Rect = new Rectangle(width * i, height * j, width, height);
                    //Bitmap bitmap = new Bitmap(image);
                    //for (int h = 0; h < bitmap.Height; h++)
                    //{
                    //    for (int w = 0; w < bitmap.Width; w++)
                    //    {
                    //        Color c = bitmap.GetPixel(w, h);
                    //        if (dist.Build != null)//有建筑则虚化背景
                    //        {
                    //            bitmap.SetPixel(w, h, Color.FromArgb(150, c.R, c.G, c.B));//色彩度最大为255，最小为0,改变透明度100
                    //        }
                    //        else
                    //        {
                    //            bitmap.SetPixel(w, h, Color.FromArgb(200, c.R, c.G, c.B));//稍微虚化背景
                    //        }
                    //    }
                    //}

                    //绘制建筑
                    if (dist.Build != null)
                    {
                        int index = dist.Build.BuildProgress < dist.Build.BuildCastTime ? -1 : dist.Build.Type;
                        Image image = Image.FromFile(Application.StartupPath + "\\image\\build\\" + index + ".png");
                        g.DrawImage(image, dist.Rect);
                    }
                    else
                    {
                        Image image = Image.FromFile(Application.StartupPath + "\\image\\district\\dist (" + (dist.TypeIndex + 1) + ").PNG");
                        g.DrawImage(image, dist.Rect);
                    }
                    g.DrawRectangle(Pens.Black, dist.Rect);//画边框
                    string name = dist.Build != null ? dist.Build.Name : Globle.DistFeature[dist.TypeIndex];
                    g.DrawString(name, new Font("黑体", 10), Brushes.Black, dist.Rect.Location);
                    //if (p.TerrainType == 0)//写上区划名称
                    //{
                    //    //g.FillRectangle(Brushes.White, rect.X, rect.Y, 50, 10);
                    //    g.DrawString(Globle.Landfeature[dist.TypeIndex], new Font("黑体", 10), Brushes.Black, dist.Rect.Location);
                    //}
                    //else if (p.TerrainType == 1)
                    //{
                    //    g.DrawString(Globle.Lakefeature[dist.TypeIndex], new Font("黑体", 10), Brushes.Black, dist.Rect.Location);
                    //}
                    //else if (p.TerrainType == 2)
                    //{
                    //    g.DrawString(Globle.Oceanfeature[dist.TypeIndex], new Font("黑体", 10), Brushes.Black, dist.Rect.Location);
                    //}
                    if (p.IsExplore == true)//已探索，则标注灵脉
                    {
                        //string reikistr = dist.Reiki.Name + "(" + dist.Reiki.Level + ")";
                        Brush brush = new SolidBrush(dist.Reiki.Color);
                        Point sPoint = new Point(dist.Rect.Right - 25, dist.Rect.Top);
                        g.FillEllipse(brush, sPoint.X, sPoint.Y, 25, 25);
                        g.DrawEllipse(Pens.Black, sPoint.X, sPoint.Y, 25, 25);
                        g.DrawString(dist.Reiki.Level.ToString(), new Font("黑体", 12, FontStyle.Bold), Brushes.White, new Point(sPoint.X + 5, sPoint.Y + 5));//画灵气等级
                        //g.DrawString(reikistr, new Font("黑体", 10), Brushes.Red, new Point(dist.Rect.X, dist.Rect.Y + dist.Rect.Height - 22));
                    }
                    ii++;
                }
            }
        }

        /// <summary>
        /// 显示灵气分布
        /// </summary>
        void ShowReiki()
        {
            humanTalent1.TextTag = "火";
            humanTalent1.Value = 0;
            humanTalent2.TextTag = "土";
            humanTalent2.Value = 0;
            humanTalent3.TextTag = "金";
            humanTalent3.Value = 0;
            humanTalent4.TextTag = "水";
            humanTalent4.Value = 0;
            humanTalent5.TextTag = "木";
            humanTalent5.Value = 0;
            int max = 0;
            foreach (var dist in p.DistrictList)
            {
                if (dist.Reiki == null)
                {
                    continue;
                }
                foreach (var ht in groupBox1.Controls)
                {
                    if (dist.Reiki.Name == ((UCControl.ProgressBar)ht).TextTag)
                    {
                        ((UCControl.ProgressBar)ht).Value += dist.Reiki.Level;//加
                        ((UCControl.ProgressBar)ht).Color = dist.Reiki.Color;
                    }
                    if (((UCControl.ProgressBar)ht).Value > max)
                    {
                        max = ((UCControl.ProgressBar)ht).Value;
                    }
                }

            }
            foreach (var ht in groupBox1.Controls)
            {
                ((UCControl.ProgressBar)ht).MaxValue = max;//所有最大值赋值为max
            }


        }
        /// <summary>
        /// 用于显示额外信息
        /// </summary>
        ToolTip toolTip = new ToolTip();
        /// <summary>
        /// 当前选中的dist
        /// </summary>
        District currentDist;
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePoint = new Point(e.X, e.Y);
            //绘制边框，显示选中对象
            if (currentDist != null && !currentDist.Rect.Contains(e.Location))
            {
                //ShowDistrictInfo(p);
                g.DrawRectangle(Pens.Black, currentDist.Rect);
            }
            foreach (var dist in p.DistrictList)
            {
                if (dist.Rect.Contains(e.Location))
                {
                    currentDist = dist;
                    //Brush brush = new SolidBrush(Color.FromArgb(50, Color.White));
                    g.DrawRectangle(Pens.White, dist.Rect);
                    //brush.Dispose();
                    pictureBox1.Invalidate();
                }
            }
            toolTip.ShowAlways = true;
            foreach (var item in p.DistrictList)
            {
                if (item.Rect.Contains(mousePoint))
                {
                    string desc = "";
                    if (item.Reiki != null)
                    {
                        desc = Globle.DistFeature[item.TypeIndex] +"|"+ item.Reiki.Desc;
                        if (item.Build != null)
                        {
                            desc += "\n" + item.Build.Desc;
                        }
                        toolTip.Show(desc, this.pictureBox1, mousePoint);
                    }
                    else
                        toolTip.Show("未探索", this.pictureBox1, mousePoint);
                    break;
                }
                else
                {
                    toolTip.Hide(this.pictureBox1);
                }
            }

        }

        /// <summary>
        /// 打开任务窗口按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            HumanList humanList = HumanList.CreateInstrance();
            humanList.ShowHumanList(Globle.AllHumansList.FindAll(o => o.SetPlace == p));
            humanList.Show();
            //if (Globle.AllHumansList.FindAll(o => o.SetPlace == p).FindAll(o => o.Mission == null).Count > 0)
            //{
            //    Globle.CurrentSect = p.Sect;//确定当前选择的门派
            //    MissionSelectForm missionForm = MissionSelectForm.CreateInstrance();
            //    missionForm.ShowMissionButton(p.Sect, p, null);
            //    missionForm.Show();
            //}
            //else
            //{
            //    MessageBox.Show("该地区没有无任务弟子！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Build build = new Build(1/*, p.DistrictList[0]*/);

            //string temp = Settings1.Default.Place_Name.Replace("·", "");
            //int currentcode = -1;
            //string output = "";
            //for (int i = 0; i < temp.Length; i++)
            //{
            //    currentcode = (int)temp[i];
            //    if (currentcode > 19968 & currentcode < 40869)
            //    {
            //        output += temp[i].ToString();
            //    }
            //}
            //string output2 = "";
            //foreach (var s in output.ToArray())
            //{
            //    if (!output2.Contains(s))
            //        output2 += s;
            //}
        }
    }
}
