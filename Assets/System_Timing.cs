using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_Timing : MonoBehaviour {

    private AC.LSky.LSkyTOD lSkyTOD;

    private void Start()
    {
        lSkyTOD = GetComponent<AC.LSky.LSkyTOD>();
        lSkyTOD.TimeReached.AddListener(TimeOut);
    }

    public void TimeOut()
    {

    }

    public IEnumerator ResetTime(float FrameDuration)
    {
        float tempval = lSkyTOD.timeline;
        for (int i = 0; i < FrameDuration; i++)
        {
            lSkyTOD.timeline = Mathf.Lerp(0, tempval, FrameDuration / i);
            yield return new WaitForEndOfFrame();
        }
    }
}
