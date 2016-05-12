    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
     
public class Chat : PhotonGUIMemberComponent
{
	public static Chat SP;
	public List<string> messages = new List<string> ();
	private int chatHeight = (int)140;
	private Vector2 scrollPos = Vector2.zero;
	private string chatInput = "";
	private float lastUnfocusTime = 0;
	
	public string fieldElement,
				  overviewElement,
				  sendButtonElement;
	
	private bool active = false;
	
	public Texture2D chatBG,
					 sendNormal,
					 sendHover;
	
	public GUIStyle bgStyle,
					player1Style,
					player2Style;
	
	private GUIStyle sourceP1,
					 sourceP2;
     
	void Awake ()
	{
		SP = this;
	}
     
	void Update()
	{
		if(Input.GetButtonDown("Chat"))
		{
			active = !active;	
		}
	}
	
	public bool IsActive()
	{
		return active;
	}
	
	void OnGUI ()
	{
		sourceP1 = guiMaster.ResolutionGUIStyle(player1Style);
		sourceP2 = guiMaster.ResolutionGUIStyle(player2Style);
		
		if(active)
		{
			GUI.SetNextControlName ("");
	     
			GUI.DrawTexture(guiMaster.GetElementRect(overviewElement), chatBG);
			GUILayout.BeginArea (guiMaster.GetElementRect(overviewElement));
			
			scrollPos = GUILayout.BeginScrollView (scrollPos);
			GUI.color = Color.white;
			if(messages != null)
			{
				for (int i = 0; i <= messages.Count -1; i++) 
				{
					string[] splits = messages[i].Split('>');
					
					string name = splits[0];
					name = name.Trim();
					
					if(PhotonNetwork.isMasterClient)
					{
						//Debug.Log("My name: <" + PhotonNetwork.player.name + ">, Message name: <" + name + ">");
						
						if(name == PhotonNetwork.player.name)
							GUILayout.Label (messages [i], sourceP1);
						else
							GUILayout.Label (messages [i], sourceP2);
					}
					else
					{
						if(name == PhotonNetwork.player.name)
							GUILayout.Label (messages [i], sourceP2);
						else
							GUILayout.Label (messages [i], sourceP1);
					}
				}
			}
			
			GUILayout.EndScrollView ();
			GUI.color = Color.white;
	     	GUILayout.EndArea ();
			
			GUI.SetNextControlName ("ChatField");
			chatInput = GUI.TextField(guiMaster.GetElementRect(fieldElement), chatInput);
	     
			if (Event.current.type == EventType.keyDown && Event.current.character == '\n') 
			{
				if (GUI.GetNameOfFocusedControl () == "ChatField") 
				{
					SendChat (PhotonTargets.All);
					lastUnfocusTime = Time.time;
					GUI.FocusControl ("");
					GUI.UnfocusWindow ();
				} 
				else 
				{
					if (lastUnfocusTime < Time.time - 0.1f) 
					{
						GUI.FocusControl ("ChatField");
					}
				}
			}
	     
			GUI.DrawTexture(guiMaster.GetElementRect(sendButtonElement), sendNormal);
				
			if(guiMaster.GetElementRect(sendButtonElement).Contains(Event.current.mousePosition))
			{
				GUI.DrawTexture(guiMaster.GetElementRect(sendButtonElement), sendHover);
				
				if(Event.current.type == EventType.mouseUp)
				{
					SendChat (PhotonTargets.All);
				}
			}
		}
	}
     
	public static void AddMessage (string text)
	{
		SP.messages.Add (text);
		if (SP.messages.Count > 15)
			SP.messages.RemoveAt (0);
	}
     
	[RPC]
	void SendChatMessage (string text, PhotonMessageInfo info)
	{
		AddMessage (info.sender + " > " + text);
	}
     
	void SendChat (PhotonTargets target)
	{
		if (chatInput != "") {
			photonView.RPC ("SendChatMessage", target, chatInput);
			chatInput = "";
		}
	}
     
	void OnLeftRoom ()
	{
		this.enabled = false;
	}
     
	void OnJoinedRoom ()
	{
		this.enabled = true;
	}

	void OnCreatedRoom ()
	{
		this.enabled = true;
	}
}