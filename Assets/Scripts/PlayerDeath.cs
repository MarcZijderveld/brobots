using UnityEngine;
using System.Collections;

public class PlayerDeath : MonoBehaviour {
    PhotonView view;
    public Transform checkpoint;
	// Use this for initialization
	void Start () {
	    view = Hierarchy.GetComponentWithTag<PhotonView>("LivesManager");
	}
	
	// Update is called once per frame
	void Update () {

	}

    void Death() {
        view.RPC("modifyLives", PhotonTargets.All, -1);
        transform.position = checkpoint.position;
    }

    void OnCollisionEnter(Collision other) {
        Debug.Log("test");
        if (other.gameObject.tag == "Death") {
            
            Death();
        }

           
    }
}
