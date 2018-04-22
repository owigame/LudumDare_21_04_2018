using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRotateByOpp : MonoBehaviour {

    public RectTransform _CircleSegment;

    private void OnEnable()
    {
        GunScript.OnOppChanged += RotateSegment;
    }

    private void OnDisable()
    {
        GunScript.OnOppChanged -= RotateSegment;
    }

    void RotateSegment(Operator _opp)
    {
        switch (_opp)
        {
            case Operator.multiply:
                _CircleSegment.localEulerAngles = new Vector3(_CircleSegment.localEulerAngles.x, _CircleSegment.localEulerAngles.y, 0);
                break;
            case Operator.divide:
                _CircleSegment.localEulerAngles = new Vector3(_CircleSegment.localEulerAngles.x, _CircleSegment.localEulerAngles.y, 180);
                break;
            case Operator.minus:
                _CircleSegment.localEulerAngles = new Vector3(_CircleSegment.localEulerAngles.x, _CircleSegment.localEulerAngles.y, 90);
                break;
            case Operator.plus:
                _CircleSegment.localEulerAngles = new Vector3(_CircleSegment.localEulerAngles.x, _CircleSegment.localEulerAngles.y, -90);
                break;
            default:
                break;
        }
    }


}
