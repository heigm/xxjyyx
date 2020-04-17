using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xxjjyx.Common;

namespace xxjjyx
{
    /// <summary>
    /// 区划类
    /// </summary>
    public class District
    {
        int _index;
        int _typeIndex;
        Rectangle rect;
        Reiki _reiki;
        Place _parentPlace;
        Build _build;
        int crossingTime;
        List<District_Modifies> modifies;


        /// <summary>
        /// district序号，唯一
        /// </summary>
        public int Index { get => _index; set => _index = value; }
        /// <summary>
        /// 类型序号
        /// </summary>
        public int TypeIndex { 
            get => _typeIndex;
            set
            {
                this.CrossingTime = int.Parse(Globle.CrossingTime[value]);
                _typeIndex = value;
            }
        }
        /// <summary>
        /// 所属place
        /// </summary>
        public Place ParentPlace { get => _parentPlace; set => _parentPlace = value; }
        /// <summary>
        /// 所属建筑
        /// </summary>
        public Build Build { get => _build; set => _build = value; }
        /// <summary>
        /// 灵气
        /// </summary>
        public Reiki Reiki
        {
            get => _reiki;
            set
            {
                _reiki = value;
            }
        }
        /// <summary>
        /// 区划显示位置
        /// </summary>
        public Rectangle Rect { get => rect; set => rect = value; }
        /// <summary>
        /// 穿越地形耗时
        /// </summary>
        public int CrossingTime { get => crossingTime; set => crossingTime = value; }

        /// <summary>
        /// 构造district
        /// </summary>
        /// <param name="index"></param>
        /// <param name="parentPlace"></param>
        /// <param name="typeIndex"></param>
        public District(int index, Place parentPlace, int typeIndex)
        {
            this.Index = index;
            this.ParentPlace = parentPlace;
            this.TypeIndex = typeIndex;
            this.CrossingTime = int.Parse(Globle.CrossingTime[typeIndex]);
        }

    }
}
