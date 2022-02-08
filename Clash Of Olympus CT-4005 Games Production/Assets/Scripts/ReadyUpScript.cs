using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadyUpScript : MonoBehaviour {
	#region Variable to assign via the unity inspector (Serialize Fields)

	[SerializeField]
	private float triggerCooldownLength = 1.0f;

	[SerializeField]
	private int transitionSceneIndex = 3;

	[SerializeField]
	private GameObject readyUpEffect = null;

	[SerializeField]
	private GameObject tournamentTracker = null;

	[SerializeField]
	private GameObject transitionShade = null;
	#endregion

	#region Variable Declarations
	private int count = 0;
	private int numberOfPlayers = 2;

	private bool readyForNewPlayer = true;
	private bool shade = false;
	private bool twoPlayerMode = false;
	#endregion

	#region Private Functions
	// Start is called before the first frame update
	void Start() {
		twoPlayerMode = GameObject.FindGameObjectsWithTag("ToggleGameModeObject")[0].GetComponent<PlayerNumberScript>().twoPlayerToggle;
		if (twoPlayerMode) {
			numberOfPlayers = 2;
		} else {
			numberOfPlayers = 4;
		}
	}

	// Update is called once per frame
	void Update() {
		if (!shade) {
			transitionShade.GetComponent<SpringDynamics>().SwitchPos();
			shade = true;
		}
		CheckCounter();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && other.gameObject.GetComponent<PlayerSelectionScript>().GetActiveModelNumber() != 0 && readyForNewPlayer && other.GetComponent<TestAttackScript>().enabled) {
			//Debug.Log("Player entered ready up zone.");
			count += 1;
			count = Mathf.Clamp(count, 0, numberOfPlayers);
			readyForNewPlayer = false;
			StartCoroutine("TriggerCooldown");
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Player" && readyForNewPlayer && other.GetComponent<TestAttackScript>().enabled) {
			//Debug.Log("Player left ready up zone.");
			count -= 1;
			count = Mathf.Clamp(count, 0, numberOfPlayers);
			readyForNewPlayer = false;
			StartCoroutine("TriggerCooldown");
		}
	}

	private IEnumerator TriggerCooldown() {
		yield return new WaitForSeconds(triggerCooldownLength);
		readyForNewPlayer = true;
	}

	/// <summary>
	/// Checks the number of players in the ready up zone and starts the game if the appropriate amount are in the zone.
	/// </summary>
	private void CheckCounter() {
		if (count >= numberOfPlayers) {
			StartGame();
		}
	}

	/// <summary>
	/// Changes scene to the next scene.
	/// </summary>
	private void StartGame() {
		tournamentTracker.GetComponent<ScoreKeepingScript>().EndRound();
	}
	#endregion

	#region Public Access Functions (Getters and Setters)

	#endregion
}