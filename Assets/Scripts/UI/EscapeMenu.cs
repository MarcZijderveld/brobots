using UnityEngine;
using System.Collections;

public class EscapeMenu : MonoBehaviour 
{

	private	GUIMaster 	_guiMaster	= null; 
	private	GUIMaster 	guiMaster
	{
		get
		{
			if (_guiMaster == null)
			{
				_guiMaster = Hierarchy.GetComponentWithTag<GUIMaster>("GUIMaster");
			}
			return _guiMaster;
		}		
	} 
	
	public string 		bgElement,
						textElement,
						resumeElement,
						quitElement;
	
	public int			depth 		= 0;
	
	private bool		active		= false;
	
	private void OnGUI()
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			active = true;
		}
		if(active)
		{
			GUI.Box(guiMaster.GetElementRect(bgElement), "");
			
			GUI.Label(guiMaster.GetElementRect(textElement), "Pause Menu");
			
			if(GUI.Button(guiMaster.GetElementRect(resumeElement), "Resume"))
			{
				active = false;
			}
			
			if(GUI.Button(guiMaster.GetElementRect(quitElement), "Quit to Menu"))
			{
				active = false;
				Application.LoadLevel("_MainMenu");
			}
		}
	}
}
