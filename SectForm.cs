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
    public partial class SectForm : Form
    {
        private static SectForm frm = null;
        private SectForm()
        {
            InitializeComponent();
            this.TopMost = true;
        }
        /// <summary>
        /// District单例模式
        /// </summary>
        /// <returns></returns>
        public static SectForm CreateInstrance()
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new SectForm();
            }
            return frm;
        }
        private Sect currentSect;
        /// <summary>
        /// 显示门派信息
        /// </summary>
        /// <param name="sect"></param>
        public void ShowSectInfo(Sect sect)
        {
            currentSect = sect;
            if (sect == null)
            {
                pictureBox1.BackColor = Color.Gray;
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\image\\flags\\flag (0).png");
                label1.Text = "无所属";
                processExt1.Visible = false;
                return;
            }
            pictureBox1.BackColor = sect.SectColor;
            pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\image\\flags\\flag (" + sect.SectFlagIndex + ").png");
            //pictureBox1.Refresh();
            label1.Text = sect.SectName + sect.SectSuffix + "\n正邪：" + sect.SectJustice;
            processExt1.Visible = true;
            processExt1.Value = sect.SectJustice;

            resShow1.ShowResources(sect.SectResources);
        }
        /// <summary>
        /// 弟子按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (currentSect == null) { return; }
            HumanList humanList = HumanList.CreateInstrance();
            humanList.ShowHumanList(Globle.AllHumansList.FindAll(obj => obj.Sect == currentSect));
            humanList.Show();
        }
    }
}
