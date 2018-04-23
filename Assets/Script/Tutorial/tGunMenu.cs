using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;
using VRTK;

public class tGunMenu : MonoBehaviour
{

    public Transform pointer;
    RaycastHit hit;

    public string TagName;
    public int LoadLevelNumber;

    bool OnStartEnter = false;


    private void Update()
    {
        if (Physics.Raycast(transform.position, pointer.forward, out hit, Mathf.Infinity))
        {

            if (hit.transform.tag == TagName)
            {
                OnStartEnter = true;
            }

        }
    }

    public void Shoot(object sender, ControllerInteractionEventArgs _args)
    {

        if(OnStartEnter == true)
        {
            SceneManager.LoadScene(LoadLevelNumber);
        }

    }
}
