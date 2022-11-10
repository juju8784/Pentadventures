using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] Image bar;
    [SerializeField] ChangeText text;
    [SerializeField] ChangeText actionPointsText;
    [SerializeField] Camera camera;
    [SerializeField] GameObject unit;
    [SerializeField] GameObject healthBarObject;
    private Health unitHealth;
    public Vector3 offset;
    public Gradient gradient;

    private void Start()
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

    public void ShowBar()
    {
        if (healthBarObject)
        {
            healthBarObject.SetActive(true);
            UpdateText();
        }
        else
        {
            Debug.Log("Missing Health Bar Object");
        }
    }

    public void HideBar()
    {
        if (healthBarObject)
        {
            healthBarObject.SetActive(false);
        }
        else
        {
            Debug.Log("Missing Health Bar Object");
        }
    }

    public void HealthBarPositioning(GameObject _unit)
    {
        unit = _unit;
        unitHealth = unit.GetComponent<Health>();
        Vector3 position = unit.transform.position + offset;
        //Debug.Log(position);
        if (healthBarObject)
        {
            if (!camera)
            {
                camera = GameObject.FindObjectOfType<Camera>();
            }
            healthBarObject.transform.position = camera.WorldToScreenPoint(position);
        }
        else
        {
            Debug.Log("Missing Health Bar Object");
        }
        UpdateText();
    }
      
    public void BarFillAmmount()
    {
        bar.fillAmount = (float)unitHealth.GetHealth() / (float)unitHealth.maxHealth;
        bar.color = gradient.Evaluate(bar.fillAmount);
        UpdateText();
    }

    public void UpdateActionsLeft(int actionPointsLeft)
    {
        actionPointsText.Change(actionPointsLeft.ToString());
    }

    void UpdateText()
    {
        text.Change(unitHealth.GetHealth().ToString());
    }
}
