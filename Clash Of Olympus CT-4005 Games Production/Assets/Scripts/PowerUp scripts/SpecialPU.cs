using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialPU : MonoBehaviour {
	#region SerializeFields
	[SerializeField]
	GameObject specialImage2;

	[SerializeField]
	private GameObject sfxPlayer = null;
	#endregion

	#region Variables
	private int newSDamage = 40;
	#endregion

	#region Functions
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			StartCoroutine(Pickup(other));
		}
	}

	IEnumerator Pickup(Collider player) {
		TestAttackScript attack = player.GetComponent<TestAttackScript>();
		GetComponent<Collider>().enabled = false;
		attack.specialAttackDamage = newSDamage;
		Instantiate(sfxPlayer, gameObject.transform);
		yield return new WaitForSeconds(3);
		attack.specialAttackDamage = 20f;
		Destroy(gameObject);
		Debug.Log("If outputs, object should be destroyed");
	}
	#endregion
}
