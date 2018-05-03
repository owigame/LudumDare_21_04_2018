using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    //

    public float radius = 20.0F;
    public float expandRate = 1;
    public GameObject ExplosionPrefabEffect;
    GameObject _explosion;
    public GameObject _numberPopUpPrefab;

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
        CameraShake._CameraShake.DoCameraShake (transform.position, 2, true);
        while (transform.localScale.x < radius) {
            _scale += new Vector3 (Time.deltaTime * expandRate, Time.deltaTime * expandRate, Time.deltaTime * expandRate);
            transform.localScale = _scale;
            _mat.SetFloat ("_BumpAmt", 20 * (1 - (transform.localScale.x / radius)));

            yield return null;
        }
        Debug.Log("### Explode Value: " + explodeValue);
        if (Scoring._scoring != null && explodeValue > 0) {
            Scoring._scoring.UpdateScore (opp, explodeValue);
            GameObject _number = Instantiate (_numberPopUpPrefab, transform.position, Quaternion.identity);
            _number.transform.LookAt (Scoring._scoring.transform);
            _number.transform.localEulerAngles = new Vector3 (0, _number.transform.localEulerAngles.y, 0);
            _number.GetComponent<UIPopUpNumber> ().SetNumber (explodeValue * Scoring._scoring.multiplier, opp);
        }
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