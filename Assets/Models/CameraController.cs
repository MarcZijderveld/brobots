using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
	private		MouseController	_mouseController;
	private		MouseController	mouseController
	{
		get
		{
			if (_mouseController == null)
			{
				_mouseController = Hierarchy.GetComponentWithTag<MouseController>("MouseController");
			}
			return _mouseController;
		}
	}
	
    public Transform 	xMinLevelArea,
						xMaxLevelArea,
						zMinLevelArea,
						zMaxLevelArea;
	
	public 	float		scrollArea 			= 25f,
	    				scrollSpeed 		= 25f,
	    				dragSpeed 			= 100f,
	    				zoomSpeed 			= 25f,
	     				zoomMin 			= 25f,
		    			zoomMax 			= 100f,
		     			panSpeed 			= 50f,
		    			panAngleMin 		= 50f,
		     			panAngleMax 		= 80f;
	
	private Vector3 translation				= Vector3.zero;
	
	private float	zoomDelta				= 0f;
	
    private void Update()
    {
        // Init camera translation for this frame.
        translation = Vector3.zero;

        // Zoom in or out
        zoomDelta = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;

        if (zoomDelta != 0)
        {
            translation -= Vector3.up * zoomSpeed * zoomDelta;
        }
		
        // Start panning camera if zooming in close to the ground or if just zooming out.
        float pan = camera.transform.eulerAngles.x - zoomDelta * panSpeed;
        pan = Mathf.Clamp(pan, panAngleMin, panAngleMax);

        if (zoomDelta < 0 || camera.transform.position.y < (zoomMax / 2))
        {
            camera.transform.eulerAngles = new Vector3(pan, 0, 0);
        }
        // Move camera with arrow keys
        translation += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	
		Interaction();
    }
	
	private void Interaction()
	{
		if (Input.GetMouseButton(2))
        {
            translation -= new Vector3(Input.GetAxis("Mouse X") * dragSpeed * Time.deltaTime, 0, Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime);
        }
        else
        {
            if (mouseController.mousePos.x < scrollArea && !mouseController.mouseInInterfaceArea)
            {
                translation += Vector3.right * -scrollSpeed * Time.deltaTime;
            }
            else if (mouseController.mousePos.x >= mouseController.interactionArea.width - scrollArea && !mouseController.mouseInInterfaceArea	)
            {
                translation += Vector3.right * scrollSpeed * Time.deltaTime;
            }
			else
				translation.x = Mathf.Lerp(translation.x, 0, .2f);
			
            if (mouseController.mousePos.y < scrollArea && !mouseController.mouseInInterfaceArea)
            {
                translation += Vector3.forward * scrollSpeed * Time.deltaTime;
            }
            else if (mouseController.mousePos.y > mouseController.interactionArea.height - scrollArea && !mouseController.mouseInInterfaceArea)
            {
                translation += Vector3.forward * -scrollSpeed * Time.deltaTime;
            }
			else
				translation.z = Mathf.Lerp(translation.z, 0, .2f);
        }
		
		Translate();
	}
	
	private void Translate()
	{
		Vector3 desiredPosition = transform.position + translation;

        if (!Contains(new Vector2(xMinLevelArea.position.x, xMaxLevelArea.position.x), new Vector2(zMinLevelArea.position.z, zMaxLevelArea.position.z), desiredPosition))
        {
            translation.x = 0;
			translation.z = 0;
        }
        if (desiredPosition.y < zoomMin || zoomMax < desiredPosition.y)
        {
            translation.y = 0;
        }
		
        transform.position += translation;
	}
	
	public bool Contains(Vector2 xMinMax, Vector2 zMinMax , Vector3 target)
	{
		if(xMinMax.x < target.x && xMinMax.y > target.x)
		{
			if(xMinMax.x < target.z && xMinMax.y > target.z)
			{
				return true;
			}
		}
		return false;
	}
	
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(Color.green.r, Color.green.g, Color.green.b, .05f);
		Gizmos.DrawCube(new Vector3(128, 0 , 128), new Vector3(xMinLevelArea.transform.position.x * -1 + xMaxLevelArea.transform.position.x, 1, zMinLevelArea.transform.position.z + zMaxLevelArea.transform.position.z));
		Gizmos.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 1f);
		Gizmos.DrawSphere(new Vector3(camera.transform.position.x, 2, camera.transform.position.z), 6f);
	}
#endif
}