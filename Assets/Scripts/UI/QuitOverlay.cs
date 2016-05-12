using UnityEngine;
using System.Collections;

public class QuitOverlay : GUIMemberComponent 
{
	public string 		overlayElement,
				  		yesElement,
				 		noElement,
				 		messageElement,
						message;
	
	public Texture2D 	yesNormal,
						noNormal,
						yesHovered,
						noHovered,
						overlay;
						
	public GUIStyle		messageStyle;
	
	private bool 		show  			= false;
	
	public int			depth			= 0;

	private GUIStyle 	sourceStyle;
	
	public AudioClip sound;
	
	public GUIMemberComponent startButton,
							  quitButton,
							  optionsButton;
	
	private void Awake()
	{
		sourceStyle = messageStyle;
	}
	
	private void OnGUI()
	{
		GUI.depth = depth;
			
		if(interactable)
		{
			quitButton.SetInteratable(false);
			startButton.SetInteratable(false);
			optionsButton.SetInteratable(false);
			
			GUI.DrawTexture(guiMaster.GetElementRect(overlayElement), overlay);
			
			messageStyle = guiMaster.ResolutionGUIStyle(sourceStyle);
		
			GUI.Label(guiMaster.GetElementRect(messageElement), message, messageStyle); 
			
			GUI.DrawTexture(guiMaster.GetElementRect(noElement), noNormal);
			
			if(guiMaster.GetElementRect(noElement).Contains(Event.current.mousePosition))
			{
				GUI.DrawTexture(guiMaster.GetElementRect(noElement), noHovered);
				if(Event.current.type == EventType.mouseUp)
				{
					
					Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
					ToggleInteractable();
					quitButton.ToggleInteractable();
					startButton.ToggleInteractable();
					optionsButton.ToggleInteractable();
					enabled = false;	
				}
			}
			
			GUI.DrawTexture(guiMaster.GetElementRect(yesElement), yesNormal);
		
			if(guiMaster.GetElementRect(yesElement).Contains(Event.current.mousePosition))
			{				
				GUI.DrawTexture(guiMaster.GetElementRect(yesElement), yesHovered);
				if(Event.current.type == EventType.mouseUp)
				{
					Hierarchy.GetComponentWithTag<SoundSettings>("SoundManager").Play(sound);
					Application.Quit();
				}
			}
		}
		else
		{
			optionsButton.SetInteratable(true);
			startButton.SetInteratable(true);
			quitButton.SetInteratable(true);
		}
	}
}
