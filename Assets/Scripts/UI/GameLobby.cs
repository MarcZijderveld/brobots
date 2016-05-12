using UnityEngine;
using System.Collections;

public class GameLobby : PhotonGUIMemberComponent 
{
	public string 	 headerElement,
					 player1Element,
					 player2Element,
					 levelSelectElement,
					 lobbyElement,
		             playersElement,
					 bgElement,
					 icon1Element,
					 icon2Element;
	
	public GUIStyle	 headerStyle,
					 meStyle,
					 otherStyle,
					 playersStyle;
	
	private GUIStyle sourceHeaderStyle,
					 meSourceStyle,
					 otherSouceStyle,
					 playersSourceStyle;
	
	public Texture2D headerBG,
					 playerBG,
					 levelSelectNormal,
					 levelSelectHover,	
					 playNormal,
					 playHover,
					 disconnectNormal,
					 disconnectHover,
					 BG,
	                 icon1,
	                 icon2,
					 levelSelectText;
	
	private string	 levelString;
	
	public string	 level1String,
					 level2String;
	
	private bool 	 levelSelect =  false;
	
	public AudioClip sound;
	
	private void OnGUI()
	{
		sourceHeaderStyle = guiMaster.ResolutionGUIStyle(headerStyle);
		meSourceStyle = guiMaster.ResolutionGUIStyle(meStyle);
		otherSouceStyle = guiMaster.ResolutionGUIStyle(otherStyle);
		playersSourceStyle = guiMaster.ResolutionGUIStyle(playersStyle);
		
		GUI.DrawTexture(guiMaster.GetElementRect(headerElement), headerBG);
		
		GUI.Label(guiMaster.GetElementRect(playersElement), "Players", playersSourceStyle);
		
		if(PhotonNetwork.room != null)
		{
			string[] names = PhotonNetwork.room.name.Split('&');
			
			GUI.Label(guiMaster.GetElementRect(headerElement), names[1], sourceHeaderStyle);
			
			if(PhotonNetwork.playerList.Length > 0)
			{
				foreach(PhotonPlayer p in PhotonNetwork.playerList)
				{
					if(p.isMasterClient)
						DrawPlayer(p, player1Element);
					else
						DrawPlayer(p, player2Element);
				}	
			}
		}
		
		if(PhotonNetwork.isMasterClient)
		{
			if(PhotonNetwork.room.maxPlayers == PhotonNetwork.room.playerCount)
				PhotonNetwork.room.open = false;
			else
				PhotonNetwork.room.open = true;
			
			if(PhotonNetwork.room.maxPlayers == PhotonNetwork.room.playerCount)
			{
				GUI.DrawTexture(guiMaster.GetElementRect(levelSelectElement), levelSelectNormal);
				
				if(!levelSelect)
				{
					if(guiMaster.GetElementRect(levelSelectElement).Contains(Event.current.mousePosition))
					{
						GUI.DrawTexture(guiMaster.GetElementRect(levelSelectElement), levelSelectHover);
						
						if(Event.current.type == EventType.mouseDown)
						{
							Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
							levelSelect = true;
							return;
						}
					}
				}	
				else if(levelSelect)
				{
					GUI.DrawTexture(guiMaster.GetElementRect(bgElement), BG);
					
					GUI.DrawTexture(guiMaster.GetElementRect(headerElement), levelSelectText);
					
					//Level1
					GUI.DrawTexture(guiMaster.GetElementRect(icon1Element), icon1);
				
					if(guiMaster.GetElementRect(icon1Element).Contains(Event.current.mousePosition))
					{
						GUI.DrawTexture(guiMaster.GetElementRect(icon1Element), icon1);
						
						if(Event.current.type == EventType.mouseDown)
						{
							Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
							levelString = level1String;
						}
					}
					
					//Level2
					GUI.DrawTexture(guiMaster.GetElementRect(icon2Element), icon2);
				
					if(guiMaster.GetElementRect(icon2Element).Contains(Event.current.mousePosition))
					{
						GUI.DrawTexture(guiMaster.GetElementRect(icon2Element), icon2);
						
						if(Event.current.type == EventType.mouseDown)
						{
							Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
							levelString = level2String;
						}
					}
					
					GUI.DrawTexture(guiMaster.GetElementRect(levelSelectElement), playNormal);
				
					if(guiMaster.GetElementRect(levelSelectElement).Contains(Event.current.mousePosition))
					{
						GUI.DrawTexture(guiMaster.GetElementRect(levelSelectElement), playHover);
						
						if(Event.current.type == EventType.mouseDown)
						{
							Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
							PhotonView view = Hierarchy.GetComponentWithTag<PhotonView>("GameUtilities");
							view.RPC("LoadLevel", PhotonTargets.All, levelString);
						}
					}
				}
			}
		}
		
		GUI.DrawTexture(guiMaster.GetElementRect(lobbyElement), disconnectNormal);
		
		if(guiMaster.GetElementRect(lobbyElement).Contains(Event.current.mousePosition))
		{
			GUI.DrawTexture(guiMaster.GetElementRect(lobbyElement), disconnectHover);
			
			if(Event.current.type == EventType.mouseUp)
			{
				Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
				PhotonNetwork.LeaveRoom();
				Application.LoadLevel("_NetworkLobby");
			}
		}
	}
	
	private void DrawPlayer(PhotonPlayer player, string element)
	{
		GUI.DrawTexture(guiMaster.GetElementRect(element), playerBG);

		if(player.isMasterClient)
		{
			GUI.Label(guiMaster.GetElementRect(element), player.name, meSourceStyle);
		}
		else
		{
			GUI.Label(guiMaster.GetElementRect(element), player.name, otherSouceStyle);
		}
	}
}
