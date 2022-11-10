using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LostCharacter : Entity
{
    [SerializeField] GameObject unit;
    [SerializeField] CharaStatBlock chara;
    [SerializeField] PartyInfoBlock pInfoBlock;

    public GameObject characterPrefab;
    public GameObject symbolPrefab;
    public GameObject buttonPrefab;
    public GameObject dialogPrefab;
    public Vector3 symbolOffset;
    public Vector3 buttonOffset;
    [SerializeField] Canvas canvas;
    //make symbol appear on top of the head
    PartyManager partyManager;
    GameObject dialogBox;
    GameObject button;
    GameObject symbol;
    float elapse;
    //public Vector3 scaleChange = new Vector3(0.01f, 0.01f, 0.01f);
    //public float waveLength = 0.5f;
    //float fraction;

    private void Start()
    {
        unit = gameObject;
        partyManager = GameManager.instance.partyManager;
        if (!canvas)
        {
            canvas = GameObject.FindObjectOfType<Canvas>();
        }
        button = Instantiate(buttonPrefab);
        button.transform.SetParent(canvas.transform, false);
        button.GetComponent<Button>().onClick.AddListener(StartCutscene);
        button.transform.SetAsFirstSibling();
        symbol = Instantiate(symbolPrefab);
        symbol.transform.SetParent(canvas.transform, false);
        symbol.transform.SetAsFirstSibling();
        HideButton();
        elapse = 0;
        for (int i = 0; i < GameManager.instance.partyInfoBlock.members.Count; i++)
        {
            if (chara.characterName == GameManager.instance.partyInfoBlock.members[i].characterName)
            {
                chara = GameManager.instance.partyInfoBlock.members[i];
                break;
            }
        }
        for (int i = 0; i < GameManager.instance.partyInfoBlock.inactives.Count; i++)
        {
            if (chara.characterName == GameManager.instance.partyInfoBlock.inactives[i].characterName)
            {
                chara = GameManager.instance.partyInfoBlock.inactives[i];
                break;

            }
        }
    }
    protected override void Run()
    {
        
        if (pInfoBlock.Contains(chara) || pInfoBlock.inactives.Contains(chara))
        {
            InitJoinTheParty();
        }

        Vector3 position = unit.transform.position + symbolOffset;
        Vector3 buttonPosition = unit.transform.position + buttonOffset;
        symbol.transform.position = Camera.main.WorldToScreenPoint(position);
        button.transform.position = Camera.main.WorldToScreenPoint(buttonPosition);

        //fraction += Time.deltaTime * 20;
        //Vector3 newPos = Vector3.Lerp(symbol.transform.localScale, directions[1].transform.position, fraction);
        //newPos.y = transform.position.y;
        //transform.position = newPos;

        //elapse += Time.deltaTime;
        //if (elapse > waveLength)
        //{
        //    //temp = Mathf.Sin(Time.deltaTime);
        //    symbol.transform.localScale += scaleChange * Time.deltaTime;
        //    if (elapse >= (waveLength * 2))
        //    {
        //        elapse = 0;
        //    }
        //}
        //else
        //{
        //    symbol.transform.localScale -= scaleChange * Time.deltaTime;
        //}
        if (currentTile)
        {
            DetectPlayerInNeighbors();
        }
    }
    public void DetectPlayerInNeighbors()
    {
        // code to check if the player is standing on any neighboring tiles
        if (currentTile.TileDistance(GameManager.instance.player.GetComponent<TestCharacterController>().currentTile) == 1)
        {
            ShowButton();
        }
        else
        {
            HideButton();
        }
    }

    void ShowButton()
    {
        button.SetActive(true);
    }
    public void HideButton()
    {
        button.SetActive(false);
    }

    public void StartCutscene()
    {
        dialogBox = Instantiate(dialogPrefab);
        dialogBox.transform.SetParent(canvas.transform, false);
        dialogBox.GetComponent<ThomasDialogButtons>().Initiate(this.GetComponent<LostCharacter>());
        // shows dialog with text talking about the character and two option buttons, to take them with you or to leave them
    }

    // function called by a button press when you choose to ignore the lost chara 
    public void Ignored()
    {
        Destroy(dialogBox, 1);
    }

    // when the game starts, remove lost character that are in the party already
    public void InitJoinTheParty()
    {
        if (dialogBox)
        {
            Destroy(dialogBox, 1);
        }
        partyManager.InitCharas(chara.prefab, pInfoBlock.Contains(chara));
        // deal with adding the character to the party, then destroy this object
        // pass in the prefabs and models for the character
        if (currentTile)
        {
            currentTile.RemoveEntity(this);
        }
        if (button)
        {
            Destroy(button);
        }
        if (symbol)
        {
            Destroy(symbol);
        }
        Destroy(this.gameObject);
        Debug.Log("LostCharacter object is going to the destroyed");
    }

    // function called by a button press when you choose to have the lost chara join your party
    public void JoinTheParty()
    {
        Destroy(dialogBox, 1);
        partyManager.NewCharacterFound(chara.prefab);
        // deal with adding the character to the party, then destroy this object
        // pass in the prefabs and models for the character
        currentTile.RemoveEntity(this);
        Destroy(button);
        Destroy(symbol);
        Destroy(this.gameObject);
        Debug.Log("LostCharacter object is going to the destroyed");
    }

    public override void Activate()
    {
        base.Activate();
        symbol.gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        symbol.gameObject.SetActive(false);
        base.Deactivate();
    }
}
