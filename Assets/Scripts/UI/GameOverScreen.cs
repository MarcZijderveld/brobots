using UnityEngine;
using System.Collections;

public class GameOverScreen : PhotonGUIMemberComponent
{
	public Texture2D image;
	public Texture2D buttonNormal,
					 buttonHover,
					 zwartePlark;
	
	public string imageElement,
				  buttonElement,
				  screenElement;
	
	public int	  depth  = 0;
	
	public AudioClip audio;
	
	public void Activate()
	{
		Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").StopAll();
		Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(audio);
		
		enabled = true;
		Time.timeScale = 0;
	}
	
	private void OnGUI()
	{
		GUI.depth = depth;
		
		GUI.DrawTexture(guiMaster.GetElementRect(screenElement), zwartePlark);	
		
		GUI.DrawTexture(guiMaster.GetElementRect(imageElement), image);
		
		GUI.DrawTexture(guiMaster.GetElementRect(buttonElement), buttonNormal);
			
		if(guiMaster.GetElementRect(buttonElement).Contains(Event.current.mousePosition))
		{
			GUI.DrawTexture(guiMaster.GetElementRect(buttonElement), buttonHover);
			if(Event.current.type == EventType.mouseUp)
			{
				PhotonView view = Hierarchy.GetComponentWithTag<PhotonView>("GameUtilities");
				view.RPC("LoadLevel", PhotonTargets.All, "_GameLobby");
			}
		}
	}
}
