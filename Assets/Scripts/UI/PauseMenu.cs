using UnityEngine;
using System.Collections;

public class PauseMenu : GUIMemberComponent 
{
	public string 	bgElement,
				    textElement,
					resumeElement,
					toLobbyElement,
					toMenuElement;
	
	public Texture2D zwartePlark,
					 resumeNormal,
					 resumeHover,
					 lobbyNormal,
					 lobbyHover,
					 menuNormal,
					 menuHover;
	
	public GUIStyle	 textStyle;
	
	private GUIStyle sourceTextStyle;
	
	private bool active = false;
	
	public AudioClip sound;
	
	private void Update()
	{
		if(Input.GetButtonDown("Menu"))
			active = true;
	}
	
	private void OnGUI()
	{
		if(active)
		{
			sourceTextStyle = guiMaster.ResolutionGUIStyle(textStyle);
			
			GUI.DrawTexture(guiMaster.GetElementRect(bgElement), zwartePlark);
			
			GUI.Label(guiMaster.GetElementRect(textElement), "Game Paused", sourceTextStyle);
			
			GUI.DrawTexture(guiMaster.GetElementRect(resumeElement), resumeNormal);
				
			if(guiMaster.GetElementRect(resumeElement).Contains(Event.current.mousePosition))
			{
				GUI.DrawTexture(guiMaster.GetElementRect(resumeElement), resumeHover);
				if(Event.current.type == EventType.mouseUp)
				{
					Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
					active = false;
				}
			}
			
			GUI.DrawTexture(guiMaster.GetElementRect(toLobbyElement), lobbyNormal);
				
			if(guiMaster.GetElementRect(toLobbyElement).Contains(Event.current.mousePosition))
			{
				GUI.DrawTexture(guiMaster.GetElementRect(toLobbyElement), lobbyHover);
				if(Event.current.type == EventType.mouseUp)
				{
					Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
					PhotonView view = Hierarchy.GetComponentWithTag<PhotonView>("GameUtilities");
					view.RPC("LoadLevel", PhotonTargets.All, "_GameLobby");
				}
			}
			
			GUI.DrawTexture(guiMaster.GetElementRect(toMenuElement), menuNormal);
				
			if(guiMaster.GetElementRect(toMenuElement).Contains(Event.current.mousePosition))
			{
				GUI.DrawTexture(guiMaster.GetElementRect(toMenuElement), menuHover);
				if(Event.current.type == EventType.mouseUp)
				{
					Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
					PhotonNetwork.LeaveRoom();
					Application.LoadLevel("_MainMenu");
				}
			}
		}
	}
}
