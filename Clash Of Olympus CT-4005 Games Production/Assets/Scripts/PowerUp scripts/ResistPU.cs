using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResistPU : MonoBehaviour
{
	#region SerializeFields
	[SerializeField]
	Image resistImage;

	[SerializeField]
	private GameObject sfxPlayer = null;
	#endregion

	#region Variables
	

	#endregion

	#region Functions
	private void Start () {
		resistImage.enabled = false;
	}

	private void OnTriggerEnter (Collider other) {
		if (other.CompareTag("Player")) {
			StartCoroutine(Pickup(other));
		}
	}

	IEnumerator Pickup (Collider player) {
		HealthScript resist = player.GetComponent<HealthScript>();
		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<Collider>().enabled = false;
		Instantiate(sfxPlayer, gameObject.transform);
		yield return new WaitForSeconds(5);
		resist.currentHeatlh *= -0.259f;
		Destroy(gameObject);
	}
	#endregion
}
