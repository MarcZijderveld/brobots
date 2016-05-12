using UnityEngine;
using System.Collections;

public class NameHUD : PhotonGUIMemberComponent 
{
	public string 	bgElement,
				  	textElement;
	
	public Texture2D player1BG,
					 player2BG;
	
	public GUIStyle  textStyle;
	
	private GUIStyle sourceTextStyle;
	
	private void OnGUI()
	{
		sourceTextStyle = guiMaster.ResolutionGUIStyle(textStyle);
		
		if(PhotonNetwork.isMasterClient)
		{
			GUI.DrawTexture(guiMaster.GetElementRect(bgElement), player1BG);
			GUI.Label(guiMaster.GetElementRect(textElement),PhotonNetwork.player.name, sourceTextStyle);
		}
		else
		{
			GUI.DrawTexture(guiMaster.GetElementRect(bgElement), player2BG);
			GUI.Label(guiMaster.GetElementRect(textElement),PhotonNetwork.player.name, sourceTextStyle);
		}
	}
}
