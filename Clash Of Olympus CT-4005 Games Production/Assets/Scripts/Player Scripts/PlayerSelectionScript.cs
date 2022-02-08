using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //Added using the Input system

public class PlayerSelectionScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (Serialize Fields)
	[SerializeField]
	Transform attackPoint;

	[SerializeField]
	LayerMask statueLayer;

	[SerializeField]
	float attackRange = 0.5f;

	[SerializeField]
	private GameObject cursorModel = null;

	[SerializeField]
	private GameObject char1Model = null;

	[SerializeField]
	private GameObject char2Model = null;

	[SerializeField]
	private GameObject char3Model = null;

	[SerializeField]
	private GameObject char4Model = null;
	#endregion

	#region Variable Declarations
	private bool attackDone = false;
	private bool characterChosen = false;
	private int activeModel = 0;
	#endregion

	#region Private Functions
	private void NormalAttack() {
		if (!attackDone && !characterChosen) {
			Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, statueLayer);
			if (hitPlayer.Length > 0) {
				cursorModel.SetActive(false);
				if (hitPlayer[0].tag == "Char1") {
					char1Model.SetActive(true);
					characterChosen = true;
					activeModel = 1;
				} else if (hitPlayer[0].tag == "Char2") {
					char2Model.SetActive(true);
					characterChosen = true;
					activeModel = 2;
				} else if (hitPlayer[0].tag == "Char3") {
					char3Model.SetActive(true);
					characterChosen = true;
					activeModel = 3;
				}else if(hitPlayer[0].tag == "Char4") {
					char4Model.SetActive(true);
					characterChosen = true;
					activeModel = 4;
				}
				hitPlayer[0].gameObject.SetActive(false);
				gameObject.GetComponent<TestAttackScript>().enabled = true;
				gameObject.GetComponent<HealthScript>().enabled = true;
			}
			attackDone = true;
		}
	}

	private void OnRightTrigger() {
		NormalAttack();
	}
	private void OnRightTriggerRelease() {
		attackDone = false;
	}
	#endregion

	#region Public Access Functions (Getters and Setters)
	public int GetActiveModelNumber() {
		return activeModel;
	}
	#endregion
}
