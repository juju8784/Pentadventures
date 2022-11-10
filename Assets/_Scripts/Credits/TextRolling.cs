using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRolling : MonoBehaviour
{
    //[SerializeField] Vector3 sPos;
    //[SerializeField] Vector3 fPos;
    //[SerializeField] Vector3 lerpPos;
    [SerializeField] Animator anim;

    //bool rolling = false;
    //public int speed = 20;
    public void StartRolling()
    {
        anim.Play("Rolling");
        //rolling = true;
        //lerpPos = sPos;
    }


    //public void Update()
    //{
    //    if (rolling)
    //    {
    //        float d = (fPos.y - sPos.y) / lerpPos.y;
    //        lerpPos = Vector3.Lerp(sPos, fPos, speed * Time.deltaTime);
    //        gameObject.transform.localPosition = lerpPos;
    //    }
    //}
}
