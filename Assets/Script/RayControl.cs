using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayControl : MonoBehaviour {

    public IEnumerator FireRay(float FrameDuration,Vector3 start,Vector3 destination)
    {
        TrailRenderer trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
        for (int i = 0; i < FrameDuration; i++)
        {
            if (trail.enabled == false)
            {
                trail.enabled = true;
            }
            transform.position = Vector3.Lerp(start,destination,i);
            yield return new WaitForEndOfFrame();

            

        }
    }
}
