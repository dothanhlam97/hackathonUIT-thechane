using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;
namespace SketchDetection {

    public class Detection {

        public enum Shape { Vertical, Horizontal, Left, Right, Zigzag, Unknown, Circumflex };


        private int number_points = 0;
        private Shape result = Shape.Unknown;
        private String message = "";
        private int THRESH_ZIGZAG = 6;
        private int THRESH_LINE = 4;
        public List <int> histogram = new List<int> ();
        private Shape shapeCode = Shape.Unknown;
        
        public Detection() {
            number_points = 0;
        }

        public Detection getConstantObject() {
            return this;
        }

        public void setShape(Shape code) {

            Debug.Log("Set shape ......... " + code);
            shapeCode = code;
        }

        private int max(int a, int b) { 
            if (a > b) {
                return a;
            } else {
                return b;
            }
        }

        private Shape predictWithHorizontal(List<Point> points, int sum_count) {
            int sumLeft = 0;
            int sumRight = 0;
            for (var i = 1; i < points.Count; i++) {
                if (points[i].X > points[i-1].X) {
                    sumRight ++;
                } else if (points[i].X < points[i-1].X) {
                    sumLeft ++;
                }
            }
            //Debug.Log(sumLeft);
            //Debug.Log(sumRight);
            if (sumLeft < sumRight / THRESH_LINE) {
                return Shape.Horizontal;
            }
            if (sumRight < sumLeft / THRESH_LINE) {
                return Shape.Horizontal;
            }
            if (sum_count > THRESH_ZIGZAG) {
                return Shape.Zigzag;
            } else {
                return Shape.Unknown;
            }
        }

        private Shape predictWithVertical(List<Point> points, int sum_count) {

            int sumLeft = 0;
            int sumRight = 0;
            for (var i = 1; i < points.Count; i++) {
                if (points[i].Y > points[i-1].Y) {
                    sumRight ++;
                } else if (points[i].Y < points[i-1].Y) {
                    sumLeft ++;
                }
            }
            //Debug.Log(sumLeft);
            //Debug.Log(sumRight);
            if (sumLeft < sumRight / THRESH_LINE) {
                return Shape.Vertical;
            }
            if (sumRight < sumLeft / THRESH_LINE) {
                return Shape.Vertical;
            }
            if (sum_count > THRESH_ZIGZAG) {
                return Shape.Zigzag;
            } else {
                return Shape.Unknown;
            }
        }

        private Shape predictWithLeft(List<Point> points, int sum_count) {
            int sumLeft = 0;
            int sumRight = 0;
            for (var i = 1; i < points.Count; i++) {
                if (points[i].X > points[i-1].X) {
                    sumRight ++;
                } else if (points[i].X < points[i-1].X) {
                    sumLeft ++;
                }
            }
            if (sumLeft < sumRight / 5) {
                return Shape.Left;
            }
            if (sumRight < sumLeft / 5) {
                return Shape.Left;
            }
            if (sum_count > THRESH_ZIGZAG) {
                return Shape.Zigzag;
            } else {
                return Shape.Unknown;
            }
        }

        private Shape predictWithRight(List<Point> points, int sum_count) {
            int sumLeft = 0;
            int sumRight = 0;
            for (var i = 1; i < points.Count; i++) {
                if (points[i].X > points[i-1].X) {
                    sumRight ++;
                } else if (points[i].X < points[i-1].X) {
                    sumLeft ++;
                }
            }
            //Debug.Log(sumLeft);
            //Debug.Log(sumRight);
            if (sumLeft < sumRight / THRESH_LINE) {
                return Shape.Right;
            }
            if (sumRight < sumLeft / THRESH_LINE) {
                return Shape.Right;
            }
            if (sum_count > THRESH_ZIGZAG) {
                return Shape.Zigzag;
            } else {
                return Shape.Unknown;
            }
        }

        private Shape predictWithZigZag(List<Point> points, List <int> his, int sum_count) {
            int sum = 0;
            int sumLeft = 0;
            int sumRight = 0;
            if (sum_count <= THRESH_ZIGZAG) {
                return Shape.Unknown;
            }
            for (var i = 1; i < points.Count; i++) {
                if (points[i].X > points[i-1].X) {
                    sumRight ++;
                } else if (points[i].X < points[i-1].X) {
                    sumLeft ++;
                }
            }
            if (sumLeft < sumRight / THRESH_LINE) {
                return Shape.Unknown;
            }
            if (sumRight < sumLeft / THRESH_LINE) {
                return Shape.Unknown;
            }
            for (int i = 0; i < 360; i++) {
                sum = 0;
                int j = 0;
                while (j < 45 && i < 360) {
                    sum += his[i];
                    i++;
                    j++;
                }
                this.message += "[" + Convert.ToString(sum) + "," + Convert.ToString(sum_count) + "]";
                if (sum > sum_count / 2) {
                    return Shape.Zigzag;
                }
            }
            return Shape.Unknown;
        }

        private Shape predictWithHistogram(List<int> his, List<Point> points) {
            int sum_count = 0;
            for (int i = 0; i < 360; i++) {
                sum_count += his[i];
            }

            this.message += "Sum count " + Convert.ToString(sum_count);
            // check line horizontal 
            int sum = 0;
            for (int i = -20; i <= 20; i++) {
                if (i < 0) {
                    sum += his[i + 360];
                } else {
                    sum += his[i];
                }
                sum += his[i + 180];
            }
            if (sum >= 2 * sum_count / 3) {
                return predictWithHorizontal(points, sum_count);
            }
            if (sum >= sum_count / 2 && sum_count > THRESH_ZIGZAG) {
                return Shape.Zigzag;
            }

            // check line vertical 
            sum = 0;
            for (int i = -20; i <= 20; i++) {
                sum += his[90 + i];
                sum += his[270 + i];
            }
            this.message += "Sum vertical " + Convert.ToString(sum);
            if (sum >= 2 * sum_count / 3) {
                return predictWithVertical(points, sum_count);
            }
            if (sum > sum_count / 2 && sum_count > THRESH_ZIGZAG) {
                return Shape.Zigzag;
            }
            // check circumflex
            int sum1 = 0, sum2 = 0;
            for (int i = -30; i <= 30; i++) {
                sum1 = sum1 + max(0, his[45 + i]);
                sum1 = sum1 + max(0, his[225 + i]);
                sum2 = sum2 + max(0, his[135 + i]);
                sum2 = sum2 + max(0, his[315 + i]);
                //Debug.Log("histogram " + Convert.ToString(45 + i) +  " " + Convert.ToString(his[45 + i]));
            }
            //Debug.Log(Convert.ToString(sum1) +  " .... " + Convert.ToString(sum2) +  " .... " + Convert.ToString(sum_count));
            if (sum1 > sum_count / 3 && sum2 > sum_count/ 3) {
                return Shape.Circumflex;
            }

            sum1 = 0;
            sum2 = 0;
            for (int i = -30; i <= 30; i++) {
                sum1 = sum1 + max(0, his[45 + i]);
                sum1 = sum1 + max(0, his[225 + i]);
                sum2 = sum2 + max(0, his[135 + i]);
                sum2 = sum2 + max(0, his[315 + i]);
            }
            // check left 
            if (sum1 > 2 * sum_count / 3) {
                return predictWithLeft(points, sum_count);
            }
            // check right
            if (sum2 > 2 * sum_count / 3) {
                return predictWithRight(points, sum_count);
            }

            // check zig zag with histogram 
            Shape str = predictWithZigZag(points, histogram, sum_count);
            if (str != Shape.Unknown) { 
                return str;
            }
            return Shape.Unknown;
        }

        public void detectArrayData(List<Point> points) {
            this.number_points = 5;
            // solve 
            histogram.Clear();
            for (var i = 0; i < 360; i++) {
                histogram.Add(0);
            }
            this.message = "" ;
            for (var i = 1; i < points.Count; i++) {
                if (points[i].X - points[i-1].X < 1.0 * Screen.width / 1000 && points[i].Y - points[i-1].Y <= 1.0 * Screen.height / 1000) {
                    continue;
                }
                Point temp = new Point(points[i].X - points[i-1].X, points[i].Y - points[i-1].Y, 0);
                // Debug.Log(points[i].getPoint());
                double tanAngle = temp.Y / (temp.X + 0.0000000001);
                double radians = Math.Atan(tanAngle);
                double angle = radians * (180.0/Math.PI);
                if (angle < 0.0) {
                    angle += 360.0;
                } 
                this.message += Convert.ToString(angle) + " ";
                //Debug.Log(Convert.ToString(angle) + temp.getPoint());
                if ((int)angle == 360) {
                    histogram[0] ++;
                } else {
                    histogram[(int)angle] ++;
                }

            }

            this.result = predictWithHistogram(histogram, points);
        }
        
        public bool checkShape(List <Point> points) {
            detectArrayData(points);
            Debug.Log(this.result + "  xvc sd " + this.shapeCode);
            return this.result == this.shapeCode;
        }

        public Shape getResult() {
            return this.result;
        }

        public int getNumberPoints() {
            return this.number_points;
        }

        public String getString() {
            return this.message;
        }
    }


}