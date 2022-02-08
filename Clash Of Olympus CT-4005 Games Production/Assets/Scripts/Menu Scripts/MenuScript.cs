using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
	#region Variables to assign via the unity inspector [SerializeFields]
	[SerializeField]
	private GameObject currentSelection = null;

	[SerializeField]
	private GameObject changeButtonSFX = null;

	[SerializeField]
	private float changeButtonSFXLength = 1.0f;

	[SerializeField]
	private GameObject chooseButtonSFX = null;

	[SerializeField]
	private float chooseButtonSFXLength = 1.0f;
	#endregion

	#region Variable Declarations
	private Vector2 movementInput;
	private bool changing = false;
	private float closest = 1000.0f;
	private GameObject closestObject;

	private bool firstSized = false;

	//Sfx variables.
	private bool changeButtonSFXPlayed = false;
	private bool chooseButtonSFXPlayed = false;
	#endregion

	#region Private Functions (Do not try to access from outside this class.)
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (!firstSized) {
			currentSelection.GetComponent<SpringDynamics>().SwitchSize();
			firstSized = true;
		}
		currentSelection.GetComponent<RectTransform>().anchoredPosition += movementInput;
		if (movementInput.magnitude > 0.6f && changing == false) {
			if (!changeButtonSFXPlayed)
			{
				changeButtonSFXPlayed = true;
				StartCoroutine("PlayChangeButtonSoundEffect");
			}
			List<GameObject> buttonArray = new List<GameObject>(GameObject.FindGameObjectsWithTag("Button"));
			buttonArray.Remove(currentSelection);
			buttonArray.ToArray();
			foreach (GameObject button in buttonArray) {
				float distance = Vector2.Distance(button.GetComponent<RectTransform>().anchoredPosition, currentSelection.GetComponent<RectTransform>().anchoredPosition + new Vector2(movementInput.x * 400, movementInput.y * 400));
				if (distance < closest) {
					closest = distance;
					closestObject = button;
				}
			}
			currentSelection.GetComponent<SpringDynamics>().SwitchSize();
			currentSelection = closestObject;
			currentSelection.GetComponent<SpringDynamics>().SwitchSize();
			closest = 1000.0f;
			changing = true;
			StartCoroutine(WaitSec(0.2f));
		}
	}

	private void OnRightTrigger() {
		currentSelection.GetComponent<SpringDynamics>().React(1.0f);
		currentSelection.GetComponent<Button>().onClick.Invoke();
		if (!chooseButtonSFXPlayed) {
			chooseButtonSFXPlayed = true;
			StartCoroutine("PlayChooseButtonSoundEffect");
		}
	}

	private void OnMovement(InputValue value) {
		movementInput = value.Get<Vector2>().normalized;
	}

	private IEnumerator WaitSec(float timeToWait) {
		yield return new WaitForSeconds(timeToWait);
		changing = false;
	}

	private IEnumerator PlayChangeButtonSoundEffect() {
		changeButtonSFX.SetActive(true);
		yield return new WaitForSeconds(changeButtonSFXLength);
		changeButtonSFX.SetActive(false);
		changeButtonSFXPlayed = false;
	}

	private IEnumerator PlayChooseButtonSoundEffect() {
		chooseButtonSFX.SetActive(true);
		yield return new WaitForSeconds(chooseButtonSFXLength);
		chooseButtonSFX.SetActive(false);
		chooseButtonSFXPlayed = false;
	}
	#endregion

	#region Public Functions
	public void ToggleChanging()
	{
		changing = !changing;
	}
	#endregion

}
