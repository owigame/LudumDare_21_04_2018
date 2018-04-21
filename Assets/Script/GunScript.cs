using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public UnityEvent<Operator, int> HitEvent;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
public enum Operator
{
plus,
minus
}