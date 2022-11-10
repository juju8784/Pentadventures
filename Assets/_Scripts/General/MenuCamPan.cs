using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamPan : MonoBehaviour
{
    public Camera cam;
    public float panTime = 10;
    float elapse;
    float temp;

    void Start()
    {
        if (!cam) {
            cam = gameObject.GetComponent<Camera>();
        }
        elapse = panTime * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        elapse += Time.deltaTime;
        if (elapse > panTime)
        {
            //temp = Mathf.Sin(Time.deltaTime);
            temp = 0.003f;
            if (elapse > (panTime * 2))
            {
                elapse = 0;
            }
        }
        else
        {
            temp = -0.003f;
        }
        cam.transform.Rotate(Vector3.up, temp);
    }
}
