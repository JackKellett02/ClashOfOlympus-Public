using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownControllerScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (Serialize Fields)
	[SerializeField]
	private GameObject crownModel = null;
	#endregion

	#region Public Access Functions (Getters and Setters).
	public void SetCrownActiveState(bool a_bActiveState) {
		crownModel.SetActive(a_bActiveState);
	}
	#endregion
}
