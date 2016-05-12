using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour 
{
	public Vector3 	hitPoint 				{get; private set;}
	
	public Vector2	mousePos				{get; private set;}
	
	public string	interactionElement		= "",
					interfaceElement		= "";

	public bool		mouseInInteractionArea 	{get; private set;}
	public bool		mouseInInterfaceArea 	{get; private set;}
	
	public Rect		interactionArea 		{get; private set;}
	public Rect		interfaceArea 			{get; private set;}
		
	private void Start()
	{
		//interactionArea 		= Master2D.Rectangle(interactionElement);
		
		//interfaceArea 			= Master2D.Rectangle(interfaceElement);
	}
	
	private void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
				
		if (Physics.Raycast(ray, out hit) && hit.normal.y > 0.999)
		{
			hitPoint = hit.point;	
		}	
	}
	
	private void OnGUI()
	{
		mousePos				= Event.current.mousePosition;
		
		//interactionArea 		= Master2D.Rectangle(interactionElement);
		//mouseInInteractionArea 	= interactionArea.Contains(mousePos);
		
		//interfaceArea 			= Master2D.Rectangle(interfaceElement);
		//mouseInInterfaceArea 	= interfaceArea.Contains(mousePos);
	}
}
