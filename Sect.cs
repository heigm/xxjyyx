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
    /// 门派类
    /// </summary>
    public class Sect
    {
        string sectName;
        string sectSuffix;
        string sectType;
        string sectStand;
        int sectJustice;
        int sectIndex;
        Color sectColor;
        int sectFlagIndex;
        Image sectFlag;
        List<Place> sectPlaceList = new List<Place>();
        List<Human> sectMember = new List<Human>();
        int isOnlySex = -1;
        List<Resource> sectResources;
        //Point position;
        /// <summary>
        /// 门派名
        /// </summary>
        public string SectName { get => sectName; set => sectName = value; }
        /// <summary>
        /// 门派后缀
        /// </summary>
        public string SectSuffix
        {
            get => sectSuffix;
            set
            {
                if (value == "楼")
                {
                    isOnlySex = 1;
                }
                else if (value == "寺")
                {
                    isOnlySex = 0;
                }
                sectSuffix = value;
            }
        }
        /// <summary>
        /// 门派类型
        /// </summary>
        public string SectType { get => sectType; set => sectType = value; }
        /// <summary>
        /// 门派立场
        /// </summary>
        public string SectStand { get => sectStand; set => sectStand = value; }
        /// <summary>
        /// 门派正邪，-1000到1000，根据立场实施任务，奇遇等
        /// </summary>
        public int SectJustice { get => sectJustice; set => sectJustice = value; }
        /// <summary>
        /// 门派弟子
        /// </summary>
        public List<Human> SectMember { get => sectMember; set => sectMember = value; }
        /// <summary>
        /// 门派序号
        /// </summary>
        public int SectIndex { get => sectIndex; set => sectIndex = value; }
        /// <summary>
        /// 门派所有place
        /// </summary>
        public List<Place> SectPlaceList { get => sectPlaceList; set => sectPlaceList = value; }
        /// <summary>
        /// 门派代表颜色
        /// </summary>
        public Color SectColor { get => sectColor; set => sectColor = value; }
        /// <summary>
        /// 门派标志图片
        /// </summary>
        public Image SectFlag { get => sectFlag; set => sectFlag = value; }
        /// <summary>
        /// 门派标志序号
        /// </summary>
        public int SectFlagIndex { get => sectFlagIndex; set => sectFlagIndex = value; }
        /// <summary>
        /// 是否唯一性别,0男1女
        /// </summary>
        public int IsOnlySex { get => isOnlySex; set => isOnlySex = value; }
        /// <summary>
        /// 门派所属资源
        /// </summary>
        public List<Resource> SectResources { get => sectResources; set => sectResources = value; }

        ///// <summary>
        ///// 门派位置
        ///// </summary>
        //public Point Position { get => position; set => position = value; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">门派名</param>
        /// <param name="suffix">门派后缀</param>
        /// <param name="type">门派类型</param>
        /// <param name="stand">门派立场</param>
        /// <param name="index">门派序号</param>
        /// <param name="place">初始位置</param>
        public Sect(int index, string name, string suffix, string type, string stand, Place place)
        {
            SectIndex = index;
            sectName = name;
            SectSuffix = suffix;
            sectType = type;
            sectStand = stand;
            this.SectPlaceList.Add(place);
            place.Sect = this;
            Initialize();
        }
        /// <summary>
        /// 初始化，资源，建筑
        /// </summary>
        void Initialize()
        {
            //资源
            SectResources = new List<Resource>();
            string[] res = Globle.Split(ResourceInfo.Default.Name);
            for (int i = 0; i < res.Length; i++)
            {
                Resource r = new Resource(i, 5000, 0);
                if (i == 0) { r.Amount = 2000; }
                else if (i == 1) { r.Amount = 500; }
                else { r.Amount = 100; }
                SectResources.Add(r);
            }
            //建筑，主殿
            sectPlaceList[0].DistrictList[0].Build = new Build(0);
            sectPlaceList[0].DistrictList[0].Build.BuildProgress = sectPlaceList[0].DistrictList[0].Build.BuildCastTime;//默认建好的
            //sectPlaceList[0].DistrictList[0].Build.IsBuilding = true;
        }
        /// <summary>
        /// 随机生成门派place
        /// </summary>
        /// <param name="allPlaces"></param>
        /// <param name="count">创建数量</param>
        /// <param name="seed">随机数种子</param>
        /// <param name="spacing">门派place的最小间距</param>
        public static void RandomCreateSectsPlace(List<Place> allPlaces, int count, int seed, int spacing)
        {
            Random random = new Random(seed);
            List<Place> tempPlaces = new List<Place>();
            bool b = true;
            int ii = 0;
            for (int i = 0; i < count; i++)
            {
                int index = random.Next(0, allPlaces.Count);
                if (allPlaces[index].TerrainType != 0)//非陆地
                {
                    //index = random.Next(0, allPlaces.Count);
                    i--;
                    continue;
                }
                for (int j = 0; j < tempPlaces.Count; j++)
                {
                    double dis = Draw.ComparePoints(tempPlaces[j].Coordinate, allPlaces[index].Coordinate);//比较两个点的距离，太近则重置
                    if (dis <= spacing * Settings1.Default.Draw_Radius)
                    {
                        ii++;
                        if (ii > 5000)//死循环后重置
                        {
                            //i = 1;
                            tempPlaces.RemoveAt(tempPlaces.Count - 1);
                        }
                        b = false;
                        break;
                    }
                }
                if (b == false)
                {
                    i--;
                    b = true;
                    continue;
                }
                tempPlaces.Add(allPlaces[index]);
            }
            List<Sect> tempSects = new List<Sect>();
            for (int i = 0; i < tempPlaces.Count; i++)
            {
                tempPlaces[i].IsExplore = true;//将place设置为已探索
                Sect sect = RandomCreateSectInfo(seed, tempPlaces[i], i);

                tempSects.Add(sect);
            }
            //比较颜色差
            for (int i = 0; i < tempSects.Count; i++)
            {
                for (int j = 0; j < tempSects.Count; j++)
                {
                    if (i != j && !CompareColor(tempSects[i].SectColor, tempSects[j].SectColor))//差距不够则重新随机
                    {
                        tempSects[i].SectColor = RandomCreateColor(random.Next(0, 1000));
                        i--;
                        break;
                    }
                }
            }
            Globle.AllSectList.AddRange(tempSects);
        }

        /// <summary>
        /// 随机设置派别名字、类型、立场、颜色、旗帜和威望等信息
        /// </summary>
        /// <param name="index">门派序号</param>
        /// <param name="place">门派初始位置</param>
        /// <param name="seed">随机数种子</param>
        /// <returns></returns>
        static Sect RandomCreateSectInfo(int seed, Place place, int index)
        {
            int number = 0;
            Random random = new Random(seed);
        A:
            string name = Globle.SectsNames[random.Next(0, Globle.SectsNames.Length)];
            //设置名字，不能重用
            if (Settings1.Default.Sect_UsedName.Contains(name))//重名则重来
            {
                //SetSects();
                if (number > 10000)//死循环
                {
                    return null;
                }
                number++;
                goto A;

            }
            Settings1.Default.Sect_UsedName += name + "|";
            int tempint = random.Next(0, 100);//66俗，11道，11释，11儒
            int typeint = 0;
            if (tempint < 66)
            {
                typeint = 0;
            }
            else if (tempint < 77)
            {
                typeint = 1;
            }
            else if (tempint < 88)
            {
                typeint = 2;
            }
            else
            {
                typeint = 3;
            }
            //int typeint = random.Next(0, sectstypes.Length);
            string type = Globle.SectsTypes[typeint];
            int suffixint = Globle.SectsSuffixs.Length - (Globle.SectsTypes.Length - typeint) * 3;
            string suffix = Globle.SectsSuffixs[random.Next(suffixint, suffixint + 3)];//随机
            if (typeint == 0)//判断类型后赋予适当的后缀名
            {
                suffix = Globle.SectsSuffixs[random.Next(0, 10)];//随机
            }

            int sindex = random.Next(0, Globle.SectsStands.Length);//立场0正1中2魔

            string stand = Globle.SectsStands[sindex];
            Sect sect = new Sect(index, name, suffix, type, stand, place);
            if (sindex == 0)//正
            {
                sect.SectJustice = 200;
            }
            else if (sindex == 1)//中
            {
                sect.SectJustice = 0;
            }
            else//魔
            {
                sect.SectJustice = -200;
            }
            //sect.SectPrestige = 100;//固定100威望
            //sect.Index = index;
            //sect.SectPlaceList.Add(place);
            sect.SectColor = RandomCreateColor(Globle.Seed + random.Next(0, 1000));//设置颜色
            sect.SectFlagIndex = random.Next(1, 193);//目前有192个flag
            bool b = true;
            while (b)
            {
                if (Globle.AllSectList.Exists(obj => obj.SectFlagIndex == sect.SectFlagIndex))//标识不能重复
                {
                    sect.SectFlagIndex = random.Next(1, 193);
                }
                else
                {
                    b = false;
                }
            }
            //sect.SectFlag = Image.FromFile("image\\flags\\flag(" + sect.SectFlagIndex + ").png");
            return sect;
        }

        /// <summary>
        /// 根据地点得到最近门派
        /// </summary>
        /// <param name="place">传入的点位</param>
        /// <returns></returns>
        public static Sect GetLatelySect(Place place)
        {
            double min = int.MaxValue;
            Sect minSect = null;
            foreach (var sect in Globle.AllSectList)
            {
                double temp = Draw.ComparePoints(sect.SectPlaceList[0].Coordinate, place.Coordinate);
                if (min > temp)
                {
                    min = temp;
                    minSect = sect;
                }
            }
            return minSect;
        }

        /// <summary>
        /// 生成随机颜色
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        static Color RandomCreateColor(int seed)
        {
            //int iSeed = 10;
            Random ran = new Random((int)(seed & 0xffffffffL) | (int)(seed >> 32));
            //long tick = DateTime.Now.Millisecond;
            //Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            int R = ran.Next(255);
            int G = ran.Next(255);
            int B = ran.Next(255);
            B = (R + G > 400) ? R + G - 400 : B;//0 : 380 - R - G;
            B = (B > 255) ? 255 : B;
            return Color.FromArgb(R, G, B);
        }
        /// <summary>
        /// 比较颜色差
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        static bool CompareColor(Color c1, Color c2)
        {
            int r = Math.Abs(c1.R - c2.R);
            int g = Math.Abs(c1.G - c2.G);
            int b = Math.Abs(c1.B - c2.B);
            if (r + g + b < 100)
            {
                return false;
            }
            return true;
        }
    }
}
