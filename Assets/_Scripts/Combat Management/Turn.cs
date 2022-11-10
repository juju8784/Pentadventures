using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    //For actual turn management
    public CombatManagement cm;
    public PlaySoundEffect attack;
    public GameObject turnattackSFX;
    public int id;
    [SerializeField] private GameObject camera;
    Vector3 offset = new Vector3(0, 13, -7);

    [SerializeField]public bool isActive = false;
    public bool toggle = false;
    //A possibly temporary thing. I kinda like the idea and it'll make this easier for now

    int poisonCount = 0;
    float elapse = 0;

    protected virtual void Start()
    {

    }

    public virtual void StartTurn()
    {
        isActive = true;
        StatsHolder st = gameObject.GetComponent<StatsHolder>();
        if (st)
        {
            if (st.poisoned)
            {
                poisonCount++;
                if (poisonCount == st.poisonLast)
                {
                    st.RemovePoisonEffect();
                    poisonCount = 0;
                }
                else
                {
                    gameObject.GetComponent<Health>().TakePoisonDamage(st.poisonDamage);
                }
            }
        }
        toggle = GameManager.instance.snapCamToggle;
        camera = GameManager.instance.cameraParent;
    }

    public virtual void EndTurn()
    {
        GameManager.instance.snapCamToggle = toggle;
        isActive = false;
        cm.NextTurn();
    }

    private void Update()
    {
        if (!GameManager.instance.paused && isActive)
        {
            TakeTurn();
            if (Input.GetKeyDown(ControlsManager.SnapCam))
            {
                toggle = !toggle;
                if (!toggle)
                {
                    elapse = 0;
                }
                //ToggleCameraSnap();
            }
            //ToggleCameraSnap();
            if (toggle)
            {
                camera.transform.position = Vector3.Lerp(camera.transform.position, this.gameObject.transform.position + offset, 0.17f);
            }
            else
            {
                elapse += Time.deltaTime;
                if (elapse < 1)
                {
                    camera.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(63, -33, -28), 0.17f);
                }
                else
                {
                    if (camera.transform.position.x > 70)
                    {
                        camera.transform.position = new Vector3(70, -33, camera.transform.position.z);
                    }
                    else if (camera.transform.position.x < 56)
                    {
                        camera.transform.position = new Vector3(56, -33, camera.transform.position.z);
                    }
                    if (camera.transform.position.z < -35)
                    {
                        camera.transform.position = new Vector3(camera.transform.position.x, -33, -35);
                    }
                    else if (camera.transform.position.z > -21)
                    {
                        camera.transform.position = new Vector3(camera.transform.position.x, -33, -21);
                    }
                }
            }
        }
    }

    public virtual void TakeTurn()
    {
        //Implement in the derivation
    }

    public virtual void Attack()
    {

    }

    public virtual void Special()
    {

    }

    public virtual void Heal()
    {

    }
}
