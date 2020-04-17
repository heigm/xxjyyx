using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xxjjyx
{
    public partial class Core : Form
    {
        public Core()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 绘制的画板(背景）
        /// </summary>
        Bitmap backbmp;
        /// <summary>
        /// 绘版，前
        /// </summary>
        Bitmap bmp;
        /// <summary>
        /// 绘制工具
        /// </summary>
        //Graphics g;
        /// <summary>
        /// 当前place
        /// </summary>
        Place currentPlace;

        private void Core_StyleChanged(object sender, EventArgs e)
        {
            //Invalidate();
        }

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            DrawMap();

            //g.Dispose();
        }

        private void DrawMap()
        {
            int radius = Settings1.Default.Draw_Radius;
            int row = Settings1.Default.Draw_Row;
            int column = Settings1.Default.Draw_Column;
            List<Point> pointList = Draw.SetHexagons(new Point(radius, radius), radius, row, column);
            backbmp = new Bitmap((pointList[column].X - pointList[0].X) * row, (pointList[1].Y - pointList[0].Y) * column + 20);//+20
            bmp= new Bitmap((pointList[column].X - pointList[0].X) * row, (pointList[1].Y - pointList[0].Y) * column + 20);//+20
            bmp.MakeTransparent(Color.White);
            Globle.G1 = Graphics.FromImage(backbmp);
            Globle.G2 = Graphics.FromImage(bmp);
            pictureBox1.BackColor = Color.Gray;
            pictureBox1.BackgroundImage = backbmp;
            pictureBox1.Image = bmp;
            pictureBox1.Size = backbmp.Size;
            //应该实现固定点位数，合理分布于picturebox中，根据画布大小变化适当缩放

            Globle.AllPlaceList = new List<Place>();
            for (int i = 0; i < pointList.Count; i++)
            {
                //g.FillEllipse(Brushes.Black, pointList[i].X, pointList[i].Y, 5, 5);
                Globle.G1.DrawString(i.ToString(), new Font("黑体", 10), Brushes.Blue, pointList[i]);
                Draw.DrawRegularPoly(Globle.G1, pointList[i], radius, 6, Pens.Black, false, Brushes.Black);//绘制
                Globle.AllPlaceList.Add(new Place(i, pointList[i], Draw.GetHexagonPoints(pointList[i], radius)));
            }
        }

        /// <summary>
        /// 鼠标单击事件,点击地块，找到地块和周围地块
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (Globle.AllPlaceList.Count <= 0) { return; }
            if (e.Button == MouseButtons.Left && e.X > 0 && e.Y > 0)
            {
                ///判断当前窗口,用于任务选择相应点
                if (Globle.IsSelect == true)
                {
                    string error = Globle.CurrentMission.JudgeError(currentPlace);
                    if (error != null)
                    {
                        MessageBox.Show(error, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    foreach (var form in Application.OpenForms)
                    {
                        if (form is MissionForm)
                        {
                            ((MissionForm)form).SetSitePlace(currentPlace);
                            ((MissionForm)form).Enabled = true;
                            Globle.IsSelect = false;
                            ((MissionForm)form).Show();
                            break;
                        }
                        else
                        {
                            //((Form)form).Enabled = true;
                            ((Form)form).Show();
                        }
                    }
                    //DistrictForm dist = DistrictForm.CreateInstrance();//换place后应重新设置区划窗口内容
                    //dist.ShowDistrictInfo(item);
                    return;
                }

                //Globle.SelectPlace = item;
                DistrictForm district = DistrictForm.CreateInstrance();//打开区划窗口
                district.ShowDistrictInfo(currentPlace);
                district.Show();
                Sect sect = Globle.AllSectList.Find(obj => obj.SectPlaceList[0].Index == currentPlace.Index);
                if (sect == null) { return; }
                //{
                SectForm sectForm = SectForm.CreateInstrance();//打开门派窗口
                sectForm.ShowSectInfo(sect);
                sectForm.Show();
                //}
                //刷新人员窗口
                if (Application.OpenForms["HumanList"] != null && sect != null)
                {
                    ((HumanList)(Application.OpenForms["HumanList"])).ShowHumanList(Globle.AllHumansList.FindAll(obj => obj.Sect == sect));
                }

            }
            else if (e.Button == MouseButtons.Right)//右键任务事件
            {
                contextMenuStrip1.Items.Clear();
                List<string> list = Mission.GetLocationMissionList(currentPlace);
                foreach (var str in list)
                {
                    contextMenuStrip1.Items.Add(str);
                }
                contextMenuStrip1.Show(pictureBox1, e.X, e.Y);
            }

        }
        /// <summary>
        /// 随机生成place事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DrawMap();
            textBox1.Text = DateTime.Now.Millisecond.ToString();
            Globle.Seed = Convert.ToInt32(textBox1.Text);
            List<Place> plainPlaceList = Terrain.GetRandomPlaces(Globle.AllPlaceList, Globle.Seed, 0.6, 8);//生成随机地块数组
            foreach (var p in plainPlaceList)
            {
                p.TerrainType = 0;
            }
            Terrain.SetUnUsedPlacesType(Globle.AllPlaceList);//设置地块地形
            Draw.DrawPlaceHexagon(Globle.G1, Globle.AllPlaceList);//绘制所有地块
            //随机设置地块区域
            Terrain.SetPlaceDistrict(Globle.AllPlaceList, Globle.Seed);
        }
        /// <summary>
        /// 生成门派按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //double d= Draw.ComparePoints(Globle.AllPlaceList[29].Coordinate, Globle.AllPlaceList[30].Coordinate);
            if (Globle.AllSectList.Count > 0)
            {
                Globle.AllSectList = new List<Sect>();
                Settings1.Default.Sect_UsedName = "";
            }
            Sect.RandomCreateSectsPlace(Globle.AllPlaceList, 20, Globle.Seed, 10);//生成sect
            for (int i = 0; i < Globle.AllSectList.Count; i++)
            {
                Draw.DrawPlaceHexagon(Globle.G1, Globle.AllSectList[i].SectPlaceList[0]);

            }
            pictureBox1.Refresh();//刷新
        }

        /// <summary>
        /// 生成人物事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (Globle.AllSectList.Count == 0)
            {
                return;
            }
            Globle.AllHumansList = new List<Human>();
            //每个门派10人（随机位置）
            Random random = new Random(Globle.Seed);
            for (int i = 0; i < (Globle.AllSectList.Count/* + 1*/) * 10; i++)
            {
                int seed = Globle.Seed + random.Next(0, 10000);
                int sex = Globle.AllSectList[i / 10].IsOnlySex;
                Human human = Human.SetHuman(seed, i, sex, Globle.AllSectList[i / 10].SectPlaceList[0]);
                human.FiveElements = Human.GetFiveElements(seed, 0.20, 0.20, 0.20, 0.20, 0.20, 0);//灵根几率
                human.Sect = Globle.AllSectList[i / 10];
                Globle.AllSectList[i / 10].SectMember.Add(human);//门派弟子
                Globle.AllHumansList.Add(human);
            }
        }

        private void Core_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                gameTimer1.IsPlay = false ? true : false;
                gameTimer1.SetTimeSpeed(gameTimer1.IsPlay, gameTimer1.Speed);
            }
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (Place p in Globle.AllPlaceList)
            {
                if (Draw.IsPointInPolygon(e.Location, p.Points))
                {
                    if (currentPlace != null && currentPlace != p)//前一个,且当前选择地块变化后则重置
                    {
                        Draw.DrawPlaceHexagon(Globle.G1, currentPlace);
                        //Place sectp = Draw.GetAroundPlaceList(Globle.AllPlaceList, currentPlace).Find(obj => obj.Sect != null);
                        //if (sectp != null)//周围有门派地块，重绘
                        //{
                        //    Draw.DrawPlaceHexagon(Globle.G, sectp);
                        //}
                    }

                    if (Globle.CurrentPathList != null && Globle.CurrentPathList.Count > 0)
                    {
                        foreach (var item in Globle.CurrentPathList)
                        {
                            Draw.DrawPlaceHexagon(Globle.G1, item.Place);//路径地点重绘，删掉线路
                        }
                        Globle.CurrentPathList = null;
                    }

                    if (Globle.IsSelect == true && Globle.StartPlace != null /*&& Globle.StartPlace != currentPlace*/)
                    {
                        List<Place> places = Globle.AllPlaceList.FindAll(obj => obj.TerrainType == 0 || obj.TerrainType == 1);
                        if (places.Contains(currentPlace))
                        {
                            Globle.EndPlace = currentPlace;//寻路终点确定
                            Globle.CurrentPathList = AStar.AStarFindPath(Globle.StartPlace, Globle.EndPlace, places);//得到路径
                            Draw.DrawPathLine(Globle.G1, Globle.CurrentPathList, null);
                        }

                    }
                    if (currentPlace != p && Globle.IsSelect == false)//减少不必要的重复绘制
                    {
                        Brush brush = new SolidBrush(Color.FromArgb(50, Color.White));
                        Draw.DrawPlaceHexagon(Globle.G1, p, Pens.White, true, brush);//鼠标位置白色覆盖变化效果
                        brush.Dispose();
                    }
                    currentPlace = p;
                    if (Globle.IsSelect == false)
                    {
                        Globle.StartPlace = null;
                        Globle.EndPlace = null;
                    }
                    pictureBox1.Invalidate();
                }
            }

        }
        /// <summary>
        /// 计时器触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            Globle.CurrentDay = gameTimer1.Days;//增加日期
            //List<Human> missionHuman = Globle.AllHumansList.FindAll(o => o.Mission != null);
            Human.DrawHuman(Globle.AllHumansList.FindAll(o => o.Mission != null));//有任务的人物
            Human.DrawHuman(Globle.AllHumansList.FindAll(o => o.Location != o.Sect.SectPlaceList[0].Coordinate));//未在门派主城的人物
            //foreach (var mission in Globle.AllMissionList)
            List<Mission> missions = Globle.AllMissionList;
            for (int i = 0; i < missions.Count; i++)
            {
                //missions[i].DrawMission();//画任务相关
                missions[i].PerformMission();//任务执行
                foreach (var human in missions[i].ExecutorList)
                {
                    human.Move(/*missions[i].Speed*/);//人物移动，根据各自速度
                }
                if (missions[i].IsComplete == 1)
                {
                    gameTimer1.Enabled = false;
                    gameTimer1.Timer.Stop();
                    //表示任务完成
                    DialogResult dialog = MessageBox.Show(missions[i].Name + missions[i].Place.PlaceName + "任务完成！", "通知", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (dialog == DialogResult.OK)
                    {
                        missions[i] = null;
                        gameTimer1.Timer.Start();
                        gameTimer1.Enabled = true;
                    }
                }
            }
            missions.RemoveAll(o => o == null);//删掉为null的
            //pictureBox1.Refresh();
            pictureBox1.Invalidate();
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            MissionForm missionForm = MissionForm.CreateInstrance();
            missionForm.ShowMission(e.ClickedItem.Text, currentPlace);
            missionForm.Show();
        }
    }
}
