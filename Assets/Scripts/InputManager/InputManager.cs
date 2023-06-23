using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    [Range(0, 1)] public float frameDeltaSmoothing;

    public delegate void TouchDelegate(TouchData data);

    public event TouchDelegate TouchStartEvent;
    public event TouchDelegate TouchContinueEvent;
    public event TouchDelegate TouchEndEvent;

    [Header("Debug Info")]
    public TouchState touchState;
    public Vector3 fingerScreenPos;
    public TouchData touchData;

    private Vector3 initialTouchPos;
    private Vector3 trailingTouchPos;

    private Finger activeFinger;

    private void Awake()
    {
        instance = this;

        EnhancedTouchSupport.Enable();

        touchState = TouchState.None;
        activeFinger = null;
        touchData = default(TouchData);

        initialTouchPos = Vector3.zero;
        trailingTouchPos = Vector3.zero;
    }

    public void OnPointerDown()
    {
        if (activeFinger != null) return;

        activeFinger = Touch.activeFingers[0];
        touchState = TouchState.Start;

        fingerScreenPos = activeFinger.screenPosition;

        initialTouchPos = fingerScreenPos / Screen.width;
        trailingTouchPos = initialTouchPos;

        touchData = GenerateTouchData();
        TouchStartEvent?.Invoke(touchData);
    }

    public void OnPointerUp()
    {
        foreach (Finger finger in Touch.activeFingers)
        {
            if (finger.currentTouch.ended)
            {
                if (finger == activeFinger)
                {
                    touchData = GenerateTouchData();

                    fingerScreenPos = activeFinger.screenPosition;

                    activeFinger = null;
                    touchState = TouchState.End;

                    TouchEndEvent?.Invoke(touchData);
                    return;
                }
            }
        }
        
    }

    private void Update()
    {
        if (touchState == TouchState.Start)
        {
            touchState = TouchState.Continue;
        }

        if (touchState == TouchState.End)
        {
            touchState = TouchState.None;
            touchData = default(TouchData);
        }

        if (touchState == TouchState.Continue)
        {
            fingerScreenPos = activeFinger.screenPosition;

            touchData = GenerateTouchData();
            TouchContinueEvent?.Invoke(touchData);

            if (frameDeltaSmoothing > 0)
            {
                trailingTouchPos = Utilities.Decay(trailingTouchPos, touchData.currentPos, 1 / frameDeltaSmoothing, Time.deltaTime);
            }

            else
            {
                trailingTouchPos = touchData.currentPos;
            }
        }
    }

    TouchData GenerateTouchData()
    {
        if (activeFinger == null) return default(TouchData);

        Vector3 currentTouchPos = activeFinger.screenPosition / Screen.width;
        Vector3 absoluteDelta = currentTouchPos - initialTouchPos;
        Vector3 frameDelta = currentTouchPos - trailingTouchPos;

        TouchData data = new TouchData
        {
            initialPos = initialTouchPos,
            currentPos = currentTouchPos,
            absoluteDelta = absoluteDelta,
            frameDelta = frameDelta
        };

        return data;
    }

    [System.Serializable]
    public struct TouchData
    {
        public Vector3 initialPos; // initial finger pos
        public Vector3 currentPos; // current finger pos
        public Vector3 absoluteDelta; // delta from initial to current finger pos
        public Vector3 frameDelta; // delta from previous frame to current frame, including smoothing
    }

    public enum TouchState
    {
        Start, Continue, End, None
    }
}
