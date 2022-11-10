using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkButtonScript : MonoBehaviour
{
    LostCharacter lost;
    void Start()
    {
        lost = FindObjectOfType<LostCharacter>();
        this.GetComponent<Button>().onClick.AddListener(lost.StartCutscene);
    }

}
