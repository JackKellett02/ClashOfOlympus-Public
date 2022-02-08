using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TestAttackScript : MonoBehaviour {
	#region SerializeFields
	[SerializeField]
	private Transform attackPoint;

	[SerializeField]
	private Transform sAttackPoint;

	[SerializeField]
	private GameObject normalAttackSFXPlayer = null;

	[SerializeField]
	private float soundEffectDuration = 1.0f;

	[SerializeField]
	private GameObject clashSFXPlayer = null;

	[SerializeField]
	private float clashSFXDuration = 1.0f;

	[SerializeField]
	private float attackRange = 0.5f;

	[SerializeField]
	private float sAttackRange = 0.5f;

	[SerializeField]
	private float attackSpeed = 0.18f;

	[SerializeField]
	private float attackVelocity = 0.8f;

	[SerializeField]
	private float attackEndDelay = 0.08f;

	[SerializeField]
	private float specialAttackAmbientIncrease = 1.0f;

	[SerializeField]
	private float dashStaminaCost = 5.0f;

	[SerializeField]
	private LayerMask playerLayer;

	[SerializeField]
	private LayerMask potLayer;

	[SerializeField]
	private int mainFightingSceneIndex;
	#endregion

	#region Variables
	//Changed these to public to access them from within each of their respected scripts.
	public float normalAttackDamage = 10;
	public float specialAttackDamage = 20;
	public float specialAttackProgress = 4.0f;

	private bool attacking = false;
	private bool dashing = false;
	private bool sAttackDone = false;
	private bool attackHit = false;
	private bool isFighting = false;

	private CharacterController controller;
	private Animator slashAnimator;
	private Animator specialSlashAnimator;
	private GameObject specialBar;

	//SFX variables.
	private bool normalAttackSFXPlayed = false;
	private bool clashSFXPlayed = false;
	#endregion

	private void Start() {
		controller = gameObject.GetComponent<CharacterController>();
		slashAnimator = attackPoint.GetChild(0).GetComponent<Animator>();
		specialSlashAnimator = sAttackPoint.GetChild(0).GetComponent<Animator>();
	}

	private void Update() {
		if (SceneManager.GetActiveScene().buildIndex == mainFightingSceneIndex && isFighting) {
			specialAttackProgress += specialAttackAmbientIncrease * Time.deltaTime;
			specialAttackProgress = Mathf.Clamp(specialAttackProgress, 0.4f, 100.0f);
			AmbientUpdateSpecialBar();
		}
	}

	//This is called when inputs are triggered
	private void SpecialAttack() {
		if (specialAttackProgress < 99) {
			Debug.Log("You do not possess the strength to use a special attack");
			UpdateSpecialBar();
		} else if (specialAttackProgress > 99) {
			Debug.Log("You possess the strength to use a special attack!");
			if (!sAttackDone) {
				Collider[] hitPlayer = Physics.OverlapSphere(sAttackPoint.position, sAttackRange, playerLayer);
				foreach (Collider player in hitPlayer) {
					Debug.Log("Enemy has been hit with a special attack!");
					specialSlashAnimator.SetTrigger("Slash");
					Camera.main.GetComponent<CameraScript>().CameraShake(0.4f);
					player.GetComponent<HealthScript>().DamagePlayer(specialAttackDamage);
					specialAttackProgress = 0.4f;
					UpdateSpecialBar();
				}
				sAttackDone = true;
			}
		}
	}


	private void NormalAttackTrigger() {
		//If there isn't currently an attack in place
		if (!attacking && SceneManager.GetActiveScene().buildIndex == mainFightingSceneIndex && isFighting) {
			if (slashAnimator) {
				slashAnimator.SetTrigger("Slash");
			}
			if (!normalAttackSFXPlayed) {
				normalAttackSFXPlayer.SetActive(true);
				normalAttackSFXPlayed = true;
				StartCoroutine("NormalAttackSFXController");
			}
			//Start the attack and add velocity in direction of slash
			attacking = true;
			//if (TaggedObjectsInRange("Player", attackRange * 5).Length <= 1) {
			//	gameObject.GetComponent<PlayerMovement>().AddVelocity(PlayerFacing() * attackVelocity);
			//}
			StartCoroutine("NormalAttack");
		}
	}

	private IEnumerator NormalAttack() {
		attackHit = false;
		//In the very middle of the slash..
		yield return new WaitForSeconds(attackSpeed / 2);
		Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);
		foreach (Collider player in hitPlayer) {
			if (!attackHit) {
				PlayerIdentificationScript playerID = gameObject.GetComponent<PlayerIdentificationScript>();
				if (player.GetComponent<PlayerIdentificationScript>().GetID() != playerID.GetID()) {
					if (player.GetComponent<TestAttackScript>().IsPlayerAttacking() && Vector3.Dot(PlayerFacing(), player.GetComponent<TestAttackScript>().PlayerFacing()) < 0) {
						Clash();
						player.GetComponent<TestAttackScript>().Clash();
						Debug.Log("Clash");
					} else {
						specialAttackProgress += 10.0f;
						UpdateSpecialBar();
						specialAttackProgress = Mathf.Clamp(specialAttackProgress, 0.4f, 100.0f);
						player.GetComponent<HealthScript>().DamagePlayer(normalAttackDamage);
						player.GetComponent<PlayerMovement>().AddVelocity(PlayerFacing() * attackVelocity);
					}
					Camera.main.GetComponent<CameraScript>().CameraShake(0.25f);
					attackHit = true;
				}
			}
		}
		//After the slash is finished, plus however long you want the player to have to wait between slashes..
		yield return new WaitForSeconds((attackSpeed / 2) + attackEndDelay);
		//Finish attack and let player attack again
		attacking = false;
	}

	private void PotAttack() {
		attackHit = false;
		Collider[] hitPot = Physics.OverlapSphere(attackPoint.position, attackRange * 2, potLayer);
		foreach (Collider pot in hitPot) {
			PlayerIdentificationScript playerID = gameObject.GetComponent<PlayerIdentificationScript>();
			Debug.Log("Player: " + playerID.GetID() + " hit a pot");
			pot.GetComponent<HealthScript>().DamagePot(normalAttackDamage);
			attackHit = true;
		}
	}

	private IEnumerator NormalAttackSFXController() {
		yield return new WaitForSeconds(soundEffectDuration);
		normalAttackSFXPlayer.SetActive(false);
		normalAttackSFXPlayed = false;
	}

	private IEnumerator PlayClashSFX() {
		clashSFXPlayer.SetActive(true);
		yield return new WaitForSeconds(clashSFXDuration);
		clashSFXPlayer.SetActive(false);
		clashSFXPlayed = false;
	}

	private IEnumerator DashCooldown() {
		yield return new WaitForSeconds(attackSpeed / 2);
		dashing = false;
	}

	private void OnDash() {
		if (!dashing && specialAttackProgress >= dashStaminaCost && SceneManager.GetActiveScene().buildIndex == mainFightingSceneIndex && isFighting) {
			dashing = true;
			gameObject.GetComponent<PlayerMovement>().AddVelocity(PlayerFacing() * attackVelocity);
			specialAttackProgress -= dashStaminaCost;
			UpdateSpecialBar();
			StartCoroutine("DashCooldown");
		}
	}

	private void OnLeftTrigger() {
		SpecialAttack();
	}
	private void OnRightTrigger() {
		NormalAttackTrigger();
		PotAttack();
	}
	private void OnRightTriggerRelease() {

	}

	private void OnLeftTriggerRelease() {
		sAttackDone = false;
	}

	/// <summary>
	/// Returns an array of objects in a specified range with a specified tag.
	/// </summary>
	/// <param name="tag"></param>
	/// <param name="range"></param>
	/// <returns></returns>
	private GameObject[] TaggedObjectsInRange(string tag, float range) {
		GameObject[] objectArray = GameObject.FindGameObjectsWithTag(tag);
		List<GameObject> objectsInRange = new List<GameObject>();
		foreach (GameObject obj in objectArray) {
			if ((obj.transform.position - transform.position).magnitude < range) {
				objectsInRange.Add(obj);
			}
		}
		return objectsInRange.ToArray();
	}

	public bool IsPlayerAttacking() {
		return attacking;
	}

	private void Clash() {
		gameObject.GetComponent<PlayerMovement>().AddVelocity(-PlayerFacing() * attackVelocity);
		attackPoint.gameObject.GetComponent<ParticleSystem>().Play();
		attackHit = true;
		
		//Play the SFX
		if (!clashSFXPlayed) {
			clashSFXPlayed = true;
			StartCoroutine("PlayClashSFX");
		}
	}

	/// <summary>
	/// Returns a vector from the player's position to their attack point to determine the way they're facing.
	/// </summary>
	/// <returns></returns>
	public Vector3 PlayerFacing() {
		return attackPoint.position - transform.position;
	}

	public void UpdateSpecialBar() {
		specialBar.GetComponent<RectTransform>().sizeDelta = new Vector2(specialAttackProgress * 3, 21.85f);
		specialBar.GetComponent<SpringDynamics>().React(1.0f);
	}

	private void AmbientUpdateSpecialBar() {
		specialBar.GetComponent<RectTransform>().sizeDelta = new Vector2(specialAttackProgress * 3, 21.85f);
	}

	public void SetSpecialBar(GameObject newSpecialBar) {
		specialBar = newSpecialBar;
	}

	public void SetIsFighting(bool a_bIsFighting) {
		isFighting = a_bIsFighting;
	}

	public float GetSpecialProgress() {
		return specialAttackProgress;
	}
}
