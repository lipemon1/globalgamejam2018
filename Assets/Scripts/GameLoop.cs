using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XInputDotNetPure;

public class GameLoop : MonoBehaviour
{

    [System.Serializable]
    private class PlayerScore
    {
        public string Name = "";
        public int Kills = 0;
        public int Deaths = 0;
        public GameObject Panel;
        public Text ScoreText;

        public PlayerScore(string name, int kills, int deaths, GameObject panel)
        {
            Name = name;
            Kills = kills;
            Deaths = deaths;
            Panel = panel;
            ScoreText = Panel.transform.GetChild(2).GetChild(0).GetComponent<Text>();
        }
    }

    public static GameLoop Instance;

    public const int MAX_ENERGY = 4;
    public const int INITIAL_ENERGY = 2;
    
    [Header("Game Configurations")]
    [SerializeField] public bool RespawnWithEnergy = false;

    [Header("General Settings")] [SerializeField] private float _startDelay = 3f;
    [SerializeField] private float _endDelay = 3f;
    [SerializeField] private Text _messageText;

    [Header("Player Config")] [SerializeField] private Material[] _playerMaterials;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] _spawnTransforms = new Transform[12];

    [Header("Other Info")] [HideInInspector] private WaitForSeconds _startWait;
    [HideInInspector] private WaitForSeconds _endWait;
    [HideInInspector] private float _timeAlive;

    [Header("Messages")] [SerializeField] private string _startMessage = "AE HOU!";
    [SerializeField] private string _finalMessage = "Terminando...";

    [Header("Tickets Available")] [SerializeField] private int _currentDeathTickets = 0;
    [SerializeField] private int _deathTicketsForMatch = 20;

    [Header("End Match Data")]
    [SerializeField] private List<PlayerScore> _playersScore = new List<PlayerScore>();
    [SerializeField] private List<GameObject> _playerPanels = new List<GameObject>();
    [SerializeField] private GameObject _finalCanvas;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        _startWait = new WaitForSeconds(_startDelay);
        _endWait = new WaitForSeconds(_endDelay);

        // Once the tanks have been created and the camera is using them as targets, start the game.
        StartCoroutine(GameLooping());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) & Input.GetKeyDown(KeyCode.R) & Input.GetKeyDown(KeyCode.S) & Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene(0);
        }
    }

    // This is called from start and will run each phase of the game one after another.
    private IEnumerator GameLooping()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundStarting());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine(RoundPlaying());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine(RoundEnding());

        SceneManager.LoadScene(0);
    }

    private IEnumerator RoundStarting()
    {
        ForceManualSettingPlayers();

        //set the game tickets
        _currentDeathTickets = _deathTicketsForMatch;

        // As soon as the round starts reset the players and make sure they can't move.
        ResetAllPlayers();
        DisablePlayersControl();

        _messageText.text = _startMessage;

        int timeCounter = (int)_startDelay;

        while (timeCounter > 0f)
        {
            _messageText.text = timeCounter.ToString();
            timeCounter--;
            yield return new WaitForSeconds(1);
        }

        _messageText.text = _startMessage;
        yield return new WaitForSeconds(1);
    }

    private IEnumerator RoundPlaying()
    {
        // As soon as the round begins playing let the players control the tanks.
        EnablePlayersControl();

        // Clear the text from the screen.
        _messageText.text = string.Empty;

        // While there is not one tank left...
        while (!GameIsEnded())
        {
            // ... return on the next frame.
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        // Stop players from moving.
        DisablePlayersControl();

        //get player scores
        for (int i = 0; i < Global.MaxPlayers; i++)
        {
            if (Global.Player[i].exist)
            {
                _playersScore.Add(new PlayerScore(("Player") + (i + 1).ToString(), Global.Player[i].kills, Global.Player[i].deaths, _playerPanels[i]));
            }
        }

        foreach (PlayerScore playerScore in _playersScore)
        {
            playerScore.ScoreText.text = ScoreValue(playerScore.Kills, playerScore.Deaths).ToString() + " PONTOS";
            playerScore.Panel.SetActive(true);
        }

        _finalCanvas.SetActive(true);

        // Get a message based on the scores and whether or not there is a game winner and display it.
        _messageText.text = "";

        Global.Player.Clear();

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return _endWait;
    }

    private int ScoreValue(int kills, int death)
    {
        int killsPoints = kills * 3;
        int deathPoints = death * -1;

        return killsPoints + deathPoints;
    }

    private void ForceManualSettingPlayers()
    {
        if (Global.Player.Count == 0)
        {
            Global.ResetAllPlayers();
            for (int i = 0; i < 4; i++) Global.Player[i].exist = true;
        }
    }

    // This function is used to turn all the tanks back on and reset their positions and properties.
    private void ResetAllPlayers()
    {
        foreach (var userPlate in NamePlateManager.Instance.UserPlates)
            userPlate.DisableNamePlate();

        for (int i = 0; i < Global.MaxPlayers; i++)
        {
            if (Global.Player[i].exist)
            {
                Global.Player[i].Instance = Instantiate(_playerPrefab, _spawnTransforms[i].position, _spawnTransforms[i].rotation)
                    .GetComponent<Player>();
                Global.Player[i].PlayerEnergy = Global.Player[i].Instance.gameObject.GetComponent<EnergyHandler>();
                Global.Player[i].Instance.gameObject.GetComponent<EnergyHandler>().RecieveSomeEnergy(INITIAL_ENERGY);
                Global.Player[i].PositionToSpawn = _spawnTransforms[i];
                Global.Player[i].Instance.Index = (PlayerIndex) i;
                Global.Player[i].Blinker = Global.Player[i].Instance.gameObject.GetComponent<Blinker>();
                Global.Player[i].PlayerAnimationsController = Global.Player[i].Instance.gameObject.GetComponent<PlayerAnimController>();

                foreach (var rend in Global.Player[i].Instance.ColoredRenderers)
                {
                    rend.material = _playerMaterials[i];
                }

                foreach (var userPlate in NamePlateManager.Instance.UserPlates)
                    if (userPlate.Index == Global.Player[i].Instance.Index)
                    {
                        userPlate.Owner = Global.Player[i].Instance;
                        userPlate.EnableNamePlate();
                    }
            }
        }
    }

    private void DisablePlayersControl()
    {
        for (int i = 0; i < Global.MaxPlayers; i++)
        {
            if (Global.Player[i].exist)
            {
                Global.Player[i].Instance.DisablePlayerControl();
            }
        }
    }

    private void EnablePlayersControl()
    {
        for (int i = 0; i < Global.MaxPlayers; i++)
        {
            if (Global.Player[i].exist)
            {
                Global.Player[i].Instance.EnablePlayerControl();
            }
        }
    }

    private bool GameIsEnded()
    {
        return (_currentDeathTickets == 0);
    }

    // Returns a string message to display at the end of each round.
    private string EndMessage()
    {
        // By default when a round ends there are no winners so the default end message is a draw.
        string message = "JOGO ACABOU";


        return message;
    }

    public void RemoveTicket()
    {
        _currentDeathTickets--;
    }
}