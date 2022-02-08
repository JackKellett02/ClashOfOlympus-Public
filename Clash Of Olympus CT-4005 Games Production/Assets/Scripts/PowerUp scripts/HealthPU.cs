using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPU : MonoBehaviour
{
	#region SerializeFields
	[SerializeField]
	private GameObject sfxPlayer = null;
	#endregion

	#region Variables
	private int healPlayer = 20;
	#endregion

	#region Functions
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			StartCoroutine(Pickup(other));
		}
	}

	IEnumerator Pickup(Collider player) {
		HealthScript healhealth = player.GetComponent<HealthScript>();
		healhealth.UpdateHealthBar();
		GetComponent<Collider>().enabled = false;
		healhealth.currentHeatlh = healhealth.currentHeatlh += 20f;
		//Okay, this should heal the player by 0.2f, will need to test this next session
		//Everything doesn't seem to work in my scene and it seems like a lot of hassle to change things when it can be tested in a session
		Instantiate(sfxPlayer, gameObject.transform);
		yield return new WaitForSeconds(3);
		Destroy(gameObject);
		Debug.Log("If outputs, object should be destroyed");
	}
	#endregion
}