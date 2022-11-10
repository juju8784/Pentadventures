using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform parentObject;
    //public ControlsManager controls = new ControlsManager();
    Vector3 motion;
    Camera cam;
    public float speedMove = 0.2f;
    public float zoomSpeed = 30;

    public float sensitivity = 1;
    public float maxZoom = 10;
    float zoomLevel;
    float zoomPosition;

    // Start is called before the first frame update
    void Start()
    {
        cam = Object.FindObjectOfType<Camera>();
        motion = new Vector3(0, 0, 0);
        zoomLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.paused == false) {
            if (Input.GetKeyDown(ControlsManager.Forward))
            {
                motion = new Vector3(0, 0, 1);
            }
            else if (Input.GetKeyDown(ControlsManager.Left))
            {
                motion = new Vector3(-1, 0, 0);
            }
            else if (Input.GetKeyDown(ControlsManager.Backwards))
            {
                motion = new Vector3(0, 0, -1);
            }
            else if (Input.GetKeyDown(ControlsManager.Right))
            {
                motion = new Vector3(1, 0, 0);
            }
            else
            {
                motion = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            }
            motion = motion.normalized;
            zoomLevel += Input.mouseScrollDelta.y * sensitivity;
            zoomLevel = Mathf.Clamp(zoomLevel, 0, maxZoom);
            zoomPosition = Mathf.MoveTowards(zoomPosition, zoomLevel, zoomSpeed * Time.deltaTime);
            transform.position = parentObject.position + (transform.forward * zoomPosition);

            //cam.transform.position += (motion * speedMove * Time.deltaTime);
            parentObject.position += (motion * speedMove * Time.deltaTime);
            //cam.transform.localPosition += zoom;
        }
    }
}
