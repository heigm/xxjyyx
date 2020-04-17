using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxjjyx
{
    /// <summary>
    /// A*寻路类
    /// </summary>
    public class AStar
    {
        static List<PathNode> openPathList;
        static List<PathNode> closePathList;
        /// <summary>
        /// A星寻路方法
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <param name="allPlaces">所有有效点</param>
        /// <returns></returns>
        public static List<PathNode> AStarFindPath(Place start, Place end, List<Place> allPlaces)
        {
            if (start == null || end == null) { return null; }
            List<PathNode> pathList = new List<PathNode>();
            PathNode startNode = new PathNode(start);
            if (start == end) { pathList.Add(startNode);return pathList; }//原地，则直接返回
            closePathList = new List<PathNode>();//开启列表
            openPathList = new List<PathNode>();//关闭列表

            
            openPathList.Add(startNode);
            PathNode endNode = new PathNode(end);
            while (true)
            {
                List<Place> places = Draw.GetAroundPlaceList(allPlaces, startNode.Place);
                foreach (var p in places)
                {
                    FindAroundNodeToOpenList(p, startNode, endNode);
                }

                //死路判断
                if (openPathList.Count == 0) { return null; }
                //应该是已经在openlist中的点要计算好相应的FGH值，判断沿着点位走到第二个点中，如果找到周边有前open点，且点数比当前点小
                //则更新点位，删掉前一个点
                //openPathList.Sort(SortOpenList);//排序，最小F的为0位
                //PathNode minNode = openPathList.Find(obj => obj.F == openPathList.Min(o => o.F));//找到最小F值的node
                openPathList.OrderBy(o => o.F);
                PathNode minNode = openPathList[0];
                closePathList.Add(minNode);//加入close列表中
                //openPathList[0].ParentNode = startNode;
                startNode = minNode;
                openPathList.Remove(minNode);//open列表中删除

                if (startNode.Place == end)
                {
                    
                    while (startNode.ParentNode != null)
                    {
                        pathList.Add(startNode);
                        startNode = startNode.ParentNode;
                    }
                    pathList.Add(startNode);
                    pathList.Reverse();//起点终点顺序
                    return pathList;
                }
            }
        }


        /// <summary>
        /// 判断计算点并放入openlist中
        /// </summary>
        /// <param name="place">需判断的点</param>
        /// <param name="parentNode">父对象</param>
        /// <param name="endNode">终点</param>
        static void FindAroundNodeToOpenList(Place place, PathNode parentNode, PathNode endNode)
        {
            //List<Place> places = Draw.GetAroundPlaceList(Globle.AllPlaceList, pathNode.Place);
            PathNode pathNode = new PathNode(place);
            if (openPathList.Exists(obj => obj.Place == place) || closePathList.Exists(obj => obj.Place == place))//已在相关列表中
            {
                return;
            }
            pathNode.ParentNode = parentNode;//赋值父对象
            pathNode.G = parentNode.G + place.DistrictList.Sum(obj => obj.CrossingTime);//父对象的G加上自身的G
            pathNode.H = (int)Draw.ComparePoints(place.Coordinate, endNode.Place.Coordinate);//算图上直线距离
            pathNode.F = pathNode.G + pathNode.H;
            openPathList.Add(pathNode);
        }
        /// <summary>
        /// A*寻路方法，得到最短路径
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="end">终点</param>
        /// <param name="places">所有地点集合</param>
        /// <returns></returns>
        public static List<PathNode> AStarPath(Place start, Place end, List<Place> places)
        {
            if (start == null || end == null) { return null; }
            if (!places.Contains(end) || !places.Contains(start)) { return null; }
            openPathList = new List<PathNode>();
            closePathList = new List<PathNode>();
            PathNode startNode = new PathNode(start);
            openPathList.Add(startNode);

            while (openPathList.Count != 0)
            {
                openPathList.OrderBy(o => o.F);
                PathNode tempStart = openPathList[0];//找到最小F值的node
                openPathList.RemoveAt(0);
                closePathList.Add(tempStart);
                //找出相邻的点
                List<Place> aroundplaces = Draw.GetAroundPlaceList(places, tempStart.Place);
                foreach (var item in aroundplaces)
                {
                    if (openPathList.Exists(o => o.Place.Index == item.Index))
                    {
                        //计算G值, 如果比原来的大, 就什么都不做, 否则设置它的父节点为当前点,并更新G和F
                        PathNode node = openPathList.Find(o => o.Place.Index == item.Index);
                        FoundPoint(tempStart, node);
                    }
                    else
                    {
                        //如果它们不在开始列表里, 就加入, 并设置父节点,并计算GHF
                        PathNode node = new PathNode(item);
                        NotFoundPoint(tempStart, end, node);
                    }
                }
                if (openPathList.Exists(o => o.Place == end))
                {
                    List<PathNode> resultPath = new List<PathNode>();
                    PathNode parent = openPathList.Find(o => o.Place == end);
                    while (parent != null)
                    {
                        resultPath.Add(parent);
                        parent = parent.ParentNode;
                    }
                    return resultPath;
                }

            }
            return null;
        }

        static void FoundPoint(PathNode tempStart, PathNode node)
        {
            var G = CalcG(node);
            if (G < node.G)
            {
                node.ParentNode = tempStart;
                node.G = G;
                node.F = node.G + node.H;
            }
        }
        static void NotFoundPoint(PathNode tempStart, Place end, PathNode node)
        {
            node.ParentNode = tempStart;
            node.G = CalcG(node);
            node.H = (int)Draw.ComparePoints(end.Coordinate, node.Place.Coordinate);
            node.F = node.G + node.H;
            openPathList.Add(node);
        }

        static int CalcG(PathNode current)
        {
            int G = current.Place.DistrictList.Sum(o => o.CrossingTime);
            int parentG = current.ParentNode != null ? current.ParentNode.G : 0;
            return G + parentG;
        }



        /// <summary>
        /// 找到最短路径的place
        /// </summary>
        /// <returns></returns>
        static Place MinPlace(List<Place> openList, Place endPlace)
        {
            Place place = null;
            foreach (Place item in openList)
            {
                //int parentG = 0;
                //if (item.ParentPlace != null)
                //{
                //    parentG = item.DistrictList.Sum(obj => obj.CrossingTime);
                //}

                int G = item.DistrictList.Sum(obj => obj.CrossingTime)/*+ parentG*/;
                int H = (int)Draw.ComparePoints(item.Coordinate, endPlace.Coordinate);
                int F = G + H;
                if (place == null || place.DistrictList.Sum(obj => obj.CrossingTime) + (int)Draw.ComparePoints(place.Coordinate, endPlace.Coordinate) > F)
                {
                    place = item;
                }

            }
            return place;
        }
    }

    /// <summary>
    /// 寻路节点类
    /// </summary>
    public class PathNode
    {
        Place place;
        int g;
        int h;
        int f;
        PathNode parentNode;
        /// <summary>
        /// 记录的place
        /// </summary>
        public Place Place { get => place; set => place = value; }
        /// <summary>
        /// 起点到该点的值
        /// </summary>
        public int G { get => g; set => g = value; }
        /// <summary>
        /// 终点到该点的值
        /// </summary>
        public int H { get => h; set => h = value; }
        /// <summary>
        /// 合计值
        /// </summary>
        public int F { get => f; set => f = value; }
        /// <summary>
        /// 该节点的父节点
        /// </summary>
        public PathNode ParentNode { get => parentNode; set => parentNode = value; }
        /// <summary>
        /// 构造pathNode函数
        /// </summary>
        /// <param name="place"></param>
        public PathNode(Place place)
        {
            this.Place = place;
        }
    }
}
