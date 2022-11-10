using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    [SerializeField] ChangeText text;
    [SerializeField] Camera camera;
    [SerializeField] GameObject unit;
    private StatsHolder stats;
    public Vector3 offset;

    public void Start()
    {
        if (!camera)
        {
            camera = GameObject.FindObjectOfType<Camera>();
        }
        //if (!unit)
        //{
        //    unit = GameManager.instance.player;
        //    unitHealth = unit.GetComponent<Health>();
        //}
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void StarPositioning(GameObject _unit)
    {
        if (!unit)
        {
            unit = _unit;
        }
        if (!stats)
        {
            stats = unit.GetComponent<EnemyStatsHolder>();
        }
        Vector3 position = unit.transform.position + offset;
        //Debug.Log(position);
        //if (star)
        //{
            if (!camera)
            {
                camera = GameObject.FindObjectOfType<Camera>();
            }
        this.transform.position = camera.WorldToScreenPoint(position);
        //}
        //else
        //{
        //    Debug.Log("Missing Star Object");
        //}
        UpdateText();
    }

    public void UpdateText()
    {
        text.Change(stats.StarLevel.ToString());
    }
}
