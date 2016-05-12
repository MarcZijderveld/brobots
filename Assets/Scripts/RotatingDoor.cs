using UnityEngine;
using System.Collections;

public class RotatingDoor : MonoBehaviour {
    private Transform player1,
                      player2;
    private float     rotation,
                      smooth = 50;
    private bool      rotating = false,
                      rotated = true;
	
	public AudioClip  sound;
	
	private bool 	  pressed = false;
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.E) && !rotating && player1 && player2 && !pressed) 
		{
			pressed = true;
			
			PhotonView view = gameObject.GetComponent<PhotonView>();
			view.RPC("StartRevolve", PhotonTargets.All, PhotonNetwork.player.name);
			
			Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
        }
	}
	
	[RPC]
	public void StartRevolve(string name)
	{
		Vector3 p1Pos = player1.transform.position,
				p2Pos = player2.transform.position;
		
		Vector3 p1Cam = player1.GetComponentInChildren<Camera>().gameObject.transform.position,
				p2Cam = player2.GetComponentInChildren<Camera>().gameObject.transform.position;
		
		Quaternion p1CamRot = player1.GetComponentInChildren<Camera>().gameObject.transform.rotation,
				   p2CamRot = player2.GetComponentInChildren<Camera>().gameObject.transform.rotation;
		
        player1.transform.position = p2Pos;
		player2.transform.position = p1Pos;
		
		player1.GetComponent<PlayerInput>().InverseMovement();
		player2.GetComponent<PlayerInput>().InverseMovement();
		
		player1.GetComponentInChildren<Camera>().transform.position = p2Cam;
		player1.GetComponentInChildren<Camera>().transform.rotation = p2CamRot;
		
		player2.GetComponentInChildren<Camera>().transform.position = p1Cam;
		player2.GetComponentInChildren<Camera>().transform.rotation = p1CamRot;
		
	}
	

    void OnTriggerEnter(Collider other) 
	{
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player1") 
            player1 = other.transform;
        if (other.gameObject.tag == "Player2")
            player2 = other.transform	;
    }

    void OnTriggerExit(Collider other) 
	{
        if (other.gameObject.tag == "Player1")
            player1 = null;
        if (other.gameObject.tag == "Player2")
            player2 = null;
    }
}
