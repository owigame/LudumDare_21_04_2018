using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

	// public Transform _lookXOverride;
	// public Transform _lookYOverride;
	// Transform lookXTarget;
	// Transform lookYTarget;
	// public Vector2 lookXMinMax;
	// public float lookXRate = 1;
	// public float lookYRate = 1;
	// Vector2 lastMousePosition;

	// void Start () {
	// 	lookXTarget = _lookXOverride == null ? transform : _lookXOverride;
	// 	lookYTarget = _lookYOverride == null ? transform : _lookYOverride;
	// 	lastMousePosition = Input.mousePosition;
	// }

	// void Update () {
	// 	Vector2 mouseInput = (Vector2)Input.mousePosition - lastMousePosition;

	// 	if (mouseInput.x != 0 || mouseInput.y != 0) {
	// 		lookXTarget.Rotate (new Vector3 (-mouseInput.y * Time.deltaTime * lookXRate, 0, 0));
	// 		// lookXTarget.localEulerAngles = new Vector3 (Mathf.Clamp (lookXTarget.localEulerAngles.x, lookXMinMax.x, lookXMinMax.y), 0, 0);
	// 		lookYTarget.Rotate (new Vector3 (0, mouseInput.x * Time.deltaTime * lookYRate, 0));
	// 	}

	// 	lastMousePosition = Input.mousePosition;
	// }

	//From http://wiki.unity3d.com/index.php/SmoothMouseLook

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public Transform _lookXOverride;
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationX = 0F;
	float rotationY = 0F;

	private List<float> rotArrayX = new List<float> ();
	float rotAverageX = 0F;

	private List<float> rotArrayY = new List<float> ();
	float rotAverageY = 0F;

	public float frameCounter = 20;

	Quaternion originalRotation;
	Quaternion originalRotationX;

	[Header ("Aim")]
	public LayerMask _mask;
	public Transform[] aimObjects;
	public Transform headCamera;
	public Transform fallbackAim;
	public Vector3 lastHitPoint;

	public bool hideMouse = true;

	void Start () {
		if (_lookXOverride == null) {
			_lookXOverride = transform;
		}
		Rigidbody rb = GetComponent<Rigidbody> ();
		if (rb)
			rb.freezeRotation = true;
		originalRotation = transform.localRotation;
		originalRotationX = _lookXOverride.localRotation;

		if (hideMouse){
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = false;
		}
	}

	void Update () {
		if (axes == RotationAxes.MouseXAndY) {
			rotAverageY = 0f;
			rotAverageX = 0f;

			rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
			rotationX += Input.GetAxis ("Mouse X") * sensitivityX;

			rotArrayY.Add (rotationY);
			rotArrayX.Add (rotationX);

			if (rotArrayY.Count >= frameCounter) {
				rotArrayY.RemoveAt (0);
			}
			if (rotArrayX.Count >= frameCounter) {
				rotArrayX.RemoveAt (0);
			}

			for (int j = 0; j < rotArrayY.Count; j++) {
				rotAverageY += rotArrayY[j];
			}
			for (int i = 0; i < rotArrayX.Count; i++) {
				rotAverageX += rotArrayX[i];
			}

			rotAverageY /= rotArrayY.Count;
			rotAverageX /= rotArrayX.Count;

			rotAverageY = ClampAngle (rotAverageY, minimumY, maximumY);
			rotAverageX = ClampAngle (rotAverageX, minimumX, maximumX);

			Quaternion yQuaternion = Quaternion.AngleAxis (rotAverageY, Vector3.left);
			Quaternion xQuaternion = Quaternion.AngleAxis (rotAverageX, Vector3.up);

			transform.localRotation = originalRotation * xQuaternion;
			_lookXOverride.localRotation = originalRotationX * yQuaternion;
		}
	}

	void LateUpdate () {
		foreach (Transform gun in aimObjects) {
			gun.LookAt (fallbackAim, Vector3.up);
		}
	}

	public static float ClampAngle (float angle, float min, float max) {
		angle = angle % 360;
		if ((angle >= -360F) && (angle <= 360F)) {
			if (angle < -360F) {
				angle += 360F;
			}
			if (angle > 360F) {
				angle -= 360F;
			}
		}
		return Mathf.Clamp (angle, min, max);
	}

	//Raycast for aiming guns
	void FixedUpdate () {
		RaycastHit _hit;
		if (Physics.Raycast (headCamera.position, headCamera.forward, out _hit, 200, _mask)) {
			fallbackAim.position = _hit.point;
		} else {
			fallbackAim.localPosition = Vector3.zero;
		}
	}

}