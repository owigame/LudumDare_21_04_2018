using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using VRTK;

public class GunScript : MonoBehaviour {

    public delegate void OppEvents (Operator _opp);
    public OppEvents OnOppChanged;

    public static GunScript _GunScript;

    [Header ("Platform")]
    public Platform _platform;
    public enum Platform {
        VR,
        Desktop
    }

    [Header ("Setup")]
    public int playerHand = 0; // 0 Left | 1 Right
    public UnityEvent<Operator, int> HitEvent;
    public Operator opp;
    public LayerMask _mask;
    public Transform pointer;
    public Animator _anim;
    public Animator _animParent;
    private AudioSource _audioSource;
    public AudioClip clip;
    public GameObject Trail;
    private GameObjectPool TrailsPool;
    public GunScript otherController;
    public int ShootVibrations = 10, DurationToVibrate = 10;
    public GameObject HitParticle;
    public GameObject _plusGunObject;
    public GameObject _minusGunObject;
    public GameObject handUI;

    public GameObject _rocketPrefab;

    float StartRotGripped;
    public float rotaryTickRate = 5;

    [Header ("Output")]
    public int CurrentAmmo;
    Vector3 lastAimPoint;

    VRTK_ControllerEvents _VRTK_ControllerEvents;

    RaycastHit hit;
    GameObject lastHit;

    [Header ("Rotary")]
    public bool gripPressed = false;

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
        _animParent = transform.parent.GetComponent<Animator> ();
        _animParent.SetBool("Left", playerHand == 0);
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

    void Update () {
        if (gripPressed) {
            AdjustMultiplier ();
        }

        if (_platform == Platform.Desktop) {
            if (Input.GetMouseButtonDown (0) && playerHand == 0) {
                PullTrigger ();
            }
            if (Input.GetMouseButtonDown (1) && playerHand == 1) {
                PullTrigger ();
            }

            //Operator Change
            if (Input.GetKeyDown (KeyCode.A) && playerHand == 0) {
                //Left Operator Change
                DoOperatorChange ();
            }
            if (Input.GetKeyDown (KeyCode.D) && playerHand == 1) {
                //Right Operator Change
                DoOperatorChange ();
            }

            //Wrist Look
            if (Input.GetKeyDown (KeyCode.Q) && playerHand == 0) {
                _animParent.SetBool ("WristLook", true);
            }
            if (Input.GetKeyDown (KeyCode.E) && playerHand == 1) {
                _animParent.SetBool ("WristLook", true);
            }
        }
    }

    #endregion /Controller setup

    #region Change gun
    public void OperatorChange (object sender, ControllerInteractionEventArgs _args) {
        DoOperatorChange ();
    }
    void DoOperatorChange () {
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

        if (OnOppChanged != null) OnOppChanged (opp);
        Debug.Log ("Operator Submit " + opp + (playerHand == 1 ? "Right Hand" : "Left Hand"));
    }

    #endregion/Change gun

    #region Shoot
    private void FireBullet () {
        (opp == Operator.plus ? _plusGunObject.GetComponent<Animator> () : _minusGunObject.GetComponent<Animator> ()).SetTrigger ("Firing");
        CurrentAmmo--;
        StartCoroutine (VibateOverFrames (DurationToVibrate));
        GameObject _rocket = Instantiate (_rocketPrefab, pointer.position, Quaternion.identity);
        _rocket.transform.eulerAngles = pointer.eulerAngles;
        _rocket.GetComponent<Rocket> ().SetRocket (opp);
    }
    #endregion/Shoot

    #region Audio

    public void Shoot (object sender, ControllerInteractionEventArgs _args) {
        PullTrigger ();
    }
    public void PullTrigger () {
        //Shoot
        _anim.SetTrigger ("Shoot");
        Debug.Log ("Shoot " + (playerHand == 1 ? "Right Hand" : "Left Hand") + gameObject.name);
        if (CurrentAmmo > 0) {
            FireBullet ();
            float fPich = Random.Range (0.85f, 1f);
            _audioSource.pitch = fPich;
            switch (opp) {
                case Operator.plus:
                    _audioSource.PlayOneShot (Player._player.Clips_Plus.WeaponFire);
                    break;
                case Operator.minus:
                    _audioSource.PlayOneShot (Player._player.Clips_Minus.WeaponFire);
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
        }
    }

    #endregion /Audio

    #region Input
    void GunTrigger (object sender, ControllerInteractionEventArgs _args) {
        // Debug.Log ("Trigger: " + _args.buttonPressure);
        _anim.Play ("Trigger", -1, _args.buttonPressure);
    }

    void GripPressed (object sender, ControllerInteractionEventArgs _args) {
        gripPressed = true;
        //Get position of controller relative to watch center
        //Rotate around watch center
        // if (_CircularDriveModded != null) _CircularDriveModded.HandGripPressed ();
        Debug.Log ("Grip Pressed " + (playerHand == 0 ? "Left" : "Right"));

        Vector3 targetDir = -otherController.handUI.transform.up;
        float angle = Vector3.Angle (targetDir, transform.forward);
        StartRotGripped = angle;

    }

    void GripReleased (object sender, ControllerInteractionEventArgs _args) {
        gripPressed = false;
        // if (_CircularDriveModded != null) _CircularDriveModded.HandGripReleased ();
        //Submit multiplier
    }

    void AdjustMultiplier () {
        Vector3 targetDir = -otherController.handUI.transform.up;
        float angle = Vector3.Angle (targetDir, transform.forward);

        Debug.Log ("*** ANGLE: " + angle + " STARTROT: " + StartRotGripped);

        float CurrentRotation = transform.up.y, rotAmo;
        rotAmo = CurrentRotation - StartRotGripped;

        // if (rotAmo >= -17.5)
        // {
        //     StartRotGripped = CurrentRotation;
        //     int _multiplier = Scoring._scoring.multiplier++;
        //     //check if larger than 9 then set to 1
        //     if (Scoring._scoring.multiplier != _multiplier) 
        //     {
        //         Scoring._scoring.UpdateMultiplier (_multiplier);
        //         VRTK_ControllerHaptics.TriggerHapticPulse (VRTK_ControllerReference.GetControllerReference (otherController.gameObject), _multiplier);
        //     }
        // 
        //     if (_multiplier > 9) 
        //     {
        //         _multiplier = 1;
        //     }
        // }
        if (Mathf.Abs (angle - StartRotGripped) > rotaryTickRate) {
            int _multiplier = Mathf.Clamp ((angle > StartRotGripped) ? Scoring._scoring.multiplier - 1 : Scoring._scoring.multiplier + 1, 1, 9);
            StartRotGripped = angle;
            Debug.Log ("**** MULTI: " + _multiplier);
            //check if larger than 9 then set to 1
            if (Scoring._scoring.multiplier != _multiplier) {
                Scoring._scoring.UpdateMultiplier (_multiplier);
                VRTK_ControllerHaptics.TriggerHapticPulse (VRTK_ControllerReference.GetControllerReference (otherController.gameObject), _multiplier);
            }

            if (_multiplier > 9) {
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