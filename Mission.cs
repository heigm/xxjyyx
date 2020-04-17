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
    /// 任务类
    /// </summary>
    public class Mission
    {
        int _index;//任务序号
        string _name;//任务名称        
        int _maxProgress;//任务工量   
        string _explain;//任务说明
        Place _place;//任务地点
        int _progress;//任务进度
        int _type;//任务类型：探索|护卫|寻宝|救人|潜入|巡视|经营|突破|修筑|炼丹|炼宝|种植|驯养|挖矿|买卖|修炼|休养|传道|著述
        //int _prestige;//奖励威望
        int _exp;//奖励历练值
        List<Resource> _resources;//奖励资源
        List<Item> _items;//奖励物品
        int _loyal;//忠诚影响
        List<PathNode> pathNodes;//任务路径

        int _startDay = -1;//任务开启时间
        int _speed;//任务速度
        int _isComplete;//任务是否完成
        List<Human> _executorList;//任务执行者集合
        bool _isLoop;//是否循环执行


        public int Index { get => _index; set => _index = value; }
        public string Name { get => _name; set => _name = value; }
        /// <summary>
        /// 任务工量
        /// </summary>
        public int MaxProgress
        {
            get => _maxProgress;
            set
            {
                Exp = value * 2;
                _maxProgress = value;
            }
        }
        public string Desc { get => _explain; set => _explain = value; }
        public Place Place
        {
            get => _place;
            set
            {
                //确定了任务地点，就可以判断极限进度了
                if (value != null)
                    MaxProgress = value.DistrictList.Sum(o => o.CrossingTime);
                _place = value;
            }
        }
        /// <summary>
        /// 任务完成工量
        /// </summary>
        public int Progress { get => _progress; set => _progress = value; }
        /// <summary>
        /// 任务类别
        /// </summary>
        public int Type { get => _type; set => _type = value; }
        public int Exp { get => _exp; set => _exp = value; }
        public List<Resource> Resources { get => _resources; set => _resources = value; }
        public List<Item> Items { get => _items; set => _items = value; }
        public int Loyal { get => _loyal; set => _loyal = value; }
        public int StartDay { get => _startDay; set => _startDay = value; }
        /// <summary>
        /// 是否完成,0进行1完成2失败
        /// </summary>
        public int IsComplete { get => _isComplete; set => _isComplete = value; }
        /// <summary>
        /// 执行者列表
        /// </summary>
        public List<Human> ExecutorList
        {
            get => _executorList;
            set
            {
                if (_executorList != null)//将相关值都变为null
                {
                    foreach (var h in _executorList)
                    {
                        h.Mission = null;
                    }
                }
                if (value != null)//重新赋值
                {
                    foreach (var h in value)
                    {
                        h.Mission = this;
                    }
                }
                _speed = value.Min(o => o.Speed);//赋值最低的速度
                //_castTime = _castTime / ExecutorList.Count;
                _executorList = value;
            }
        }
        public bool IsLoop { get => _isLoop; set => _isLoop = value; }
        /// <summary>
        /// 任务路径
        /// </summary>
        public List<PathNode> PathNodes
        {
            get => pathNodes;
            set
            {
                pathNodes = value;
                this.Place = pathNodes[pathNodes.Count - 1].Place;
            }
        }
        /// <summary>
        /// 任务速度
        /// </summary>
        public int Speed { get => _speed; set => _speed = value; }

        /// <summary>
        /// 任务构造
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="explain">任务说明</param>
        /// <param name="place">任务地点</param>
        /// <param name="type"></param>
        /// <param name="exp"></param>
        /// <param name="res">奖励资源，可为null</param>
        /// <param name="items">奖励物品，可为null</param>
        /// <param name="loyal"></param>
        public Mission(int index, string name, string explain, Place place, int type, List<Resource> res, List<Item> items, int loyal)
        {
            this.Index = index;
            this.Name = name;
            //this._timeLimit = timeLimit;
            this.Desc = explain;
            this.Place = place;
            //this.CastTime = castTime;
            this.Type = type;
            //this.Exp = exp;
            if (res != null)
            {
                this.Resources = res;
            }
            if (items != null)
            {
                this.Items = items;
            }
            this.Loyal = loyal;
        }
        public Mission(int type, Place place)
        {
            this.Type = type;
            this.Name = Globle.Split(MissionInfo.Default.Name)[type];
            this.Desc = Globle.Split(MissionInfo.Default.Desc)[type];
            this.MaxProgress = place.DistrictList.Sum(o => o.CrossingTime);
            this.Exp = MaxProgress * 2;
            this.Place = place;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="humen">执行任务的弟子</param>
        public void PerformMission()
        {
            if (IsComplete != 0) { return; }//任务不在进行状态
            foreach (var human in ExecutorList)
            {
                if (human.SetPlace != this.Place)//表示有人未到达目的地,没有全部到达任务不开始
                {
                    return;
                }
            }
            if (this.StartDay == -1)
            {
                this.StartDay = Globle.CurrentDay;//赋值开始日期
            }
            //执行任务工作，增加工量
            this.Progress += this.ExecutorList.Sum(o => o.Level) + 1 * ExecutorList.Count;
            //int remain = this.StartDay + this.Progress - Globle.CurrentDay;
            if (MaxProgress <= Progress)//达到工量
            {
                //任务完成,应根据type反馈
                switch (Type)
                {
                    case 0:                        //探索
                        this.Place.IsExplore = true;
                        break;
                    case 3://占据
                        if (Place.Sect == null)
                        {
                            Place.Sect = this.ExecutorList[0].Sect;//将该据点占据，并建设据点建筑
                            Place.DistrictList[0].Build = new Build(1);
                        }
                        if (Place.DistrictList[0].Build.BuildProgress < Place.DistrictList[0].Build.BuildCastTime)
                        {
                            //还未结束任务
                            Place.DistrictList[0].Build.BuildProgress += this.ExecutorList.Sum(o => o.Level) + 1 * ExecutorList.Count;
                            return;
                        }
                        break;
                    case 13:                        //巡视
                        Place.OrderValue += ExecutorList.Sum(o => o.Level) + 1;//根据人员增加
                        break;
                    default:
                        break;
                }
                Draw.DrawPlaceHexagon(Globle.G1, Place);//直接更新地块
                //foreach (var human in ExecutorList)
                for (int i = ExecutorList.Count - 1; i >= 0; i--)
                {
                    ExecutorList[i].Exp += this.Exp /*/ ExecutorList.Count*/;//平分exp
                    ExecutorList[i].Mission = null;//人物当前任务设置为null
                }
                //this.ExecutorList = null;
                this.IsComplete = 1;

            }

        }
        /// <summary>
        /// 画任务
        /// </summary>
        public void DrawMission()
        {
            //画任务路线,当前点画到终点，终点描边
            Draw.DrawPathLine(Globle.G1, pathNodes, this.ExecutorList[0]);

        }
        /// <summary>
        /// 判断选择点位是否错误
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        public string JudgeError(Place place)
        {
            if ((Globle.CurrentPathList == null))
            {
                return "没有到达路径！请重新选择地点。";
            }
            if (this.Type == 0 && place.IsExplore == true)//探索
            {
                return "该地点已经探索完毕！请重新选择地点。";
            }
            if (this.Type == 3 && (place.IsExplore == false || place.Sect != null))//占据
            {
                return "该地点未探索或已被占据！请重新选择地点。";
            }
            if (this.Type == 12 && (place.Sect == null || place.Sect != Globle.CurrentSect))//巡视
            {
                //表示当前点与任务者的门派不同，不能巡视
                return "选择地点不是本门派占有，不能进行巡视！";
            }
            return null;
        }

        /// <summary>
        /// 得到当地可用任务集合
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        public static List<string> GetLocationMissionList(Place place)
        {
            List<string> mList = new List<string>();
            foreach (var mType in Globle.Split(MissionInfo.Default.Name))
            {
                if (place.IsExplore == false && mType == "探索")
                {
                    mList.Add(mType);
                }
                if (place.IsExplore == true && place.Sect == null && mType == "占据")
                {
                    mList.Add(mType);
                }
                if (place.Sect != null && mType == "巡视")
                {
                    mList.Add(mType);
                }
            }

            return mList;
        }

    }
}
