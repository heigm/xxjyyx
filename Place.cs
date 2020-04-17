using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxjjyx
{
    public class Place
    {
        int terrainType = -1;
        int terrainFeature;
        List<District> districtList /*= new List<District>(9)*/;
        Point coordinate = new Point();
        Point[] points;
        int index;
        bool isExplore;//是否探索过,探索以发现任务，异常
        Sect sect;

        int orderValue = 50;//初始50
        int population;//当地人口
        int maxPoplation;//最大承载人口
        string placeName;//地名

        /// <summary>
        /// 地形类型，0陆地1湖泊2海洋
        /// </summary>
        public int TerrainType { get => terrainType; set => terrainType = value; }

        /// <summary>
        /// 坐标，空间
        /// </summary>
        public Point Coordinate { get => coordinate; set => coordinate = value; }
        /// <summary>
        /// 组成place的周边点位，空间坐标
        /// </summary>
        public Point[] Points { get => points; set => points = value; }
        /// <summary>
        /// 地块序号
        /// </summary>
        public int Index { get => index; set => index = value; }
        /// <summary>
        /// 地形特征（地物，森林，矿产之类）
        /// </summary>
        public int TerrainFeature { get => terrainFeature; set => terrainFeature = value; }
        /// <summary>
        /// 区域，默认9个
        /// </summary>
        public List<District> DistrictList
        {
            get => districtList;
            set
            {
                //if (value.Count > 0)
                //    this.crossingTime = value.Sum(obj => obj.CrossingTime);
                districtList = value;
            }
        }
        /// <summary>
        /// 是否已探索
        /// </summary>
        public bool IsExplore
        {
            get => isExplore;
            set
            {
                if (value == true)//已探索，则执行
                {
                    SetReikiAndPop();
                }
                isExplore = value;
            }
        }
        /// <summary>
        /// 所属门派
        /// </summary>
        public Sect Sect { get => sect; set => sect = value; }
        /// <summary>
        /// 统治度，决定该地产值，人口增减（50），坏事几率，最高100
        /// </summary>
        public int OrderValue
        {
            get => orderValue;
            set
            {
                if (value > 100) { value = 100; }
                orderValue = value;
            }
        }
        /// <summary>
        /// 当地人口，初始根据地形随机生成
        /// </summary>
        public int Population { get => population; set => population = value; }
        /// <summary>
        /// 最大承载人口
        /// </summary>
        public int MaxPoplation { get => maxPoplation; set => maxPoplation = value; }
        /// <summary>
        /// 地名
        /// </summary>
        public string PlaceName { get => placeName; set => placeName = value; }

        /// <summary>
        /// 穿越耗时
        /// </summary>
        //public int CrossingTime { get => crossingTime; }

        /// <summary>
        /// 构造函数，地块信息
        /// </summary>
        /// <param name="index">序号</param>
        /// <param name="coordinate">实际空间坐标</param>
        /// <param name="points">形成地块的周边6个点位</param>
        public Place(int index, Point coordinate, Point[] points)
        {
            this.index = index;
            this.coordinate = coordinate;
            this.points = points;
            SetPlaceName();
        }

        void SetPlaceName()
        {
            Random random = new Random(DateTime.Now.Millisecond + index);
            string tempName = "";// names[random.Next(0, names.Length)] + names[random.Next(0, names.Length)];
            bool isUsed = true;
            while (isUsed)
            {
                for (int i = 0; i < 2; i++)
                {
                    tempName += Globle.PlaceNames[random.Next(0, Globle.PlaceNames.Length)];
                }
                string[] used = Settings1.Default.Place_UsedName.Split('|');
                
                foreach (var str in used)
                {
                    if (str == tempName)
                    {
                        isUsed = true;
                        break;
                    }
                    else
                    {
                        isUsed = false;
                    }
                }
                if (isUsed==false)
                {
                    Settings1.Default.Place_UsedName += tempName + "|";
                    placeName = tempName;
                    break;
                }
            }

        }

        /// <summary>
        /// 设置灵脉和人口
        /// </summary>
        void SetReikiAndPop()
        {
            Random random = new Random(Globle.Seed + index);
            for (int i = 0; i < districtList.Count; i++)
            {
                int r = random.Next(0, Globle.ReikiType.Length);
                int l = random.Next(1, 3);
                districtList[i].Reiki = new Common.Reiki(r, l);
                int popint = (25 - districtList[i].CrossingTime) < 0 ? 0 : (25 - districtList[i].CrossingTime);
                population += popint * random.Next(100, 200);//设置人口25为限制
                maxPoplation += popint * 1000;
            }
        }

    }
}
