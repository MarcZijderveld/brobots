using UnityEngine;
using System.Collections;

public class TriggerDoor : MonoBehaviour 
{
	public string tag;
	
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == tag)
		{
			animation.Play();
		}
	}
}
