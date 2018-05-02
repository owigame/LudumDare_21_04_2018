using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour {
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;

	// How long the object should shake for.
	public float shakeDuration = 0f;
	float _duration;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	float _shake;
	public float decreaseFactor = 1.0f;
	public float maxDistance = 5;

	Vector3 originalPos;

	public static CameraShake _CameraShake;

	void Awake () {
		_CameraShake = this;
		if (camTransform == null) {
			camTransform = transform;
		}
	}

	public void DoCameraShake (Vector3 _explodePosition, float duration = 0, bool overrideDistance = false) {
		float dist = (transform.position - _explodePosition).magnitude;
		if (dist <= maxDistance || overrideDistance) {
			_shake = overrideDistance ? shakeAmount : (maxDistance - dist) * shakeAmount;
			originalPos = camTransform.localPosition;
			if (duration == 0) duration = shakeDuration;
			_duration = duration;
		}
	}

	void Update () {
		if (_duration > 0) {
			camTransform.localPosition = originalPos + Random.insideUnitSphere * _shake;

			_duration -= Time.deltaTime * decreaseFactor;
		} else {
			_duration = 0f;
			camTransform.localPosition = originalPos;
		}
	}
}