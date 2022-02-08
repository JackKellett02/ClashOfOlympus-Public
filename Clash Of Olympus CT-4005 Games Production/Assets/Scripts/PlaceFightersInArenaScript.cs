using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceFightersInArenaScript : MonoBehaviour {
	#region Variables to assign via the unity inspector
	[SerializeField]
	private GameObject fightingPosOne = null;

	[SerializeField]
	private GameObject fightingPosTwo = null;

	[SerializeField]
	private GameObject notFightingAreaPos = null;

	[SerializeField]
	private GameObject notFightingAreaPos2 = null;

	[SerializeField]
	private GameObject healthBarLeft = null;

	[SerializeField]
	private GameObject healthBarRight = null;

	[SerializeField]
	private GameObject specialBarLeft = null;

	[SerializeField]
	private GameObject specialBarRight = null;

	[SerializeField]
	private GameObject char1Left = null;

	[SerializeField]
	private GameObject char1ReadyLeft = null;

	[SerializeField]
	private GameObject char2Left = null;

	[SerializeField]
	private GameObject char2ReadyLeft = null;

	[SerializeField]
	private GameObject char3Left = null;

	[SerializeField]
	private GameObject char3ReadyLeft = null;

	[SerializeField]
	private GameObject char4Left = null;

	[SerializeField]
	private GameObject char4ReadyLeft = null;

	[SerializeField]
	private GameObject char1Right = null;

	[SerializeField]
	private GameObject char1ReadyRight = null;

	[SerializeField]
	private GameObject char2Right = null;

	[SerializeField]
	private GameObject char2ReadyRight = null;

	[SerializeField]
	private GameObject char3Right = null;

	[SerializeField]
	private GameObject char3ReadyRight = null;

	[SerializeField]
	private GameObject char4Right = null;

	[SerializeField]
	private GameObject char4ReadyRight = null;
	#endregion

	#region Variable Declarations
	private GameObject tournamentTracker = null;
	private GameObject playerOne = null;
	private GameObject playerTwo = null;
	private bool gotTracker = false;
	private bool twoPlayerMode = false;
	private float distanceBetweenPlayers = 0.0f;
	private int leftActiveModel = 0;
	private int rightActiveModel = 0;
	#endregion

	#region Private Functions
	// Start is called before the first frame update
	void Start() {
		tournamentTracker = GameObject.FindGameObjectsWithTag("TournamentTracker")[0];
		//PlacePlayersInCorrectPosition();
		twoPlayerMode = GameObject.FindGameObjectsWithTag("ToggleGameModeObject")[0].GetComponent<PlayerNumberScript>().twoPlayerToggle;
	}

	// Update is called once per frame
	void Update() {
		if (!gotTracker) {
			tournamentTracker = GameObject.FindGameObjectsWithTag("TournamentTracker")[0];
			StartCoroutine("WaitOneSecond");
		} else {
			CalculateDistanceBetweenPlayers();
			UpdateLeftModelIcons(leftActiveModel);
			UpdateRightModelIcons(rightActiveModel);
		}
	}

	/// <summary>
	/// Finds the players and places them in the correct positions in the arena.
	/// </summary>
	private void PlacePlayersInCorrectPosition() {
		//Get the lists.
		List<int> fightingList = tournamentTracker.GetComponent<ScoreKeepingScript>().GetFightingList();
		List<GameObject> notFighers = new List<GameObject>();
		//Move the players to the correct positions and assign the health bars.
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		if (fightingList.Count > 0 && players.Length > 0) {
			for (int i = 0; i < players.Length; i++) {
				players[i].GetComponent<HealthScript>().HealPlayerToMax();
				if (players[i].GetComponent<PlayerIdentificationScript>().GetID() == fightingList[0]) {
					//Debug.Log("Player" + players[i].GetComponent<PlayerIdentificationScript>().GetID() + " in pos.");
					//Move player to correct position.
					players[i].transform.position = fightingPosOne.transform.position;
					Debug.Log("Moved fighter one to fighting pos 1.");

					//Make sure attack script is enabled.
					players[i].GetComponent<TestAttackScript>().SetIsFighting(true);

					//Get player model number.
					leftActiveModel = players[i].GetComponent<PlayerSelectionScript>().GetActiveModelNumber();

					//Assign health and special bars.
					players[i].GetComponent<HealthScript>().SetHealthBar(healthBarLeft);
					players[i].GetComponent<TestAttackScript>().SetSpecialBar(specialBarLeft);
					playerOne = players[i];
				} else if (players[i].GetComponent<PlayerIdentificationScript>().GetID() == fightingList[1]) {
					//Debug.Log("Player" + players[i].GetComponent<PlayerIdentificationScript>().GetID() + " in pos.");
					//Move player to the correct position.
					players[i].transform.position = fightingPosTwo.transform.position;
					Debug.Log("Moved fighter two to fighting pos 2.");

					//Make sure attack script is enabled.
					players[i].GetComponent<TestAttackScript>().SetIsFighting(true);

					//Get player model number.
					rightActiveModel = players[i].GetComponent<PlayerSelectionScript>().GetActiveModelNumber();

					//Assign health and special bars.
					players[i].GetComponent<HealthScript>().SetHealthBar(healthBarRight);
					players[i].GetComponent<TestAttackScript>().SetSpecialBar(specialBarRight);
					playerTwo = players[i];
				} else {
					//Move not fighting players to the stands and make them not able to attack.
					notFighers.Add(players[i]);
					players[i].GetComponent<TestAttackScript>().SetIsFighting(false);
				}
			}
		}
		if (!twoPlayerMode) {
			notFighers[0].transform.position = notFightingAreaPos.transform.position;
			notFighers[1].transform.position = notFightingAreaPos2.transform.position;
			Debug.Log("4 player mode.");
		}
	}

	private void UpdateLeftModelIcons(int model) {
		if (playerOne.GetComponent<TestAttackScript>().GetSpecialProgress() < 100.0f) {
			switch (model) {
				case 1: {
						char1Left.SetActive(true);
						char1ReadyLeft.SetActive(false);
						char2Left.SetActive(false);
						char2ReadyLeft.SetActive(false);
						char3Left.SetActive(false);
						char3ReadyLeft.SetActive(false);
						char4Left.SetActive(false);
						char4ReadyLeft.SetActive(false);
						break;
					}
				case 2: {
						char1Left.SetActive(false);
						char1ReadyLeft.SetActive(false);
						char2Left.SetActive(true);
						char2ReadyLeft.SetActive(false);
						char3Left.SetActive(false);
						char3ReadyLeft.SetActive(false);
						char4Left.SetActive(false);
						char4ReadyLeft.SetActive(false);
						break;
					}
				case 3: {
						char1Left.SetActive(false);
						char1ReadyLeft.SetActive(false);
						char2Left.SetActive(false);
						char2ReadyLeft.SetActive(false);
						char3Left.SetActive(true);
						char3ReadyLeft.SetActive(false);
						char4Left.SetActive(false);
						char4ReadyLeft.SetActive(false);
						break;
					}
				case 4: {
						char1Left.SetActive(false);
						char1ReadyLeft.SetActive(false);
						char2Left.SetActive(false);
						char2ReadyLeft.SetActive(false);
						char3Left.SetActive(false);
						char3ReadyLeft.SetActive(false);
						char4Left.SetActive(true);
						char4ReadyLeft.SetActive(false);
						break;
					}
				default: {
						break;
					}
			}
		} else {
			switch (model) {
				case 1: {
						char1Left.SetActive(false);
						char1ReadyLeft.SetActive(true);
						char2Left.SetActive(false);
						char2ReadyLeft.SetActive(false);
						char3Left.SetActive(false);
						char3ReadyLeft.SetActive(false);
						char4Left.SetActive(false);
						char4ReadyLeft.SetActive(false);
						break;
					}
				case 2: {
						char1Left.SetActive(false);
						char1ReadyLeft.SetActive(false);
						char2Left.SetActive(false);
						char2ReadyLeft.SetActive(true);
						char3Left.SetActive(false);
						char3ReadyLeft.SetActive(false);
						char4Left.SetActive(false);
						char4ReadyLeft.SetActive(false);
						break;
					}
				case 3: {
						char1Left.SetActive(false);
						char1ReadyLeft.SetActive(false);
						char2Left.SetActive(false);
						char2ReadyLeft.SetActive(false);
						char3Left.SetActive(false);
						char3ReadyLeft.SetActive(true);
						char4Left.SetActive(false);
						char4ReadyLeft.SetActive(false);
						break;
					}
				case 4: {
						char1Left.SetActive(false);
						char1ReadyLeft.SetActive(false);
						char2Left.SetActive(false);
						char2ReadyLeft.SetActive(false);
						char3Left.SetActive(false);
						char3ReadyLeft.SetActive(false);
						char4Left.SetActive(false);
						char4ReadyLeft.SetActive(true);
						break;
					}
				default: {
						break;
					}
			}
		}
	}

	private void UpdateRightModelIcons(int model) {
		if (playerTwo.GetComponent<TestAttackScript>().GetSpecialProgress() < 100.0f) {
			switch (model) {
				case 1: {
						char1Right.SetActive(true);
						char1ReadyRight.SetActive(false);
						char2Right.SetActive(false);
						char2ReadyRight.SetActive(false);
						char3Right.SetActive(false);
						char3ReadyRight.SetActive(false);
						char4Right.SetActive(false);
						char4ReadyRight.SetActive(false);
						break;
					}
				case 2: {
						char1Right.SetActive(false);
						char1ReadyRight.SetActive(false);
						char2Right.SetActive(true);
						char2ReadyRight.SetActive(false);
						char3Right.SetActive(false);
						char3ReadyRight.SetActive(false);
						char4Right.SetActive(false);
						char4ReadyRight.SetActive(false);
						break;
					}
				case 3: {
						char1Right.SetActive(false);
						char1ReadyRight.SetActive(false);
						char2Right.SetActive(false);
						char2ReadyRight.SetActive(false);
						char3Right.SetActive(true);
						char3ReadyRight.SetActive(false);
						char4Right.SetActive(false);
						char4ReadyRight.SetActive(false);
						break;
					}
				case 4: {
						char1Right.SetActive(false);
						char1ReadyRight.SetActive(false);
						char2Right.SetActive(false);
						char2ReadyRight.SetActive(false);
						char3Right.SetActive(false);
						char3ReadyRight.SetActive(false);
						char4Right.SetActive(true);
						char4ReadyRight.SetActive(false);
						break;
					}
				default: {
						break;
					}
			}
		} else {
			switch (model) {
				case 1: {
						char1Right.SetActive(false);
						char1ReadyRight.SetActive(true);
						char2Right.SetActive(false);
						char2ReadyRight.SetActive(false);
						char3Right.SetActive(false);
						char3ReadyRight.SetActive(false);
						char4Right.SetActive(false);
						char4ReadyRight.SetActive(false);
						break;
					}
				case 2: {
						char1Right.SetActive(false);
						char1ReadyRight.SetActive(false);
						char2Right.SetActive(false);
						char2ReadyRight.SetActive(true);
						char3Right.SetActive(false);
						char3ReadyRight.SetActive(false);
						char4Right.SetActive(false);
						char4ReadyRight.SetActive(false);
						break;
					}
				case 3: {
						char1Right.SetActive(false);
						char1ReadyRight.SetActive(false);
						char2Right.SetActive(false);
						char2ReadyRight.SetActive(false);
						char3Right.SetActive(false);
						char3ReadyRight.SetActive(true);
						char4Right.SetActive(false);
						char4ReadyRight.SetActive(false);
						break;
					}
				case 4: {
						char1Right.SetActive(false);
						char1ReadyRight.SetActive(false);
						char2Right.SetActive(false);
						char2ReadyRight.SetActive(false);
						char3Right.SetActive(false);
						char3ReadyRight.SetActive(false);
						char4Right.SetActive(false);
						char4ReadyRight.SetActive(true);
						break;
					}
				default: {
						break;
					}
			}
		}
	}

	private float CalculateDistanceBetweenPlayers() {
		Vector3 vectorBetweenPlayers = playerTwo.transform.position - playerOne.transform.position;

		return vectorBetweenPlayers.magnitude;
	}

	private IEnumerator WaitOneSecond() {
		yield return new WaitForSeconds(0.01f);
		PlacePlayersInCorrectPosition();
		gotTracker = true;
	}
	#endregion

	#region Public Access Functions (Getters and Setters)
	public float GetDistanceBetweenPlayers() {
		return distanceBetweenPlayers;
	}
	#endregion
}
