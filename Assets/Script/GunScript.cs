using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using VRTK;

public class GunScript : MonoBehaviour {

    public delegate void OppEvents (Operator _opp);
    public OppEvents OnOppChanged;

    public static GunScript _GunScript;

    [Header ("Setup")]
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
    public GameObject otherController;
    public int ShootVibrations = 10, DurationToVibrate = 10;
    public GameObject HitParticle;
    public GameObject _plusGunObject;
    public GameObject _minusGunObject;

    public GameObject BulletPlus, BulletMinus;


    float StartRotGripped;

    [Header ("Output")]
    public int CurrentAmmo;

    [Header ("UI")]
    public Text _operatorText;

    VRTK_ControllerEvents _VRTK_ControllerEvents;

    RaycastHit hit;
    GameObject lastHit;

    [Header ("Rotary")]
    public CircularDriveModded _CircularDriveModded;
    public bool gripPressed = false;
    public LinearMapping _linearMapping;
    public float rotaryValue;

    #region Controller setup
    private void Awake () {
        _GunScript = this;
        _audioSource = GetComponent<AudioSource> ();
        if (GetComponent<VRTK_ControllerEvents> () == null) {
            VRTK_Logger.Error (VRTK_Logger.GetCommonMessage (VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerEvents_ListenerExample", "VRTK_ControllerEvents", "the same"));
            return;
        }

        _VRTK_ControllerEvents = GetComponent<VRTK_ControllerEvents> ();
    }

    void Start () {
        if (OnOppChanged != null) OnOppChanged (opp);
        TrailsPool = new GameObjectPool (Trail);
    }

    private void OnEnable () {
        _VRTK_ControllerEvents.TouchpadPressed += new ControllerInteractionEventHandler (OperatorChange);
        _VRTK_ControllerEvents.TriggerClicked += new ControllerInteractionEventHandler (Shoot);
        _VRTK_ControllerEvents.TriggerAxisChanged += new ControllerInteractionEventHandler (GunTrigger);
        _VRTK_ControllerEvents.GripPressed += new ControllerInteractionEventHandler (GripPressed);
        _VRTK_ControllerEvents.GripReleased += new ControllerInteractionEventHandler (GripReleased);
    }

    private void OnDisable () {
        _VRTK_ControllerEvents.TouchpadPressed -= new ControllerInteractionEventHandler (OperatorChange);
        _VRTK_ControllerEvents.TriggerClicked -= new ControllerInteractionEventHandler (Shoot);
        _VRTK_ControllerEvents.TriggerAxisChanged -= new ControllerInteractionEventHandler (GunTrigger);
        _VRTK_ControllerEvents.GripPressed -= new ControllerInteractionEventHandler (GripPressed);
        _VRTK_ControllerEvents.GripReleased -= new ControllerInteractionEventHandler (GripReleased);
    }

    void Update()
    {
        if (gripPressed && _CircularDriveModded != null)
        {
            _CircularDriveModded.HandGripPressed();
            rotaryValue = _linearMapping.value;
            int _multiplier = Mathf.Clamp(Mathf.FloorToInt(rotaryValue * 10), 1, 9);
            if (Scoring._scoring.multiplier != _multiplier)
            {
                Scoring._scoring.UpdateMultiplier(_multiplier);
                VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(otherController), _multiplier);
            }

            if(gripPressed == true)
            {
                AdjustMultiplier();
            }

        }
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
    #endregion /Controller setup

    #region Shoot
    private void Shoot () {
        (opp == Operator.plus ? _plusGunObject.GetComponent<Animator> () : _minusGunObject.GetComponent<Animator> ()).SetTrigger("Firing");
        if (Physics.Raycast (transform.position, pointer.forward, out hit, Mathf.Infinity, _mask)) {
            Debug.DrawLine (transform.position, hit.transform.position, Color.green, 10);

            if (hit.transform.tag == "Enemy" && lastHit != hit.transform.gameObject) {
                lastHit = hit.transform.gameObject;
                Zombie _zombie = hit.transform.GetComponent<Zombie> ();
                Scoring._scoring.UpdateScore (opp, _zombie.Value);
                _zombie.Die ();
                if (HitParticle != null) Instantiate (HitParticle, hit.transform.position, Quaternion.identity);
                StartCoroutine (BulletTrail (hit.point));

                switch (opp)
                {
                    case Operator.plus:
                        Instantiate(BulletPlus, transform.position, transform.rotation);
                        break;
                    case Operator.minus:
                        Instantiate(BulletMinus, transform.position, transform.rotation);
                        break;
                    
                }
            }
        } else {
            Debug.DrawLine (transform.position, transform.position + transform.forward * 10, Color.red, 10);
            StartCoroutine (BulletTrail (pointer.position + pointer.forward * 100));
        }
        CurrentAmmo--;
        StartCoroutine (VibateOverFrames (DurationToVibrate));
    }
    #endregion/Shoot

    #region Change gun
    public void OperatorChange (object sender, ControllerInteractionEventArgs _args) {
        if (opp == Operator.minus) {
            VRTK_ControllerHaptics.TriggerHapticPulse (VRTK_ControllerReference.GetControllerReference (gameObject), 0.1f);
            opp = Operator.plus;
            _plusGunObject.SetActive (true);
            _minusGunObject.SetActive (false);
            //switch gun model
        } else if (opp == Operator.plus) {
            VRTK_ControllerHaptics.TriggerHapticPulse (VRTK_ControllerReference.GetControllerReference (gameObject), 0.1f);
            opp = Operator.minus;
            _plusGunObject.SetActive (false);
            _minusGunObject.SetActive (true);
        }

        // float _X = _args.touchpadAxis.x;
        // float _Y = _args.touchpadAxis.y;
        // if (_X > 0 /*&& Mathf.Abs (_Y) < 0.3f*/ ) {
        //     if (opp == Operator.minus) {
        //         VRTK_ControllerHaptics.TriggerHapticPulse (VRTK_ControllerReference.GetControllerReference (gameObject), 0.1f);
        //     }
        //     opp = Operator.plus;
        //     _operatorText.text = "+";
        // }
        // if (_X < 0 /*&& Mathf.Abs (_Y) < 0.3f*/ ) {
        //     if (opp == Operator.plus) {
        //         VRTK_ControllerHaptics.TriggerHapticPulse (VRTK_ControllerReference.GetControllerReference (gameObject), 0.1f);
        //     }
        //     opp = Operator.minus;
        //     _operatorText.text = "-";
        // }
        //if (_Y > 0 && Mathf.Abs (_X) < 0.3f) {
        //    opp = Operator.multiply;
        //    _operatorText.text = "*";
        //}
        //if (_Y < 0 && Mathf.Abs (_X) < 0.3f) {
        //    opp = Operator.divide;
        //    _operatorText.text = "/";
        //}
        if (OnOppChanged != null) OnOppChanged (opp);
        Debug.Log ("Operator Submit " + opp + (playerHand == 1 ? "Right Hand" : "Left Hand"));
    }
    #endregion/Change gun

    #region Audio

    public void Shoot (object sender, ControllerInteractionEventArgs _args) {
        //Shoot
        _anim.SetTrigger ("Shoot");
        Debug.Log ("Shoot " + (playerHand == 1 ? "Right Hand" : "Left Hand") + gameObject.name);
        if (CurrentAmmo > 0) 
        {
            Shoot ();
            //_audioSource.PlayOneShot(clip);
            float fPich = Random.Range (0.85f, 1f);
            _audioSource.pitch = fPich;
            switch (opp) {
                case Operator.plus:
                    _audioSource.PlayOneShot (Player._player.Clips_Plus.WeaponFire);
                    break;
                case Operator.minus:
                    _audioSource.PlayOneShot (Player._player.Clips_Minus.WeaponFire);
                    break;
                case Operator.multiply:
                    _audioSource.PlayOneShot (Player._player.Clips_Mul.WeaponFire);
                    break;
                case Operator.divide:
                    _audioSource.PlayOneShot (Player._player.Clips_Div.WeaponFire);
                    break;
                default:
                    break;
            }
        } else {
            switch (opp) {
                case Operator.plus:
                    _audioSource.PlayOneShot (Player._player.Clips_Plus.ClipEmpty);
                    break;
                case Operator.minus:
                    _audioSource.PlayOneShot (Player._player.Clips_Minus.ClipEmpty);
                    break;
                case Operator.multiply:
                    _audioSource.PlayOneShot (Player._player.Clips_Mul.ClipEmpty);
                    break;
                case Operator.divide:
                    _audioSource.PlayOneShot (Player._player.Clips_Div.ClipEmpty);
                    break;
                default:
                    break;
            }
        }
    }

    public void Reload () {
        Debug.Log ("Reloading");
        CurrentAmmo = 16;
        float fPich = Random.Range (0.85f, 1f);
        _audioSource.pitch = fPich;
        switch (opp) {
            case Operator.plus:
                _audioSource.PlayOneShot (Player._player.Clips_Plus.Reload);
                break;
            case Operator.minus:
                _audioSource.PlayOneShot (Player._player.Clips_Minus.Reload);
                break;
            case Operator.multiply:
                _audioSource.PlayOneShot (Player._player.Clips_Mul.Reload);
                break;
            case Operator.divide:
                _audioSource.PlayOneShot (Player._player.Clips_Div.Reload);
                break;
            default:
                break;
        }
    }

    #endregion /Audio

    #region Input
    void GunTrigger (object sender, ControllerInteractionEventArgs _args) {
        Debug.Log ("Trigger: " + _args.buttonPressure);
        _anim.Play ("Trigger", -1, _args.buttonPressure);
    }

    void GripPressed (object sender, ControllerInteractionEventArgs _args) {
        gripPressed = true;
        //Get position of controller relative to watch center
        //Rotate around watch center
        if (_CircularDriveModded != null) _CircularDriveModded.HandGripPressed ();
        Debug.Log ("Grip Pressed " + (playerHand == 0 ? "Left" : "Right"));

        StartRotGripped = transform.forward.y;

    }

    void GripReleased (object sender, ControllerInteractionEventArgs _args) {
        gripPressed = false;
        if (_CircularDriveModded != null) _CircularDriveModded.HandGripReleased ();
        //Submit multiplier
    }

    void AdjustMultiplier()
    {
        float CurrentRotation = transform.forward.y,rotAmo;
        rotAmo = CurrentRotation - StartRotGripped;

        if(rotAmo >= -17.5)
        {
            StartRotGripped = CurrentRotation;
            int _multiplier = Scoring._scoring.multiplier++;
            //check if larger than 9 then set to 1
            if (Scoring._scoring.multiplier != _multiplier)
            {
                Scoring._scoring.UpdateMultiplier(_multiplier);
                VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(otherController), _multiplier);
            }

            if(_multiplier > 9)
            {
                _multiplier = 1;
            }
        }
        if (rotAmo <= 17.5)
        {
            StartRotGripped = CurrentRotation;
            int _multiplier = Scoring._scoring.multiplier--;
            //check if larger than 9 then set to 1
            if (Scoring._scoring.multiplier != _multiplier)
            {
                Scoring._scoring.UpdateMultiplier(_multiplier);
                VRTK_ControllerHaptics.TriggerHapticPulse(VRTK_ControllerReference.GetControllerReference(otherController), _multiplier);
            }

            if (_multiplier > 9)
            {
                _multiplier = 1;
            }
        }


    }

    #endregion /Input

    #region Trail
    IEnumerator BulletTrail (Vector3 _Dest) {
        Vector3 _Origin = pointer.position;

        GameObject trail = Instantiate (Trail, _Origin, Quaternion.identity) as GameObject;
        trail.transform.position = _Origin;

        yield return 0;
        trail.transform.position = Vector3.Lerp (_Origin, _Dest, 0);
        yield return 0;
        trail.transform.position = Vector3.Lerp (_Origin, _Dest, 0.25f);
        yield return 0;
        trail.transform.position = Vector3.Lerp (_Origin, _Dest, 0.5f);
        yield return 0;
        trail.transform.position = Vector3.Lerp (_Origin, _Dest, 0.75f);
        yield return 0;
        trail.transform.position = Vector3.Lerp (_Origin, _Dest, 1);

        yield return new WaitForSeconds (0.0125f);
        Destroy (trail);
    }
    IEnumerator VibateOverFrames (int FrameDuration) {
        for (int i = 0; i < FrameDuration; i++) {
            VRTK_ControllerHaptics.TriggerHapticPulse (VRTK_ControllerReference.GetControllerReference (gameObject), ShootVibrations);
            yield return new WaitForEndOfFrame ();
        }
    }
    #endregion /Trail
}