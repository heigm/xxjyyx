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
    public partial class HumanList : Form
    {
        private static HumanList frm = null;
        private HumanList()
        {
            InitializeComponent();
            this.TopMost = true;
        }
        /// <summary>
        /// HumanList单例模式
        /// </summary>
        /// <returns></returns>
        public static HumanList CreateInstrance()
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new HumanList();
            }
            return frm;
        }

        private List<Human> currentHumanList;
        /// <summary>
        /// 展示弟子信息
        /// </summary>
        /// <param name="humansList"></param>
        public void ShowHumanList(List<Human> humansList)
        {
            currentHumanList = humansList;
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }

            dataGridView1.ClearSelection();
            for (int i = 0; i < humansList.Count; i++)
            {
                dataGridView1.Rows.Add();
                DataGridViewRow row = dataGridView1.Rows[i];
                
                row.Cells[0].Value = humansList[i].Sect.SectName+humansList[i].Sect.SectSuffix;
                row.Cells[1].Value = humansList[i].Last + humansList[i].Name;
                row.Cells[2].Value = humansList[i].Sex == false ? "女" : "男";
                row.Cells[3].Value = humansList[i].Age + "/" + humansList[i].MaxAge;
                row.Cells[4].Value = humansList[i].Mission == null ? "无" : humansList[i].Mission.Name;//任务
                row.Cells[5].Value = humansList[i].Exp + "/" + humansList[i].MaxExp;
                row.Cells[6].Value = humansList[i].GetLevel();
                //row.Cells[5].Value = humansList[i].FiveElements[1];
                //row.Cells[6].Value = humansList[i].FiveElements[2];
                //row.Cells[7].Value = humansList[i].FiveElements[3];
                //row.Cells[8].Value = humansList[i].FiveElements[4];
                row.Cells[9].Value = humansList[i].Index;
                row.Cells[0].Style.ForeColor = humansList[i].Sect.SectColor;//门派颜色
            }
        }
        /// <summary>
        /// 点击表单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || e.X < 0 || e.Y < 0)
            {
                return;
            }

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0 || currentHumanList == null)
            {
                //ShowHumanList(currentHumanList);
                return;
            }
            if (dataGridView1.SelectedRows[0].Cells[9].Value == null)
            {
                dataGridView1.ClearSelection();
                return;
            }
            int index = (int)(dataGridView1.SelectedRows[0].Cells[9].Value);
            HumanForm humanForm = HumanForm.CreateInstrance();
            humanForm.ShowHumanInfo(currentHumanList.Find(obj => obj.Index == index));
            humanForm.Show();
        }
    }
}
