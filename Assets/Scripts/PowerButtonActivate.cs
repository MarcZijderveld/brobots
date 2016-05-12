using UnityEngine;
using System.Collections;

public class PowerButtonActivate : MonoBehaviour 
{
    public Transform linkedObject;
	
    string nameThisAnimation,
           nameOtherAnimation;
	
	private bool   show = false,
				   inverse = false;
	
	public AudioClip clickSound;
	
    void Start() 
	{
        nameThisAnimation = gameObject.GetComponent<Animation>().clip.name;
		
		if(linkedObject != null)
         	nameOtherAnimation = linkedObject.gameObject.GetComponent<Animation>().clip.name;
    }
    void OnTriggerEnter() 
    {		
		if(inverse == false)
		{
	        animation[nameThisAnimation].speed = 1f;
	      //  animation[nameThisAnimation].time = 0;
	        transform.animation.Play();
			
			if(linkedObject != null)
			{
		        linkedObject.animation[nameOtherAnimation].speed = 1f;
		       	linkedObject.animation[nameOtherAnimation].time = 0;
		        linkedObject.animation.Play();
			}
			
			inverse = true;
		}
		else if(inverse == true)
		{
			animation[nameThisAnimation].speed = -1f;
	       //animation[nameThisAnimation].time = animation[nameThisAnimation].time;
	        transform.animation.Play();
			
			if(linkedObject != null)
			{
		        linkedObject.animation[nameOtherAnimation].speed = -1f;
		        linkedObject.animation[nameOtherAnimation].time = linkedObject.animation[nameOtherAnimation].length;	
		        linkedObject.animation.Play();
			}
			
			inverse = false;
		}
		
		Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(clickSound);
    }
}
