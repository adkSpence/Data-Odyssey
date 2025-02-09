using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UIElements;

public class Heatmap3D : MonoBehaviour
{
    public GameObject cubePrefab; // Prefab for the heatmap cube/bar
    public float cubeSize = 1f; // Size of each cube
    public float heightMultiplier = 1f; // Scale height based on the data value
    public Material baseMaterial; // Base material for cubes (for coloring)

    public float roomWidth = 10f; // Match room dimensions from AircraftVisualization
    public float roomHeight = 5f;
    public float roomDepth = 10f;

    public Dictionary<string, Dictionary<string, int>> heatmapData;
    private List<string> primaryRoles; // Z-axis
    private List<string> countries;    // X-axis

    void Start()
    {
        // Example: Replace with actual heatmap data
        heatmapData = new Dictionary<string, Dictionary<string, int>>()
{
    { "Biplane Fighter", new Dictionary<string, int>
        {
            { "Australia", 0 }, { "Canada", 0 }, { "China", 0 }, { "Czechoslovakia", 1 },
            { "France", 0 }, { "Germany", 0 }, { "Italy", 2 }, { "Japan", 0 },
            { "Netherlands", 0 }, { "Poland", 0 }, { "Romania", 0 }, { "Russia", 1 },
            { "United Kingdom", 1 }, { "United States", 1 }, { "Yugoslavia", 0 }
        }
    },
    { "Dive Bomber", new Dictionary<string, int>
        {
            { "Australia", 0 }, { "Canada", 0 }, { "China", 0 }, { "Czechoslovakia", 0 },
            { "France", 0 }, { "Germany", 1 }, { "Italy", 0 }, { "Japan", 3 },
            { "Netherlands", 0 }, { "Poland", 0 }, { "Romania", 0 }, { "Russia", 1 },
            { "United Kingdom", 2 }, { "United States", 4 }, { "Yugoslavia", 0 }
        }
    },
    { "Fighter", new Dictionary<string, int>
        {
            { "Australia", 1 }, { "Canada", 0 }, { "China", 1 }, { "Czechoslovakia", 0 },
            { "France", 4 }, { "Germany", 6 }, { "Italy", 11 }, { "Japan", 17 },
            { "Netherlands", 1 }, { "Poland", 3 }, { "Romania", 1 }, { "Russia", 11 },
            { "United Kingdom", 5 }, { "United States", 14 }, { "Yugoslavia", 1 }
        }
    },
    { "Glider", new Dictionary<string, int>
        {
            { "Australia", 0 }, { "Canada", 0 }, { "China", 0 }, { "Czechoslovakia", 0 },
            { "France", 0 }, { "Germany", 2 }, { "Italy", 0 }, { "Japan", 0 },
            { "Netherlands", 0 }, { "Poland", 0 }, { "Romania", 0 }, { "Russia", 0 },
            { "United Kingdom", 2 }, { "United States", 1 }, { "Yugoslavia", 0 }
        }
    },
    { "Ground Attack Aircraft", new Dictionary<string, int>
        {
            { "Australia", 0 }, { "Canada", 0 }, { "China", 0 }, { "Czechoslovakia", 0 },
            { "France", 0 }, { "Germany", 2 }, { "Italy", 0 }, { "Japan", 2 },
            { "Netherlands", 0 }, { "Poland", 0 }, { "Romania", 0 }, { "Russia", 2 },
            { "United Kingdom", 0 }, { "United States", 2 }, { "Yugoslavia", 0 }
        }
    },
    { "Heavy Bomber", new Dictionary<string, int>
        {
            { "Australia", 0 }, { "Canada", 0 }, { "China", 0 }, { "Czechoslovakia", 0 },
            { "France", 1 }, { "Germany", 2 }, { "Italy", 1 }, { "Japan", 1 },
            { "Netherlands", 0 }, { "Poland", 0 }, { "Romania", 0 }, { "Russia", 2 },
            { "United Kingdom", 4 }, { "United States", 4 }, { "Yugoslavia", 0 }
        }
    },
    { "Heavy Fighter", new Dictionary<string, int>
        {
            { "Australia", 0 }, { "Canada", 0 }, { "China", 0 }, { "Czechoslovakia", 1 },
            { "France", 4 }, { "Germany", 0 }, { "Italy", 1 }, { "Japan", 0 },
            { "Netherlands", 0 }, { "Poland", 0 }, { "Romania", 0 }, { "Russia", 0 },
            { "United Kingdom", 6 }, { "United States", 1 }, { "Yugoslavia", 0 }
        }
    },
    { "Jet Fighter", new Dictionary<string, int>
        {
            { "Australia", 0 }, { "Canada", 0 }, { "China", 0 }, { "Czechoslovakia", 0 },
            { "France", 0 }, { "Germany", 0 }, { "Italy", 0 }, { "Japan", 3 },
            { "Netherlands", 0 }, { "Poland", 1 }, { "Romania", 0 }, { "Russia", 0 },
            { "United Kingdom", 1 }, { "United States", 2 }, { "Yugoslavia", 0 }
        }
    },
    { "Light Bomber", new Dictionary<string, int>
        {
            { "Australia", 0 }, { "Canada", 0 }, { "China", 0 }, { "Czechoslovakia", 1 },
            { "France", 1 }, { "Germany", 1 }, { "Italy", 4 }, { "Japan", 1 },
            { "Netherlands", 0 }, { "Poland", 0 }, { "Romania", 0 }, { "Russia", 1 },
            { "United Kingdom", 3 }, { "United States", 1 }, { "Yugoslavia", 0 }
        }
    },
    { "Medium Bomber", new Dictionary<string, int>
        {
            { "Australia", 0 }, { "Canada", 0 }, { "China", 0 }, { "Czechoslovakia", 0 },
            { "France", 5 }, { "Germany", 6 }, { "Italy", 5 }, { "Japan", 8 },
            { "Netherlands", 0 }, { "Poland", 1 }, { "Romania", 0 }, { "Russia", 2 },
            { "United Kingdom", 5 }, { "United States", 11 }, { "Yugoslavia", 0 }
        }
    },
    { "Night Fighter", new Dictionary<string, int>
        {
            { "Australia", 0 }, { "Canada", 0 }, { "China", 0 }, { "Czechoslovakia", 0 },
            { "France", 0 }, { "Germany", 2 }, { "Italy", 0 }, { "Japan", 1 },
            { "Netherlands", 0 }, { "Poland", 0 }, { "Romania", 0 }, { "Russia", 0 },
            { "United Kingdom", 0 }, { "United States", 0 }, { "Yugoslavia", 1 }
        }
    },
    { "Prototype Aircraft", new Dictionary<string, int>
        {
            { "Australia", 0 }, { "Canada", 0 }, { "China", 0 }, { "Czechoslovakia", 0 },
            { "France", 5 }, { "Germany", 3 }, { "Italy", 2 }, { "Japan", 0 },
            { "Netherlands", 0 }, { "Poland", 0 }, { "Romania", 0 }, { "Russia", 1 },
            { "United Kingdom", 2 }, { "United States", 0 }, { "Yugoslavia", 0 }
        }
    }
};


        // Extract unique categories for axes
        primaryRoles = new List<string>(heatmapData.Keys);
        countries = new List<string>();
        foreach (var roleData in heatmapData.Values)
        {
            foreach (var country in roleData.Keys)
            {
                if (!countries.Contains(country))
                    countries.Add(country);
            }
        }

        // Generate the 3D heatmap
        GenerateHeatmap();
    }

    void GenerateHeatmap()
    {
        for (int z = 0; z < primaryRoles.Count; z++)
        {
            string primaryRole = primaryRoles[z];
            for (int x = 0; x < countries.Count; x++)
            {
                string country = countries[x];

                // Get the value for the current (X, Z) cell
                int value = heatmapData.ContainsKey(primaryRole) && heatmapData[primaryRole].ContainsKey(country)
                    ? heatmapData[primaryRole][country]
                    : 0;

                // Skip if no data for this cell
                if (value == 0) continue;

                // Create the cube/bar
                Vector3 position = new Vector3(x * cubeSize, (value * heightMultiplier) / 2f, z * cubeSize);
                GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
                cube.transform.localScale = new Vector3(cubeSize, value * heightMultiplier, cubeSize);
                cube.transform.SetParent(transform);

                // Set the cube's color based on value
                Renderer renderer = cube.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = new Material(baseMaterial);
                    renderer.material.color = Color.Lerp(Color.blue, Color.red, (float)value / 10f); // Normalize color
                }

                // Add a number label on top of the bar
                GameObject numberLabel = new GameObject($"Value {value}");
                TextMesh numberText = numberLabel.AddComponent<TextMesh>();
                numberText.text = value.ToString();
                numberText.fontSize = 16; // Adjust size
                numberText.characterSize = 0.1f;
                numberText.anchor = TextAnchor.MiddleCenter;
                numberText.color = Color.black;

                // Position the label slightly above the bar
                numberLabel.transform.position = new Vector3(position.x, position.y + (value * heightMultiplier) / 2f + 0.2f, position.z);

                // **Rotation Fix: Rotate number labels to face upwards**
                numberLabel.transform.rotation = Quaternion.Euler(90, 0, 0);

                numberLabel.transform.SetParent(transform);
            }
        }

        // Add X-axis (Country) labels
        AddAxisLabels(countries, "Country", cubeSize, new Vector3(0, -0.5f, 0), Axis.X);

        // Add Z-axis (Primary Role) labels
        AddAxisLabels(primaryRoles, "Primary Role", cubeSize, new Vector3(-0.5f, 0, 0), Axis.Z);

        // Adjust the camera position to view the heatmap
        Camera.main.transform.position = new Vector3(countries.Count * cubeSize / 2f, 10f, primaryRoles.Count * cubeSize / 2f);
        Camera.main.transform.LookAt(transform.position);
    }

    enum Axis { X, Z }

    void AddAxisLabels(List<string> labels, string axisName, float spacing, Vector3 offset, Axis axis)
    {
        for (int i = 0; i < labels.Count; i++)
        {
            string label = labels[i];
            Vector3 position;

            if (axis == Axis.X)
            {
                // Position labels for X-axis (countries)
                position = new Vector3(i * spacing, offset.y, offset.z);
            }
            else // Axis.Z
            {
                // Position labels for Z-axis (primary roles)
                position = new Vector3(offset.x, offset.y, i * spacing);
            }

            GameObject textObject = new GameObject($"{axisName} Label {label}");
            TextMesh textMesh = textObject.AddComponent<TextMesh>();
            textMesh.text = label;
            textMesh.fontSize = 12; // Adjust size as needed
            textMesh.characterSize = 0.1f;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.color = Color.black;

            // **Rotation Fix: Rotate aircraft names properly**
            if (axis == Axis.X)
            {
                textMesh.transform.rotation = Quaternion.Euler(90, 0, 0); // Rotate X-axis labels
            }
            else
            {
                textMesh.transform.rotation = Quaternion.Euler(90, 0, 90); // Rotate Z-axis labels
            }

            textObject.transform.position = position;
            textObject.transform.SetParent(transform);
        }
    }
}
