using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (Serialize fields)
	[SerializeField]
	private int mainFightSceneIndex = 1;

	[SerializeField]
	private float waitTime = 4.0f;
	#endregion

	#region Variable Declarations
	private GameObject[] players;
	List<int> fightingList;

	//Positions.
	Vector3 notVsPosOne = new Vector3(-1.5f, 8f, -5.0f);
	Vector3 notVsPosTwo = new Vector3(1.5f, 8f, -5.0f);
	Vector3 vsPosOne = new Vector3(-2.0f, 4.5f, -9.0f);
	Vector3 vsPosTwo = new Vector3(2.0f, 4.5f, -9.0f);

	private bool twoPlayerMode = false;
	#endregion

	#region Private Functions
	// Start is called before the first frame update
	void Start() {
		StartCoroutine("WaitForSeconds");
		players = GameObject.FindGameObjectsWithTag("Player");
		fightingList = GameObject.FindGameObjectsWithTag("TournamentTracker")[0].GetComponent<ScoreKeepingScript>().GetFightingList();
		twoPlayerMode = GameObject.FindGameObjectsWithTag("ToggleGameModeObject")[0].GetComponent<PlayerNumberScript>().twoPlayerToggle;
	}

	// Update is called once per frame
	void Update() {
		PlacePlayersInCorrectPosition();
	}

	private IEnumerator WaitForSeconds() {
		yield return new WaitForSeconds(waitTime);
		SceneManager.LoadScene(mainFightSceneIndex);
	}

	/// <summary>
	/// Finds the players and places them in the correct positions in the transition scene.
	/// </summary>
	private void PlacePlayersInCorrectPosition() {
		//Get the lists.
		List<GameObject> notFighers = new List<GameObject>();
		//Move the players to the correct positions and assign the health bars.
		if (fightingList.Count > 0 && players.Length > 0) {
			for (int i = 0; i < players.Length; i++) {
				players[i].GetComponent<HealthScript>().HealPlayerToMax();
				if (players[i].GetComponent<PlayerIdentificationScript>().GetID() == fightingList[0]) {
					//Debug.Log("Player" + players[i].GetComponent<PlayerIdentificationScript>().GetID() + " in pos.");
					//Move player to correct position.
					players[i].transform.position = vsPosOne;
				} else if (players[i].GetComponent<PlayerIdentificationScript>().GetID() == fightingList[1]) {
					//Debug.Log("Player" + players[i].GetComponent<PlayerIdentificationScript>().GetID() + " in pos.");
					//Move player to the correct position.
					players[i].transform.position = vsPosTwo;
				} else {
					//Move not fighting players to the stands and make them not able to attack.
					notFighers.Add(players[i]);
				}
			}
		}
		if (!twoPlayerMode) {
			notFighers[0].transform.position = notVsPosOne;
			notFighers[1].transform.position = notVsPosTwo;
		}
	}
	#endregion

	#region Public Access Functions (Getters and setters)

	#endregion
}
