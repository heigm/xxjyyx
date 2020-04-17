using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxjjyx
{
    public static class Globle
    {
        static Graphics g1;
        static Graphics g2;
        static List<Place> allPlaceList = new List<Place>();
        static int seed;
        //static Place selectPlace;
        static List<Sect> allSectList = new List<Sect>();
        static List<Human> allHumansList = new List<Human>();
        static List<District> allDistrictList = new List<District>();
        static bool isSelect = false;
        static List<Mission> allMissionList = new List<Mission>();
        static Place startPlace;
        static Place endPlace;
        static List<PathNode> currentPathList = new List<PathNode>();
        static int currentDay;
        static Mission currentMission;
        static Sect currentSect;
        static string[] lasts = Settings1.Default.Human_LastName.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        static string[] malenames = Settings1.Default.Human_MaleName.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        static string[] femalenames = Settings1.Default.Human_FemaleName.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        static string[] allnames = Settings1.Default.Human_AllName.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        static string[] sectsNames = Settings1.Default.Sect_Name.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        static string[] sectsSuffixs = Settings1.Default.Sect_Suffix.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        static string[] sectsTypes = Settings1.Default.Sect_Type.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        static string[] sectsStands = Settings1.Default.Sect_Stand.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        static string[] terraintypes = Settings1.Default.Terrain_Type.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        static string[] distFeature = Settings1.Default.District_Feature.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        //static string[] lakefeature = Settings1.Default.Terrain_Lake_Feature.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        //static string[] oceanfeature = Settings1.Default.Terrain_Ocean_Feature.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        //static string[] missionType = Settings1.Default.Mission_Type.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);//任务类别
        static string[] reikiType = Settings1.Default.Reiki_Type.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);//灵气类别
        static string[] reikiTypeDesc = Settings1.Default.Reiki_Type_Desc.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);//灵气类别说明
        static string[] crossingTime = Settings1.Default.Crossing_Time.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);//灵气类别说明
        static string[] humanCondition = Settings1.Default.Human_Condition.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);//灵气类别说明
        //static string[] missionTypeDesc = Settings1.Default.Mission_Type_Desc.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);//灵气类别说明
        static string[] reikiColor = Settings1.Default.Reiki_Color.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);//灵气类别说明
        static string[] humanLevel = Settings1.Default.Human_Level.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);//灵气类别说明
        static char[] placeNames = Settings1.Default.Place_Name.ToArray();

        /// <summary>
        /// 所有地块
        /// </summary>
        public static List<Place> AllPlaceList { get => allPlaceList; set => allPlaceList = value; }
        /// <summary>
        /// 随机数种子
        /// </summary>
        public static int Seed { get => seed; set => seed = value; }
        /// <summary>
        /// 地块区划特征数组
        /// </summary>
        public static string[] DistFeature { get => distFeature; set => distFeature = value; }
        ///// <summary>
        ///// 湖泊特征数组
        ///// </summary>
        //public static string[] Lakefeature { get => lakefeature; set => lakefeature = value; }
        ///// <summary>
        ///// 海洋特征数组
        ///// </summary>
        //public static string[] Oceanfeature { get => oceanfeature; set => oceanfeature = value; }
        /// <summary>
        /// 当前选择的place
        /// </summary>
        //public static Place SelectPlace { get => selectPlace; set => selectPlace = value; }
        /// <summary>
        /// 所有门派集合
        /// </summary>
        public static List<Sect> AllSectList { get => allSectList; set => allSectList = value; }
        /// <summary>
        /// 门派名库
        /// </summary>
        public static string[] SectsNames { get => sectsNames; set => sectsNames = value; }
        /// <summary>
        /// 门派后缀名库
        /// </summary>
        public static string[] SectsSuffixs { get => sectsSuffixs; set => sectsSuffixs = value; }
        /// <summary>
        /// 门派类型库
        /// </summary>
        public static string[] SectsTypes { get => sectsTypes; set => sectsTypes = value; }
        /// <summary>
        /// 门派立场库
        /// </summary>
        public static string[] SectsStands { get => sectsStands; set => sectsStands = value; }
        /// <summary>
        /// 人姓
        /// </summary>
        public static string[] Lasts { get => lasts; set => lasts = value; }
        /// <summary>
        /// 男名
        /// </summary>
        public static string[] Malenames { get => malenames; set => malenames = value; }
        /// <summary>
        /// 女名
        /// </summary>
        public static string[] Femalenames { get => femalenames; set => femalenames = value; }
        /// <summary>
        /// 通用名
        /// </summary>
        public static string[] Allnames { get => allnames; set => allnames = value; }
        /// <summary>
        /// 所有人物集合
        /// </summary>
        public static List<Human> AllHumansList { get => allHumansList; set => allHumansList = value; }
        /// <summary>
        /// 所有区划集合
        /// </summary>
        public static List<District> AllDistrictList { get => allDistrictList; set => allDistrictList = value; }
        /// <summary>
        /// 任务类别type
        /// </summary>
        //public static string[] MissionType { get => missionType; set => missionType = value; }
        /// <summary>
        /// 灵气类型名称
        /// </summary>
        public static string[] ReikiType { get => reikiType; set => reikiType = value; }
        /// <summary>
        /// 灵气类型描述
        /// </summary>
        public static string[] ReikiTypeDesc { get => reikiTypeDesc; set => reikiTypeDesc = value; }
        /// <summary>
        /// 穿越地形时间
        /// </summary>
        public static string[] CrossingTime { get => crossingTime; set => crossingTime = value; }
        /// <summary>
        /// 人物状况（正常，受伤，被俘，失踪，死亡等）
        /// </summary>
        public static string[] HumanCondition { get => humanCondition; set => humanCondition = value; }
        /// <summary>
        /// 是否处于选择状态
        /// </summary>
        public static bool IsSelect { get => isSelect; set => isSelect = value; }
        /// <summary>
        /// 所有执行任务集合
        /// </summary>
        public static List<Mission> AllMissionList { get => allMissionList; set => allMissionList = value; }
        /// <summary>
        /// 任务类型描述数组
        /// </summary>
        //public static string[] MissionTypeDesc { get => missionTypeDesc; set => missionTypeDesc = value; }
        /// <summary>
        /// 地图绘制工具
        /// </summary>
        public static Graphics G1 { get => g1; set => g1 = value; }
        /// <summary>
        /// 寻路起点
        /// </summary>
        public static Place StartPlace { get => startPlace; set => startPlace = value; }
        /// <summary>
        /// 寻路终点
        /// </summary>
        public static Place EndPlace { get => endPlace; set => endPlace = value; }
        /// <summary>
        /// 当前寻路路径记录集合
        /// </summary>
        public static List<PathNode> CurrentPathList { get => currentPathList; set => currentPathList = value; }
        /// <summary>
        /// 当前日
        /// </summary>
        public static int CurrentDay { get => currentDay; set => currentDay = value; }
        /// <summary>
        /// 灵气颜色
        /// </summary>
        public static string[] ReikiColor { get => reikiColor; set => reikiColor = value; }
        /// <summary>
        /// 当前选择的任务
        /// </summary>
        public static Mission CurrentMission { get => currentMission; set => currentMission = value; }
        /// <summary>
        /// 当前选择的门派
        /// </summary>
        public static Sect CurrentSect { get => currentSect; set => currentSect = value; }
        /// <summary>
        /// 绘版2，前景
        /// </summary>
        public static Graphics G2 { get => g2; set => g2 = value; }
        /// <summary>
        /// 人物等阶名称数组
        /// </summary>
        public static string[] HumanLevel { get => humanLevel; set => humanLevel = value; }
        /// <summary>
        /// 地名用字数组集合
        /// </summary>
        public static char[] PlaceNames { get => placeNames; set => placeNames = value; }
        /// <summary>
        /// 分割string，默认'|'
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string[] Split(string info)
        {
            return info.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
