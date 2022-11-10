using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyObject : MonoBehaviour
{
    public CharacterAnimationManager animator;
    public PartyManager partyManager;

    [SerializeField] private GameObject camera;
    Vector3 offset = new Vector3(0, 11, -5);
    Vector3 lastPosition;
    bool toggle = false;
    float elapse = 0;

    private void Start()
    {
        animator = gameObject.GetComponent<CharacterAnimationManager>();
        if (!partyManager)
        {
            partyManager = GameManager.instance.partyManager;
        }
        lastPosition = camera.transform.position;
    }
    

    public void Update()
    {
        if (!GameManager.instance.paused && !GameManager.instance.isCombat)
        {
            //if (Input.GetKeyDown(ControlsManager.SnapCam))
            //{
            //    toggle = !toggle;
            //    if (!toggle)
            //    {
            //        elapse = 0;
            //    }
            //}
            //if (toggle)
            //{
            //    camera.transform.position = Vector3.Lerp(camera.transform.position, this.gameObject.transform.position + offset, 0.18f);
            //}
            //else
            //{
            //    elapse += Time.deltaTime;
            //    if (elapse < 1)
            //    {
            //        camera.transform.position = Vector3.Lerp(camera.transform.position, this.gameObject.transform.position + lastPosition, 0.17f);
            //    }
            //}
            camera.transform.position = Vector3.Lerp(camera.transform.position, this.gameObject.transform.position + offset, 0.18f);
        }
    }

    public void JoinNewCharacter()
    {

    }

    public void PassInNewCharacterToAnimator(GameObject pc)
    {
        Animator anim = pc.GetComponent<Animator>();
        animator.AddAnimation(anim);
    }

}
