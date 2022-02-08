using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to get which player is which. List is added to when a player spawns.
/// </summary>
public class PlayerIdentificationScript : MonoBehaviour {
	#region Variable Declarations
	private static int playersAdded = 0;
	private int ID = 0;
	private bool assignedID = false;
	#endregion

	#region Private Functions
	private void Awake() {
		if (!assignedID) {
			GameObject.FindGameObjectsWithTag("TournamentTracker")[0].GetComponent<ScoreKeepingScript>().AddPlayerToGameObjectList(gameObject);
			ID = playersAdded + 1;
			playersAdded += 1;
			GameObject.FindGameObjectsWithTag("TournamentTracker")[0].GetComponent<ScoreKeepingScript>().AddPlayerToTrackingList(ID);
			assignedID = true;
		}
	}

	private void Update() {
		if (!assignedID) {
			GameObject.FindGameObjectsWithTag("TournamentTracker")[0].GetComponent<ScoreKeepingScript>().AddPlayerToGameObjectList(gameObject);
			ID = playersAdded + 1;
			playersAdded += 1;
			GameObject.FindGameObjectsWithTag("TournamentTracker")[0].GetComponent<ScoreKeepingScript>().AddPlayerToTrackingList(ID);
			assignedID = true;
		}
	}
	#endregion

	#region Public Access Functions (Getters and setters)
	public int GetID() {
		return ID;
	}

	static public void ResetPlayerCounter() {
		playersAdded = 0;
	}
	#endregion
}
