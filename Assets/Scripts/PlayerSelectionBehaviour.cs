using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public enum SelectionState
{
    FreshStart,
    WaitingTwoPlayers,
    WaitingOthers
}

[System.Serializable]
public class SelectionPanel
{
    public bool Entering;
    public float Timer;
    public float normalizedTime;

    public SelectionPanel()
    {
        Entering = false;
        Timer = 0;
        normalizedTime = 0;
    }
}

public class PlayerSelectionBehaviour : MonoBehaviour
{
    public float TimeToConfirm = 2f;

    [SerializeField] private float MaxTime = 5f;
    [SerializeField] private float Timer = 0;

    private SelectionState state;
    [SerializeField] private List<SelectionPanel> SelectionPlayer;

    //Testing
    private KeyCode[] key = { KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8 };

    private void Awake()
    {
        Global.ResetAllPlayers();
        for (int i = 0; i < Global.MaxPlayers; i++)
            SelectionPlayer.Add(new SelectionPanel());
    }

    private void Update()
    {
        if (state != SelectionState.FreshStart)
        {

        }
        switch (state)
        {
            case SelectionState.FreshStart:
                state++;
                break;
            case SelectionState.WaitingTwoPlayers:
                CheckInputs();
                bool _first = false;
                foreach (PlayerInfo _player in Global.Player)
                {
                    if (_player.exist)
                    {
                        if (_first)
                        {
                            state++;
                            break;
                        }
                        _first = true;
                    }
                }
                break;
            case SelectionState.WaitingOthers:
                CheckInputs();
                Timer += Time.deltaTime;
                for (int i = 0; i < Global.MaxPlayers; i++)
                {
                    if (AnyKey(i)&&!Global.Player[i].exist)
                    {
                        Timer = 0;
                        break;
                    }
                }
                if (Timer > MaxTime)
                {
                    EndSelection();
                }
                break;
            default:
                break;
        }
    }

    private bool AnyKeyDown(int _index)
    {
        return JoystickInputController.Instance.GetAnyKeyDown((PlayerIndex)_index);
    }
    private bool AnyKey(int _index)
    {
        return JoystickInputController.Instance.GetAnyKey((PlayerIndex)_index);
    }
    private bool AnyKeyUp(int _index)
    {
        return JoystickInputController.Instance.GetAnyKeyUp((PlayerIndex)_index);
    }


    private void CheckInputs()
    {
        for (int i = 0; i < Global.MaxPlayers; i++)
        {

            if (AnyKeyDown(i) && !SelectionPlayer[i].Entering && !Global.Player[i].exist)
            {
                Debug.Log("Player " + i.ToString() + " apertou o botão");
                StartCoroutine(Entering(i));
            }
            if (AnyKeyUp(i))
            {
                Debug.Log("Player " + i.ToString() + " soltou o botão");
                SelectionPlayer[i].Entering = false;
            }
        }
    }


    private IEnumerator Entering(int _index)
    {
        SelectionPlayer[_index].Entering = true;
        SelectionPlayer[_index].Timer = 0;
        while (AnyKey(_index) && !Global.Player[_index].exist)
        {
            Debug.Log("Player " + _index.ToString() + " está segurando o botão");
            if (!SelectionPlayer[_index].Entering)
                yield break;

            SelectionPlayer[_index].Timer += Time.deltaTime;
            SelectionPlayer[_index].normalizedTime = (SelectionPlayer[_index].Timer / TimeToConfirm);

            if (SelectionPlayer[_index].Timer > TimeToConfirm)
            {
                Global.Player[_index].exist = true;
                Debug.Log("Player " + _index.ToString() + " Pronto");
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void EndSelection()
    {
        int _counter = 0;
        foreach (PlayerInfo _player in Global.Player)
        {
            if (_player.exist)
                _counter++;
        }
        Debug.Log("Iniciando partida com " + _counter + " Players");
        SceneManager.LoadScene("Main");
    }

}
