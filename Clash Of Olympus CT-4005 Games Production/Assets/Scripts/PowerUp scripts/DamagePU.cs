using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePU : MonoBehaviour
{
	#region SerializeFields
	[SerializeField]
	private GameObject sfxPlayer = null;
	#endregion

	#region Variables
	private int newDamage = 20;
	#endregion

	#region Functions
	private void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Player")) {
			StartCoroutine(Pickup(other));
		}
	}

	IEnumerator Pickup(Collider player) {
		TestAttackScript attack = player.GetComponent<TestAttackScript>();
		GetComponent<Collider>().enabled = false;
		attack.normalAttackDamage = newDamage;
		Instantiate(sfxPlayer, gameObject.transform);
		yield return new WaitForSeconds(3);
		attack.normalAttackDamage = 10f;
		Destroy(gameObject);
		Debug.Log("If outputs, object should be destroyed");
	}
	#endregion
}
