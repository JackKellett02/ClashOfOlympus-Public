using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteControllerScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (Serialize Fields)
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

	[SerializeField]
	private GameObject crownModel = null;
	#endregion

	#region Variable Declarations
	private int activeModel = 0;
	private float plusRotation = 180.0f;

	private Vector3 rotation = new Vector3();
	private Vector3 cursorRotation = new Vector3();
	#endregion

	#region Private Functions
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		UpdateRotation();
	}

	private void UpdateRotation() {
		//Update the rotation variable.
		rotation = gameObject.transform.rotation.ToEulerAngles();
		cursorRotation = gameObject.transform.rotation.ToEulerAngles();

		//Calculate the opposite in the y axis of rotation to keep sprites facing the camera.
		rotation.y = 0 - rotation.y;
		cursorRotation.y = 0 - cursorRotation.y + plusRotation;
		cursorRotation.z = -45;

		//Apply new rotation to sprites.
		UpdateSpriteRotations();
	}

	private void UpdateSpriteRotations() {
		//Convert rotation to quaternion.
		Quaternion rotationOfSprites = Quaternion.Euler(rotation);
		Quaternion cursorRotationOfSprite = Quaternion.Euler(cursorRotation);

		//Check the direction of travel and flip sprite if needed.
		CheckDirectionOfSprites();

		//Update the models.
		cursorModel.transform.rotation = Quaternion.Slerp(cursorModel.transform.rotation, cursorRotationOfSprite, 1);
		char1Model.transform.rotation = Quaternion.Slerp(char1Model.transform.rotation, rotationOfSprites, 1);
		char2Model.transform.rotation = Quaternion.Slerp(char2Model.transform.rotation, rotationOfSprites, 1);
		char3Model.transform.rotation = Quaternion.Slerp(char3Model.transform.rotation, rotationOfSprites, 1);
		char4Model.transform.rotation = Quaternion.Slerp(char4Model.transform.rotation, rotationOfSprites, 1);
		crownModel.transform.rotation = Quaternion.Slerp(crownModel.transform.rotation, rotationOfSprites, 1);
	}

	private void CheckDirectionOfSprites() {
		if(rotation.y >= 0.0f) {
			plusRotation = 0.0f;
			char1Model.GetComponent<SpriteRenderer>().flipX = true;
			char2Model.GetComponent<SpriteRenderer>().flipX = false;
			char3Model.GetComponent<SpriteRenderer>().flipX = false;
			char4Model.GetComponent<SpriteRenderer>().flipX = false;
			crownModel.GetComponent<SpriteRenderer>().flipX = false;
		} else if(rotation.y < 0.0f) {
			plusRotation = 180.0f;
			char1Model.GetComponent<SpriteRenderer>().flipX = false;
			char2Model.GetComponent<SpriteRenderer>().flipX = true;
			char3Model.GetComponent<SpriteRenderer>().flipX = true;
			char4Model.GetComponent<SpriteRenderer>().flipX = true;
			crownModel.GetComponent<SpriteRenderer>().flipX = true;
		}
	}
	#endregion

	#region Public Access Functions (Getters and Setters)

	#endregion
}
