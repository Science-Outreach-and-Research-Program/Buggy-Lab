using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;

public class XYPoint
{
    public float X;
    public float Y;
}

public class ChartManager : MonoBehaviour
{
    public ScatterChart chart;
    [SerializeField] private GameObject text;
    private List<XYPoint> points;
    private bool _doDrawBestFitLine;
    public bool started = false;

    void Start()
    {
        if (!started)
        {
            Init();
        }
    }

    public void Init()
    {
        _doDrawBestFitLine = false;
        chart.GetOrAddChartComponent<Title>().text = "Position versus Time";

        chart.RemoveData();
        chart.AddSerie<Scatter>("scatter").symbol.type = SymbolType.Rect;
        chart.GetChartComponent<XAxis>().interval = 0.5;
        chart.GetChartComponent<YAxis>().interval = 0.25;

        points = new List<XYPoint>();
        started = true;
    }

    public ScatterChart AddData(float x, float y)
    {
        if (!started)
            Start();

        chart.AddData("scatter", x, y);
        points.Add(new XYPoint() {X = x, Y = y});

        if (_doDrawBestFitLine)
            DrawBestFitLine();

        return chart;
    }

    public void DrawBestFitLine()
    {
        chart.RemoveSerie<Line>();
        chart.AddSerie<Line>("bestFitLine").symbol.show = false;
        chart.GetSerie("bestFitLine").lineType = LineType.Smooth;

        float k, b;
        double rSquare;

        GenerateLinearBestFit(points, out k, out b, out rSquare);

        float startingX = 0.1f;
        float startingY = k * startingX + (-b);
        float endingX = points.Max(point => point.X);
        float endingY = k * endingX + (-b);

        chart.AddData("bestFitLine", startingX, startingY);
        chart.AddData("bestFitLine", endingX, endingY);

        text.GetComponent<Text>().text = string.Format("Best fit line slope: {0:N3}\nR^2 = {1:N3}", k, rSquare);

        _doDrawBestFitLine = true;
    }

    public static void GenerateLinearBestFit(List<XYPoint> points, out float a, out float b, out double rSquare)
    {
        int numPoints = points.Count;
        float meanX = points.Average(point => point.X);
        float meanY = points.Average(point => point.Y);

        float sumXSquared = points.Sum(point => point.X * point.X);
        float sumYSquared = points.Sum(point => point.Y * point.Y);
        float sumXY = points.Sum(point => point.X * point.Y);
        float sumX = points.Sum(point => point.X);
        float sumY = points.Sum(point => point.Y);

        a = (sumXY / numPoints - meanX * meanY) / (sumXSquared / numPoints - meanX * meanX);
        b = (a * meanX - meanY);

        float rNumerator = (numPoints * sumXY) - (sumX * sumY);
        float rDenom = (numPoints * sumXSquared - (sumX * sumX)) * (numPoints * sumYSquared - (sumY * sumY));
        double dblR = rNumerator / Math.Sqrt(rDenom);

        rSquare = dblR * dblR;
    }
}