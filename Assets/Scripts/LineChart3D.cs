using UnityEngine;
using System.Collections.Generic;

public class LineChart3D : MonoBehaviour
{
    public GameObject pointPrefab;
    public Transform xrOriginCamera; // Reference to XR Origin Main Camera
    public float spacing = 0.89f; // Further reduced by 2/3
    public float chartScale = 0.44f; // Further reduced by 2/3
    public float minScale = 0.22f; // Further reduced by 2/3
    public float maxScale = 0.89f; // Further reduced by 2/3

    void Start()
    {
        // Hardcoded data for aircraft production years
        Dictionary<int, int> aircraftProduction = new Dictionary<int, int>
        {
            { 1930, 5 },
            { 1935, 12 },
            { 1940, 20 },
            { 1945, 35 },
            { 1950, 28 },
            { 1955, 40 },
            { 1960, 45 },
            { 1965, 30 },
            { 1970, 20 }
        };

        PositionChart();
        CreateLineChart(aircraftProduction);
        CreateAxes(aircraftProduction);
        CreateAxisLabels(aircraftProduction);
    }

    void PositionChart()
    {
        if (xrOriginCamera != null)
        {
            transform.position = xrOriginCamera.position + xrOriginCamera.forward * 0.89f; // Adjusted distance
            transform.LookAt(xrOriginCamera); // Orient towards the camera
            AdjustScale();
        }
    }

    void AdjustScale()
    {
        if (xrOriginCamera != null)
        {
            float distance = Vector3.Distance(transform.position, xrOriginCamera.position);
            float scaleFactor = Mathf.Clamp(distance / 5f, minScale, maxScale); // Adjust scale dynamically
            transform.localScale = Vector3.one * scaleFactor;
            foreach (Transform child in transform)
            {
                child.localScale = Vector3.one * scaleFactor;
            }
        }
    }

    void CreateLineChart(Dictionary<int, int> data)
    {
        int index = 0;
        Vector3 previousPosition = Vector3.zero;

        foreach (var entry in data)
        {
            Vector3 position = new Vector3(index * spacing, entry.Value * 0.22f, 0);
            GameObject point = Instantiate(pointPrefab, position, Quaternion.identity);
            point.transform.SetParent(transform);

            CreateTextLabel(entry.Key.ToString(), position + new Vector3(0, -0.44f, 0), Color.red);
            CreateTextLabel(entry.Value.ToString(), position + new Vector3(0.44f, 0, 0), Color.green);

            if (index > 0)
            {
                DrawLine(previousPosition, position, Color.white);
            }

            previousPosition = position;
            index++;
        }
    }

    void CreateTextLabel(string text, Vector3 position, Color color)
    {
        GameObject label = new GameObject("TextLabel");
        TextMesh textMesh = label.AddComponent<TextMesh>();
        textMesh.text = text;
        textMesh.color = color;
        textMesh.characterSize = 0.13f; // Adjusted for better visibility
        textMesh.anchor = TextAnchor.MiddleCenter;
        label.transform.position = position;
        label.transform.SetParent(transform);
    }

    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject line = new GameObject("Line");
        line.transform.SetParent(transform);
        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.startWidth = 0.044f; // Further reduced by 2/3
        lr.endWidth = 0.044f; // Further reduced by 2/3
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = color;
        lr.endColor = color;
    }

    void CreateAxes(Dictionary<int, int> data)
    {
        // X-Axis
        DrawLine(new Vector3(-0.14f, 0, 0), new Vector3(data.Count * spacing, 0, 0), Color.red);

        // Y-Axis
        DrawLine(new Vector3(0, -0.14f, 0), new Vector3(0, 22.22f, 0), Color.green);

        // Z-Axis
        DrawLine(new Vector3(0, 0, -0.14f), new Vector3(0, 0, 2.44f), Color.blue);
    }

    void CreateAxisLabels(Dictionary<int, int> data)
    {
        GameObject xLabel = new GameObject("X-Axis Label");
        TextMesh xText = xLabel.AddComponent<TextMesh>();
        xText.text = "Year";
        xText.color = Color.red;
        xText.characterSize = 0.22f;
        xLabel.transform.position = new Vector3(data.Count * spacing + 0.89f, 0, 0);
        xLabel.transform.SetParent(transform);

        GameObject yLabel = new GameObject("Y-Axis Label");
        TextMesh yText = yLabel.AddComponent<TextMesh>();
        yText.text = "Production Count";
        yText.color = Color.green;
        yText.characterSize = 0.22f;
        yLabel.transform.position = new Vector3(0, 23.11f, 0);
        yLabel.transform.SetParent(transform);
    }
}
