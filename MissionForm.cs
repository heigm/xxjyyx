using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xxjjyx.Common;

namespace xxjjyx
{
    public partial class MissionForm : Form
    {
        private static MissionForm frm = null;
        private MissionForm()
        {
            InitializeComponent();
            this.TopMost = true;
        }
        /// <summary>
        /// 单例模式
        /// </summary>
        /// <returns></returns>
        public static MissionForm CreateInstrance()
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new MissionForm();
            }
            return frm;
        }
        string name;
        string desc;
        int typeIndex;
        List<Human> humen;
        Place targetPlace;
        /// <summary>
        /// 当前任务
        /// </summary>
        Mission currentMission;
        /// <summary>
        /// 展示任务
        /// </summary>
        public void ShowMission(string type, Place place, Human human)
        {
            this.name = type;
            this.typeIndex = Globle.Split(MissionInfo.Default.Name).ToList().IndexOf(type);
            this.desc = Globle.Split(MissionInfo.Default.Desc)[typeIndex];

            button2.Enabled = true;
            currentMission = new Mission(typeIndex, place);
            Globle.CurrentMission = currentMission;
            label1.Text = "任务类型：" + name;
            label3.Text = "任务描述：" + desc;
            Random random = new Random();
            pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\image\\mission\\0 (" + random.Next(1, 17) + ").jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            checkedListBox1.Items.Clear();
            if (place == null) { return; }
            if (human != null)
            {
                humen = new List<Human>() { human };
            }
            else
            {
                //目前是传入当地块无任务的弟子
                humen = Globle.AllHumansList.FindAll(obj => obj.SetPlace == place).FindAll(o => o.Mission == null);
            }
            foreach (var h in humen)
            {
                if (h.Mission == null)
                {
                    string temp = h.Last + h.Name + " " /*+ (h.Sex == false ? "女" : "男") + " " */+ h.GetLevel();
                    temp = StringAlign(temp);
                    checkedListBox1.Items.Add(temp, false);
                }

            }
            if (humen.Count == 1)
            {
                checkedListBox1.SetItemChecked(0, true);//只有一个默认选中
                checkedListBox1.Enabled = false;
            }
            else
            {
                checkedListBox1.Enabled = true;
            }
        }

        private static string StringAlign(string str)
        {
            string[] strs = str.Split(new char[] { ' ' });
            int max = 10;//十个格
            string tempstr = "";
            for (int i = 0; i < strs.Length; i++)
            {
                //if (i == 1) { max = 4; }else if (i == 2) { max = 10; }
                tempstr += strs[i] + new string(' ', max - Encoding.Default.GetBytes(strs[i]).Length);
            }
            //templist.Add(tempstr);
            //}
            return tempstr;
        }

        /// <summary>
        /// 展示任务
        /// </summary>
        /// <param name="mType">任务类别名称</param>
        /// <param name="place">任务地点</param>
        public void ShowMission(string mType, Place place)
        {
            int typeIndex = Globle.Split(MissionInfo.Default.Name).ToList().FindIndex(o => o == mType);
            currentMission = new Mission(typeIndex, place);
            label1.Text = "任务类型：" + currentMission.Name;
            label2.Text = "任务地点：" + currentMission.Place.PlaceName;
            label3.Text = "任务描述：" + currentMission.Desc;
            label4.Text = "任务工量：" + currentMission.MaxProgress;
            label5.Text = "历练奖励：" + currentMission.Exp;
            button2.Enabled = false;
            Random random = new Random();
            pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\image\\mission\\" + typeIndex+" (" + random.Next(1, 6) + ").jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            checkedListBox1.Items.Clear();
            if (place.Sect != null)
            {
                foreach (var h in place.Sect.SectMember)
                {
                    if (h.Mission != null) { continue; }//跳过有任务的
                    string temp = h.Last + h.Name + " " /*+ (h.Sex == false ? "女" : "男") + " "*/ + h.GetLevel() + " " + (h.SetPlace == place ? "本地" : "");//判断是否本地人
                    temp = StringAlign(temp);
                    checkedListBox1.Items.Add(temp, false);
                }
            }
            else
            {
                //测试用,找最近的门派弟子
                Sect sect = Sect.GetLatelySect(place);
                if (sect == null) { return; }
                foreach (var h in sect.SectMember)
                {
                    if (h.Mission != null) { continue; }//跳过有任务的
                    string temp = h.Last + h.Name + " " /*+ (h.Sex == false ? "女" : "男") + " " */+ h.GetLevel() + " " + (h.SetPlace == place ? "本地" : "");//判断是否本地人
                    temp = StringAlign(temp);
                    checkedListBox1.Items.Add(temp, false);
                }
            }
        }
        /// <summary>
        /// 设置目标任务点
        /// </summary>
        /// <param name="place"></param>
        public void SetSitePlace(Place targetPlace)
        {
            label2.Text = "任务地点：" + targetPlace.PlaceName;
            int castTime = 0;
            foreach (var dist in targetPlace.DistrictList)
            {
                castTime += dist.CrossingTime;
            }
            int exp = castTime * 2 * 2;//2倍数时间*2倍exp率
            //currentMission.Progress = castTime;
            //currentMission.MaxProgress = castTime;
            currentMission.Exp = exp;
            label4.Text = "任务工量：" + currentMission.MaxProgress;
            //currentMission.CastTime
            label5.Text = "历练奖励：" + currentMission.Exp;

            currentMission.PathNodes = Globle.CurrentPathList;//赋值路径
            //label6.Text = "路程距离：" + currentMission.PathNodes[currentMission.PathNodes.Count - 1].G + "距";
            this.targetPlace = targetPlace;
        }
        /// <summary>
        /// 选择地点按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (humen == null || humen.Count == 0) { MessageBox.Show("没有弟子可以执行任务！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            //this.Hide();
            Globle.IsSelect = true;
            Globle.StartPlace = humen[0].SetPlace;//寻路起点确定
            foreach (var form in Application.OpenForms)
            {
                if (((Form)form).Name != "Core")
                {
                    //((Form)form).Enabled = false;
                    ((Form)form).Hide();
                }
            }
            //((MissionSelectForm)Application.OpenForms["MissionSelectForm"]).Enabled = false;
        }
        /// <summary>
        /// 确认任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (currentMission.Place == null) { MessageBox.Show("未选择任务地点！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (checkedListBox1.CheckedItems.Count == 0) { MessageBox.Show("未选择任务人员！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            //label4.Text = "任务耗时：" + limit + " 天";

            //Mission mission = new Mission(Globle.AllMissionList.Count, name, desc, targetPlace, limit, typeIndex, exp, null, null, 1);
            List<Human> templist = new List<Human>();
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                string name = checkedListBox1.CheckedItems[i].ToString().Split(' ')[0];//得到人物名字
                Human human = Globle.AllHumansList.Find(obj => obj.Last + obj.Name == name);
                if (human != null)
                {
                    templist.Add(human);
                }
            }
            currentMission.ExecutorList = templist;//赋值人员给任务
            //checkedListBox1.Refresh();
            Globle.AllMissionList.Add(currentMission);//任务加入全任务集合中
            currentMission.DrawMission();//画当前任务
            //Globle.IsSelect = false;
            //下面为测试
            //targetPlace.IsExplore = true;
            //Draw.DrawPlaceHexagon(Globle.G, targetPlace);
            this.Close();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count == 0) { return; }
            //currentMission.CastTime = currentMission.LimitTime / checkedListBox1.CheckedItems.Count;
            label4.Text = "任务耗时：" + currentMission.Progress + " 天";
            //for (int i = 0; i < checkedListBox1.Items.Count; i++)
            //{
            //    string name = checkedListBox1.Items[i].ToString().Split(' ')[0];//得到人物名字
            //    Human human = humen.Find(obj => obj.Last + obj.Name == name);
            //    if (checkedListBox1.CheckedItems.Contains(checkedListBox1.Items[i]))
            //    {
            //        if (human != null)
            //        {
            //            //templist.Add(human);
            //            human.PathNodes = AStar.AStarFindPath(human.SetPlace, targetPlace, Globle.AllPlaceList);
            //            human.Mission = currentMission;//给人物赋予任务
            //        }
            //    }
            //    else
            //    {
            //        human.PathNodes = null;
            //        human.Mission = null;
            //    }
            //}

        }
    }
}
