/// <summary>
/// helper class to allow for quicker programming for controller input
/// </summary>
using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Controller{

    //Constructor setting ID for player 
    public Controller(int _Id)
    {
        id = _Id;
    }

    //Id relating to which controller is being used
    int id;

    public int ID
    {
        get
        {
            return id;
        }

        set
        {
            id = Mathf.Clamp(value, 0, 3);
        }
    }

    //States for controller
    GamePadState state, prevState;

    public void updateStates()
    {      
        prevState = state;
        state = GamePad.GetState((PlayerIndex)id);
    }

    public void updateStates(int _ID)
    {
        prevState = state;
        state = GamePad.GetState((PlayerIndex)_ID);
    }

    private float rumble
    {
        set
        {
            GamePad.SetVibration((PlayerIndex)id, value, value);
        }
    }

    public IEnumerator setRumble(float time, float strength)
    {
        GamePad.SetVibration((PlayerIndex)id, strength, strength);
        yield return new WaitForSeconds(time);
        GamePad.SetVibration((PlayerIndex)id, 0, 0);
    }

    public bool IsConnected
    {
        get
        {  
            return state.IsConnected;
        }
    }

    public uint packetNumber
    {
        get
        {
            return state.PacketNumber;
        }
    }

    public float leftTrigger
    {
        get
        {
            return state.Triggers.Left;
        }
    }

    public float rightTrigger
    {
        get
        {
            return state.Triggers.Right;
        }
    }

    public Vector2 leftStick
    {
        get
        {
            return new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
        }
    }

    public Vector2 rightStick
    {
        get
        {
            return new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
        }
    }

    public bool anyKey
    {
        get
        {
            return state.Buttons.A == ButtonState.Pressed ||
                state.Buttons.B == ButtonState.Pressed ||
                state.Buttons.X == ButtonState.Pressed ||
                state.Buttons.Y == ButtonState.Pressed ||
                state.Buttons.LeftShoulder == ButtonState.Pressed ||
                state.Buttons.RightShoulder == ButtonState.Pressed ||
                state.DPad.Up == ButtonState.Pressed ||
                state.DPad.Left == ButtonState.Pressed ||
                state.DPad.Right == ButtonState.Pressed ||
                state.DPad.Down == ButtonState.Pressed ||
                state.Buttons.Back == ButtonState.Pressed ||
                state.Buttons.Start == ButtonState.Pressed ||
                state.Buttons.Guide == ButtonState.Pressed ||
                state.Buttons.LeftStick == ButtonState.Pressed ||
                state.Buttons.RightStick == ButtonState.Pressed;
        }
    }

    public bool anyKeyDown
    {
        get
        {
            return (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released) ||
                (state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released) ||
                (state.Buttons.X == ButtonState.Pressed && prevState.Buttons.X == ButtonState.Released) ||
                (state.Buttons.Y == ButtonState.Pressed && prevState.Buttons.Y == ButtonState.Released) ||
                (state.Buttons.LeftShoulder == ButtonState.Pressed && prevState.Buttons.LeftShoulder == ButtonState.Released) ||
                (state.Buttons.RightShoulder == ButtonState.Pressed && prevState.Buttons.RightShoulder == ButtonState.Released) ||
                (state.DPad.Up == ButtonState.Pressed && prevState.DPad.Up == ButtonState.Released) ||
                (state.DPad.Left == ButtonState.Pressed && prevState.DPad.Left == ButtonState.Released) ||
                (state.DPad.Right == ButtonState.Pressed && prevState.DPad.Right == ButtonState.Released) ||
                (state.DPad.Down == ButtonState.Pressed && prevState.DPad.Down == ButtonState.Released) ||
                (state.Buttons.Back == ButtonState.Pressed && prevState.Buttons.Back == ButtonState.Released) ||
                (state.Buttons.Start == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released) ||
                (state.Buttons.Guide == ButtonState.Pressed && prevState.Buttons.Guide == ButtonState.Released) ||
                (state.Buttons.LeftStick == ButtonState.Pressed && prevState.Buttons.LeftStick == ButtonState.Released) ||
                (state.Buttons.RightStick == ButtonState.Pressed && prevState.Buttons.RightStick == ButtonState.Released);
        }
    }

    public bool isKey(XKeyCode key)
    {
        switch (key)
        {
            case XKeyCode.A:
                return state.Buttons.A == ButtonState.Pressed;

            case XKeyCode.B:
                return state.Buttons.B == ButtonState.Pressed;

            case XKeyCode.X:
                return state.Buttons.X == ButtonState.Pressed;

            case XKeyCode.Y:
                return state.Buttons.Y == ButtonState.Pressed;

            case XKeyCode.LeftShoulder:
                return state.Buttons.LeftShoulder == ButtonState.Pressed;

            case XKeyCode.RightShoulder:
                return state.Buttons.RightShoulder == ButtonState.Pressed;

            case XKeyCode.DLeft:
                return state.DPad.Left == ButtonState.Pressed;

            case XKeyCode.DUp:
                return state.DPad.Up == ButtonState.Pressed;

            case XKeyCode.DRight:
                return state.DPad.Right == ButtonState.Pressed;

            case XKeyCode.DDown:
                return state.DPad.Down == ButtonState.Pressed;

            case XKeyCode.Back:
                return state.Buttons.Back == ButtonState.Pressed;

            case XKeyCode.Start:
                return state.Buttons.Start == ButtonState.Pressed;

            case XKeyCode.Guide:
                return state.Buttons.Guide == ButtonState.Pressed;

            case XKeyCode.LeftStick:
                return state.Buttons.LeftStick == ButtonState.Pressed;

            case XKeyCode.RightStick:
                return state.Buttons.RightStick == ButtonState.Pressed;

            default:
                Debug.LogWarning("No valid XKeyCode found");
                return false;
        }
    }

    public bool isKeyDown(XKeyCode key)
    {
        switch (key)
        {
            case XKeyCode.A:
                return state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A == ButtonState.Released;

            case XKeyCode.B:
                return state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released;

            case XKeyCode.X:
                return state.Buttons.X == ButtonState.Pressed && prevState.Buttons.X == ButtonState.Released;

            case XKeyCode.Y:
                return state.Buttons.Y == ButtonState.Pressed && prevState.Buttons.Y == ButtonState.Released;

            case XKeyCode.LeftShoulder:
                return state.Buttons.LeftShoulder == ButtonState.Pressed && prevState.Buttons.LeftShoulder == ButtonState.Released;

            case XKeyCode.RightShoulder:
                return state.Buttons.RightShoulder == ButtonState.Pressed && prevState.Buttons.RightShoulder == ButtonState.Released;

            case XKeyCode.DLeft:
                return state.DPad.Left == ButtonState.Pressed && prevState.DPad.Left == ButtonState.Released;

            case XKeyCode.DUp:
                return state.DPad.Up == ButtonState.Pressed && prevState.DPad.Up == ButtonState.Released;

            case XKeyCode.DRight:
                return state.DPad.Right == ButtonState.Pressed && prevState.DPad.Right == ButtonState.Released;

            case XKeyCode.DDown:
                return state.DPad.Down == ButtonState.Pressed && prevState.DPad.Down == ButtonState.Released;

            case XKeyCode.Back:
                return state.Buttons.Back == ButtonState.Pressed && prevState.Buttons.Back == ButtonState.Released;

            case XKeyCode.Start:
                return state.Buttons.Start == ButtonState.Pressed && prevState.Buttons.Start == ButtonState.Released;

            case XKeyCode.Guide:
                return state.Buttons.Guide == ButtonState.Pressed && prevState.Buttons.Guide == ButtonState.Released;

            case XKeyCode.LeftStick:
                return state.Buttons.LeftStick == ButtonState.Pressed && prevState.Buttons.LeftStick == ButtonState.Released;

            case XKeyCode.RightStick:
                return state.Buttons.RightStick == ButtonState.Pressed && prevState.Buttons.RightStick == ButtonState.Released;

            default:
                Debug.LogWarning("No valid XKeyCode found");
                return false;
        }
    }

    public bool isKeyUp(XKeyCode key)
    {
        switch (key)
        {
            case XKeyCode.A:
                return state.Buttons.A == ButtonState.Released && prevState.Buttons.A == ButtonState.Pressed;

            case XKeyCode.B:
                return state.Buttons.B == ButtonState.Released && prevState.Buttons.B == ButtonState.Pressed;

            case XKeyCode.X:
                return state.Buttons.X == ButtonState.Released && prevState.Buttons.X == ButtonState.Pressed;

            case XKeyCode.Y:
                return state.Buttons.Y == ButtonState.Released && prevState.Buttons.Y == ButtonState.Pressed;

            case XKeyCode.LeftShoulder:
                return state.Buttons.LeftShoulder == ButtonState.Released && prevState.Buttons.LeftShoulder == ButtonState.Pressed;

            case XKeyCode.RightShoulder:
                return state.Buttons.RightShoulder == ButtonState.Released && prevState.Buttons.RightShoulder == ButtonState.Pressed;

            case XKeyCode.DLeft:
                return state.DPad.Left == ButtonState.Released && prevState.DPad.Left == ButtonState.Pressed;

            case XKeyCode.DUp:
                return state.DPad.Up == ButtonState.Released && prevState.DPad.Up == ButtonState.Pressed;

            case XKeyCode.DRight:
                return state.DPad.Right == ButtonState.Released && prevState.DPad.Right == ButtonState.Pressed;

            case XKeyCode.DDown:
                return state.DPad.Down == ButtonState.Released && prevState.DPad.Down == ButtonState.Pressed;

            case XKeyCode.Back:
                return state.Buttons.Back == ButtonState.Released && prevState.Buttons.Back == ButtonState.Pressed;

            case XKeyCode.Start:
                return state.Buttons.Start == ButtonState.Released && prevState.Buttons.Start == ButtonState.Pressed;

            case XKeyCode.Guide:
                return state.Buttons.Guide == ButtonState.Released && prevState.Buttons.Guide == ButtonState.Pressed;

            case XKeyCode.LeftStick:
                return state.Buttons.LeftStick == ButtonState.Released && prevState.Buttons.LeftStick == ButtonState.Pressed;

            case XKeyCode.RightStick:
                return state.Buttons.RightStick == ButtonState.Released && prevState.Buttons.RightStick == ButtonState.Pressed;

            default:
                Debug.LogWarning("No valid XKeyCode found");
                return false;
        }
    }
}
