using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour 
{
	
	public float movementSpeed = 0f,
				 jumpHeight	   = 0f;
	
	private float distanceToGround = 0f;
	public float groundedOffset = -1f;
	
	public bool inverseMovement = false;
	
	private PhotonView photonView;
	
	public Animation animationHandler;

	private Vector3 previousPos;
	
	private float idleTimer = 0.0f;
	
	public List<string> animations = new List<string>();
	
	private bool playingIdle = false,
				 playingWalk = false,
				 playingJump = false;
	
	public bool  testPlayer  = false;
	
	private void Start()
	{		
		distanceToGround = this.GetComponentInChildren<CapsuleCollider>().bounds.extents.y;
		
		if(inverseMovement)
			InverseMovement();	
		
		photonView = GetComponent<PhotonView>();
		
		previousPos = transform.position;
	}
	
	private void Update () 
	{		
		if(photonView.isMine || testPlayer)
		{
			rigidbody.velocity = (new Vector3(rigidbody.velocity.x ,rigidbody.velocity.y,Input.GetAxis("Horizontal") * movementSpeed));
			
			if(!playingIdle && !playingJump && !playingWalk)
			{
				animationHandler.Play("Idle1");
				playingIdle = true;
			}
			else if (Input.GetButtonDown("Jump") && IsGrounded())
	        {
				if(playingIdle || playingWalk)
				{
					animationHandler.Stop();
					playingIdle = false;
					playingWalk = false;
				}
				
				rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpHeight, rigidbody.velocity.z);
			
				animationHandler.Play("JumpEdited");
				playingJump = true;
			}
			if (Input.GetAxis("Horizontal") > 0)
	        {
				if(playingIdle)
				{
					animationHandler.Stop();
					playingIdle = false;
				}
	           			
				if(movementSpeed > 0)
					animationHandler.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
				else
					animationHandler.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
				
				if(!animationHandler.isPlaying)
				{
					animationHandler.Play("Walking");
					playingWalk = true;
				}
	        }
	        if (Input.GetAxis("Horizontal") < 0)
	        {
				if(playingIdle)
				{
					animationHandler.Stop();
					playingIdle = false;
				}
	         
				if(movementSpeed > 0)
					animationHandler.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
				else
					animationHandler.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
				
				if(!animationHandler.isPlaying)
				{
					animationHandler.Play("Walking");
					playingWalk = true;
				}
			}
		}
		
		float velocity = Vector3.Distance(transform.position, previousPos) / Time.deltaTime;
		previousPos = transform.position;
		
		if(velocity > 0.001)
		{
			idleTimer = 0.0f;
		}
		else
			idleTimer += Time.deltaTime;
		
		
		if(Mathf.RoundToInt(idleTimer) == 4 && !playingJump && !playingWalk)
		{
			playingIdle = true;
			
			if(!animationHandler.isPlaying)
				animationHandler.Play(animations[Random.Range(0,3)]);
			
			idleTimer = 0;
		}
    }
	
	private bool IsGrounded()
    {
//		Debug.Log(distanceToGround);
        return Physics.Raycast(transform.position, -transform.up, distanceToGround + groundedOffset);
    }

    private void OnDrawGizmos()
    {
		Gizmos.color = Color.magenta;	
        Gizmos.DrawRay(transform.position, -transform.up + new Vector3(0, distanceToGround + groundedOffset, 0));
    }
	
	public void InverseMovement()
	{
		movementSpeed *= -1;
	}
}