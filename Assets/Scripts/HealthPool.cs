using UnityEngine;
using System.Collections;

public class HealthPool : Photon.MonoBehaviour 
{
	public int maxHealth = 6;
	
	public int currentHealth;
	
	private void Start()
	{
		currentHealth = maxHealth;
	}
	
	public int GetCurrentHealth()
	{
		return currentHealth;
	}
	
	[RPC]
	public void SubstractHealth(int amount)
	{
		currentHealth -= amount;
	}
	
	public int GetMaxHealth()
	{
		return maxHealth;
	}
	
	private void Update()
	{
		if(currentHealth <= 0)
		{
			Hierarchy.GetComponentWithTag<GameFlow>("GameFlow").ToggleGameOver();
		}
	}
}
