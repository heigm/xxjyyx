using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xxjjyx
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int time = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 30;
            timer1.Start();
        }

        /// <summary>
        /// 计时器触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            time = time + 1;
            Time.TimeSet();
            int[] datetime = Time.GetTime();
            this.Text = "( " + datetime[2] + " 年 " + datetime[1] + " 月 " + datetime[0] + " 日 )( 总天数：" + (time).ToString() + " )";

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                //生成
                //random = new Random((int)DateTime.Now.Ticks);
                //Thread.Sleep(15);
                Human human = Human.SetHuman(Globle.Seed, i, -1, null);
                string sex = human.Sex == false ? "女" : "男";

                listBox1.Items.Add("姓名：" + human.Last + " " + human.Name + ",性别：" + sex);
            }
            label1.Text = Settings1.Default.Human_UsedName.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Length.ToString();
            //foreach (string str in Settings1.Default.usedname.Split(new char[] { '|' },StringSplitOptions.RemoveEmptyEntries))
            //{
            //    listBox1.Items.Add("姓名：" + str);
            //}
        }

        /// <summary>
        /// 生成门派
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            for (int i = 0; i < 20; i++)
            {
                //Sect sects = Sect.RandomCreateSectInfo(Globle.Seed);
                //listBox1.Items.Add("门派：" + sects.SectName +sects.SectSuffix+ "|类型：" + sects.SectType + "|立场：" + sects.SectStand);
            }
        }
    }
}
