using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour {

    public int Value;

    public float speed;

    public bool imExploding;
    public GameObject ZombieMesh;

    Explosion explosion;
    NavMeshAgent _agent;
    Animator _anim;
    Rigidbody _rBody;

    private void Start () {
        _agent = GetComponent<NavMeshAgent> ();
        _rBody = GetComponent<Rigidbody> ();
        _anim = transform.GetChild (0).GetComponent<Animator> ();
        _agent.destination = Player._player.transform.position;
        _agent.speed = speed * 2;
        _anim.speed = speed * 2;

        explosion = new Explosion ();
    }

    public void Die () {
        StartCoroutine (DoDie ());
        if (Value == 0) {
            explosion.Explode (transform.position);
        }
    }

    IEnumerator DoDie () {
        gameObject.layer = 0;
        Debug.Log ("Zombie killed");
        _anim.SetFloat ("DeadIndex", Random.Range (0, 4));
        _anim.SetBool ("Dead", true);
        // _agent.isStopped = true;
        _agent.enabled = false;
        _rBody.isKinematic = true;
        // gameObject.SetActive (false);
        // yield return null;
        foreach (Collider col in ZombieMesh.GetComponents<Collider> ()) {
            col.enabled = false;
        }
        yield return new WaitForSeconds (5);

        Vector3 targetLoc = transform.position + (Vector3.down * 5);
        while ((transform.position - targetLoc).magnitude > 0.01f) {
            transform.Translate (-Vector3.up * speed * Time.deltaTime);
            Debug.Log ("In Will Loop");
            yield return null;
        }

        Destroy (gameObject); //TEMP

    }

    void Update () {
        // float step = speed * Time.deltaTime;
        // if (Player._player != null && !imExploding){
        //     transform.position = Vector3.MoveTowards(transform.position, Player._player.playerObject.transform.position, step); 
        // }
    }

    private void OnTriggerEnter (Collider other) {
        if (other.tag == "Player") {
            Destroy (gameObject);
            Scoring._scoring.TakeDamage ();
        }
    }

}