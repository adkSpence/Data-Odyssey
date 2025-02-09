using UnityEngine;
using System.Collections.Generic;

public class AircraftVisualization : MonoBehaviour
{
    public GameObject barPrefab;  // Prefab for 3D bars
    public GameObject axisPrefab;  // Prefab for axes
    public Material[] roleMaterials; // Assign different materials for different roles in the inspector

    // Room dimensions for scaling
    public float roomWidth = 5f;
    public float roomHeight = 2.5f;
    public float roomDepth = 5f;

    private Dictionary<string, int> aircraftRoleCounts = new Dictionary<string, int>();

    void Start()
    {
        LoadHardcodedData();
        Create3DBarChart();
        CreateAxes();
    }

    void LoadHardcodedData()
    {
        // Hardcoded aircraft data (Role, Country)
        string[,] aircraftData = {
            { "Medium Bomber", "France" },
            { "Light Bomber", "Japan" },
            { "Reconnaissance Aircraft", "Japan" },
            { "Medium Bomber", "France" },
            { "Ground Attack Aircraft", "United States" },
            { "Fighter", "United States" },
            { "Fighter", "Germany" },
            { "Reconnaissance Aircraft", "United Kingdom" },
            { "Transport", "Russia" },
            { "Ground Attack Aircraft", "Russia" }
        };

        for (int i = 0; i < aircraftData.GetLength(0); i++)
        {
            string role = aircraftData[i, 0];
            if (!aircraftRoleCounts.ContainsKey(role))
                aircraftRoleCounts[role] = 0;
            aircraftRoleCounts[role]++;
        }
    }

   void Create3DBarChart()
{
    // Global scaling factor to reduce graph size by 1/3
    float scaleFactor = 1.0f / 3.0f;

    // Calculate dynamic scaling factors
    float maxBarHeight = roomHeight * 0.8f * scaleFactor; // Leave some margin
    float barSpacing = (roomWidth / aircraftRoleCounts.Count) * scaleFactor; // Dynamic spacing
    float barWidth = barSpacing * 0.6f; // Bar width as a fraction of spacing

    float maxCount = Mathf.Max(new List<int>(aircraftRoleCounts.Values).ToArray()); // Find the highest count
    float heightScalingFactor = maxBarHeight / maxCount; // Scale factor for bar height

    float x = -roomWidth / 2 * scaleFactor + barSpacing / 2; // Start position (centered graph)
    int index = 0;

    foreach (var role in aircraftRoleCounts)
    {
        // Create and scale the bar
        float barHeight = role.Value * heightScalingFactor; // Normalize height
        GameObject bar = Instantiate(barPrefab, new Vector3(x, barHeight / 2.0f, 0), Quaternion.identity);
        bar.transform.localScale = new Vector3(barWidth, barHeight, barWidth); // Adjust bar width and depth
        bar.name = role.Key;

        // Apply color based on role
        if (roleMaterials != null && roleMaterials.Length > 0)
        {
            int colorIndex = index % roleMaterials.Length;
            Renderer barRenderer = bar.GetComponent<Renderer>();
            if (barRenderer != null)
            {
                barRenderer.material = roleMaterials[colorIndex];
            }
        }

        // Add text label below each bar
        CreateText(role.Key, new Vector3(x, -0.2f * scaleFactor, 0), 90, scaleFactor); // Labels slightly below bars
        x += barSpacing; // Move to the next position
        index++;
    }
}

void CreateText(string text, Vector3 position, float rotationY = 0, float scaleFactor = 1.0f)
{
    GameObject textObj = new GameObject("Text_" + text);
    TextMesh textMesh = textObj.AddComponent<TextMesh>();
    textMesh.text = text;
    textMesh.fontSize = 16; // Smaller font size for better clarity
    textMesh.color = Color.black;
    textMesh.anchor = TextAnchor.MiddleCenter;
    textMesh.characterSize = 0.1f * scaleFactor; // Reduce text size by the scaling factor
    textMesh.transform.position = position;
    textMesh.transform.rotation = Quaternion.Euler(90, rotationY, 0); // Rotate for better readability
}

    void CreateAxes()
    {
        float scaleFactor = 1.0f / 3.0f;

        // X-axis
        GameObject xAxis = Instantiate(axisPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        xAxis.transform.localScale = new Vector3(roomWidth * scaleFactor, 0.1f * scaleFactor, 0.1f * scaleFactor);
        xAxis.name = "X-Axis";

        // Add X-axis label
        CreateText("Aircraft Role", new Vector3(roomWidth * scaleFactor / 2, -0.3f * scaleFactor, 0), 0, scaleFactor);

        // Y-axis
        GameObject yAxis = Instantiate(axisPrefab, new Vector3(-roomWidth / 2 * scaleFactor, roomHeight / 2 * scaleFactor, 0), Quaternion.Euler(0, 0, 90));
        yAxis.transform.localScale = new Vector3(roomHeight * scaleFactor, 0.1f * scaleFactor, 0.1f * scaleFactor);
        yAxis.name = "Y-Axis";

        // Add Y-axis label
        CreateText("Count", new Vector3(-roomWidth * scaleFactor / 2 - 0.5f, roomHeight * scaleFactor / 2, 0), 90, scaleFactor);
    }
}
