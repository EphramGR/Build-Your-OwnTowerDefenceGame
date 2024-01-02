using UnityEngine;

namespace Eldemarkki.MarchingSquares
{
    [RequireComponent(typeof(Camera))]
    public class CameraControl : MonoBehaviour
    {
        [Header("Zoom")]
        [SerializeField] private float zoomSpeed = 300;
        [SerializeField] private float minCameraSize = 5;
        [SerializeField] private float maxCameraSize = 200;

        private Vector2 startDragMousePosition;

        private Camera cam;

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        void Update()
        {
            Vector2 mouseWorldPosition = cam.ScreenToWorldPoint(Input.mousePosition);

            // Zoom
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                float zoomAmount = Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
                transform.position += (Vector3)(mouseWorldPosition - (Vector2)transform.position) * (zoomAmount / cam.orthographicSize);
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - zoomAmount, minCameraSize, maxCameraSize);
            }

            // Dragging movement
            if (Input.GetMouseButtonDown(1))
            {
                startDragMousePosition = mouseWorldPosition;
            }

            if (Input.GetMouseButton(1))
            {
                transform.position -= (Vector3)(mouseWorldPosition - startDragMousePosition);
            }

            float height = 2*cam.orthographicSize;
            float width = height*cam.aspect;

            if(transform.position.x > 9950f - cam.orthographicSize * 1.78f)
            {
                Vector3 x = new Vector3(9950f - (cam.orthographicSize * 1.78f), transform.position.y, -10f);
                transform.position = x;
            }

            if(transform.position.y > 9950f - cam.orthographicSize)
            {
                Vector3 y = new Vector3(transform.position.x, 9950f - cam.orthographicSize, -10f);
                transform.position = y;
            }


            //Debug.Log(mouseWorldPosition);
            if(transform.position.y < cam.orthographicSize + 50)
            {
                Vector3 y2 = new Vector3(transform.position.x, cam.orthographicSize + 50, -10f);
                transform.position = y2;
            }
            
            Vector3 botLeft = new Vector3();
            botLeft = (Camera.main.ScreenToWorldPoint(Vector3.zero));

            if(botLeft[0] < 50)
            {
                Vector3 x2 = new Vector3(transform.position.x + 50f - botLeft[0], transform.position.y, -10f);
                transform.position = x2;
            }

        }

    }
}