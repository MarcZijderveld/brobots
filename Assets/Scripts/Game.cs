using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : Photon.MonoBehaviour 
{
	[RPC]
	public void LoadLevel(string levelName)
	{
		Debug.Log("loaded level: " + levelName);
		Time.timeScale = 1;
		PhotonNetwork.LoadLevel(levelName);
	}
	
	private void OnLevelWasLoaded()
	{
		if(PhotonNetwork.isMasterClient)
		{
			List<PhotonView> views = Hierarchy.GetComponentsWithTag<PhotonView>("PlayerSpawner");
			
			foreach(PhotonView view in views)
			{
				view.RPC("SpawnPlayer", PhotonTargets.All, PhotonNetwork.player.name);
			}
		}	
	}
}
