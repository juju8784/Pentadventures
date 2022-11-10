using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteDialog : MonoBehaviour
{
    public void OK()
    {
        Destroy(this.gameObject);
    }
}
