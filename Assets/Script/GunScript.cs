using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using VRTK;
using UnityEngine.UI;

public class GunScript : MonoBehaviour {

    public delegate void OppEvents(Operator _opp);
    public OppEvents OnOppChanged;

    public static GunScript _GunScript;

    [Header("Setup")]
    public int playerHand = 0; // 0 Left | 1 Right
    public UnityEvent<Operator, int> HitEvent;
    public Operator opp;
    public LayerMask _mask;
    public Transform pointer;
    public Animator _anim;
    private AudioSource _audioSource;
    public AudioClip clip;
    public GameObject Trail;
    private GameObjectPool TrailsPool;


    public int CurrentAmmo;

    [Header("UI")]
    public Text _operatorText;

    VRTK_ControllerEvents _VRTK_ControllerEvents;

    RaycastHit hit;
    GameObject lastHit;

    [Header("Rotary")]
    public CircularDriveModded _CircularDriveModded;

    private void Shoot () {
        if (Physics.Raycast (transform.position, pointer.forward, out hit, Mathf.Infinity, _mask)) {
            Debug.DrawLine (transform.position, hit.transform.position, Color.green, 10);
            if (hit.transform.tag == "Enemy" && lastHit != hit.transform.gameObject) {
                lastHit = hit.transform.gameObject;
                Zombie _zombie = hit.transform.GetComponent<Zombie>();
                Scoring._scoring.UpdateScore(opp, _zombie.Value);
                _zombie.Die();
                StartCoroutine(TrailsPool.GetObject().GetComponent<RayControl>().FireRay(15, pointer.position, hit.transform.position));
            }
        } else {
            Debug.DrawLine (transform.position, transform.position + transform.forward * 10, Color.red, 10);
        }
        CurrentAmmo--;
    }



    private void Awake () {
        _GunScript = this;
        _audioSource = GetComponent<AudioSource>();
        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerEvents_ListenerExample", "VRTK_ControllerEvents", "the same"));
            return;
        }

        _VRTK_ControllerEvents = GetComponent<VRTK_ControllerEvents>();
    }

    void Start () {
        if (OnOppChanged != null) OnOppChanged(opp);
    }

    private void OnEnable () {
        _VRTK_ControllerEvents.TouchpadAxisChanged += new ControllerInteractionEventHandler (OperatorChange);
        _VRTK_ControllerEvents.TriggerClicked += new ControllerInteractionEventHandler (Shoot);
        _VRTK_ControllerEvents.TriggerAxisChanged += new ControllerInteractionEventHandler (GunTrigger);
        _VRTK_ControllerEvents.GripPressed += new ControllerInteractionEventHandler (GripPressed);
        _VRTK_ControllerEvents.GripReleased += new ControllerInteractionEventHandler (GripReleased);
    }

    private void OnDisable () {
        _VRTK_ControllerEvents.TouchpadAxisChanged -= new ControllerInteractionEventHandler(OperatorChange);
        _VRTK_ControllerEvents.TriggerClicked -= new ControllerInteractionEventHandler(Shoot);
        _VRTK_ControllerEvents.TriggerAxisChanged -= new ControllerInteractionEventHandler (GunTrigger);
        _VRTK_ControllerEvents.GripPressed -= new ControllerInteractionEventHandler (GripPressed);
        _VRTK_ControllerEvents.GripReleased -= new ControllerInteractionEventHandler (GripReleased);
    }

    void Update () {

    }

    public void OperatorChange (object sender, ControllerInteractionEventArgs _args) {
        float _X = _args.touchpadAxis.x;
        float _Y = _args.touchpadAxis.y;
        if (_X > 0 && Mathf.Abs (_Y) < 0.3f) {
            opp = Operator.plus;
            _operatorText.text = "+";
        }
        if (_X < 0 && Mathf.Abs (_Y) < 0.3f) {
            opp = Operator.minus;
            _operatorText.text = "-";
        }
        if (_Y > 0 && Mathf.Abs (_X) < 0.3f) {
            opp = Operator.multiply;
            _operatorText.text = "*";
        }
        if (_Y < 0 && Mathf.Abs (_X) < 0.3f) {
            opp = Operator.divide;
            _operatorText.text = "/";
        }
        if (OnOppChanged != null) OnOppChanged(opp);
        Debug.Log ("Operator Submit " + opp + (playerHand == 1 ? "Right Hand" : "Left Hand"));
    }

    public void Shoot (object sender, ControllerInteractionEventArgs _args) {
        //Shoot
        _anim.SetTrigger("Shoot");
        Debug.Log("Shoot " + (playerHand == 1 ? "Right Hand" : "Left Hand") + gameObject.name);
        if(CurrentAmmo >0)
        {
            Shoot();
            //_audioSource.PlayOneShot(clip);
            switch (opp)
            {
                case Operator.plus:
                    _audioSource.PlayOneShot(Player._player.Clips_Plus.WeaponFire);
                    break;
                case Operator.minus:
                    _audioSource.PlayOneShot(Player._player.Clips_Minus.WeaponFire);
                    break;
                case Operator.multiply:
                    _audioSource.PlayOneShot(Player._player.Clips_Mul.WeaponFire);
                    break;
                case Operator.divide:
                    _audioSource.PlayOneShot(Player._player.Clips_Div.WeaponFire);
                    break;
                default:
                    break;
            }
        } else
        {
            switch (opp)
            {
                case Operator.plus:
                    _audioSource.PlayOneShot(Player._player.Clips_Plus.ClipEmpty);
                    break;
                case Operator.minus:
                    _audioSource.PlayOneShot(Player._player.Clips_Minus.ClipEmpty);
                    break;
                case Operator.multiply:
                    _audioSource.PlayOneShot(Player._player.Clips_Mul.ClipEmpty);
                    break;
                case Operator.divide:
                    _audioSource.PlayOneShot(Player._player.Clips_Div.ClipEmpty);
                    break;
                default:
                    break;
            }
        }
    }

    public void Reload()
    {
        Debug.Log("Reloading");
        CurrentAmmo = 16;
        switch (opp)
        {
            case Operator.plus:
                _audioSource.PlayOneShot(Player._player.Clips_Plus.Reload);
                break;
            case Operator.minus:
                _audioSource.PlayOneShot(Player._player.Clips_Minus.Reload);
                break;
            case Operator.multiply:
                _audioSource.PlayOneShot(Player._player.Clips_Mul.Reload);
                break;
            case Operator.divide:
                _audioSource.PlayOneShot(Player._player.Clips_Div.Reload);
                break;
            default:
                break;
        }
    }

    void GunTrigger(object sender, ControllerInteractionEventArgs _args)
    {
        Debug.Log("Trigger: " + _args.buttonPressure);
        _anim.Play("Trigger", -1, _args.buttonPressure);
    }

    void GripPressed(object sender, ControllerInteractionEventArgs _args)
    {
        //Get position of controller relative to watch center
        //Rotate around watch center

    }

    void GripReleased(object sender, ControllerInteractionEventArgs _args)
    {
        //Submit multiplier
    }
}