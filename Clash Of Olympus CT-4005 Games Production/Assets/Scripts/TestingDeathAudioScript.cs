using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingDeathAudioScript : MonoBehaviour {
	[SerializeField]
	private GameObject audioHolderObject = null;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		gameObject.GetComponent<HealthScript>().DamagePlayer(5 * Time.deltaTime);
		Debug.Log(gameObject.GetComponent<HealthScript>().GetCurrentHealth());
	}
}
