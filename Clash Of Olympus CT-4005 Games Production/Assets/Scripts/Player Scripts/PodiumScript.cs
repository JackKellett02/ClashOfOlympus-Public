using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (Serialize Fields)
	[SerializeField]
	private GameObject firstPosition = null;

	[SerializeField]
	private GameObject secondPosition = null;

	[SerializeField]
	private GameObject thirdPosition = null;

	[SerializeField]
	private GameObject fourthPosition = null;
	#endregion

	#region Variable Declarations
	private GameObject tournamentTracker = null;
	private List<GameObject> podiumPlayerList = new List<GameObject>();
	private bool gotTournamentTracker = false;
	#endregion

	#region Private Functions
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (!gotTournamentTracker) {
			tournamentTracker = GameObject.FindGameObjectsWithTag("TournamentTracker")[0];
			podiumPlayerList = tournamentTracker.GetComponent<ScoreKeepingScript>().GetPodiumList();
			AssignPlayersToPodiums(podiumPlayerList);
			gotTournamentTracker = true;
		}
	}

	private void AssignPlayersToPodiums(List<GameObject> a_gPlayerList) {
		for(int i = 0; i < a_gPlayerList.Count; i++) {
			if(i == 0) {
				a_gPlayerList[i].transform.position = firstPosition.transform.position;
			} else if(i == 1) {
				a_gPlayerList[i].transform.position = secondPosition.transform.position;
			} else if(i == 2) {
				a_gPlayerList[i].transform.position = thirdPosition.transform.position;
			} else if(i == 3) {
				a_gPlayerList[i].transform.position = fourthPosition.transform.position;
			}

			if(i < 4) {
				Debug.Log("Moved and disabled player: " + a_gPlayerList[i].GetComponent<PlayerIdentificationScript>().GetID() + " from index: " + i + " of the player list.");
				a_gPlayerList[i].GetComponent<PlayerMovement>().enabled = false;
				a_gPlayerList[i].GetComponent<TestAttackScript>().enabled = false;
			}
		}
	}
	#endregion

	#region Public Access Functions (getters and setters)

	#endregion
}
