using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ImageTracker : MonoBehaviour
{
    [System.Serializable]
    public class MarkerPrefabPair
    {
        public string markerName;
        public GameObject prefab;
    }

    [SerializeField]
    private List<MarkerPrefabPair> markerPrefabPairs;

    private ARTrackedImageManager trackedImageManager;
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            string markerName = trackedImage.referenceImage.name;
            var markerPair = markerPrefabPairs.Find(x => x.markerName == markerName);

            if (markerPair != null && !spawnedObjects.ContainsKey(markerName))
            {
                // Offset spawn position slightly to prevent overlap
                Vector3 spawnPosition = trackedImage.transform.position + new Vector3(0, 0.1f, 0);

                GameObject spawnedObject = Instantiate(markerPair.prefab, trackedImage.transform.position, Quaternion.Euler(-90, 0, 0));
                spawnedObject.AddComponent<ObjectManipulator>();  // Add touch control

                spawnedObjects.Add(markerName, spawnedObject);
            }
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            if (spawnedObjects.TryGetValue(trackedImage.referenceImage.name, out GameObject spawnedObject))
            {
                // Update only if tracking state is good
                if (trackedImage.trackingState == TrackingState.Tracking)
                {
                    spawnedObject.transform.position = trackedImage.transform.position;
                }
            }
        }
    }
}
