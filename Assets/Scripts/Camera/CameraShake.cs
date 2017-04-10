using UnityEngine;
using System.Collections;

/*--------------------------------------------------------------------------------------*/
/*																						*/
/*	CameraShake: Shakes the camera.														*/
/*			NOTE: Attached to GameMaster												*/
/*																						*/
/*																						*/
/*		Functions:																		*/
/*			Start ()																	*/
/*			Shake (float amount, float lenght)											*/
/*			DoShake ()																	*/
/*			StopShake ()																*/
/*																						*/
/*--------------------------------------------------------------------------------------*/
public class CameraShake : MonoBehaviour 
{
	public static CameraShake cameraShakeEffect;

	//	Public Variables
	public Camera mainCamera; 					//	Refernce to Main camera
	public Transform target;					//	Reference to player
	public float shakeAmount = 0;				//	How violent the shake is

	public float nextTimeToSearch = 0;				//	How long unitl the camera searches for the target again

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Start: Runs once at the begining of the game. Initalizes variables.					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void Start()
	{
		if (mainCamera == null)
		{
			mainCamera = Camera.main;
		}

		if (cameraShakeEffect == null)
		{
			cameraShakeEffect = GameObject.FindGameObjectWithTag ("Camera").GetComponent<CameraShake> ();
		}

		if (target == null)
		{
			FindPlayer();
		}
		//player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControls> ();
	}

	void FindPlayer()
	{
		if (nextTimeToSearch <= Time.time)
		{
			GameObject result = GameObject.FindGameObjectWithTag ("Player");
			if (result != null)
				target = result.transform;

			nextTimeToSearch = Time.time + 2.0f;
		}
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	Shake: Starts the beginning of the shake											*/
	/*		param:	float amount - how violent the shake is									*/
	/*				float length - how many seconds the shake goes on for					*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	public void Shake(float amount, float length)
	{
		shakeAmount = amount;
		InvokeRepeating ("DoShake", 0, 0.01f);
		Invoke ("StopShake", length);
	}

	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	DoShake: The actual shaking happens here											*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void DoShake()
	{
		if (shakeAmount > 0)
		{
			Vector3 cameraPosition = mainCamera.transform.position;
			float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
			float offsetY = Random.value * shakeAmount * 2 - shakeAmount;
			cameraPosition.x += offsetX;
			cameraPosition.y += offsetY;

			mainCamera.transform.position = cameraPosition;
		}
	}
		
	/*--------------------------------------------------------------------------------------*/
	/*																						*/
	/*	StopShake: Stops the camera shaking													*/
	/*																						*/
	/*--------------------------------------------------------------------------------------*/
	void StopShake()
	{
		CancelInvoke ("DoShake");
		mainCamera.transform.localPosition = new Vector3(0, 0, -10);

	}	

	void Update()
	{
		if (target == null)
		{
			FindPlayer ();
			return;
		}
	}	
}
