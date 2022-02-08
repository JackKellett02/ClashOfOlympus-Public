using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Tracks who should be fighting who depending on the round we are in.
/// Tracks the players scores.
/// At the end of the tournament creates a list with the player positions for the tournament based
/// on the number of wins.
/// </summary>
public class ScoreKeepingScript : MonoBehaviour {
	#region Variables to assign via the unity inspector
	[SerializeField]
	private int lastRoundNumber = 5;

	[SerializeField]
	private int transitionSceneIndex = 3;
	#endregion

	#region Variable Declarations.
	private static int roundCounter = 0;

	private static bool roundChanged = false;
	private static bool assignedStuff = false;

	private bool scoreTwoPlayerToggle = false;

	private List<GameObject> playerGameObjects = new List<GameObject>();
	private List<PlayerScores> playerTracking = new List<PlayerScores>();
	private List<int> fightingList = new List<int>();
	private List<GameObject> podiumList = new List<GameObject>();
	#endregion

	#region Private Functions
	// Start is called before the first frame update
	void Start() {
		DontDestroyOnLoad(this.gameObject);

		//Get the gamemode object.
		scoreTwoPlayerToggle = GameObject.FindGameObjectsWithTag("ToggleGameModeObject")[0].GetComponent<PlayerNumberScript>().twoPlayerToggle;
		Debug.Log("Two Player Mode: " + scoreTwoPlayerToggle);
		if (scoreTwoPlayerToggle) {
			lastRoundNumber = 2;
		}
	}

	// Update is called once per frame
	void Update() {
		if (playerGameObjects.Count > 0 && roundCounter > 0) {
			UpdateIsDead();
			RoundTracker();
		}
	}

	private void RoundTracker() {
		if (roundCounter == 1 && roundChanged) {
			fightingList.Clear();
			fightingList.Add(playerTracking[0].GetID());
			fightingList.Add(playerTracking[1].GetID());
			roundChanged = false;
		} else if (roundCounter == 2 && roundChanged && !scoreTwoPlayerToggle) {
			fightingList.Clear();
			fightingList.Add(playerTracking[2].GetID());
			fightingList.Add(playerTracking[3].GetID());
			roundChanged = false;
		} else if (roundCounter == 3 && roundChanged && !scoreTwoPlayerToggle) {
			fightingList.Clear();
			UpdateFightingList();
			roundChanged = false;
		} else if (roundCounter == 4 && roundChanged && !scoreTwoPlayerToggle) {
			fightingList.Clear();
			UpdateFightingList();
			roundChanged = false;
		} else if ((roundCounter == 5 && !scoreTwoPlayerToggle || roundCounter == 2 && scoreTwoPlayerToggle) && roundChanged && SceneManager.GetActiveScene().buildIndex != transitionSceneIndex) {
			//GAME OVER
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			roundChanged = false;
		}

		if (roundCounter != lastRoundNumber) {
			//Check if a player has won
			if (playerTracking[fightingList[0] - 1].GetIsDead()) {
				//Increment their win counter.
				playerTracking[fightingList[1] - 1].IncrementWinCounter();

				//Activate the winners crown and make sure the other crowns are deactivated..
				ActivateCrown(fightingList[1]);

				HealAllPlayersToMax();
				EndRound();
			} else if (playerTracking[fightingList[1] - 1].GetIsDead()) {
				//Increment their win counter.
				playerTracking[fightingList[0] - 1].IncrementWinCounter();

				//Activate the winners crown and make sure the other crowns are deactivated..
				ActivateCrown(fightingList[0]);

				HealAllPlayersToMax();
				EndRound();
			}
		}
	}

	/// <summary>
	/// Activates the crown of the winning player and deactivates the crowns of all other players.
	/// </summary>
	/// <param name="a_iPlayerID"></param>
	private void ActivateCrown(int a_iPlayerID) {
		//Get all the players.
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

		//Check all the players vs their ID and activate the crown if the ID matches and deactivate if it doesn't.
		for (int i = 0; i < players.Length; i++) {
			if (players[i].GetComponent<PlayerIdentificationScript>().GetID() == a_iPlayerID) {
				players[i].GetComponent<CrownControllerScript>().SetCrownActiveState(true);
			} else {
				players[i].GetComponent<CrownControllerScript>().SetCrownActiveState(false);
			}
		}
	}

	/// <summary>
	/// Sets the crown model on all the prefabs to inactive.
	/// </summary>
	private void DeactivateAllPlayerCrowns() {
		//Get all the players.
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

		//Make sure their crown model is deactivated.
		for (int i = 0; i < players.Length; i++) {
			players[i].GetComponent<CrownControllerScript>().SetCrownActiveState(false);
		}
	}

	/// <summary>
	/// Increase the health of all players back to the maximum value.
	/// </summary>
	private void HealAllPlayersToMax() {
		for (int i = 0; i < playerGameObjects.Count; i++) {
			if (fightingList[0] == playerGameObjects[i].GetComponent<PlayerIdentificationScript>().GetID()) {
				playerGameObjects[i].GetComponent<HealthScript>().HealPlayerToMax();
			} else if (fightingList[1] == playerGameObjects[i].GetComponent<PlayerIdentificationScript>().GetID()) {
				playerGameObjects[i].GetComponent<HealthScript>().HealPlayerToMax();
			}
		}
	}

	/// <summary>
	/// Increase round counter by 1.
	/// </summary>
	private void IncrememntRoundCounter() {
		roundCounter += 1;
	}

	/// <summary>
	/// Checks the death state of each game object and updates the player tracking list accordingly.
	/// </summary>
	private void UpdateIsDead() {
		if (playerGameObjects.Count > 0) {
			for (int i = 0; i < playerTracking.Count; i++) {
				bool tempBool = playerGameObjects[i].GetComponent<HealthScript>().GetDeathState();
				playerTracking[i].SetIsDead(tempBool);
			}
		}
	}

	/// <summary>
	/// Update the fighting list to have the correct players in it for the round.
	/// </summary>
	private void UpdateFightingList() {
		if (roundCounter == 3) {
			for (int i = 0; i < playerTracking.Count; i++) {
				if (playerTracking[i].GetWinCounter() == 1 && fightingList.Count < 2) {
					fightingList.Add(playerTracking[i].GetID());
					playerTracking[i].IncrementWinCounter();
				}
			}
		} else if (roundCounter == 4) {
			for (int i = 0; i < playerTracking.Count; i++) {
				if (playerTracking[i].GetWinCounter() == 0 && fightingList.Count < 2) {
					fightingList.Add(playerTracking[i].GetID());
				}
			}
		}
	}

	/// <summary>
	/// Orders the player tracking list based on each items win counter from high to low.
	/// </summary>
	private void OrderPlayerTrackingFromHighToLow() {
		//Declare function variables.
		List<PlayerScores> tempPlayerScores = playerTracking;
		PlayerScores playerScoresTemp;

		//Swap the values if the score of one is higher than the next one untill all of the values have been swapped.
		for (int i = 1; i < playerTracking.Count; i++) {
			if (tempPlayerScores[i].GetWinCounter() > tempPlayerScores[i - 1].GetWinCounter()) {
				playerScoresTemp = tempPlayerScores[i - 1];
				tempPlayerScores[i - 1] = tempPlayerScores[i];
				tempPlayerScores[i] = playerScoresTemp;
			}
		}
		for (int i = 1; i < playerTracking.Count; i++) {
			if (tempPlayerScores[i].GetWinCounter() > tempPlayerScores[i - 1].GetWinCounter()) {
				playerScoresTemp = tempPlayerScores[i - 1];
				tempPlayerScores[i - 1] = tempPlayerScores[i];
				tempPlayerScores[i] = playerScoresTemp;
			}
		}

		//Reset the player tracking list to be the tempPlayerScores list.
		playerTracking = tempPlayerScores;
	}

	private void SetUpPodiumList() {
		//Reorder player tracking list.
		OrderPlayerTrackingFromHighToLow();

		//Add correct game objects to the podium list.
		for (int i = 0; i < playerTracking.Count; i++) {
			podiumList.Add(playerGameObjects[playerTracking[i].GetID() - 1]);
		}
		DeactivateAllPlayerCrowns();
	}
	#endregion

	#region Public Access Functions
	static public void ResetRoundStuff() {
		roundCounter = 0;
		roundChanged = false;
	}

	/// <summary>
	/// Adds a player to the list.
	/// </summary>
	/// <param name="player"></param>
	public void AddPlayerToGameObjectList(GameObject player) {
		playerGameObjects.Add(player);
	}

	/// <summary>
	/// Creates a new player score tracking object and adds it to the list.
	/// </summary>
	/// <param name="ID"></param>
	public void AddPlayerToTrackingList(int ID) {
		playerTracking.Add(new PlayerScores(ID));
	}

	/// <summary>
	/// increments the round counter and lets the script know the round has changed.
	/// </summary>
	public void EndRound() {
		IncrememntRoundCounter();
		roundChanged = true;

		//If the new round is going to be the podium scene assign the correct players to the correct positions in the list for the podium.
		if (roundCounter == lastRoundNumber) {
			fightingList.Clear();
			SetUpPodiumList();
		} else {
			SceneManager.LoadScene(transitionSceneIndex);
		}
	}

	/// <summary>
	/// The fighting list contains the ID's of the players who are currently fighting.
	/// </summary>
	/// <returns>fightingList from ScoreKeepingScript.cs</returns>
	public List<int> GetFightingList() {
		return fightingList;
	}

	/// <summary>
	/// The playerGameObjects list contains all the player game objects once they have all been spawned.
	/// </summary>
	/// <returns>playerGameObjects from ScoreKeepingScript.cs</returns>
	public List<GameObject> GetPlayerGameObjectList() {
		return playerGameObjects;
	}

	/// <summary>
	/// Can be used to check what round we're on.
	/// </summary>
	/// <returns>roundCounter from ScoreKeepingScript.cs</returns>
	public int GetRoundCounter() {
		return roundCounter;
	}

	/// <summary>
	/// Only use this function when the final round has been completed as before then it has not been populated.
	/// </summary>
	/// <returns>podiumList from ScoreKeepingScript.cs</returns>
	public List<GameObject> GetPodiumList() {
		return podiumList;
	}
	#endregion
}


#region Player Score Class
/// <summary>
/// This class is used to track the scores of each player. Whenever a player game object is spawned and assigned an ID via the PlayerIdentificationScript.cs
/// a list of this class has an object of this class instantiated with the same ID as the game object and is then used to track the number of wins
/// of that player and whether or not they have died or not so the win counter has to be incremented and so on.
/// </summary>
class PlayerScores {
	#region Private Variables
	private int playerID;
	private int winCounter;
	private bool isDead;
	#endregion

	#region Private Functions

	#endregion

	#region Constructor
	public PlayerScores(int ID) {
		playerID = ID;
		isDead = false;
		winCounter = 0;
	}
	#endregion

	#region Get And Set Functions
	public void IncrementWinCounter() {
		winCounter += 1;
	}

	public int GetWinCounter() {
		return winCounter;
	}

	public void SetIsDead(bool boolean) {
		isDead = boolean;
	}

	public bool GetIsDead() {
		return isDead;
	}

	public int GetID() {
		return playerID;
	}
	#endregion
}


#endregion