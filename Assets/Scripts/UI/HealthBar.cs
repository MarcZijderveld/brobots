using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : GUIMemberComponent 
{
	[System.Serializable]
	public class HealthDot
	{
		public string element;
		public Texture2D active,
						 inactive;
	}
	
	public List<HealthDot> dots = new List<HealthDot>();
	
	public string bgElement;

	public Texture2D bg;
	
	private HealthPool healthPool;
	
	
	private void Start()
	{
		healthPool = Hierarchy.GetComponentWithTag<HealthPool>("HealthPool");
	}
	
	private void OnGUI()
	{
		GUI.DrawTexture(guiMaster.GetElementRect(dots[healthPool.GetCurrentHealth() - 	1 ].element), dots[healthPool.GetMaxHealth() - 1].active);
	}
}
