﻿using UnityEngine;
using System.Collections;

public class GUIMemberComponent : MonoBehaviour
{

	private	GUIMaster 	_guiMaster	= null; 
	public	GUIMaster 	guiMaster
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
	
	public bool interactable {get; private set;}
	
	private void Start()
	{
		interactable = true;
	}
	
	public void ToggleInteractable()
	{
//		Debug.Log("toggle");
		interactable = !interactable;
	}	
	
	public void SetInteratable(bool interact)
	{
		interactable = interact;
	}
}