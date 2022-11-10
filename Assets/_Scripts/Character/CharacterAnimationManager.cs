using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationManager : MonoBehaviour
{
    public List<Animator> anim;

    public string standTrigger;
    public string runTrigger;
    public string attackTrigger;

    private TestCharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<TestCharacterController>();
    }

    public void AddAnimation(Animator pc)
    {
        anim.Add(pc);
    }

    public void RemoveAnimation(Animator pc)
    {
        anim.Remove(pc);
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.stopped)
        {
            foreach (var item in anim)
            {
                item.SetTrigger(runTrigger);
            }
        }
        else
        { 
            foreach (var item in anim)
            {
                item.SetTrigger(standTrigger);
            }
        }
    }
}
