using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForcedAspect : MonoBehaviour {

	[Header("UI Element to reference")]
	public RectTransform _rectTransform;

	[Header("Explicit Aspect")]
	public float aspectWidth;
	public float aspectHeight;

	[Header("Result")]
	public float aspectRatio;

	void Start () {
		UpdateCameraAspect();
	}
	
	// void OnEnable () {
	// 	ScreenSizeChange.OnScreenSizeChanged += UpdateCameraAspect;
	// }

	// void OnDisable () {
	// 	ScreenSizeChange.OnScreenSizeChanged -= UpdateCameraAspect;
	// }

	public void UpdateCameraAspect(){
		if (_rectTransform){
			aspectRatio = _rectTransform.rect.width/_rectTransform.rect.height;
			GetComponent<Camera>().aspect = aspectRatio;
		} else {
			aspectRatio = aspectWidth/aspectHeight;
			GetComponent<Camera>().aspect = aspectRatio;
		}
	}
}
