using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBrowser : PhotonGUIMemberComponent
{
	public string 		gameBrowserElem 	= "",
				  		nameElement 		= "",
				 		hostElement 		= "",
				 		lockedElemente 		= "",
				  		joinElement 		= "";
	
	public GUIStyle 	textStyle,
						noGameStyle;
	
	public string 		noRoomMessage 		= "";
	
	private GUIStyle 	sourceStyle,
					 	noGameSourceStyle;
	
	private RoomInfo 	selectedRoom;
	
	private Rect 		selectedRect;
	
	public Texture2D 	redOverlay,
					 	joinNormal,
					 	joinHover;
	
	private List<RoomInfo> roomInfos 		= new List<RoomInfo>();
	
	private Vector2 scrollPosition = Vector2.zero;
	
	private void OnGUI()
	{
		sourceStyle = guiMaster.ResolutionGUIStyle(textStyle);
		noGameSourceStyle = guiMaster.ResolutionGUIStyle(noGameStyle);
	
		if(PhotonNetwork.connectionState != ConnectionState.Disconnected)
		{
			if (PhotonNetwork.GetRoomList().Length == 0)
	        {
				GUI.Label(guiMaster.GetElementRect(gameBrowserElem), noRoomMessage, noGameSourceStyle);
			}
			else
			{
				roomInfos = new List<RoomInfo>(PhotonNetwork.GetRoomList());
				
				if(selectedRoom != null)
				{
					GUI.DrawTexture(selectedRect, redOverlay);
				}
				
				if(roomInfos.Count > 0)
				{
					Rect elementRect = guiMaster.GetElementRect(gameBrowserElem);
					
					Rect scrollArea = new Rect(0, guiMaster.GetElementRect(nameElement).y, elementRect.width, (guiMaster.GetElementRect(nameElement).height * (roomInfos.Count + 1)));
					
					scrollPosition = GUI.BeginScrollView(guiMaster.GetElementRect(gameBrowserElem), scrollPosition, scrollArea, false, false);
					
					for (int i = 0; i < roomInfos.Count; i++)
					{			
						string[] names = roomInfos[i].name.Split('&');
						
						elementRect = guiMaster.GetElementRect(nameElement);
						Rect drawRect = new Rect(elementRect.x, elementRect.y + (elementRect.height * i), elementRect.width, elementRect.height);
						GUI.Label(drawRect, names[1], sourceStyle);
						
						elementRect = guiMaster.GetElementRect(hostElement);
						drawRect = new Rect(elementRect.x, elementRect.y + (elementRect.height * i), elementRect.width, elementRect.height);
						GUI.Label(drawRect, names[0], sourceStyle);
						
						elementRect = guiMaster.GetElementRect(lockedElemente);
						drawRect = new Rect(elementRect.x, elementRect.y + (elementRect.height * i), elementRect.width, elementRect.height);
						
						if(roomInfos[i].open)
							GUI.Label(drawRect, "Open", sourceStyle);
						else
							GUI.Label(drawRect, "Locked", sourceStyle);
						
						Rect interactionRect = new Rect(guiMaster.GetElementRect(nameElement).x, drawRect.y, guiMaster.GetElementRect(gameBrowserElem).width, guiMaster.GetElementRect(nameElement).height * 1.2f);
						
						if(interactionRect.Contains(Event.current.mousePosition))
						{
							if(Event.current.type == EventType.mouseUp)
							{
								if(roomInfos[i].open)
								{
									selectedRect = interactionRect;
									selectedRoom = roomInfos[i];
								}
							}
						}
					} 
					
					GUI.EndScrollView(false);
				}
			}
			if(selectedRoom != null)
			{
				Rect rect = guiMaster.GetElementRect(joinElement);
				GUI.DrawTexture(rect, joinNormal);
				
				if(rect.Contains(Event.current.mousePosition))
				{
					GUI.DrawTexture(rect, joinHover);
					if(Event.current.type == EventType.mouseUp)
					{
						PhotonNetwork.JoinRoom(selectedRoom.name);
						GUI.DrawTexture(rect, joinHover);
						Application.LoadLevel("_GameLobby");
					}
				}
			}
		}
		else
		{
			
		}
	}
}
