using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NameSelectionScreen : GUIMemberComponent 
{
	public List<GUIMemberComponent> otherComponents = new List<GUIMemberComponent>();
	public List<GUIMemberComponent> myComponents = new List<GUIMemberComponent>();
	
	public string 		labelFieldElement 	= "",
					  	labelTextElement 	= "",
					  	nicknameElement 	= "",
					  	text 				= "";
	
	public GUIStyle 	labelFieldStyle,
						labelTextStyle;
	
	public int 			depth;
	
	private GUIStyle	sourceFieldStyle,
						sourceLabelStyle;
	
	public Texture2D 	nicknameTexture;
	
	private void Start()
	{		
		if(PlayerPrefs.HasKey("PlayerName"))
		{
			PhotonNetwork.player.name = PlayerPrefs.GetString("PlayerName");
		}
	}
	
	public void OnGUI()
	{		
		GUI.depth = depth;
		
		if(interactable)
		{
			foreach(GUIMemberComponent gmc in otherComponents)
				gmc.SetInteratable(false);
			foreach(GUIMemberComponent gmc in myComponents)
			{
				gmc.SetInteratable(true);
				gmc.enabled = true;
			}
			
			sourceLabelStyle = guiMaster.ResolutionGUIStyle(labelTextStyle);
			sourceFieldStyle = guiMaster.ResolutionGUIStyle(labelFieldStyle);
			
			GUI.DrawTexture(guiMaster.GetElementRect(nicknameElement), nicknameTexture);
			GUI.Label(guiMaster.GetElementRect(labelTextElement), text, sourceLabelStyle);

			PhotonNetwork.player.name = GUI.TextField(guiMaster.GetElementRect(labelFieldElement), PhotonNetwork.player.name, sourceFieldStyle);
			
			PlayerPrefs.SetString("PlayerName", PhotonNetwork.player.name);
		}
		else
		{
			foreach(GUIMemberComponent gmc in otherComponents)
				gmc.ToggleInteractable();
			foreach(GUIMemberComponent gmc in myComponents)
			{
				gmc.SetInteratable(false);
				gmc.enabled = false;
			}
		}
	}
}
