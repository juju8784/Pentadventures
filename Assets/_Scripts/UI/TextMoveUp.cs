using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMoveUp : MonoBehaviour
{
    public float speed = 2;
    public float speed2;
    public float waitTime = 0.3f;
    public float firstStrechTime = 0.01f;
    Vector3 offset;
    float elapse = 0;

    void Start()
    {
        offset = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        elapse += Time.deltaTime;
        if (elapse < firstStrechTime)
        {
            offset = new Vector3(0, speed, 0);
            //gameObject.transform.position += offset;
            gameObject.GetComponent<RectTransform>().position += offset;
        }
        else if (elapse < waitTime)
        {
            offset = new Vector3(0, speed2, 0);
            //offset = new Vector3(0, speed * 0.05f, 0);
            gameObject.GetComponent<RectTransform>().position += offset;
        }
    }
}
