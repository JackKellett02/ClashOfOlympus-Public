using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For the obstacle prefab instances of this script add the position of the top left corner of the arena.
/// And finally Add the width (x axis) and the length (z axis) of the arena.
/// </summary>
public class OnAwakeObstaclePrefabScript : MonoBehaviour {
	#region Variables to assign via the unity inspector [SerialiseField]
	[SerializeField]
	private float obstacleHeight = 1.0f;
	#endregion

	#region Variable Declarations
	private GameObject TopLeftPosOfArena = null;
	private Vector3 positionToPlaceObstacle;
	private float arenaWidth;//X Axis
	private float arenaLength;//Z Axis
	#endregion

	#region Private Functions
	private void Awake() {
		//Get the top left pos of the arena.
		TopLeftPosOfArena = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerationScript>().GetTopLeftPosOfArenaObject();

		//Get Length and Width.
		arenaWidth = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerationScript>().GetArenaWidth();
		arenaLength = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerationScript>().GetArenaLength();

		//Generate the position in the boundaries of the arena.
		float xPos = Random.Range(TopLeftPosOfArena.transform.position.x + 1.0f, TopLeftPosOfArena.transform.position.x + arenaWidth - 1.0f);
		float zPos = Random.Range(TopLeftPosOfArena.transform.position.z - arenaLength + 1.0f, TopLeftPosOfArena.transform.position.z - 1.0f);

		//Set the position.
		positionToPlaceObstacle = new Vector3(xPos, (TopLeftPosOfArena.transform.position.y + obstacleHeight / 2.0f), zPos);

		//Update the transform position.
		transform.position = positionToPlaceObstacle;
	}

	//private void OnDisable() {
	//	if (!isLevelGenerator) {
	//		obstacleList.Remove(gameObject);
	//	}
	//}
	#endregion

	#region Public Access Functions (Getters and setters)

	#endregion
}
