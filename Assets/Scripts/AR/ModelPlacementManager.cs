using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ModelPlacementManager : MonoBehaviour
{
    [SerializeField] private GameObject modelPrefab;  // This makes it visible in Inspector
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float floatAmplitude = 0.5f;
    [SerializeField] private float floatFrequency = 1f;

    private GameObject spawnedModel;
    private ARRaycastManager raycastManager;
    private Vector3 originalPosition;
    private float floatTimer;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceModel(Input.GetTouch(0).position);
        }

        if (spawnedModel != null)
        {
            // Rotate the model
            spawnedModel.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Float up and down
            floatTimer += Time.deltaTime;
            float newY = originalPosition.y + Mathf.Sin(floatTimer * floatFrequency) * floatAmplitude;
            Vector3 newPosition = spawnedModel.transform.position;
            newPosition.y = newY;
            spawnedModel.transform.position = newPosition;
        }
    }

    private void PlaceModel(Vector2 touchPosition)
    {
        var hits = new List<ARRaycastHit>();

        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            if (spawnedModel == null)
            {
                spawnedModel = Instantiate(modelPrefab, hitPose.position, hitPose.rotation);
                originalPosition = spawnedModel.transform.position;
            }
            else
            {
                spawnedModel.transform.position = hitPose.position;
                originalPosition = hitPose.position;
            }
        }
    }
}