using UnityEngine;
using System.Collections;

public class Respawner : MonoBehaviour 
{
	public Transform respawnLocation;
	
	private bool running = false;
	
	private void OnCollisionEnter(Collision col)
	{
		if(!running)
			StartCoroutine("Respawn", col.collider.transform);
	}
	
	private IEnumerator Respawn(Transform obj)
	{
		running = true;
		
		obj.GetComponent<PlayerInput>().animationHandler.Play("Death");
		
		while(obj.GetComponent<PlayerInput>().animationHandler.isPlaying)
			yield return null;
		
		obj.position = respawnLocation.transform.position;
		
		Hierarchy.GetComponentWithTag<PhotonView>("HealthPool").RPC("SubstractHealth", PhotonTargets.All, 1);
		running = false;
	}
	
	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "respawner", true	);
		Gizmos.color = Color.red;	
		Gizmos.DrawWireCube(transform.position, collider.bounds.size);
	}
}
