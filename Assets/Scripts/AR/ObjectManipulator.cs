using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectManipulator : MonoBehaviour
{
    private Vector2 touchStartPos;
    private Vector3 objectStartPos;
    private bool isDragging = false;

    private float rotationSpeed = 5f;
    private float moveSpeed = 0.005f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.transform == transform)
                {
                    isDragging = true;
                    touchStartPos = touch.position;
                    objectStartPos = transform.position;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 touchDelta = touch.position - touchStartPos;

                if (Input.touchCount == 1)
                {
                    // Move object based on touch movement
                    transform.position = objectStartPos + new Vector3(touchDelta.x * moveSpeed, 0, touchDelta.y * moveSpeed);
                }
                else if (Input.touchCount == 2)
                {
                    // Rotate object based on second finger movement
                    transform.Rotate(Vector3.up, -touchDelta.x * rotationSpeed * Time.deltaTime, Space.World);
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }
}
