using UnityEngine;

public class StarHaver : MonoBehaviour
{
    Star star;
    [SerializeField] private GameObject starPrefab;
    [SerializeField] protected Camera cam;
    [SerializeField] Canvas canvas;

    GameObject starObject;
    public void Start()
    {
        if (!canvas)
        {
            canvas = GameManager.instance.canvas.GetComponent<Canvas>();
        }
        if (!cam)
        {
            cam = GameObject.FindObjectOfType<Camera>();
        }

        if (starPrefab)
        {
            starObject = Instantiate(starPrefab);
            starObject.transform.SetParent(canvas.transform);
            starObject.transform.SetAsFirstSibling();
            star = starObject.GetComponent<Star>();
            star.StarPositioning(this.gameObject);
            star.UpdateText();
        }

    }
    private void Update()
    {
        if (!GameManager.instance.isCombat)
        {
            if (star)
            {
                star.Show();
                star.StarPositioning(this.gameObject);
            }
        }
        else
        {
            if (star)
            {
                star.Hide();
            }
        }
    }

    private void OnDestroy()
    {
        DestroyStar();
    }
    public void DestroyStar()
    {
        Destroy(star);
    }

}
