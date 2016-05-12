using UnityEngine;
using System.Collections;

public class DisconnectCheck : MonoBehaviour 
{
	public void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Application.LoadLevel("_GameLobby");
		
		//Debug.Log("MOOOOO");
	}
}
