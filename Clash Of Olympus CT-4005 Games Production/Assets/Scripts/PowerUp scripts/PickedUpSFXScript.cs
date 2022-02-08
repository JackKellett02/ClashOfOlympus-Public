using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickedUpSFXScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (Serialize Fields)
	[SerializeField]
	private float sfxDuration = 0.35f;
	#endregion

	#region Variable Declarations

	#endregion

	#region Private Functions.
	private void Awake() {
		StartCoroutine("DestroySFXPlayerAfterDuration");
	}

	private IEnumerator DestroySFXPlayerAfterDuration() {
		yield return new WaitForSeconds(sfxDuration);
		Destroy(this.gameObject);
	}
	#endregion

	#region Public Access Functions (Getters and Setters).

	#endregion
}