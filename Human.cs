using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxjjyx
{
    public class Human
    {
        bool sex;
        string last;
        string name;
        int level;
        int age;
        int maxAge;
        int exp;
        int maxExp;
        int hp;
        int attck;
        int defense;
        int speed;
        Sect sect;//默认无门派
        Place setPlace;
        Place targetPlace;
        int status;
        int[] fiveElements = new int[5];
        int index;
        int picIndex;
        Mission mission;
        int condition;
        PointF location;
        List<PathNode> pathNodes;//人物的行动路线

        string character;
        /// <summary>
        /// 性别
        /// </summary>
        public bool Sex { get => sex; set => sex = value; }
        /// <summary>
        /// 姓
        /// </summary>
        public string Last { get => last; set => last = value; }
        /// <summary>
        /// 名
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// 等阶
        /// </summary>
        public int Level
        {
            get => level;
            set
            {
                LevelUp(value - level);
                level = value;

            }
        }
        /// <summary>
        /// 当前年龄
        /// </summary>
        public int Age
        {
            get => age;
            set
            {
                if (age > maxAge)
                {
                    //死亡
                    age = maxAge;
                }
                age = value;
            }
        }
        /// <summary>
        /// 最高年龄，突破可提高
        /// </summary>
        public int MaxAge { get => maxAge; set => maxAge = value; }
        /// <summary>
        /// 历练值
        /// </summary>
        public int Exp
        {
            get => exp;
            set
            {
                if (value > maxExp)//历练值不能超过极限历练值
                {
                    value = maxExp;
                }
                exp = value;
            }
        }
        /// <summary>
        /// 当前最高历练值，需突破提高
        /// </summary>
        public int MaxExp { get => maxExp; set => maxExp = value; }
        public int Hp { get => hp; set => hp = value; }
        public int Attck { get => attck; set => attck = value; }
        public int Defense { get => defense; set => defense = value; }
        public int Speed { get => speed; set => speed = value; }
        /// <summary>
        /// 门派序号
        /// </summary>
        public Sect Sect { get => sect; set => sect = value; }
        /// <summary>
        /// 身份地位
        /// </summary>
        public int Status { get => status; set => status = value; }
        /// <summary>
        /// 人物五行
        /// </summary>
        public int[] FiveElements { get => fiveElements; set => fiveElements = value; }
        /// <summary>
        /// 所处位置
        /// </summary>
        public Place SetPlace { get => setPlace; set => setPlace = value; }
        /// <summary>
        /// 人物序号，唯一
        /// </summary>
        public int Index { get => index; set => index = value; }
        /// <summary>
        /// 形象序号，对应图片
        /// </summary>
        public int PicIndex { get => picIndex; set => picIndex = value; }
        /// <summary>
        /// 人物当前任务
        /// </summary>
        public Mission Mission
        {
            get => mission;
            set
            {
                if (value == null)//赋值任务为空，则把任务执行者中自身删除
                {
                    mission.ExecutorList.Remove(this);
                    targetPlace = null;
                    pathNodes = null;
                }
                else
                {
                    pathNodes = AStar.AStarFindPath(this.SetPlace, value.Place, Globle.AllPlaceList);
                }
                mission = value;
            }
        }
        /// <summary>
        /// 人物当前状态（正常，受伤，被俘）
        /// </summary>
        public int Condition { get => condition; set => condition = value; }
        /// <summary>
        /// 所处坐标
        /// </summary>
        public PointF Location { get => location; set => location = value; }
        /// <summary>
        /// 行动目标点
        /// </summary>
        public Place TargetPlace { get => targetPlace; set => targetPlace = value; }
        /// <summary>
        /// 人物行动路线
        /// </summary>
        public List<PathNode> PathNodes
        {
            get => pathNodes;
            set
            {
                if (value == null)
                {
                    targetPlace = null;//行动路线如果设置为null，则目标也得为null
                }
                pathNodes = value;
            }
        }


        /// <summary>
        /// 构造人
        /// </summary>
        public Human(int index, string last, string name, bool sex, int age, int picIndex, Place setPlace)
        {
            this.index = index;
            this.Last = last;
            this.name = name;
            this.sex = sex;
            this.age = age;
            this.picIndex = picIndex;
            this.SetPlace = setPlace;
            this.Speed = 2;//默认速度
            this.Location = SetPlace.Coordinate;

            LevelUp(level);
            //sex = false;
            //if (random.Next(0, 2) == 1) { sex = true; }
            //name = Globle.SetName(sex);
        }
        /// <summary>
        /// 按规定速度移动，组队模式
        /// </summary>
        /// <param name="speed">移动速度</param>
        public void Move(int speed)
        {
            if (Mission == null || Mission.PathNodes == null) { return; }
            int index = Mission.PathNodes.FindIndex(o => o.Place == setPlace);
            if (index == Mission.PathNodes.Count - 1) { return; }//表示到最后地块了，不需要移动

            //当前地块
            Place current = SetPlace;
            //目标地块

            //if (Mission.PathNodes.Count == index + 1) {/* Mission.PathNodes = null;*/targetPlace = null; return; }//到达目的地了
            TargetPlace = Mission.PathNodes[index + 1].Place;
            //根据坐标距离和通过距离/speed计算坐标每日偏移量Draw.ComparePoints(current.Coordinate, target.Coordinate) + 
            float dis = current.DistrictList.Sum(o => o.CrossingTime) /*+ targetPlace.DistrictList.Sum(o => o.CrossingTime)*/;
            float days = dis / speed;//需移动天数
            int disX = current.Coordinate.X - TargetPlace.Coordinate.X;//得到两个点的坐标距离
            int disY = current.Coordinate.Y - TargetPlace.Coordinate.Y;
            float offsetX = disX / days;
            float offsetY = disY / days;
            if (Math.Abs(TargetPlace.Coordinate.X - location.X) <= Math.Abs(offsetX) && Math.Abs(TargetPlace.Coordinate.Y - location.Y) <= Math.Abs(offsetY))
            {
                SetPlace = TargetPlace;//设置human当前place
                Location = new PointF(TargetPlace.Coordinate.X, TargetPlace.Coordinate.Y);
                return;
            }
            Location = new PointF(location.X - offsetX, location.Y - offsetY);//坐标移动
        }
        /// <summary>
        /// 按弟子自身速度移动
        /// </summary>
        public void Move()
        {
            //Move(this.speed);
            if (pathNodes == null) { return; }//没有路径不能移动
            int index = PathNodes.FindIndex(o => o.Place == setPlace);
            if (index == PathNodes.Count - 1) { return; }//表示到最后地块了，不需要移动
            //if (setPlace == targetPlace || pathNodes.Count <= 1) { targetPlace = null; /*pathNodes = null;*/ return; }//到达目的地，删除路线不用移动
            TargetPlace = PathNodes[index + 1].Place;
            //根据坐标距离和通过距离/speed计算坐标每日偏移量Draw.ComparePoints(current.Coordinate, target.Coordinate) + 
            float dis = setPlace.DistrictList.Sum(o => o.CrossingTime) /*+ targetPlace.DistrictList.Sum(o => o.CrossingTime)*/;
            float days = dis / speed;//需移动天数
            int disX = setPlace.Coordinate.X - TargetPlace.Coordinate.X;//得到两个点的坐标距离
            int disY = setPlace.Coordinate.Y - TargetPlace.Coordinate.Y;
            float offsetX = disX / days;
            float offsetY = disY / days;
            if (Math.Abs(TargetPlace.Coordinate.X - location.X) <= Math.Abs(offsetX) && Math.Abs(TargetPlace.Coordinate.Y - location.Y) <= Math.Abs(offsetY))
            {
                SetPlace = TargetPlace;//设置human当前place
                Location = new PointF(TargetPlace.Coordinate.X, TargetPlace.Coordinate.Y);
                return;
            }
            Location = new PointF(location.X - offsetX, location.Y - offsetY);//坐标移动
        }


        /// <summary>
        /// 得到五行属性
        /// </summary>
        /// <param name="seed">种子</param>
        /// <param name="fire">火几率</param>
        /// <param name="soil">土几率</param>
        /// <param name="gold">金几率</param>
        /// <param name="water">水几率</param>
        /// <param name="wood">木几率</param>
        /// <param name="exclude">排除数，用于清除多项,不能大于4</param>
        /// <returns></returns>
        public static int[] GetFiveElements(int seed, double fire, double soil, double gold, double water, double wood, int exclude)
        {
            Random random = new Random(seed);//应该五行相加，再分配几率？？？
            double sum = fire + soil + gold + water + wood;
            fire = fire / sum;//根据总量计算几率
            soil = soil / sum;
            gold = gold / sum;
            water = water / sum;
            wood = wood / sum;
            int[] five = new int[5];
            five[0] = random.Next(0, (int)(fire * 100));
            five[1] = random.Next(0, (int)(soil * 100));
            five[2] = random.Next(0, (int)(gold * 100));
            five[3] = random.Next(0, (int)(water * 100));
            five[4] = random.Next(0, (int)(wood * 100));
            //int[] temp = (int[])five.Clone();

            for (int i = 0; i < exclude + 1; i++)
            {
                for (int j = 0; j < five.Length; j++)
                {
                    if (i < exclude)
                    {
                        if (five[j] == 0) { five[j] = int.MaxValue; }
                        if (five[j] > 0 && five[j] == five.Min())//大于0且最小
                        {
                            five[j] = int.MaxValue;
                            break;
                        }
                    }
                    else
                    {
                        if (five[j] == int.MaxValue)//将最大值设回0
                        {
                            five[j] = 0;
                        }
                    }
                }

            }
            int count = five.Sum();
            for (int i = 0; i < five.Length; i++)
            {
                if (five[i] == 0) { continue; }
                five[i] = (int)(((double)five[i] / (double)count) * 100);
            }
            count = five.Sum();
            if (count < 100)//总额不满100则加到最高值上
            {
                if (count == 0)
                    five[random.Next(0, five.Length)] = 100;
                for (int i = 0; i < five.Length; i++)
                {
                    if (five[i] == five.Max())
                    {
                        five[i] = five[i] + (100 - count);
                    }
                }
            }
            count = five.Sum();
            return five;
        }
        /// <summary>
        /// 设置人物
        /// </summary>
        /// <param name="isOnlySex">性别确定，-1随机，0男，1女</param>
        public static Human SetHuman(int seed, int index, int isOnlySex, Place setPlace)
        {
            int number = 0;
        A:
            Random random = new Random(seed + DateTime.Now.Millisecond);
            bool tempsex = false;
            if (isOnlySex == -1)//随机
            {
                if (random.Next(0, 2) == 0)
                {
                    tempsex = true;
                }
            }
            else if (isOnlySex == 0)//男
            {
                tempsex = true;
            }
            else if (isOnlySex == 1)//女
            {
                tempsex = false;
            }
            string templast = Globle.Lasts[random.Next(0, Globle.Lasts.Length)];
            string tempname = "";
            if (tempsex)//男
            {
                int tempint = random.Next(0, Globle.Malenames.Length + Globle.Allnames.Length);
                if (tempint >= Globle.Malenames.Length)//超出了
                {
                    tempname = Globle.Allnames[tempint - Globle.Malenames.Length];
                }
                else
                {
                    tempname = Globle.Malenames[tempint];
                }
            }
            else
            {
                int tempint = random.Next(0, Globle.Femalenames.Length + Globle.Allnames.Length);
                if (tempint >= Globle.Femalenames.Length)//超出了
                {
                    tempname = Globle.Allnames[tempint - Globle.Femalenames.Length];
                }
                else
                {
                    tempname = Globle.Femalenames[tempint];
                }
            }
            if (!Settings1.Default.Human_UsedName.Contains(templast + tempname))//没有重名重姓
            {
                Settings1.Default.Human_UsedName += templast + tempname + "|";
            }
            else
            {
                number++;
                goto A;
            }
            int age = random.Next(16, 25);
            int picIndex = -1;

            while (picIndex == -1 || Globle.AllHumansList.Exists(obj => obj.PicIndex == picIndex))
            {
                if (!tempsex)
                {
                    picIndex = random.Next(1, 846);
                }
                else
                {
                    picIndex = random.Next(1, 338);//337,845数量
                }
            }
            Human human = new Human(index, templast, tempname, tempsex, age, picIndex, setPlace)
            {
                Level = random.Next(0, 6)//测试
            };
            return human;
        }
        /// <summary>
        /// 升级涉及事件（年龄，攻防等）
        /// </summary>
        /// <param name="levelup">升级数</param>
        void LevelUp(int levelup)
        {
            if (levelup < 0) { return; }
            Random random = new Random(Globle.Seed + index + DateTime.Now.Millisecond);
            if (maxAge == 0)
            {
                maxAge = random.Next(70, 86);//基础年龄
            }
            if (MaxExp == 0)
            {
                MaxExp = 1000;//初始最大历练值
            }
            for (int i = 0; i < levelup; i++)
            {
                //    random = new Random(Globle.Seed + index + i + DateTime.Now.Millisecond);
                this.maxAge += random.Next(10, 14);//升级增加最大年龄值
                this.MaxExp += 1000 * level;
            }

        }
        /// <summary>
        /// 得到等阶XX层
        /// </summary>
        /// <returns></returns>
        public string GetLevel()
        {
            string temp;
            int lv = level / 10;
            int ceng = level % 10 ;
            if (level == 0)
            {
                temp = Globle.HumanLevel[lv];
            }
            else
            {
                if (ceng == 0)
                {
                    temp = Globle.HumanLevel[lv + 1]+ "凝练";
                }
                else
                {
                    temp = Globle.HumanLevel[lv + 1] + ceng + "层";
                }
                
            }
            return temp;
        }
        /// <summary>
        /// 画人
        /// </summary>
        /// <param name="humen"></param>
        public static void DrawHuman(List<Human> humen)
        {
            foreach (var human in humen)//错开绘制
            {
                Draw.DrawPlaceHexagon(Globle.G1, human.SetPlace);
                if (human.TargetPlace != null)
                    Draw.DrawPlaceHexagon(Globle.G1, human.TargetPlace);
                //绘制路线
                Draw.DrawPathLine(Globle.G1, human.pathNodes, human);
            }
            foreach (var human in humen)
            {
                Rectangle rect = new Rectangle((int)human.Location.X - 5, (int)human.Location.Y - 5, 10, 10);
                Globle.G1.DrawEllipse(Pens.White, rect);
                Brush brush = new SolidBrush(human.Sect.SectColor);
                Globle.G1.FillEllipse(brush, rect);
                brush.Dispose();
            }

        }
    }
}
