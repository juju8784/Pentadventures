using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernDoor : MonoBehaviour
{
    //List<GameObject> door = new List<GameObject>();
    //private void Start()
    //{
    //    Transform[] temp = gameObject.GetComponentsInChildren<Transform>();
    //    foreach (var item in temp)
    //    {
    //        door.Add(item.gameObject);
    //    }
    //    door.RemoveAt(0);
    //}
    public void OpenDoor()
    {
        gameObject.transform.RotateAround(new Vector3(-1f, 0, 2.2f), new Vector3(0, 1, 0), -6);
        //foreach (var item in door)
        //{
        //    item.transform.RotateAround(new Vector3(-1f, 0, 2.2f), new Vector3(0, 1, 0), -10);
        //}
    }

    public void CloseDoor()
    {
        gameObject.transform.RotateAround(new Vector3(-1f, 0, 2.2f), new Vector3(0, 1, 0), 6);
        //foreach (var item in door)
        //{
        //    item.transform.RotateAround(new Vector3(-1f, 0, 2.2f), new Vector3(0, 1, 0), 10);
        //}
    }
}
