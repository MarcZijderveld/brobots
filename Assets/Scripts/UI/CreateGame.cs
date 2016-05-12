using UnityEngine;
using System.Collections;

public class CreateGame : GUIMemberComponent 
{
	public string 		labelFieldElement 	= "",
					  	labelTextElement 	= "",
					  	gamenameElement 	= "",
						createElement		= "",
					  	text 				= "";
	
	public GUIStyle 	labelFieldStyle,
						labelTextStyle;
	
	public int 			depth;
	
	private GUIStyle	sourceFieldStyle,
						sourceLabelStyle;
	
	private string		gameName = "";
	
	public Texture2D	normalButton,
						highlightButton;
	
	private Vector2 	scrollPos = Vector2.zero;
	
	public AudioClip	sound;
	
	private void OnGUI () 
	{
		GUI.depth = depth;
	
		sourceLabelStyle = guiMaster.ResolutionGUIStyle(labelTextStyle);
		sourceFieldStyle = guiMaster.ResolutionGUIStyle(labelFieldStyle);
		
		GUI.Label(guiMaster.GetElementRect(labelTextElement), text, sourceLabelStyle);

		gameName = GUI.TextField(guiMaster.GetElementRect(gamenameElement), gameName, 50, sourceFieldStyle);
		
		Rect rect = guiMaster.GetElementRect(createElement	);
		GUI.DrawTexture(rect, normalButton);
		
		if(rect.Contains(Event.current.mousePosition))
		{
			GUI.DrawTexture(rect, highlightButton);
			if(Event.current.type == EventType.MouseDown)
			{
				Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
				PhotonNetwork.CreateRoom(PhotonNetwork.player.name + "&" + gameName, true, true, 2); 
				Application.LoadLevel("_GameLobby");
			}
		}
	}
}
