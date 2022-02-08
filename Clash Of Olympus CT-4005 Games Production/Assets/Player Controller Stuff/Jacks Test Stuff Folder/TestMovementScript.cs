using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestMovementScript : MonoBehaviour {
	[SerializeField]
	Transform attackPoint;

	[SerializeField]
	Transform sAttackPoint;

	[SerializeField]
	float attackRange = 0.5f;

	[SerializeField]
	float sAttackRange = 0.5f;

	[SerializeField]
	LayerMask playerLayer;

	private float normalAttackDamage = 10;
	//This is called when inputs are triggered

	private CharacterController controller;
	private Vector3 playerVelocity;
	private bool groundedPlayer;
	private float playerSpeed = 12f;
	private float gravityValue = -9.81f;
	private Vector2 i_movement;
	private bool attackDone = false;

	// Start is called before the first frame update
	void Start() {
		controller = gameObject.GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update() {
		groundedPlayer = controller.isGrounded;
		if (groundedPlayer && playerVelocity.y < 0.0f) {
			playerVelocity.y = 0.0f;
		}

		Vector3 move = new Vector3(i_movement.x, 0.0f, i_movement.y);
		controller.Move(move * playerSpeed * Time.deltaTime);

		if (move != Vector3.zero) {
			gameObject.transform.forward = move;
		}

		playerVelocity.y += gravityValue * Time.deltaTime;
		controller.Move(playerVelocity * Time.deltaTime);
	}

	//void SwitchToPlayer() {
	//	playerModel.SetActive(true);
	//	cursorModel.SetActive(false);
	//	gameObject.GetComponent<PlayerSelectionScript>().enabled = false;
	//	gameObject.GetComponent<AttackScript>().enabled = true;
	//	gameObject.GetComponent<HealthScript>().enabled = true;
	//}

	private void OnLeftJoystick(InputValue value) {
		i_movement = value.Get<Vector2>();
	}

	private void OnButtonWest() {
		//SwitchToPlayer();
	}

	private void OnLeftTrigger() {

	}

	private void OnRightTrigger() {
		if (!attackDone) {
			Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
			foreach (Collider player in hitPlayer) {
				Debug.Log("Enemy has been hit with a normal attack!");
				player.GetComponent<HealthScript>().DamagePlayer(normalAttackDamage);
				if (player.GetComponent<HealthScript>().GetCurrentHealth() < 0) {
					GameObject.Destroy(player);
				}
				//specialAttackProgress += 10;
			}
			attackDone = true;
		}
	}

	private void OnRightTriggerRelease() {
		attackDone = false;
	}
}
