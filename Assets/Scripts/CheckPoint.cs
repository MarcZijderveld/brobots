using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour 
{
    void OnTriggerEnter(Collider other) 
	{
        if (other.gameObject.tag == "Player1" || other.gameObject.tag == "Player2")
            other.transform.parent.GetComponent<PlayerDeath>().checkpoint = transform;
    }
}
