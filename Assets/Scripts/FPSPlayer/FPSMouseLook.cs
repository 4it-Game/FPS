using UnityEngine;
using System.Collections;

public class FPSMouseLook : MonoBehaviour {

	public enum RotationAxes{ MouseXAndY, MouseX, MouseY }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivity = 4;
	public float aimSensitivity = 2;

	[HideInInspector]
	public float sensitivityX = 15;
	[HideInInspector]
	public float sensitivityY = 15;
	//float minimumX = -360;
	//float maximumX = 360;

	public float minimumY = -80;
	public float maximumY = 80;

	float rotationY = 0;

	//Added for sniper scope
	[HideInInspector]
	public float currentSensitivity;

	void Update (){
		if(Time.timeScale < 0.01f)
			return;
		if (axes == RotationAxes.MouseXAndY){
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX){
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}else{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}

		currentSensitivity = sensitivity;

		sensitivityX = currentSensitivity;
		sensitivityY = currentSensitivity;
	}
}
