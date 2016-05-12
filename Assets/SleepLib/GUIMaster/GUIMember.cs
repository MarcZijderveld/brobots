using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GUIMember : MonoBehaviour 
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
	
	public string 	identifier = "NEW";
	public Rect 	rect = new Rect(100,100,100,100);
	public bool		show = true;
	
	private Rect 	scaledRect;
	
	public ResolutionManager.scaleMode scaleMode = ResolutionManager.scaleMode.scaleWithResolution;
	
	public Texture2D	 previewTexture = null;
	
	private void Awake()
	{
		if(guiMaster != null)
			guiMaster.AddMember(identifier, this);
	}
	
	#if UNITY_EDITOR
	private void Update()
	{
		if(guiMaster != null && !Application.isPlaying)
			guiMaster.UpdateMember(identifier, this);
	}
#endif
	
	private void OnDestroy()
	{
		if(guiMaster != null)
			guiMaster.RemoveMember(identifier);
	}
	
	public void ChangeKey(string oldKey, string newKey)
	{
		if(guiMaster != null)
			guiMaster.ChangeIdentifier(oldKey, newKey);
	}
	
	public void UpdateToMaster()
	{
		this.gameObject.name = "GUIMEMBER(" + identifier + ")";
		if(guiMaster != null)
			guiMaster.UpdateMember(identifier, this);
	}
	
	public void SetScaling(Rect scaleRect)
	{
		scaledRect = scaleRect;
	}
	
	public Rect GetScaledRect()
	{
		return scaledRect;
	}
}
