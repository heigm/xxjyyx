using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxjjyx.Common;

namespace xxjjyx
{
    /// <summary>
    /// 资源类
    /// </summary>
    public class Resource
    {
        int type;
        string name;
        int amount;
        int maxAmount;
        /// <summary>
        /// 资源序号，唯一
        /// </summary>
        public int Type { get => type; set => type = value; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// 资源数量
        /// </summary>
        public int Amount
        {
            get => amount;
            set
            {
                if (value > maxAmount)
                {
                    value = maxAmount;
                }
                amount = value;
            }
        }
        /// <summary>
        /// 资源限额
        /// </summary>
        public int MaxAmount { get => maxAmount; set => maxAmount = value; }

        public Resource(int type, int maxAmount,int amount)
        {
            this.Type = type;
            this.name = Globle.Split(ResourceInfo.Default.Name)[type];
            this.maxAmount = maxAmount;
            this.Amount = amount;
        }
        /// <summary>
        /// 根据输入字符串得到资源字典
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetResDic(string str)
        {
            Dictionary<string, int> keys = new Dictionary<string, int>();
            string[] strs = str.Split(',');
            foreach (var s in strs)
            {
                keys.Add(s.Split(':')[0], int.Parse(s.Split(':')[1]));
            }
            return keys;
        }
      
    }
}
