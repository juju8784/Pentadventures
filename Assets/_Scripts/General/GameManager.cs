using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public GameObject canvas;
    private GameObject GUI;
    private GameObject PlayerSpawner;
    public GameObject player;
    public GameObject rogue;
    public BaseTile playerTile;
    public bool isCombat;
    public bool paused;
    public bool snapCamToggle;
    //public CombatManager CombatManager;
    //public PartyManager PartyManager;
    public TurnManager TurnManager;
    public GameObject cameraParent;
    public CombatManagement combatManagement;
    public CombatUIManager combatUIManager;
    public AudioManager audioManager;
    public AudioUIManager audioUIManager;
    public Music music;
    public WorldAITurn aiTurns;
    public PartyManager partyManager;
    public PartyUIManager uiManager;
    public GraphicRaycaster gRaycaster;
    public EventSystem eSystem;
    public EnemyManager enemyManager;
    public HexGrid overGrid;
    public HexGrid combatGrid;
    public QuestManager questManager;

    public GameObject rogueInCombat { get; set; }

    public PartyInfoBlock partyInfoBlock;
    //public List<List<CharaStatBlock>> characters = new List<List<CharaStatBlock>>();

    //[SerializeField] List<CharaStatBlock> Alessia = new List<CharaStatBlock>();
    //[SerializeField] List<CharaStatBlock> Erin = new List<CharaStatBlock>();
    //[SerializeField] List<CharaStatBlock> Galahad = new List<CharaStatBlock>();
    //[SerializeField] List<CharaStatBlock> Leonidas = new List<CharaStatBlock>();
    //[SerializeField] List<CharaStatBlock> Thomas = new List<CharaStatBlock>();
    //[SerializeField] List<CharaStatBlock> Vivienne = new List<CharaStatBlock>();
    //[SerializeField] List<CharaStatBlock> Savannah = new List<CharaStatBlock>();
    //[SerializeField] List<CharaStatBlock> Soma = new List<CharaStatBlock>();

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        instance.canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
        instance.GUI = GameObject.Find("GUI");
        instance.PlayerSpawner = GameObject.Find("Player Spawner");
        instance.paused = false;
        instance.isCombat = false;
        instance.snapCamToggle = false;
        instance.gRaycaster = GameObject.FindObjectOfType<GraphicRaycaster>();
        instance.eSystem = GameObject.FindObjectOfType<EventSystem>();
        //instance.CombatManager = GameObject.FindObjectOfType<CombatManager>();
        //instance.PartyManager = GameObject.FindObjectOfType<PartyManager>();
        instance.TurnManager = GameObject.FindObjectOfType<TurnManager>();
        instance.cameraParent = GameObject.Find("parent(forZoomPurpuses)");
        instance.combatManagement = GameObject.FindObjectOfType<CombatManagement>().GetComponent<CombatManagement>();
        instance.combatUIManager = FindObjectOfType<CombatUIManager>().GetComponent<CombatUIManager>();
        instance.player = GameObject.FindObjectOfType<PartyStatsHolder>().gameObject;
        instance.audioManager = GameObject.FindObjectOfType<AudioManager>();
        instance.audioUIManager = GameObject.FindObjectOfType<AudioUIManager>();
        instance.music = GameObject.FindObjectOfType<Music>();
        instance.aiTurns = GetComponent<WorldAITurn>();
        instance.partyManager = GameObject.FindObjectOfType<PartyManager>();
        instance.uiManager = GameObject.FindObjectOfType<PartyUIManager>();
        instance.enemyManager = GetComponentInChildren<EnemyManager>();
        instance.overGrid = GameObject.Find("Grid").GetComponent<HexGrid>();
        instance.combatGrid = GameObject.Find("CombatGrid").GetComponent<HexGrid>();
        instance.questManager = GameObject.FindObjectOfType<QuestManager>();
    }

    private void Start()
    {
        //instance.PlayerSpawner.GetComponent<PlayerSpawner>().SpawnPlayer();
        //instance.PlayerSpawner.GetComponent<PlayerSpawner>().SpawnPlayer();
        //characters.Add(Alessia);
        //characters.Add(Erin);
        //characters.Add(Galahad);
        //characters.Add(Leonidas);
        //characters.Add(Savannah);
        //characters.Add(Soma);
        //characters.Add(Thomas);
        //characters.Add(Vivienne);
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    PauseGame();
        //}
    }

    //public void Upgrade(CharaStatBlock chara)
    //{
    //    int nextIndex = chara.starLevel;
    //    CharaStatBlock temp = characters[chara.managerIndex][nextIndex];
    //    if (partyManager.pInfoBlock.Contains(chara))
    //    {
    //        partyManager.pInfoBlock.RemoveMember(chara);
    //        partyManager.pInfoBlock.AddMember(temp);
    //    }
    //    else if (partyManager.pInfoBlock.inactives.Contains(chara))
    //    {
    //        partyManager.pInfoBlock.RemoveInactiveMember(chara);
    //        partyManager.pInfoBlock.AddInactiveMember(temp);
    //    }
    //}
    public void PauseGame()
    {
        paused = !paused;
        combatUIManager.OnPause(paused);
    }

}
