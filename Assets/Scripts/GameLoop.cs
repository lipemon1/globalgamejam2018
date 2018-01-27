using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XInputDotNetPure;

public class GameLoop : MonoBehaviour
{
    public static GameLoop Instance;

    public const int MAX_ENERGY = 4;

    [Header("General Settings")] [SerializeField] private float _startDelay = 3f;
    [SerializeField] private float _endDelay = 3f;
    [SerializeField] private Text _messageText;

    [Header("Players Controllers")] [SerializeField] private List<Player> _playersControllersList = new List<Player>();

    [Header("Player Config")] [SerializeField] private Material[] _playerMaterials;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private int _initialEnergy;
    [SerializeField] private Transform[] _spawnTransforms = new Transform[12];

    [Header("Other Info")] [HideInInspector] private WaitForSeconds _startWait;
    [HideInInspector] private WaitForSeconds _endWait;
    [HideInInspector] private float _timeAlive;

    [Header("Messages")] [SerializeField] private string _startMessage = "Começando...";
    [SerializeField] private string _finalMessage = "Terminando...";

    [Header("Tickets Available")] [SerializeField] private int _currentDeathTickets = 0;
    [SerializeField] private int _deathTicketsForMatch = 20;

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

        yield return _startWait;
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

        // Get a message based on the scores and whether or not there is a game winner and display it.
        _messageText.text = _finalMessage;

        Global.Player.Clear();

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return _endWait;
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
                Global.Player[i].Instance.gameObject.GetComponent<EnergyHandler>().RecieveSomeEnergy(_initialEnergy);
                Global.Player[i].Instance.Index = (PlayerIndex) i;
                Global.Player[i].Instance.GetComponent<Renderer>().material = _playerMaterials[i];

                foreach (var userPlate in NamePlateManager.Instance.UserPlates)
                    if (userPlate.Index == Global.Player[i].Instance.Index)
                    {
                        userPlate.Owner = Global.Player[i].Instance;
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
}