using System;
using System.Collections;
using UnityEngine;
using XInputDotNetPure;

public enum Ds4Button
{
    Cross,
    Circle,
    Square,
    Triangle,
    R1,
    L1,
    Left,
    Down,
    Right,
    Up
}

public enum Ds4Trigger
{
    None = -1,
    R_Trigger,
    L_Trigger
}

public class JoystickInputController : MonoBehaviour
{
    #region Variables

    private static JoystickInputController _instance;

    private GamePadState _joy1LastState;
    private GamePadState _joy1CurState;

    private GamePadState _joy2LastState;
    private GamePadState _joy2CurState;

    private GamePadState _joy3LastState;
    private GamePadState _joy3CurState;

    private GamePadState _joy4LastState;
    private GamePadState _joy4CurState;

    #endregion

    #region Properties

    public static JoystickInputController Instance
    {
        get
        {
            if (_instance) return _instance;
            _instance = new GameObject("JoystickInputController").AddComponent<JoystickInputController>();
            DontDestroyOnLoad(_instance.gameObject);
            return _instance;
        }
    }

    #endregion

    #region UnityCalls

    private void Update()
    {
        UpdatePadStates();
    }

    #endregion

    public void Vibrate(PlayerIndex playerIndex, float leftMotor, float rightMotor, float duration)
    {
        StartCoroutine(IEVibrate(playerIndex, leftMotor, rightMotor, duration));
    }

    private void UpdatePadStates()
    {
        _joy1LastState = _joy1CurState;
        _joy1CurState = GamePad.GetState(PlayerIndex.One);

        _joy2LastState = _joy2CurState;
        _joy2CurState = GamePad.GetState(PlayerIndex.Two);

        _joy3LastState = _joy3CurState;
        _joy3CurState = GamePad.GetState(PlayerIndex.Three);

        _joy4LastState = _joy4CurState;
        _joy4CurState = GamePad.GetState(PlayerIndex.Four);
    }

    private static IEnumerator IEVibrate(PlayerIndex playerIndex, float leftMotor, float rightMotor, float duration)
    {
        GamePad.SetVibration(playerIndex, leftMotor, rightMotor);
        yield return new WaitForSeconds(duration);
        GamePad.SetVibration(playerIndex, 0, 0);
    }

    #region Getters

    public Vector2 LeftStick(PlayerIndex playerIndex)
    {
        switch (playerIndex)
        {
            case PlayerIndex.One:
                return new Vector2(_joy1CurState.ThumbSticks.Left.X, _joy1CurState.ThumbSticks.Left.Y);
            case PlayerIndex.Two:
                return new Vector2(_joy2CurState.ThumbSticks.Left.X, _joy2CurState.ThumbSticks.Left.Y);
            case PlayerIndex.Three:
                return new Vector2(_joy3CurState.ThumbSticks.Left.X, _joy3CurState.ThumbSticks.Left.Y);
            case PlayerIndex.Four:
                return new Vector2(_joy4CurState.ThumbSticks.Left.X, _joy4CurState.ThumbSticks.Left.Y);
        }

        return Vector2.zero;
    }

    public Vector2 RightStick(PlayerIndex playerIndex)
    {
        switch (playerIndex)
        {
            case PlayerIndex.One:
                return new Vector2(_joy1CurState.ThumbSticks.Right.X, _joy1CurState.ThumbSticks.Right.Y);
            case PlayerIndex.Two:
                return new Vector2(_joy2CurState.ThumbSticks.Right.X, _joy2CurState.ThumbSticks.Right.Y);
            case PlayerIndex.Three:
                return new Vector2(_joy3CurState.ThumbSticks.Right.X, _joy3CurState.ThumbSticks.Right.Y);
            case PlayerIndex.Four:
                return new Vector2(_joy4CurState.ThumbSticks.Right.X, _joy4CurState.ThumbSticks.Right.Y);
        }

        return Vector2.zero;
    }

    public float GetPadTriggers(PlayerIndex playerIndex, Ds4Trigger trig)
    {
        var curState = GetCurState(playerIndex);
        switch (trig)
        {
            case Ds4Trigger.L_Trigger:
                return curState.Triggers.Left;
            case Ds4Trigger.R_Trigger:
                return curState.Triggers.Right;
        }
        return -1;
    }

    public bool GetAnyKey(PlayerIndex playerIndex)
    {
        return GetButton(playerIndex, Ds4Button.Cross) ||
               GetButton(playerIndex, Ds4Button.Circle) ||
               GetButton(playerIndex, Ds4Button.Square) ||
               GetButton(playerIndex, Ds4Button.Triangle) ||
               GetButton(playerIndex, Ds4Button.R1) ||
               GetButton(playerIndex, Ds4Button.L1) ||
               GetButton(playerIndex, Ds4Button.Right) ||
               GetButton(playerIndex, Ds4Button.Down) ||
               GetButton(playerIndex, Ds4Button.Left) ||
               GetButton(playerIndex, Ds4Button.Up);
    }
    
    public bool GetAnyKeyDown(PlayerIndex playerIndex)
    {
        return GetButtonDown(playerIndex, Ds4Button.Cross) ||
               GetButtonDown(playerIndex, Ds4Button.Circle) ||
               GetButtonDown(playerIndex, Ds4Button.Square) ||
               GetButtonDown(playerIndex, Ds4Button.Triangle) ||
               GetButtonDown(playerIndex, Ds4Button.R1) ||
               GetButtonDown(playerIndex, Ds4Button.L1) ||
               GetButtonDown(playerIndex, Ds4Button.Right) ||
               GetButtonDown(playerIndex, Ds4Button.Down) ||
               GetButtonDown(playerIndex, Ds4Button.Left) ||
               GetButtonDown(playerIndex, Ds4Button.Up);
    }
    
    public bool GetAnyKeyUp(PlayerIndex playerIndex)
    {
        return GetButtonUp(playerIndex, Ds4Button.Cross) ||
               GetButtonUp(playerIndex, Ds4Button.Circle) ||
               GetButtonUp(playerIndex, Ds4Button.Square) ||
               GetButtonUp(playerIndex, Ds4Button.Triangle) ||
               GetButtonUp(playerIndex, Ds4Button.R1) ||
               GetButtonUp(playerIndex, Ds4Button.L1) ||
               GetButtonUp(playerIndex, Ds4Button.Right) ||
               GetButtonUp(playerIndex, Ds4Button.Down) ||
               GetButtonUp(playerIndex, Ds4Button.Left) ||
               GetButtonUp(playerIndex, Ds4Button.Up);
    }

    public bool GetButton(PlayerIndex playerIndex, Ds4Button btn)
    {
        var curState = GetCurState(playerIndex);
        var lastState = GetLastState(playerIndex);

        switch (btn)
        {
            case Ds4Button.Cross:
                return (curState.Buttons.A == ButtonState.Pressed);
            case Ds4Button.Circle:
                return (curState.Buttons.B == ButtonState.Pressed);
            case Ds4Button.Square:
                return (curState.Buttons.X == ButtonState.Pressed);
            case Ds4Button.Triangle:
                return (curState.Buttons.Y == ButtonState.Pressed);
            case Ds4Button.R1:
                return (curState.Buttons.RightShoulder == ButtonState.Pressed);
            case Ds4Button.L1:
                return (curState.Buttons.LeftShoulder == ButtonState.Pressed);

            //Pad
            case Ds4Button.Left:
                return (curState.DPad.Left == ButtonState.Pressed);
            case Ds4Button.Down:
                return (curState.DPad.Down == ButtonState.Pressed);
            case Ds4Button.Right:
                return (curState.DPad.Right == ButtonState.Pressed);
            case Ds4Button.Up:
                return (curState.DPad.Up == ButtonState.Pressed);
        }
        return false;
    }

    public bool GetButtonDown(PlayerIndex playerIndex, Ds4Button btn)
    {
        var curState = GetCurState(playerIndex);
        var lastState = GetLastState(playerIndex);

        switch (btn)
        {
            case Ds4Button.Cross:
                return (curState.Buttons.A == ButtonState.Pressed && lastState.Buttons.A == ButtonState.Released);
            case Ds4Button.Circle:
                return (curState.Buttons.B == ButtonState.Pressed && lastState.Buttons.B == ButtonState.Released);
            case Ds4Button.Square:
                return (curState.Buttons.X == ButtonState.Pressed && lastState.Buttons.X == ButtonState.Released);
            case Ds4Button.Triangle:
                return (curState.Buttons.Y == ButtonState.Pressed && lastState.Buttons.Y == ButtonState.Released);
            case Ds4Button.R1:
                return (curState.Buttons.RightShoulder == ButtonState.Pressed && lastState.Buttons.RightShoulder == ButtonState.Released);
            case Ds4Button.L1:
                return (curState.Buttons.LeftShoulder == ButtonState.Pressed && lastState.Buttons.LeftShoulder == ButtonState.Released);

            //Pad
            case Ds4Button.Left:
                return (curState.DPad.Left == ButtonState.Pressed && lastState.DPad.Left == ButtonState.Released);
            case Ds4Button.Down:
                return (curState.DPad.Down == ButtonState.Pressed && lastState.DPad.Down == ButtonState.Released);
            case Ds4Button.Right:
                return (curState.DPad.Right == ButtonState.Pressed && lastState.DPad.Right == ButtonState.Released);
            case Ds4Button.Up:
                return (curState.DPad.Up == ButtonState.Pressed && lastState.DPad.Up == ButtonState.Released);
        }
        return false;
    }

    public bool GetButtonUp(PlayerIndex playerIndex, Ds4Button btn)
    {
        var curState = GetCurState(playerIndex);
        var lastState = GetLastState(playerIndex);

        switch (btn)
        {
            case Ds4Button.Cross:
                return (curState.Buttons.A == ButtonState.Released && lastState.Buttons.A == ButtonState.Pressed);
            case Ds4Button.Circle:
                return (curState.Buttons.B == ButtonState.Released && lastState.Buttons.B == ButtonState.Pressed);
            case Ds4Button.Square:
                return (curState.Buttons.X == ButtonState.Released && lastState.Buttons.X == ButtonState.Pressed);
            case Ds4Button.Triangle:
                return (curState.Buttons.Y == ButtonState.Released && lastState.Buttons.Y == ButtonState.Pressed);
            case Ds4Button.R1:
                return (curState.Buttons.RightShoulder == ButtonState.Released && lastState.Buttons.RightShoulder == ButtonState.Pressed);
            case Ds4Button.L1:
                return (curState.Buttons.LeftShoulder == ButtonState.Released && lastState.Buttons.LeftShoulder == ButtonState.Pressed);

            //Pad
            case Ds4Button.Left:
                return (curState.DPad.Left == ButtonState.Released && lastState.DPad.Left == ButtonState.Pressed);
            case Ds4Button.Down:
                return (curState.DPad.Down == ButtonState.Released && lastState.DPad.Down == ButtonState.Pressed);
            case Ds4Button.Right:
                return (curState.DPad.Right == ButtonState.Released && lastState.DPad.Right == ButtonState.Pressed);
            case Ds4Button.Up:
                return (curState.DPad.Up == ButtonState.Released && lastState.DPad.Up == ButtonState.Pressed);
        }
        return false;
    }

    private GamePadState GetCurState(PlayerIndex playerIndex)
    {
        switch (playerIndex)
        {
            case PlayerIndex.One:
                return _joy1CurState;
            case PlayerIndex.Two:
                return _joy2CurState;
            case PlayerIndex.Three:
                return _joy3CurState;
            case PlayerIndex.Four:
                return _joy4CurState;
        }

        throw new NotImplementedException();
    }

    private GamePadState GetLastState(PlayerIndex playerIndex)
    {
        switch (playerIndex)
        {
            case PlayerIndex.One:
                return _joy1LastState;
            case PlayerIndex.Two:
                return _joy2LastState;
            case PlayerIndex.Three:
                return _joy3LastState;
            case PlayerIndex.Four:
                return _joy4LastState;
        }

        throw new NotImplementedException();
    }

    #endregion
}