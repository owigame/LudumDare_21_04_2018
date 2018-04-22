using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using VRTK;
using UnityEngine.UI;

public class GunScript : MonoBehaviour {

    [Header("Setup")]
    public int playerHand = 0; // 0 Left | 1 Right
    public UnityEvent<Operator, int> HitEvent;
    public Operator opp;
    public LayerMask _mask;
    public Transform pointer;
    public Animator _anim;
    private AudioSource AudioSource;
    public AudioClip clip;
    public GameObject Trail;
    private GameObjectPool TrailsPool;

    public GameObject Trails;

    public int CurrentAmmo;

    [Header("UI")]
    public Text _operatorText;

    VRTK_ControllerEvents _VRTK_ControllerEvents;

    RaycastHit hit;
    GameObject lastHit;

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
        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerEvents_ListenerExample", "VRTK_ControllerEvents", "the same"));
            return;
        }

        _VRTK_ControllerEvents = GetComponent<VRTK_ControllerEvents>();
    }

    void Start () {
        
    }

    private void OnEnable () {
        _VRTK_ControllerEvents.TouchpadAxisChanged += new ControllerInteractionEventHandler (OperatorChange);
        _VRTK_ControllerEvents.TriggerClicked += new ControllerInteractionEventHandler (Shoot);
        // _VRTK_e.TouchpadReleased += OperatorChange;
        // _VRTK_e.TriggerClicked += Shoot;
    }

    private void OnDisable () {

    }

    void Update () {

    }

    public void OperatorChange (object sender, ControllerInteractionEventArgs _args) {
        float _X = _args.touchpadAxis.x;
        float _Y = _args.touchpadAxis.y;
        if (_X > 0 && Mathf.Abs (_Y) < 0.2f) {
            opp = Operator.plus;
            _operatorText.text = "+";
        }
        if (_X < 0 && Mathf.Abs (_Y) < 0.2f) {
            opp = Operator.minus;
            _operatorText.text = "-";
        }
        if (_Y > 0 && Mathf.Abs (_X) < 0.2f) {
            opp = Operator.multiply;
            _operatorText.text = "*";
        }
        if (_Y < 0 && Mathf.Abs (_X) < 0.2f) {
            opp = Operator.divide;
            _operatorText.text = "/";
        }
        Debug.Log ("Operator Submit " + opp + (playerHand == 1 ? "Right Hand" : "Left Hand"));
    }

    public void Shoot (object sender, ControllerInteractionEventArgs _args) {
        //Shoot
        _anim.SetTrigger("Shoot");
        Debug.Log("Shoot " + (playerHand == 1 ? "Right Hand" : "Left Hand") + gameObject.name);
        if(CurrentAmmo >0)
        {
            Shoot();
        }
    }

    public void Reload()
    {
        Debug.Log("Reloading");
        CurrentAmmo = 16;
    }

}