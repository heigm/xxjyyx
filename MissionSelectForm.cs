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
    public partial class MissionSelectForm : Form
    {
        private static MissionSelectForm frm = null;
        private MissionSelectForm()
        {
            InitializeComponent();
            this.TopMost = true;
        }
        /// <summary>
        /// 单例模式
        /// </summary>
        /// <returns></returns>
        public static MissionSelectForm CreateInstrance()
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new MissionSelectForm();
            }
            return frm;
        }

        List<Button> buttonList = new List<Button>();
        /// <summary>
        /// 当前的门派
        /// </summary>
        Sect sect;
        Place place;
        Human human;
        /// <summary>
        /// 展示任务相关的按钮
        /// </summary>
        /// <param name="sect"></param>
        public void ShowMissionButton(Sect sect, Place place, Human human)
        {
            //int i = tabControl1.Controls.Count;
            this.sect = sect;
            this.place = place;
            this.human = human;
            SetButtonText();
            if (Globle.AllPlaceList.Exists(obj => obj.IsExplore == false))//如果还存在未探索地块
            {
                buttonList.Find(obj => obj.Text == "探索").Enabled = true;
            }
            if (sect != null && sect.SectPlaceList.Exists(o => o.OrderValue < 100))//门派中存在统治值未到100的地块
            {
                buttonList.Find(obj => obj.Text == "巡视").Enabled = true;
            }
            if(Globle.AllPlaceList.Exists(o=>o.IsExplore==true&&o.Sect==null))//已探索未占据
            {
                buttonList.Find(obj => obj.Text == "占据").Enabled = true;
            }
        }

        /// <summary>
        /// 设置按钮text内容
        /// </summary>
        private void SetButtonText()
        {
            buttonList = new List<Button>();
            foreach (var item in tabControl1.Controls)
            {
                if (item is TabPage)
                {
                    for (int i = 0; i < ((TabPage)item).Controls.Count; i++)
                    {
                        if (((TabPage)item).Controls[i] is Button)
                        {
                            Button button = (Button)((TabPage)item).Controls[i];
                            //button.Name = /*((TabPage)item).Name +*/ "btn" + i;
                            buttonList.Add(button);
                        }
                    }
                }
            }
            for (int i = 0; i < buttonList.Count; i++)
            {
                //int index = int.Parse(buttonList[i].Name.Replace("btn", ""));
                //buttonList[i].Text = Globle.MissionType[index - 1] /*+ "(" + index + ")"*/;
                //buttonList[i].Tag = index - 1;
                buttonList[i].Enabled = false;//默认全无交互
                buttonList[i].Click += Btn_Click;
            }
            //return buttons;
        }

        /// <summary>
        /// 所有按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //MessageBox.Show(((Button)sender).Name);
            MissionForm missionForm = MissionForm.CreateInstrance();
            missionForm.ShowMission(((Button)sender).Text, place, human);
            missionForm.Show();
            this.Close();
        }
    }
}
