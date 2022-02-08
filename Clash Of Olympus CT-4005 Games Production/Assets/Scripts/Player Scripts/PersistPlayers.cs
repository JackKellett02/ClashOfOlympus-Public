using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistPlayers : MonoBehaviour
{
	private void Awake () {
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
		DontDestroyOnLoad(this.gameObject);
	}
}
