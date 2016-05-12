using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour 
{
	public Transform 	location1,
						location2;
	
	[RPC]
	public void SpawnPlayer(string playerName)
	{
		if(PhotonNetwork.isMasterClient)
		{
			GameObject temp = PhotonNetwork.Instantiate("Player1", location1.transform.position, Quaternion.identity, 0);
			temp.GetComponentInChildren<Camera>().enabled = true;
			temp.GetComponentInChildren<AudioListener>().enabled = true;
			temp.GetComponentInChildren<USpeaker>().enabled = true;
			temp.GetComponentInChildren<DefaultTalkController>().SetShow(true);
		}
		else
		{
			GameObject temp = PhotonNetwork.Instantiate("Player2", location2.transform.position, Quaternion.identity, 0);
			temp.GetComponentInChildren<Camera>().enabled = true;
			temp.GetComponentInChildren<AudioListener>().enabled = true;
			temp.GetComponentInChildren<USpeaker>().enabled = true;
			temp.GetComponentInChildren<DefaultTalkController>().SetShow(true);
		}
	}
}
