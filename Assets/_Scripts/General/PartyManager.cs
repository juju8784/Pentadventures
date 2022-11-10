using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PartyManager : MonoBehaviour
{
    public List<GameObject> inactive;
    public ActiveParty active;
    public List<GameObject> dead;
    public PartyInfoBlock pInfoBlock;

    public GameObject partyObject;
    [SerializeField] private PlayerSpawner plSpawner;
    [SerializeField] private OverworldSpawning overSpawn;
    bool initializedBefore = false;

    public void Initalize()
    {
        if (!initializedBefore)
        {
            if (!active)
            {
                active = GameObject.FindObjectOfType<ActiveParty>();
            }
            if (!GameManager.instance.player)
            {
                GameManager.instance.player = FindObjectOfType<TestCharacterController>().gameObject;
            }

            // temp addition of these guys
            // TODO
            // Later find the prefabs from the party management stats holder or something

            for (int i = 0; i < pInfoBlock.GetPartyCount(); i++)
            {
                GameObject chara = Instantiate(pInfoBlock.GetPrefab(i));
                chara.SetActive(false);
                chara.GetComponent<TestCharacterController>().gRaycaster = GameManager.instance.gRaycaster;
                chara.GetComponent<TestCharacterController>().eSystem = GameManager.instance.eSystem;
                //chara.transform.position = plSpawner.GetRandomPositionInCombat(); //new Vector3(58.5907f, 1f, -27.725f + (i * 3));
                active.AddMember(chara);
            }

            if (active.count == 0)
            {
                Debug.Log("Error loading party. Party count is 0");
            }
            initializedBefore = true;
            overSpawn.RepositionEntities();
        }
    }

    public void CharacterDiedDuringCombat(GameObject pc)
    {
        if (active.RemoveMember(pc))
        {
            //dead.Add(pc);
            GameManager.instance.combatManagement.deadCharacters.Add(pc.GetComponent<StatsHolder>().block.characterName);
            pc.GetComponent<PlayerTurn>().Death();
            GameManager.instance.combatManagement.RemoveTurn(pc.GetComponent<PlayerTurn>());
            pc.GetComponent<Health>().healthBar.HideBar();
            GameManager.instance.uiManager.UpdatePartyUI();
            //pc.SetActive(false);
            pInfoBlock.RemoveMember(pc.GetComponent<StatsHolder>().block);
            pc.GetComponent<TestCharacterController>().currentTile.entities.Remove(pc);
            Destroy(pc, 1.2f);
            if(active.GetCount() == 0)
            {
                GameManager.instance.combatManagement.EndCombat(false);
            }
        }
    }
    public void InitCharas(GameObject pc, bool activeMember)
    {
        if (activeMember)
        {
            GameObject temp = Instantiate(pc);
            temp.SetActive(false);
            temp.GetComponent<TestCharacterController>().gRaycaster = GameManager.instance.gRaycaster;
            temp.GetComponent<TestCharacterController>().eSystem = GameManager.instance.eSystem;
            temp.transform.position = plSpawner.GetRandomPositionInCombat(); //new Vector3(61.5907f, 1f, -33.725f + (active.count * 2));
            //active.AddMember(temp);
            AddToPartyObject(temp);
            GameManager.instance.uiManager.InitializeUI();
        }
        else
        {
            inactive.Add(pc);
        }
    }
    public void NewCharacterFound(GameObject pc)
    {
        if (!active.AddMember(pc))
        {
            inactive.Add(pc);
            pInfoBlock.AddInactiveMember(pc.GetComponent<StatsHolder>().block);
            GameManager.instance.questManager.FindNewCharacter();
        }
        else
        {
            GameObject temp = Instantiate(pc);
            temp.SetActive(false);
            temp.GetComponent<TestCharacterController>().gRaycaster = GameManager.instance.gRaycaster;
            temp.GetComponent<TestCharacterController>().eSystem = GameManager.instance.eSystem;
            temp.transform.position = new Vector3(61.5907f, 1f, -33.725f + (active.count * 2));
            active.RemoveMember(pc);
            active.AddMember(temp);
            AddToPartyObject(temp);
            GameManager.instance.uiManager.UpdatePartyUIOutOfCombat();
            GameManager.instance.questManager.FindNewCharacter();
            pInfoBlock.AddMember(pc.GetComponent<StatsHolder>().block);
        }// adds to active if there is space, if not, adds to the guild hall
    }

    void AddToPartyObject(GameObject pc)
    {
        Transform[] te = pc.GetComponentsInChildren<Transform>();
        GameObject model = Instantiate(te[1].gameObject);
        partyObject.GetComponent<PartyObject>().PassInNewCharacterToAnimator(model);

        Transform[] t = partyObject.GetComponentsInChildren<Transform>();
        foreach (var item in t)
        {
            if(item.tag == "PartyMembers" && item.childCount == 0)
            {
                model.transform.SetParent(item);
                model.transform.localPosition = new Vector3(0, 0, 0);
                model.transform.localScale = new Vector3(1, 1, 1);
                model.transform.localRotation = Quaternion.identity;
                break;
            }
        }
    }

    void DeactivateMember(GameObject pc)
    {
        if (active.RemoveMember(pc))
        {
            inactive.Add(pc);
        }
    }

    void ActivateMember(GameObject pc)
    {
        if (active.AddMember(pc))
        {
            if (inactive.Contains(pc))
            {
                inactive.Remove(pc);
            }
        }
    }
}
