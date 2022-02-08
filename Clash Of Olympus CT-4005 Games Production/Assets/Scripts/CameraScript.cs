using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is to be placed on the camera, and will move towards the midpoint between the midpoint of every target + an offset determined by
/// the difference between the midpoint and where the camera starts in the scene.
/// There will also be a camera shake function that can be called.
/// </summary>
public class CameraScript : MonoBehaviour {
	#region Variables to assign in the unity inspector [SerializeFields]
	[SerializeField]
	private float cameraSpeed = 0.01f;
	[SerializeField]
	private float traumaRate = 0.005f;
	[SerializeField]
	private float shakeFrequency = 5.0f;
	[SerializeField]
	private float maxAngle = 6.0f;
	#endregion

	#region Variable declarations
	private Vector3 cameraOffset;
	private GameObject[] targetArray = new GameObject[2];
	private float trauma = 0;
	private Vector3 originalAngle;
	#endregion

	#region Private Functions (Do not try to access from outside this class.)
	/// <summary>
	/// Here I place each target transform into an array to use them within a for loop, as well as set the camera offset based on where it starts
	/// in comparison to the midpoint of every target. I also set the original camera angle for the camera shake function.
	/// </summary>
	void Start() {
		GameObject tournamentTracker = GameObject.FindGameObjectWithTag("TournamentTracker");
		List<int> fightList = tournamentTracker.GetComponent<ScoreKeepingScript>().GetFightingList();
		GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in playerList) {
			if (player.GetComponent<PlayerIdentificationScript>().GetID() == fightList[0]) {
				targetArray[0] = player;
			} else if (player.GetComponent<PlayerIdentificationScript>().GetID() == fightList[1]) {
				targetArray[1] = player;
			}
		}
		cameraOffset = transform.position - FindMidpoint();
		originalAngle = transform.rotation.eulerAngles;
	}

	/// <summary>
	/// In update I move the camera towards the midpoint.
	/// </summary>
	void FixedUpdate() {
		transform.position += ((FindMidpoint() + cameraOffset) - transform.position) * cameraSpeed;
		CameraShake(0.0f);
	}

	/// <summary>
	/// Gets the midpoint of all 4 targets.
	/// </summary>
	/// <returns></returns>
	private Vector3 FindMidpoint() {
		Vector3 finalVector = Vector3.zero;
		for (int i = 0; i < targetArray.Length; i++) {
			finalVector += targetArray[i].transform.position;
		}
		return finalVector / 4;
	}
	#endregion

	#region Public Functions
	/// <summary>
	/// This function is in control of handling and adding camera shake. Pass in hitvalue to determine how much the screen shakes because of it.
	/// If it's being called from update, pass in 0. This hitValue must be between 1 and 0. If your shake isn't strong enough even though you're
	/// passing in high hit values, try increasing maxAngle.
	/// </summary>
	public void CameraShake(float hitValue) {
		trauma += hitValue;
		trauma = Mathf.Clamp(trauma, 0.0f, 1.0f);
		//If there's no current shake, just return the angle to normal.
		if (trauma <= 0) {
			transform.rotation = Quaternion.Euler(originalAngle);
		} else {
			//The amount the camera should be shaking is the current trauma squared.
			float shake = trauma * trauma;
			//Dissection of this equation:
			//This sets the new transform rotation for the camera. originalAngle + is used here to have the original camera angle as a basis.
			//maxAngle is used here to determine how far out the shake can turn the camera and acts as a basis for the rest.
			//* shake is how much the camera is currently shaking.
			//the next part samples perlin noise at separate points based on the time since the game has started to keep it moving linearly
			//it also multiplies it by the frequency which changes the speed at which it scrolls through the noise, then turns the 0 to 1 value
			//to a -1 to 1 value by multiplying it by 2 and then subtracting 1.
			transform.rotation = Quaternion.Euler(new Vector3(
				originalAngle.x + maxAngle * shake * (Mathf.PerlinNoise(Time.realtimeSinceStartup * shakeFrequency, Time.realtimeSinceStartup * shakeFrequency) * 2 - 1),
				originalAngle.y + maxAngle * shake * (Mathf.PerlinNoise(Time.realtimeSinceStartup * shakeFrequency, Time.realtimeSinceStartup * shakeFrequency + 10) * 2 - 1),
				originalAngle.z + maxAngle * shake * (Mathf.PerlinNoise(Time.realtimeSinceStartup * shakeFrequency, Time.realtimeSinceStartup * shakeFrequency + 20) * 2 - 1)));
		}
		//Linearly decreases trauma based on rate.
		trauma -= traumaRate;
	}
	#endregion
}
