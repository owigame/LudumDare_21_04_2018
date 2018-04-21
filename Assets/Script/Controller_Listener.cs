using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class Controller_Listener : MonoBehaviour {

    VRTK_ControllerEvents _VRTK_ControllerEvents;

    [System.Serializable]
    public class OnControlEvents : UnityEvent<MonoBehaviour> { }
    [SerializeField]
    public OnControlEvents OnTriggerClicked;
    public OnControlEvents OnTouchPadReleased;

    private void Start () {
        if (GetComponent<VRTK_ControllerEvents> () == null) {
            VRTK_Logger.Error (VRTK_Logger.GetCommonMessage (VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerEvents_ListenerExample", "VRTK_ControllerEvents", "the same"));
            return;
        }

        _VRTK_ControllerEvents = GetComponent<VRTK_ControllerEvents> ();

        //Setup controller event listeners
        _VRTK_ControllerEvents.TriggerPressed += new ControllerInteractionEventHandler (DoTriggerPressed);
        _VRTK_ControllerEvents.TriggerReleased += new ControllerInteractionEventHandler (DoTriggerReleased);

        _VRTK_ControllerEvents.TriggerTouchStart += new ControllerInteractionEventHandler (DoTriggerTouchStart);
        _VRTK_ControllerEvents.TriggerTouchEnd += new ControllerInteractionEventHandler (DoTriggerTouchEnd);

        _VRTK_ControllerEvents.TriggerHairlineStart += new ControllerInteractionEventHandler (DoTriggerHairlineStart);
        _VRTK_ControllerEvents.TriggerHairlineEnd += new ControllerInteractionEventHandler (DoTriggerHairlineEnd);

        _VRTK_ControllerEvents.TriggerClicked += new ControllerInteractionEventHandler (DoTriggerClicked);
        _VRTK_ControllerEvents.TriggerUnclicked += new ControllerInteractionEventHandler (DoTriggerUnclicked);

        _VRTK_ControllerEvents.TriggerAxisChanged += new ControllerInteractionEventHandler (DoTriggerAxisChanged);

        _VRTK_ControllerEvents.GripPressed += new ControllerInteractionEventHandler (DoGripPressed);
        _VRTK_ControllerEvents.GripReleased += new ControllerInteractionEventHandler (DoGripReleased);

        _VRTK_ControllerEvents.GripTouchStart += new ControllerInteractionEventHandler (DoGripTouchStart);
        _VRTK_ControllerEvents.GripTouchEnd += new ControllerInteractionEventHandler (DoGripTouchEnd);

        _VRTK_ControllerEvents.GripHairlineStart += new ControllerInteractionEventHandler (DoGripHairlineStart);
        _VRTK_ControllerEvents.GripHairlineEnd += new ControllerInteractionEventHandler (DoGripHairlineEnd);

        _VRTK_ControllerEvents.GripClicked += new ControllerInteractionEventHandler (DoGripClicked);
        _VRTK_ControllerEvents.GripUnclicked += new ControllerInteractionEventHandler (DoGripUnclicked);

        _VRTK_ControllerEvents.GripAxisChanged += new ControllerInteractionEventHandler (DoGripAxisChanged);

        _VRTK_ControllerEvents.TouchpadPressed += new ControllerInteractionEventHandler (DoTouchpadPressed);
        _VRTK_ControllerEvents.TouchpadReleased += new ControllerInteractionEventHandler (DoTouchpadReleased);

        _VRTK_ControllerEvents.TouchpadTouchStart += new ControllerInteractionEventHandler (DoTouchpadTouchStart);
        _VRTK_ControllerEvents.TouchpadTouchEnd += new ControllerInteractionEventHandler (DoTouchpadTouchEnd);

        _VRTK_ControllerEvents.TouchpadAxisChanged += new ControllerInteractionEventHandler (DoTouchpadAxisChanged);

        _VRTK_ControllerEvents.ButtonOnePressed += new ControllerInteractionEventHandler (DoButtonOnePressed);
        _VRTK_ControllerEvents.ButtonOneReleased += new ControllerInteractionEventHandler (DoButtonOneReleased);

        _VRTK_ControllerEvents.ButtonOneTouchStart += new ControllerInteractionEventHandler (DoButtonOneTouchStart);
        _VRTK_ControllerEvents.ButtonOneTouchEnd += new ControllerInteractionEventHandler (DoButtonOneTouchEnd);

        _VRTK_ControllerEvents.ButtonTwoPressed += new ControllerInteractionEventHandler (DoButtonTwoPressed);
        _VRTK_ControllerEvents.ButtonTwoReleased += new ControllerInteractionEventHandler (DoButtonTwoReleased);

        _VRTK_ControllerEvents.ButtonTwoTouchStart += new ControllerInteractionEventHandler (DoButtonTwoTouchStart);
        _VRTK_ControllerEvents.ButtonTwoTouchEnd += new ControllerInteractionEventHandler (DoButtonTwoTouchEnd);

        _VRTK_ControllerEvents.StartMenuPressed += new ControllerInteractionEventHandler (DoStartMenuPressed);
        _VRTK_ControllerEvents.StartMenuReleased += new ControllerInteractionEventHandler (DoStartMenuReleased);

        _VRTK_ControllerEvents.ControllerEnabled += new ControllerInteractionEventHandler (DoControllerEnabled);
        _VRTK_ControllerEvents.ControllerDisabled += new ControllerInteractionEventHandler (DoControllerDisabled);

        _VRTK_ControllerEvents.ControllerIndexChanged += new ControllerInteractionEventHandler (DoControllerIndexChanged);
    }

    private void DebugLogger (uint index, string button, string action, ControllerInteractionEventArgs e) {
        VRTK_Logger.Info ("Controller on index '" + index + "' " + button + " has been " + action +
            " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
    }

    private void DoTriggerPressed (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "TRIGGER", "pressed", e);
    }

    private void DoTriggerReleased (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "TRIGGER", "released", e);
    }

    private void DoTriggerTouchStart (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "TRIGGER", "touched", e);
    }

    private void DoTriggerTouchEnd (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "TRIGGER", "untouched", e);
    }

    private void DoTriggerHairlineStart (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "TRIGGER", "hairline start", e);
    }

    private void DoTriggerHairlineEnd (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "TRIGGER", "hairline end", e);
    }

    private void DoTriggerClicked (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "TRIGGER", "clicked", e);
        if (OnTriggerClicked != null) OnTriggerClicked.Invoke(null);
    }

    private void DoTriggerUnclicked (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "TRIGGER", "unclicked", e);
    }

    private void DoTriggerAxisChanged (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "TRIGGER", "axis changed", e);
    }

    private void DoGripPressed (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "GRIP", "pressed", e);
    }

    private void DoGripReleased (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "GRIP", "released", e);
    }

    private void DoGripTouchStart (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "GRIP", "touched", e);
    }

    private void DoGripTouchEnd (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "GRIP", "untouched", e);
    }

    private void DoGripHairlineStart (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "GRIP", "hairline start", e);
    }

    private void DoGripHairlineEnd (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "GRIP", "hairline end", e);
    }

    private void DoGripClicked (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "GRIP", "clicked", e);
    }

    private void DoGripUnclicked (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "GRIP", "unclicked", e);
    }

    private void DoGripAxisChanged (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "GRIP", "axis changed", e);
    }

    private void DoTouchpadPressed (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "TOUCHPAD", "pressed down", e);
    }

    private void DoTouchpadReleased (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "TOUCHPAD", "released", e);
        if (OnTouchPadReleased != null) OnTouchPadReleased.Invoke(null);
    }

    private void DoTouchpadTouchStart (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "TOUCHPAD", "touched", e);
    }

    private void DoTouchpadTouchEnd (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "TOUCHPAD", "untouched", e);
    }

    private void DoTouchpadAxisChanged (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "TOUCHPAD", "axis changed", e);
    }

    private void DoButtonOnePressed (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "BUTTON ONE", "pressed down", e);
    }

    private void DoButtonOneReleased (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "BUTTON ONE", "released", e);
    }

    private void DoButtonOneTouchStart (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "BUTTON ONE", "touched", e);
    }

    private void DoButtonOneTouchEnd (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "BUTTON ONE", "untouched", e);
    }

    private void DoButtonTwoPressed (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "BUTTON TWO", "pressed down", e);
    }

    private void DoButtonTwoReleased (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "BUTTON TWO", "released", e);
    }

    private void DoButtonTwoTouchStart (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "BUTTON TWO", "touched", e);
    }

    private void DoButtonTwoTouchEnd (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "BUTTON TWO", "untouched", e);
    }

    private void DoStartMenuPressed (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "START MENU", "pressed down", e);
    }

    private void DoStartMenuReleased (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "START MENU", "released", e);
    }

    private void DoControllerEnabled (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "CONTROLLER STATE", "ENABLED", e);
    }

    private void DoControllerDisabled (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "CONTROLLER STATE", "DISABLED", e);
    }

    private void DoControllerIndexChanged (object sender, ControllerInteractionEventArgs e) {
        DebugLogger (VRTK_ControllerReference.GetRealIndex (e.controllerReference), "CONTROLLER STATE", "INDEX CHANGED", e);
    }
}