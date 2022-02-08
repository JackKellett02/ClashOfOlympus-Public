using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupGenerator : MonoBehaviour {
	#region [SerializeField]
	[SerializeField]
	private GameObject damagePU;

	[SerializeField]
	private GameObject specialPU;

	[SerializeField]
	private GameObject healthPU;

	[SerializeField]
	private GameObject resistPU;

	[SerializeField]
	private Transform spawnPoint;
	#endregion

	#region Variables
	private int amountOfSpawnablePUs = 1;

	#endregion

	#region Functions
	void Start () {

	}
	public void PickPowerUp () {
		int random = Random.Range(1, 100);
		

		for (int i = 0; i < amountOfSpawnablePUs; i++) {
			if (random <= 25) {
				Instantiate(specialPU, spawnPoint.position, spawnPoint.rotation);
			}
			else if (random <= 50) {
				Instantiate(healthPU, spawnPoint.position, spawnPoint.rotation);
			}
			else if (random <= 75) {
				Instantiate(damagePU, spawnPoint.position, spawnPoint.rotation);
			}
			else if (random <= 100) { 
				Instantiate(resistPU, spawnPoint.position, spawnPoint.rotation);
			}
		}
	}
	#endregion
}
