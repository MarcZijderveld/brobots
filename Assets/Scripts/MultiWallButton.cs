using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiWallButton : MonoBehaviour 
{
    public List<Transform> linkedObjects = new List<Transform>();
	
    string nameThisAnimation;
	
	private List<string> nameOtherAnimations = new List<string>();
	
	private bool   show = false,
				   inverse = false;
	
	public AudioClip clickSound;
	
     void Start() 
	{
        nameThisAnimation = gameObject.GetComponent<Animation>().clip.name;
		
		for(int i = 0; i <linkedObjects.Count; i++)
		{
			if(linkedObjects[i] != null)
        		nameOtherAnimations.Add(linkedObjects[i].gameObject.GetComponent<Animation>().clip.name);
		}
    }
    void OnTriggerEnter() 
    {		
		if(inverse == false)
		{
	        animation[nameThisAnimation].speed = 1f;
	       // animation[nameThisAnimation].time = 0;
	        transform.animation.Play();
			
			for(int i = 0; i < linkedObjects.Count; i++)
			{
				Transform linkedObject = linkedObjects[i];
				
				if(linkedObjects != null)
				{
					
					linkedObject.animation[nameOtherAnimations[i]].speed = 1f;
		       		linkedObject.animation[nameOtherAnimations[i]].time = 0;
		        	linkedObject.animation.Play();
				}
			}
			
			inverse = true;
		}
		else if(inverse == true)
		{
			animation[nameThisAnimation].speed = -1f;
	       // animation[nameThisAnimation].time = animation[nameThisAnimation].time;
	        transform.animation.Play();
			
			for(int i = 0; i < linkedObjects.Count; i++)
			{
				Transform linkedObject = linkedObjects[i];
				
				if(linkedObjects != null)
				{
					linkedObject.animation[nameOtherAnimations[i]].speed = -1f;
		       		linkedObject.animation[nameOtherAnimations[i]].time = linkedObject.animation[nameOtherAnimations[i]].length;
		        	linkedObject.animation.Play();
				}
			}
			
			inverse = false;
		}
		
		Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(clickSound);
    }
}

