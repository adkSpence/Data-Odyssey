using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TapInteraction : MonoBehaviour
{
    private ARRaycastManager arRaycastManager;
    private Camera arCamera;
    private GameObject lastTappedObject;

    void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        arCamera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Input.GetTouch(0).position;

                // Perform a raycast
                Ray ray = arCamera.ScreenPointToRay(touchPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    Debug.Log("Tapped on: " + hitObject.name);

                    // Highlight the tapped object
                    HighlightBar(hitObject);
                }
            }

            // Handle gestures
            HandleZoom();
            HandleRotation();
        }
    }

    private void HighlightBar(GameObject bar)
    {
        if (lastTappedObject != null)
        {
            // Reset the previous bar's scale and color
            ResetBarColor(lastTappedObject);
            lastTappedObject.transform.localScale = Vector3.one;
        }

        // Highlight and scale the new bar
        Renderer renderer = bar.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.green; // Highlight in green
        }
        bar.transform.localScale = Vector3.one * 1.2f; // Scale up by 20%

        // Save the last tapped object
        lastTappedObject = bar;
    }

    private void ResetBarColor(GameObject bar)
    {
        Renderer renderer = bar.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white; // Reset to white
        }
    }
    private void HandleZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Calculate the pinch distance delta
            float prevTouchDeltaMag = (touch1.position - touch2.position).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Zoom based on delta
            arCamera.transform.position += arCamera.transform.forward * deltaMagnitudeDiff * 0.01f;
        }
    }

    private void HandleRotation()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Touch touch = Input.GetTouch(0);

            // Rotation sensitivity
            float rotationSpeed = 0.2f;

            // Rotate graph (assuming the parent object of bars is called "BarChart")
            GameObject barChart = GameObject.Find("BarChart");
            if (barChart != null)
            {
                barChart.transform.Rotate(Vector3.up, -touch.deltaPosition.x * rotationSpeed, Space.World);
                barChart.transform.Rotate(Vector3.right, touch.deltaPosition.y * rotationSpeed, Space.World);
            }
        }
    }
}
