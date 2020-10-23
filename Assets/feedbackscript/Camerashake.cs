using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Camerashake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	


	// How long the object should shake for.
	[SerializeField] float shakeDuration = 1f;   // duration of the shake 
	[SerializeField] float shakeIntensity = 0.7f;

	Vector3 originalPos;
	bool canshake = true;
	float shakeTime;
	void OnEnable()
	{
		originalPos = transform.localPosition;
		shakeTime = shakeDuration;
	}
	
	public void Shake()
    {
		if (canshake)
		{
			canshake = false;
			StartCoroutine(ShakeCoroutine());
		}
    }
	// Update is called once per frame
	IEnumerator ShakeCoroutine()
	{
		while (shakeTime > 0)
		{
			transform.localPosition = originalPos + Random.insideUnitSphere * shakeIntensity;
			shakeTime -= Time.deltaTime;
			yield return null;
		}
		shakeTime = shakeDuration;
		transform.localPosition = originalPos;
		canshake = true;
	}
}
