using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xxjjyx
{
    public partial class HumanForm : Form
    {
        private static HumanForm frm = null;
        private HumanForm()
        {
            InitializeComponent();
            this.TopMost = true;
        }
        /// <summary>
        ///单例模式
        /// </summary>
        /// <returns></returns>
        public static HumanForm CreateInstrance()
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new HumanForm();
            }
            return frm;
        }
        Human human;
        /// <summary>
        /// 展示人物信息
        /// </summary>
        /// <param name="human"></param>
        public void ShowHumanInfo(Human human)
        {
            this.human = human;
            int sum = 0;
            foreach (var item in human.FiveElements)
            {
                if (item > 0)
                {
                    sum++;
                }
            }
            label2.Text = sum + "灵根";
            label1.Text = human.Last + human.Name + "\t年龄：" + human.Age + " / " + human.MaxAge;
            Sect sect = Globle.AllSectList.Find(obj => obj == human.Sect);
            pictureBox1.BackColor = sect.SectColor;//人物背景颜色和门派一致
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            Image backimage = Image.FromFile(Application.StartupPath + "\\image\\flags\\flag (" + sect.SectFlagIndex + ").png");
            pictureBox1.BackgroundImage = backimage;
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            string str = human.Sex == false ? "female" : "male";

            pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\image\\human\\" + str + "\\" + str + " (" + human.PicIndex + ").png");
            //pictureBox1.Refresh();
            

            fiveGraph1.SetFiveGraph(human.FiveElements);
        }
        /// <summary>
        /// 任务按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (human.Mission != null)
            {
                MessageBox.Show(human.Last + human.Name + "已有" + human.Mission.Name + "任务！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Globle.StartPlace = human.SetPlace;//设置任务起点
            Globle.CurrentSect = human.Sect;//设置当前选择的门派
            MissionSelectForm missionSelectForm = MissionSelectForm.CreateInstrance();
            missionSelectForm.ShowMissionButton(human.Sect, human.SetPlace, human);
            missionSelectForm.Show();
            //Globle.IsSelect = true;//开始选终点
            //MissionSelectForm missionSelectForm=
        }
    }
}
