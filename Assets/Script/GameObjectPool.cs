using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class GameObjectPool
{
    GameObject BaseGameObject;
    private List<GameObject> Active,InActive;

    public GameObjectPool(GameObject baseObject)
    {
        BaseGameObject = baseObject;
        Active = new List<GameObject>();
        InActive = new List<GameObject>();
    }
    public GameObject GetObject()
    {
        if (InActive.Count>0)
        {
            var TempObj = InActive[0];
            InActive.RemoveAt(0);
            return TempObj;
        }
        else
        {
            GameObject Temp = GameObject.Instantiate(BaseGameObject);
            Active.Add(Temp);
            return Temp;
        }
    }
    public void DisableObject(GameObject gameObject)
    {
        if (Active.Contains(gameObject))
        {
            gameObject.SetActive(false);
            InActive.Add(gameObject);
            Active.Remove(gameObject);
        }
    }
}
