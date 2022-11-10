using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] Transform startPos;
    [SerializeField] Transform finishPos;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject UIcanvas;
    [SerializeField] TextRolling roller;

    bool animating = false;
    bool animatingBack = false;
    Vector3 poslerp;
    Quaternion rotationlerp;

    public void AnimateCamera()
    {
        animating = true;
        camera.gameObject.transform.SetPositionAndRotation(startPos.position, startPos.rotation);
        poslerp = startPos.position;
        rotationlerp = startPos.rotation;
    }

    public void AnimateCameraBack()
    {
        animatingBack = true;
        camera.gameObject.transform.SetPositionAndRotation(finishPos.position, finishPos.rotation);
        //poslerp = finishPos.position;
        //rotationlerp = finishPos.rotation;
    }

    public void Update()
    {
        if (animating)
        {
            poslerp = Vector3.Lerp(poslerp, finishPos.position, 0.3f * Time.deltaTime);
            rotationlerp = Quaternion.Lerp(rotationlerp, finishPos.rotation, 0.3f * Time.deltaTime);

            camera.gameObject.transform.SetPositionAndRotation(poslerp, rotationlerp);

            if(camera.gameObject.transform.position.x <= finishPos.position.x + 0.2f)
            {
                animating = false;
                canvas.SetActive(true);
                roller.StartRolling();
            }
        }
        if (animatingBack)
        {
            poslerp = Vector3.Lerp(poslerp, startPos.position, 0.3f * Time.deltaTime);
            rotationlerp = Quaternion.Lerp(rotationlerp, startPos.rotation, 0.3f * Time.deltaTime);

            camera.gameObject.transform.SetPositionAndRotation(poslerp, rotationlerp);
            if (camera.gameObject.transform.position.z <= startPos.position.z + 0.1f)
            {
                animatingBack = false;
                canvas.SetActive(false);
                UIcanvas.SetActive(true);
                camera.gameObject.SetActive(false);
            }
        }
    }
}
