using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxjjyx
{
    /// <summary>
    /// 地形类
    /// </summary>
    public class Terrain
    {
        /// <summary>
        /// 随机方式获得listplace中的点位，用于生成集结地块（生成陆地，海洋等）
        /// </summary>
        /// <param name="placeList">可用地块</param>
        /// <param name="seed">随机种子</param>
        /// <param name="percent">生成百分比</param>
        /// <param name="scatterPoint">初始点位数</param>
        /// <returns></returns>
        public static List<Place> GetRandomPlaces(List<Place> placeList, int seed, double percent, int scatterPoint)
        {
            if (percent > 1) { return null; }
            if (placeList == null || placeList.Count == 0) { return null; }
            if (scatterPoint == 0 || scatterPoint >= placeList.Count) { return null; }
            Random random = new Random(seed);
            //List<Place> tempPlaces = new List<Place>();
            List<Place> usedplaces = new List<Place>();
            int s = placeList.Count / scatterPoint;//用于划区
            for (int i = 0; i < scatterPoint; i++)
            {
                int index = random.Next(0, s) + (s * i);
                if (usedplaces.Exists(obj => obj.Index == index)) { i--; continue; }//不能重复
                usedplaces.Add(placeList.Find(obj => obj.Index == index));//找到对应index的place并写入list
            }
            //下面按比例生成地块
            int count = (int)(placeList.Count * percent);//得到需生成的地块数

            //usedplaces.AddRange(tempPlaces);
            for (int i = 0; i < count; i++)
            {
                Place tempPlace = usedplaces[random.Next(0, usedplaces.Count)];//先随机到已使用的某个点位
                List<Place> findPlaces = Draw.GetAroundPlaceList(placeList, tempPlace);//找周围可用的点位，
                foreach (Place p in usedplaces)//注意排除已使用的
                {
                    findPlaces.Remove(p);
                }
                if (findPlaces.Count == 0) { i--; continue; }//没有地块则重来
                usedplaces.Add(findPlaces[random.Next(0, findPlaces.Count)]);//将随机到的点位加入已使用点位list中
            }
            return usedplaces;
        }
        /// <summary>
        /// 设置未使用的place的地形type
        /// </summary>
        /// <param name="placeList">所有的place</param>
        public static void SetUnUsedPlacesType(List<Place> placeList)
        {
            int row = Settings1.Default.Draw_Row;
            int column = Settings1.Default.Draw_Column;
            List<Place> seaPlaces = new List<Place>();
            for (int i = 0; i < row; i++)//横向的
            {
                if (placeList[i * column].TerrainType == -1)
                {
                    placeList[i * column].TerrainType = 2;//2ocean海洋
                    seaPlaces.Add(placeList[i * column]);
                }
                if (i > 0 && placeList[i * column - 1].TerrainType == -1)
                {
                    placeList[i * column - 1].TerrainType = 2;
                    seaPlaces.Add(placeList[i * column - 1]);
                }

            }
            for (int i = 0; i < column; i++)//纵向的
            {
                if (placeList[i].TerrainType == -1)
                {
                    placeList[i].TerrainType = 2;
                    seaPlaces.Add(placeList[i]);
                }
                if (i > 0 && placeList[row * column - i].TerrainType == -1)
                {
                    placeList[row * column - i].TerrainType = 2;
                    seaPlaces.Add(placeList[row * column - i]);
                }
            }
            List<Place> unUsedPlaces = placeList.FindAll(obj => obj.TerrainType == -1);//找到所有未设定的地块
            for (int j = 0; j < 2; j++)//多遍历一次
            {
                for (int i = 0; i < unUsedPlaces.Count; i++)//遍历
                {
                    List<Place> tempplaces = Draw.GetAroundPlaceList(placeList, unUsedPlaces[i]);
                    if (tempplaces.Exists(obj => obj.TerrainType == 2))
                    {
                        unUsedPlaces[i].TerrainType = 2;
                        seaPlaces.Add(unUsedPlaces[i]);
                        unUsedPlaces.Remove(unUsedPlaces[i]);
                        i = 0;
                    }

                }
            }
            foreach (Place p in unUsedPlaces)
            {
                //剩下的就是湖泊
                p.TerrainType = 1;
            }

        }
        /// <summary>
        /// 给地块赋值区域（默认9块），根据地块terrain类型进行赋值
        /// </summary>
        /// <param name="allPlaces">需要赋值的地块</param>
        /// <param name="seed">随机数种子</param>
        public static void SetPlaceDistrict(List<Place> allPlaces, int seed)
        {
            //int[] districts = new int[9];
            Random random = new Random(seed);
            //List<District> districts=new List<District>();
            foreach (Place p in allPlaces)
            {
                p.DistrictList = new List<District>();//加入区划
                for (int i = 0; i < 9; i++)//9是默认区划数
                {
                    int r = random.Next(0, Globle.DistFeature.Length - 8);//默认陆地
                    District district = new District(Globle.AllDistrictList.Count + i, p, r);
                    p.DistrictList.Add(district);
                    //p.DistrictList[i].TypeIndex = r;
                }
                Globle.AllDistrictList.AddRange(p.DistrictList);//加入到全局数据中
                if (p.TerrainType == 0)//陆地
                {
                    if (p.DistrictList.Any(obj => obj.TypeIndex == 3) && !p.DistrictList.Any(obj => obj.TypeIndex == 2))//表示有绿洲且无沙漠,则清除绿洲
                    {
                        for (int i = 0; i < p.DistrictList.Count; i++)
                        {
                            if (p.DistrictList[i].TypeIndex == 3)
                            {
                                int r = random.Next(0, Globle.DistFeature.Length - 8);//排除湖泊和海洋区划类型
                                p.DistrictList[i].TypeIndex = r;
                                if (r == 2)//有沙漠则不再循环
                                { break; }
                                else
                                {
                                    i = 0;
                                    continue;
                                }
                            }
                        }
                    }
                }
                else if (p.TerrainType == 1)//湖泊
                {
                    bool isLand = Draw.GetAroundPlaceList(allPlaces, p).Exists(obj => obj.TerrainType == 0);//判断是否靠岸
                    for (int i = 0; i < p.DistrictList.Count; i++)
                    {
                        int r = random.Next(11, Globle.DistFeature.Length - 4);//11开始，排除非湖泊类区划
                        if (isLand == false && r == 12 || r == 13)//没有陆地,不能形成河流和湿地,12为河流13湿地
                        {
                            i--;
                            continue;
                        }
                        p.DistrictList[i].TypeIndex = r;
                        if (p.DistrictList[i].TypeIndex != 14)
                        {
                            if (random.Next(0, 4) == 0)//设置来偏移湖泊内容，目前1/4的几率非湖泊
                            {
                                p.DistrictList[i].TypeIndex = r;
                            }
                            else
                            {
                                p.DistrictList[i].TypeIndex = 14;//设为湖泊
                            }
                        }
                    }
                }
                else if (p.TerrainType == 2)//海洋
                {
                    bool isLand = Draw.GetAroundPlaceList(allPlaces, p).Exists(obj => obj.TerrainType == 0);//判断是否靠岸
                    for (int i = 0; i < p.DistrictList.Count; i++)
                    {
                        int r = random.Next(15, Globle.DistFeature.Length);
                        if (isLand == false && r == 17)//没有陆地,不能形成半岛,17为半岛
                        {
                            i--;
                            continue;
                        }
                        p.DistrictList[i].TypeIndex = r;
                        if (p.DistrictList[i].TypeIndex != 18)
                        {
                            if (random.Next(0, 4) == 0)//设置来偏移海洋内容，目前1/4的几率非海洋
                            {
                                p.DistrictList[i].TypeIndex = r;
                            }
                            else
                            {
                                p.DistrictList[i].TypeIndex = 18;//设为海洋
                            }
                        }

                    }

                }
            }
        }
    }
}
