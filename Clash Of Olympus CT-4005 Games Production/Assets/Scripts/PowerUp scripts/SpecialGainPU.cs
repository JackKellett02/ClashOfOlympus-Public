using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialGainPU : MonoBehaviour {
	#region SerializeFields
	[SerializeField]
	private GameObject sfxPlayer = null;

	[SerializeField]
	GameObject specialGainImage;
	#endregion

	#region Variables
	private int newSGain = 20;
	#endregion

	#region Functions
	private void Start () {

	}
	private void OnTriggerEnter (Collider other) {
		if (other.CompareTag("Player")) {
			StartCoroutine(Pickup(other));
		}
	}

	IEnumerator Pickup (Collider player) {
		TestAttackScript attack = player.GetComponent<TestAttackScript>();
		GetComponent<Collider>().enabled = false;
		attack.specialAttackProgress += 20;
		attack.UpdateSpecialBar();
		Instantiate(sfxPlayer, gameObject.transform);
		yield return new WaitForSeconds(3);
		Destroy(gameObject);
		Debug.Log("If outputs, object should be destroyed");
	}
}
#endregion