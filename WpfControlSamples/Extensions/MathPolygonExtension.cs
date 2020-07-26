using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace WpfControlSamples.Extensions
{
    static class MathLineExtension
    {
        // 直線の妥当性チェック
        public static bool IsValidLine(Point p0, Point p1) => p0 != p1;

        // 距離
        public static double GetDistance(Point p0, Point p1)
        {
            var x = p1.X - p0.X;
            var y = p1.Y - p0.Y;
            return Math.Sqrt((x * x) + (y * y));
        }

        // 傾き
        public static double GetSlope(Point p0, Point p1)
        {
            var bb = p1.X - p0.X;
            return bb == 0 ? 0 : (p1.Y - p0.Y) / bb;
        }

        // 切片
        public static double GetIntercept(Point p0, Point p1)
        {
            var (_, b) = GetSlopeIntercept(p0, p1);
            return b;
        }

        // 傾きと切片
        public static (double slope, double intercept)GetSlopeIntercept(Point p0, Point p1)
        {
            var a = GetSlope(p0, p1);
            var b = p0.Y - (a * p0.X);
            return (a, b);
        }

        // 2本の直線(傾き/切片)から交点を求める
        public static Point GetIntersectionPoint(double a, double b, double c, double d)
        {
            var bb = a - c;
            if (bb == 0) throw new ArgumentException();

            var x = (d - b) / bb;
            var y = (a * d - b * c) / bb;
            return new Point(x, y);
        }

        // 2点の中点
        public static Point GetMiddlePoint(Point p0, Point p1)
        {
            var x = (p1.X - p0.X) / 2.0;
            var y = (p1.Y - p0.Y) / 2.0;
            return new Point(p0.X + x, p0.Y + y);
        }
    }

    static class MathTriangleExtension
    {
        /*  三角形の５心  http://yasu21.o.oo7.jp/sub1.htm
         *    重心：３つの中線の交点
         *    内心：３つの角の２等分線の交点
         *    傍心：１つの頂点における内角の２等分線と，他の２つの頂点における外角の２等分線の交点
         *    外心：３つの垂直二等分線の交点
         *    垂心：各頂点から対辺またはその延長上に下ろした垂線の交点
         */

        /// <summary>
        /// 多角形の妥当性チェック
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static bool IsValidTriangle(this Point[] points)
        {
            if (points.Length != 3) throw new ArgumentException("not a triangle.");

            // 重複した点があれば Invalid
            if (points.GroupBy(p => p).Any(ps => ps.Count() > 1)) return false;

            // 一直線なら Invalid (double誤差を弾くため最大/最小差から判定)
            if (points.Select(p => p.X).Max() - points.Select(p => p.X).Min() <= 0.0001) return false;
            if (points.Select(p => p.Y).Max() - points.Select(p => p.Y).Min() <= 0.0001) return false;

            return true;
        }

        /// <summary>
        /// 三角形の面積
        /// </summary>
        public static double GetTriangleArea(Point p0, Point p1, Point p2)
        {
            var a = p0.X * p1.Y + p1.X * p2.Y + p2.X * p0.Y;
            var b = p0.Y * p1.X + p1.Y * p2.X + p2.Y * p0.X;
            return Math.Abs((a - b) / 2.0);
        }

        /// <summary>
        /// 三角形の面積
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static double GetTriangleArea(this Point[] points)
        {
            if (points.Length != 3) throw new ArgumentException("not a triangle.");
            return GetTriangleArea(points[0], points[1], points[2]);
        }

        /// <summary>
        /// 三角形の重心を求める(重心：３つの中線の交点)
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Point GetTriangleCenterPoint(Point p0, Point p1, Point p2)
        {
            var x = (p0.X + p1.X + p2.X) / 3.0;
            var y = (p0.Y + p1.Y + p2.Y) / 3.0;
            return new Point(x, y);
        }

        /// <summary>
        /// 三角形の重心を求める(重心：３つの中線の交点)
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Point GetTriangleCenterPoint(this Point[] points)
        {
            if (points.Length != 3) throw new ArgumentException("not a triangle.");
            return GetTriangleCenterPoint(points[0], points[1], points[2]);
        }

        /// <summary>
        /// 点ABC から ∠B
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static double GetAngle(this Point[] ps)
        {
            if (ps.Length != 3) throw new ArgumentException("cannot calculate angle.");

            // 三角形になっていなければ計算不可
            if (!IsValidTriangle(ps)) return double.NaN;

            var ba = ps[0] - ps[1];
            var bc = ps[2] - ps[1];

            var babc = (ba.X * bc.X) + (ba.Y * bc.Y);
            var ban = (ba.X * ba.X) + (ba.Y * ba.Y);
            var bcn = (bc.X * bc.X) + (bc.Y * bc.Y);
            var bb = Math.Sqrt(ban * bcn);

            if (bb == 0) return double.NaN;
            var radian = Math.Acos(babc / bb);

            var angle = radian * 180d / Math.PI;
            return angle;
        }

        /// <summary>
        /// 三角形の辺の長さの合計値（点ABC に対して、対辺abcの順で返す）
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static IEnumerable<double> GetTriangleLineLength(this Point[] points)
        {
            if (points.Length != 3) throw new ArgumentException("cannot calculate angle.");
            if (!IsValidTriangle(points)) yield break;

            // 順序にルールがあるので変えないこと
            yield return MathLineExtension.GetDistance(points[1], points[2]);
            yield return MathLineExtension.GetDistance(points[2], points[0]);
            yield return MathLineExtension.GetDistance(points[0], points[1]);
        }

        /// <summary>
        /// 三角形に外接する円の中心(外心：３つの垂直二等分線の交点)
        /// https://ja.wikipedia.org/wiki/%E5%A4%96%E6%8E%A5%E5%86%86#%E5%A4%96%E5%BF%83%E3%81%AE%E4%BD%8D%E7%BD%AE
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Point GetCircumscribedCirclePoint(this Point[] points)
        {
            if (points.Length != 3) throw new ArgumentException("not a triangle.");
            if (!IsValidTriangle(points)) return default;

            var line0 = GetLineParam(points[0], points[1]);
            var line1 = GetLineParam(points[0], points[2]);

            var bb = line0.param0 - line1.param0;
            if (bb == 0) throw new ArgumentException();

            var x = (line1.param1 - line0.param1) / bb;
            var y = (line1.param1 * line0.param0 - line0.param1 * line1.param0) / bb;
            return new Point(x, y);

            static (double param0, double param1) GetLineParam(Point p0, Point p1)
            {
                var x0 = p0.X + p1.X;
                var x1 = p0.X - p1.X;
                var y0 = p0.Y + p1.Y;
                var y1 = p0.Y - p1.Y;
                var z = (x0 * x1 + y0 * y1) / 2.0;

                if (y0 == 0 && y1 == 0) throw new ArgumentException();
                return (y1 != 0) ? (-x1 / y1, z / y1) : (-x0 / y0, z / y0);
            }
        }

        /// <summary>
        /// 三角形に外接する円の直径
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static double GetCircumscribedCircleDiameter(this Point[] points)
        {
            if (points.Length != 3) throw new ArgumentException("not a triangle.");

            var circlePoint = GetCircumscribedCirclePoint(points);
            var halfLength = MathLineExtension.GetDistance(points[0], circlePoint);
            return halfLength * 2.0;
        }

        /// <summary>
        /// 三角形に内接する円の中心(内心：３つの角の２等分線の交点)
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Point GetInscribedCirclePoint(this Point[] points)
        {
            if (points.Length != 3) throw new ArgumentException("not a triangle.");
            if (!IsValidTriangle(points)) return default;

            var lines = points.GetTriangleLineLength().ToArray();
            var bb = lines.Sum();
            if (bb == 0) throw new ArgumentException();

            var x = (lines[0] * points[0].X + lines[1] * points[1].X + lines[2] * points[2].X) / bb;
            var y = (lines[0] * points[0].Y + lines[1] * points[1].Y + lines[2] * points[2].Y) / bb;
            return new Point(x, y);
        }

        /// <summary>
        /// 三角形に内接する円の直径
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static double GetInscribedCircleDiameter(this Point[] points)
        {
            if (points.Length != 3) throw new ArgumentException("not a triangle.");
            if (!IsValidTriangle(points)) return default;

            var bb = points.GetTriangleLineLength().Sum();
            if (bb == 0) throw new ArgumentException();

            var area = GetTriangleArea(points);

            var halfLength = (2.0 * area) / bb;
            return halfLength * 2.0;
        }
    }

    static class MathQuadrangleExtension
    {
        /// <summary>
        ///  四点から面積を求める(座標法) https://ja.wikipedia.org/wiki/%E5%BA%A7%E6%A8%99%E6%B3%95
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static double GetQuadrangleArea(this Point[] ps)
        {
            if (ps.Length != 4) throw new ArgumentException("not a quadrangle.");

            var area = (ps[0].X * ps[1].Y - ps[1].X * ps[0].Y
                      + ps[1].X * ps[2].Y - ps[2].X * ps[1].Y
                      + ps[2].X * ps[3].Y - ps[3].X * ps[2].Y
                      + ps[3].X * ps[0].Y - ps[0].X * ps[3].Y) / 2.0;
            return Math.Abs(area);
        }

        /// <summary>
        /// 四角形の重心を求める
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        //public static Point GetQuadrangleCenterPoint(this Point[] points)
        //{
        //    if (points.Length != 4) throw new ArgumentException("not a triangle.");

        //    // ◆実装中
        //    //var sortedPoints = MathPolygonExtension.SortPolygonPoints(points);
        //    var sortedPoints = points;

        //    var line0 = MathLineExtension.GetSlopeIntercept(sortedPoints[0], sortedPoints[2]);
        //    var line1 = MathLineExtension.GetSlopeIntercept(sortedPoints[1], sortedPoints[3]);
        //    return MathLineExtension.GetIntersectionPoint(line0.slope, line0.intercept, line1.slope, line1.intercept);
        //}
    }

    static class MathPolygonExtension
    {
        /// <summary>
        /// 多角形の面積
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static double GetPolygonArea(this Point[] points)
        {
            if (points.Length <= 2) throw new ArgumentException("not a polygon.");
            if (points.Length == 3) return MathTriangleExtension.GetTriangleArea(points);
            if (points.Length == 4) return MathQuadrangleExtension.GetQuadrangleArea(points);

            var length = points.Length;
            var dArea = 0d;
            for (int i = 0; i < length; ++i)
            {
                ref var p0 = ref points[i];
                ref var p1 = ref points[(i + 1) % length];
                dArea += (p0.X * p1.Y - p0.Y * p1.X);
            }
            return Math.Abs(dArea / 2.0);
        }

        /// <summary>
        /// 多角形の妥当性チェック
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static bool IsValidPolygon(this Point[] points)
        {
            if (points.Length <= 2) throw new ArgumentException("not a polygon.");
            if (points.Length == 3) return MathTriangleExtension.IsValidTriangle(points);

            // 重複した点があれば Invalid
            var duplicate = points.GroupBy(p => p).Any(ps => ps.Count() > 1);
            if (duplicate) return false;

            // 一直線なら Invalid (double誤差を弾くため最大/最小差から判定)
            if (points.Select(p => p.X).Max() - points.Select(p => p.X).Min() <= 0.0001) return false;
            if (points.Select(p => p.Y).Max() - points.Select(p => p.Y).Min() <= 0.0001) return false;

            // 多角形の点がめくられてたら Invalid
            if (IsTurnOverPolygonPoints(points)) return false;

            return true;
        }

        /// <summary>
        /// 枠点のめくられチェック
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        private static bool IsTurnOverPolygonPoints(Point[] points)
        {
            if (points.Length <= 3) return false;

            // ◆未実装 NotImplementedException()
            return false;
        }

#if false
        /// <summary>
        /// 枠点をめくられないよう並べ替える(面積が最大を採用する実装)
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Point[] SortPolygonPoints(this Point[] points)
        {
            if (!IsValidPolygon(points)) return points;
            return Enumerate(points).OrderByDescending(x => GetPolygonArea(x)).First();
        }

        // 全ての要素を使った順列を求める https://qiita.com/gushwell/items/8780fc2b71f2182f36ac
        private static IEnumerable<T[]> Enumerate<T>(IEnumerable<T> items)
        {
            if (items.Count() == 1)
            {
                yield return new T[] { items.First() };
                yield break;
            }
            foreach (var item in items)
            {
                var leftside = new T[] { item };
                var unused = items.Except(leftside);
                foreach (var rightside in Enumerate(unused))
                {
                    yield return leftside.Concat(rightside).ToArray();
                }
            }
        }
#endif

        /// <summary>
        /// 多角形の重心を求める
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Point GetPolygonCenterPoint(this Point[] points)
        {
            if (points.Length < 3) throw new ArgumentException("cannot calculate.");
            if (points.Length == 3) return MathTriangleExtension.GetTriangleCenterPoint(points);
            //if (points.Length == 4) return MathQuadrangleExtension.GetQuadrangleCenterPoint(points);

            // 多角形を三角形に割った面積 * 重心
            var sum = (Area: 0d, CenterX: 0d, CenterY: 0d);
            for (int i = 0; i < points.Length; ++i)
            {
                var j = (i + 1) % points.Length;
                var k = (i + 2) % points.Length;
                var area = MathTriangleExtension.GetTriangleArea(points[i], points[j], points[k]);
                var center = MathTriangleExtension.GetTriangleCenterPoint(points[i], points[j], points[k]);

                sum.Area += area;
                sum.CenterX += center.X * area;
                sum.CenterY += center.Y * area;
            }

            if (sum.Area == 0) return new Point();  // 計算不可
            return new Point(sum.CenterX / sum.Area, sum.CenterY / sum.Area);
        }

        /// <summary>
        /// 多角形に外接する四角形
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Rect GetCircumscribedRectangle(this Point[] points)
        {
            var xmin = points.Min(p => p.X);
            var ymin = points.Min(p => p.Y);
            var xmax = points.Max(p => p.X);
            var ymax = points.Max(p => p.Y);
            return new Rect(new Point(xmin, ymin), new Point(xmax, ymax));
        }

        /// <summary>
        /// 多角形に外接する四角形の四隅の点
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static IEnumerable<Point> GetCircumscribedRectanglePoints(this Point[] points)
        {
            var rect = GetCircumscribedRectangle(points);

            yield return rect.TopLeft;
            yield return rect.TopRight;
            yield return rect.BottomRight;
            yield return rect.BottomLeft;
        }
    }
}
