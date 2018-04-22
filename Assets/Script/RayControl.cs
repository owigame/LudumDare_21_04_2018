using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayControl : MonoBehaviour {

    IEnumerator FireRay(float FrameDuration,Vector3 start,Vector3 destination)
    {
        for (int i = 0; i < FrameDuration; i++)
        {
            transform.position = Vector3.Lerp(start,destination,i);
            yield return new WaitForEndOfFrame();
        }
    }
}
