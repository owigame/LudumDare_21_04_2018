using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    //

    public float radius = 20.0F;
    public float expandRate = 1;
    public GameObject ExplosionPrefabEffect;
    GameObject _explosion;

    [Header ("Output")]
    public int explodeValue = 0;
    Operator opp;

    //ON ZOMMBIE
    public void Explode (Vector3 explosionPosition, Operator _opp) {
        opp = _opp;
        _explosion = Instantiate (ExplosionPrefabEffect, explosionPosition, Quaternion.identity);
        _explosion.GetComponent<Explosion> ().DoExplode (opp);
    }

    //ON EXPLODE PREFAB
    public void DoExplode (Operator _opp) {
        opp = _opp;
        StartCoroutine (ExpandAndConsume ());
    }

    IEnumerator ExpandAndConsume () {
        Vector3 _scale = new Vector3 (0, 0, 0);
        Material _mat = GetComponent<MeshRenderer> ().material;
        _mat.EnableKeyword ("_BumpAmt");
        _mat.SetFloat ("_BumpAmt", 20);
        while (transform.localScale.x < radius) {
            _scale += new Vector3 (Time.deltaTime * expandRate, Time.deltaTime * expandRate, Time.deltaTime * expandRate);
            transform.localScale = _scale;
            _mat.SetFloat ("_BumpAmt", 20 * (1 - (transform.localScale.x / radius)));

            yield return null;
        }
        Scoring._scoring.UpdateScore (opp, explodeValue);
        Destroy (gameObject);
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.layer == 8) {
            Zombie _zombie = other.attachedRigidbody.GetComponent<Zombie> ();
            Debug.Log ("Explosion Hit Enemy: " + _zombie.transform.name);
            explodeValue += _zombie.Value;
            _zombie.Die ();
        }
    }
}