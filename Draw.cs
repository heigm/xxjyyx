using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxjjyx
{
    /// <summary>
    /// 绘制类
    /// </summary>
    public class Draw
    {


        /// <summary>
        /// 使用外切圆的方法绘制一个正多边形
        /// </summary>
        /// <param name="stage">要绘制图形的设备</param>
        /// <param name="center">正多边形外切圆的圆心</param>
        /// <param name="radius">正多边形外切圆的半径</param>
        /// <param name="sideCount">正多边形的边数</param>
        /// <param name="sideColor">边色</param>
        /// <param name="isFill">是否填充</param>
        /// <param name="fillColor">填充颜色</param>
        public static void DrawRegularPoly(Graphics stage, Point center, int radius, int sideCount, Pen sideColor, bool isFill, Brush fillColor)
        {
            // 多边形至少要有3条边，边数不达标就返回。
            if (sideCount < 3) return;
            // 每条边对应的圆心角角度，精确为浮点数。使用弧度制，360度角为2派
            double arc = 2 * Math.PI / sideCount;
            // 为多边形创建所有的顶点列表
            var pointList = new List<Point>();
            for (int i = 0; i < sideCount; i++)
            {
                var curArc = arc * i; // 当前点对应的圆心角角度
                var pt = new Point();
                // 就是简单的三角函数正余弦根据圆心角和半径算点坐标。这里都取整就行
                pt.X = center.X + (int)(radius * Math.Cos(curArc));
                pt.Y = center.Y + (int)(radius * Math.Sin(curArc));
                pointList.Add(pt);
            }
            // 在给出的场景中用黑笔把这个多边形画出来
            //stage.FillEllipse(Brushes.Red, pointList[0].X, pointList[0].Y, 3, 3);//画第一个点的位置
            //stage.FillEllipse(Brushes.Blue, pointList[1].X, pointList[1].Y, 3, 3);//画第2个点的位置
            stage.DrawPolygon(Pens.Black, pointList.ToArray());
            if (isFill)
            {
                stage.FillPolygon(fillColor, pointList.ToArray());
            }
            //stage.FillPolygon(Brushes.Red, pointList.ToArray());//填充
        }
        /// <summary>
        /// 绘制和填充地块六边形
        /// </summary>
        /// <param name="g">绘版</param>
        /// <param name="place">要绘制的地块</param>
        /// <param name="sideColor">描边颜色</param>
        /// <param name="isFill">是否填充</param>
        /// <param name="fillColor">填充颜色</param>
        public static void DrawPlaceHexagon(Graphics g, Place place, Pen sideColor, bool isFill, Brush fillColor)
        {
            if (isFill)
            {
                g.FillPolygon(fillColor, place.Points);
            }
            g.DrawPolygon(sideColor, place.Points);
        }
        /// <summary>
        /// 绘制和填充地块六边形，按默认颜色填充传入的所有place
        /// </summary>
        /// <param name="g">绘版</param>
        /// <param name="allPlaces">需填充的place集合</param>
        public static void DrawPlaceHexagon(Graphics g, List<Place> allPlaces)
        {
            foreach (Place p in allPlaces)
            {
                //Draw.DrawRegularPoly(g, p.Coordinate,30,6, Pens.Black, true, Brushes.Green);
                if (p.TerrainType == 0)//plain
                {
                    Draw.DrawPlaceHexagon(g, p, Pens.Black, true, Brushes.LightGreen);
                }
                else if (p.TerrainType == 2)//ocean
                {
                    Draw.DrawPlaceHexagon(g, p, Pens.Black, true, Brushes.LightSkyBlue);
                }
                else if (p.TerrainType == 1)//lake
                {
                    Draw.DrawPlaceHexagon(g, p, Pens.Black, true, Brushes.SkyBlue);
                }
                Brush brush;
                Color color;
                if (p.IsExplore == false)//未探索
                {
                    color = Color.FromArgb(150, 100, 100, 100);//覆盖
                    brush = new SolidBrush(color);
                    g.FillPolygon(brush, p.Points);
                }
                if (p.Sect != null)//有门派
                {
                    color = Color.FromArgb(50, p.Sect.SectColor);//覆盖颜色
                    brush = new SolidBrush(p.Sect.SectColor);
                    g.FillPolygon(brush, p.Points);
                    DrawStringPathAndFill(g, p.Sect.SectName + p.Sect.SectSuffix, null, p.Coordinate, null, true, null);//绘制门派名称
                }
                ////测试点位通过时间
                //if (p.DistrictList != null)
                //    g.DrawString(p.DistrictList.Sum(obj => obj.CrossingTime).ToString(), new Font("黑体", 10), Brushes.White, p.Coordinate);
            }
        }
        /// <summary>
        /// 绘制和填充地块六边形，按默认颜色填充传入的单个place
        /// </summary>
        /// <param name="g"></param>
        /// <param name="place"></param>
        public static void DrawPlaceHexagon(Graphics g, Place place)
        {
            List<Place> places = new List<Place>();
            places.Add(place);
            DrawPlaceHexagon(g, places);
        }
        /// <summary>
        /// 绘制和填充文字
        /// </summary>
        /// <param name="g"></param>
        /// <param name="str">文字内容</param>
        /// <param name="font"></param>
        /// <param name="point">文本起点位置</param>
        /// <param name="pen">描边，可为null</param>
        /// <param name="isFill">是否填充</param>
        /// <param name="brush">填充,可为null</param>
        public static void DrawStringPathAndFill(Graphics g, string str, Font font, Point point, Pen pen, bool isFill, Brush brush)
        {
            GraphicsPath path = new GraphicsPath();//描边醒目
            if (font == null)
            {
                font = new Font("黑体", 15, FontStyle.Bold);
            }
            float emSize = g.DpiY * font.SizeInPoints / 72;
            path.AddString(str, font.FontFamily, (int)font.Style, emSize, point, StringFormat.GenericTypographic);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (pen == null)
            {
                pen = Pens.Black;
            }
            if (brush == null)
            {
                brush = Brushes.White;
            }
            g.DrawPath(pen, path);
            if (isFill != false)
            {
                g.FillPath(brush, path);
            }

        }
        /// <summary>
        /// 绘制路线，默认
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pathList">路线集合</param>
        public static void DrawPathLine(Graphics g, List<PathNode> pathList, Human human)
        {
            if (pathList == null || pathList.Count <= 1) { return; }

            List<Point> points = new List<Point>();
            //Brush brush = new SolidBrush(Color.FromArgb(50, Color.White));
            using (Brush brush = new SolidBrush(Color.FromArgb(50, Color.White)))
            {
                if (human == null)
                {
                    DrawPlaceHexagon(g, pathList[0].Place, Pens.White, true, brush);//start
                    DrawPlaceHexagon(g, pathList[pathList.Count - 1].Place, Pens.White, true, brush);//target
                    int sum = 0;
                    foreach (var item in pathList)
                    {
                        points.Add(item.Place.Coordinate);
                        sum += item.Place.DistrictList.Sum(o => o.CrossingTime);
                    }
                }
                else
                {
                    if (human.SetPlace == pathList[pathList.Count - 1].Place)
                    {
                        //到达最终点
                        return;
                    }
                    bool isAdd = false;
                    for (int i = 0; i < pathList.Count; i++)
                    {
                        //Draw.DrawPlaceHexagon(g, pathList[i].Place);
                        if (human.SetPlace == pathList[i].Place)
                        {
                            Point point = new Point((int)human.Location.X, (int)human.Location.Y);
                            points.Add(point);
                            isAdd = true;
                            continue;
                        }
                        if (isAdd)
                        {
                            points.Add(pathList[i].Place.Coordinate);
                        }
                    }
                }
                //if (points.Count <= 1) { return; }
                g.DrawLines(Pens.White, points.ToArray());
            }
            //g.DrawString("路程：" + sum.ToString(), new Font("黑体", 10, FontStyle.Bold), Brushes.Red, pathList[pathList.Count - 1].Place.Coordinate);
            //brush.Dispose();

        }

        /// <summary>
        /// 使用外切圆的方法绘制一个正多边形,并返回相关的点
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="sideCount"></param>
        /// <returns></returns>
        public static List<Point> GetRegularPoly(Point center, int radius, int sideCount)
        {
            // 多边形至少要有3条边，边数不达标就返回。
            if (sideCount < 3) return null;
            // 每条边对应的圆心角角度，精确为浮点数。使用弧度制，360度角为2派
            double arc = 2 * Math.PI / sideCount;
            // 为多边形创建所有的顶点列表
            var pointList = new List<Point>();
            for (int i = 0; i < sideCount; i++)
            {
                var curArc = arc * i; // 当前点对应的圆心角角度
                var pt = new Point();
                // 就是简单的三角函数正余弦根据圆心角和半径算点坐标。这里都取整就行
                pt.X = center.X + (int)(radius * Math.Cos(curArc));
                pt.Y = center.Y + (int)(radius * Math.Sin(curArc));
                pointList.Add(pt);
            }
            return pointList;
        }
        /// <summary>
        /// 直接获得六边形的六个顶点
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Point[] GetHexagonPoints(Point center, int radius)
        {
            return GetRegularPoly(center, radius, 6).ToArray();
        }
        /// <summary>
        /// 根据传入的居中点，算出周边六边形的居中点
        /// </summary>
        /// <param name="center">传入的居中点</param>
        /// <param name="radius">半径</param>
        /// <returns></returns>
        public static List<Point> GetHexagonAroundCenterPoints(Point center, int radius)
        {
            List<Point> centerList = new List<Point>();
            //传入第一个的位置，根据第一个位置找周边6个
            Point[] points = GetHexagonPoints(center, radius);//先得到六个顶点
            //右上角↗
            centerList.Add(new Point(points[5].X + radius, points[5].Y));
            //右下角1↘
            centerList.Add(new Point(points[1].X + radius, points[1].Y));
            //最下↓
            centerList.Add(new Point(center.X, center.Y - (center.Y - points[1].Y) * 2));
            //左下角↙
            centerList.Add(new Point(points[2].X - radius, points[2].Y));
            //左上↖
            centerList.Add(new Point(points[4].X - radius, points[4].Y));
            //最上↑
            centerList.Add(new Point(center.X, center.Y + (points[4].Y - center.Y) * 2));

            return centerList;
        }

        /// <summary>
        /// 设置起点和行列数，生成六边形集合
        /// </summary>
        /// <param name="startPoint">起点</param>
        /// <param name="radius">半径</param>
        /// <param name="row">行数量</param>
        /// <param name="column">列数量</param>
        public static List<Point> SetHexagons(Point startPoint, int radius, int row, int column)
        {
            List<Point> pointList = new List<Point>();
            //pointList.Add(startPoint);//先把起点加入
            List<Point> points = GetHexagonAroundCenterPoints(startPoint, radius);//得到起点周围的六个六边形居中点

            //平铺
            //根据数量开方，按正方形分布
            //int row = (int)(Math.Sqrt(number));
            int ii = 0;
            for (int i = 0; i < row; i++)//→
            {
                Point tempPoint = points[2];
                if (i == 0)
                {
                    tempPoint = startPoint;
                }
                if (i % 2 > 0)
                {
                    points = GetHexagonAroundCenterPoints(pointList[pointList.Count - column], radius);//重新赋值
                    tempPoint = points[1];
                }
                else if (i > 0)
                {
                    points = GetHexagonAroundCenterPoints(pointList[pointList.Count - column], radius);//重新赋值
                    tempPoint = points[0];
                }
                for (int j = 0; j < column; j++)//↓
                {
                    if (j > 0)
                    {
                        //先竖着
                        tempPoint = points[2];
                    }
                    pointList.Add(tempPoint);
                    points = GetHexagonAroundCenterPoints(pointList[pointList.Count - 1], radius);//重新赋值
                }
                ii++;
            }

            return pointList;
        }

        /// <summary>
        /// 根据所选地块得到周围地块
        /// </summary>
        /// <param name="placeList">全部有效地块</param>
        /// <param name="place">所选地块</param>
        /// <returns></returns>
        public static List<Place> GetAroundPlaceList(List<Place> placeList, Place place)
        {
            int index = place.Index;
            List<Place> aroundPlaceList = new List<Place>();
            List<Point> points = GetHexagonAroundCenterPoints(place.Coordinate, Settings1.Default.Draw_Radius);//得到周边地块的中心点
            foreach (var p in points)
            {
                foreach (var item in placeList)
                {
                    if (ComparePoints(item.Coordinate, p) <= 1)
                    {
                        aroundPlaceList.Add(item);
                        break;
                    }
                }
            }
            return aroundPlaceList;
        }


        /// <summary>
        /// 计算两个point之间的距离
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double ComparePoints(Point a, Point b)
        {
            double value = Math.Sqrt(Math.Abs(a.X - b.X) * Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) * Math.Abs(a.Y - b.Y));
            return value;
        }

        /// <summary>
        /// 判断点是否在多边形内
        /// </summary>
        /// <param name="position">点位置</param>
        /// <param name="points">多边形点位数组</param>
        /// <returns></returns>
        public static bool IsPointInPolygon(Point position, Point[] points)
        {
            GraphicsPath myGraphicsPath = new GraphicsPath();
            Region myRegion = new Region();
            myGraphicsPath.Reset();

            myGraphicsPath.AddPolygon(points);
            myRegion.MakeEmpty();
            myRegion.Union(myGraphicsPath);
            //返回判断点是否在多边形里
            bool myPoint = myRegion.IsVisible(position);
            return myPoint;
        }
    }
}
