using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using DictionaryExtension;

[ExecuteInEditMode]
/// <summary>
/// *----------------------------------------------------------------------*
/// | PLEASE LEAVE THIS SUMMARY INSIDE THIS SCRIPT AT ALL TIMES WHEN USED! |
/// *----------------------------------------------------------------------*
/// 
/// This is the Root / Master script for the GUIMaster system, each component subscribes to the master and the master will take care of all the scaling of the elements.
/// 
/// Changelog: 
/// - 1.2 > 26/11/2013 > Modified scaling for more precise scaling, added GUIStyle scaling for GUIContent like text. 
/// - 1.3 > 17/12/2013 > Changed the way the updates work to drastically improve performance (Overhead used to be 20% reduced to 0.1% as of now)
/// 
/// Author: Marc Zijderveld
/// Original Date: 02/11/2013
/// Version: 1.3
/// 
/// [Components]
/// - GUIMember: The Gameobject element containing all the rectangles.
/// - GUIMemberComponent: A small include which you can inherit from for GUIElements.
/// - DictionaryExtensions: Added dictionary functionality.
/// - ResolutionManager: Contains information about the default resolution and triggers resolution changes.
/// </summary>

public class GUIMaster : MonoBehaviour 
{
	//The GUIStyle used to draw the rectangles in the game window.
	public GUIStyle box;
	
	//The scale values in the width and height.
	private float scaleX,
				  scaleY;

	/// <summary>
	/// All the GUIMembers are stored here.
	/// </summary>
	private static Dictionary<string, GUIMember> GUIMembers = new Dictionary<string, GUIMember>();
	
	private void Start()
	{
		//Simulating the first resolution change to get everything up and running.
		OnResolutionChanged();	
	}
	/// <summary>
	/// Change the GUIMembers identifier in the dictionary.
	/// </summary>
	public bool ChangeIdentifier(string oldIdentifier, string newIdentifier)
	{
		return GUIMembers.ChangeKey(oldIdentifier, newIdentifier);
	}
	
	/// <summary>
	/// Update the GUIMembers content.
	/// </summary>
	public void UpdateMember(string identifier, GUIMember member)
	{
		OnResolutionChanged();
		GUIMembers[identifier] = member;
	}
	
	/// <summary>
	/// Add a GUIMember to the dictionary.
	/// </summary>
	public void AddMember(string identifier, GUIMember member)
	{
		if(!GUIMembers.ContainsKey(identifier))
			GUIMembers.Add(identifier, member);
	}
	
	/// <summary>
	/// Remove a GUIMember from the dictionary.
	/// </summary>
	public void RemoveMember(string identifier)
	{
		GUIMembers.Remove(identifier);
	}
	/// <summary>
	/// Scale the source retangle in the GUIMember with a certain scale based on the default resolution set in the ResolutionManager.cs.
	/// </summary>
	public Rect ResolutionRect(Rect rectangle, ResolutionManager.scaleMode mode)
	{
		Rect returnRect = new Rect(0,0,0,0);	
		
		scaleX = Screen.width / ResolutionManager.GetDefaultResolution().x;
		scaleY = Screen.height / ResolutionManager.GetDefaultResolution().y;
		
//		Debug.Log("scaleX: " + scaleX + " " + " scaleY: " + scaleY);
		
		
		//Different scaling modes for each GUIMember. (Default is ScaleWithResolution)
		switch(mode)
		{
			case ResolutionManager.scaleMode.keepPixelSize:
				returnRect = new Rect(rectangle.x * scaleX, rectangle.y * scaleY, rectangle.width, rectangle.height);
				break;
			case ResolutionManager.scaleMode.scaleWidth:
				returnRect = new Rect(rectangle.x * scaleX, rectangle.y * scaleY, rectangle.width * scaleX, rectangle.height);
				break;
			case ResolutionManager.scaleMode.scaleHeight:
				returnRect = new Rect(rectangle.x * scaleX, rectangle.y * scaleY, rectangle.width, rectangle.height * scaleY);
				break;
			case ResolutionManager.scaleMode.scaleWithResolution:
				returnRect = new Rect(rectangle.x * scaleX, rectangle.y * scaleY, rectangle.width * scaleX, rectangle.height * scaleY);
				break;
		}
		
		//The return value of the scaled rectangle of the source rect.
		returnRect = new Rect(Mathf.Round(returnRect.x), Mathf.Round(returnRect.y), Mathf.Round(returnRect.width), Mathf.Round(returnRect.height));
		
		return returnRect;
	}
	
#if UNITY_EDITOR
	///<summary>
	/// The graphical representation on the game window for all the GUIMembers retangles. 
	/// (Wont compile in the actual game because of #if UNITY_EDITOR)
	/// </summary>
	private void OnGUI()
	{

		List<Transform> transforms = new List<Transform>(Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable));
		
		if(!Application.isPlaying)
		{
			foreach(KeyValuePair<string, GUIMember> r in GUIMembers)
			{
				if(r.Value != null)
				{
					if(!r.Value.show)
						continue;
		
					if(r.Value.previewTexture != null)
					{
						GUIStyle style = new GUIStyle();
						style.normal.background = r.Value.previewTexture;
						GUI.Box(r.Value.GetScaledRect(), "", style);
					}
					
					if(transforms.Contains(r.Value.transform))
					{
						GUI.color = Color.red;	
						OnResolutionChanged();
					}

					
					GUI.Box(r.Value.GetScaledRect(), "", box);
					
					GUI.color = Color.white;
				}
			}
		}
		
		GetMainGameView().Repaint();
	}
#endif	
	
	/// <summary>
	/// Toggle rectangle update when the game window changes sizes.
	/// </summary>
	public void OnResolutionChanged()
	{
//		Debug.Log(GUIMembers.Count);
		foreach(KeyValuePair<string, GUIMember> r in GUIMembers)
		{
//			Debug.Log(r.Value.name);
			r.Value.SetScaling(ResolutionRect(r.Value.rect, r.Value.scaleMode));
		}
	}
	
	/// <summary>
	///Returns the rectangle of a certain element.
	///</summary>
	public Rect GetElementRect(string element)
	{
		if(GUIMembers.ContainsKey(element))
			return GUIMembers[element].GetScaledRect();
		else
			Debug.LogWarning("GUIMember with name: " + element + " is not present in the dictionary!");
		
		return new Rect(0,0,0,0);
	}
	
	/// <summary>
	/// Scales the input guiStyle and all its properties based on the current resolution.
	/// </summary>
	public GUIStyle ResolutionGUIStyle(GUIStyle input)
	{
		GUIStyle style = new GUIStyle(input);
		style.fontSize = Mathf.RoundToInt(input.fontSize * scaleX);
		style.contentOffset = new Vector2(input.contentOffset.x * scaleX, input.contentOffset.y * scaleY);
		return style;
	}
	
	//
#if UNITY_EDITOR	
	/// <summary>
	/// Small hack to get the the main game view to update it manually each frame.
	/// </summary>
	public EditorWindow GetMainGameView() 
	{
       System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
       System.Reflection.MethodInfo GetMainGameView = T.GetMethod("GetMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
       System.Object Res = GetMainGameView.Invoke(null,null);
       return (EditorWindow)Res;
	}
#endif
}
