using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Is attached to an empty object to instantiate the obstacles.
/// The generator instance of this script will spawn the number of objects Specified.
/// 
/// To use this script make sure to add on the level generator object the number of obstacles to place down.
/// Also assign the obstacle prefabs to this instance.
/// </summary>
public class LevelGenerationScript : MonoBehaviour {
	#region Variables to assign via the unity inspector [SerialiseField]
	[SerializeField]
	private GameObject TopLeftPosOfArena = null;

	[SerializeField]
	private float arenaWidth = 0.0f;//X axis

	[SerializeField]
	private float arenaLength = 0.0f;//Z axis

	[SerializeField]
	private int numberOfObstaclesToPlaceInWorld = 0;

	//The two prefabs must have this script attached with isLevelGenerator being set to false in the unity inspector.
	[SerializeField]
	private GameObject obstaclePrefab1 = null;

	[SerializeField]
	private GameObject obstaclePrefab2 = null;
	#endregion

	#region Variable Declarations

	#endregion

	#region Private Functions
	// Start is called before the first frame update
	void Start() {
		for (int i = 0; i < numberOfObstaclesToPlaceInWorld; i++) {
			PopulateArenaWithObstacles();
		}
	}

	private void PopulateArenaWithObstacles() {
		int random = Random.Range(0, 50);
		if (random % 2 == 0) {
			Instantiate(obstaclePrefab1);
		} else {
			Instantiate(obstaclePrefab2);
		}
	}
	#endregion

	#region Public Access Functions (Getters and setters)
	public GameObject GetTopLeftPosOfArenaObject() {
		return TopLeftPosOfArena;
	}

	public float GetArenaWidth() {
		return arenaWidth;
	}

	public float GetArenaLength() {
		return arenaLength;
	}
	#endregion
}
