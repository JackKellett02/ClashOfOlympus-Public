using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will store the health of a player.
/// It will have a function that can take away health from a player.
/// It will have a function that can add health to a player.
/// It will have a function that can get the player health.
/// </summary>
public class HealthScript : MonoBehaviour{
	#region Variables to assign via the unity inspector [Serialise Fields].
	[SerializeField]
	private float maxHealth = 100.0f;

	[SerializeField]
	private float minimumHeight = -5.0f;

	[SerializeField]
	private GameObject isHitSFXPlayer = null;

	[SerializeField]
	private float isHitSFXDuration = 0.35f;
	#endregion

	#region Variable Declarations
	private int hitsTaken = 0;
	//Changed currentHealth to public so I can access it within healthPU
	public float currentHeatlh = 0.0f;
	private bool isDead = false;
	private bool isHitSFXPlayed = false;
	private GameObject healthBar;
	#endregion

	#region Private Functions (Do not try to access from outside this class.)
	private void Start() {
		currentHeatlh = maxHealth;
	}

	private void Update() {
		//Check if the player is below the minimum height limit.
		if(gameObject.transform.position.y <= minimumHeight) {
			DamagePlayer(200.0f);
		}

		//Check if the player is dead.
		if(currentHeatlh <= 0.0f) {
			isDead = true;
		} else {
			isDead = false;
			ClampHealth();
		}
	}

	/// <summary>
	/// Called when changing health value to make sure it doesn't go
	/// too high or too low.
	/// </summary>
	/// <returns></returns>
	private float ClampHealth() {
		if (currentHeatlh >= maxHealth) {
			return maxHealth;
		} else if (currentHeatlh <= 0.0f) {
			return 0.0f;
		} else {
			return currentHeatlh;
		}
	}

	/// <summary>
	/// Responsible for activating the requisite changes to the health bar whenever currenthealth changes.
	/// </summary>
	public void UpdateHealthBar() {
        if (healthBar){
			healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(currentHeatlh * 1.5f, 21.85f);
			healthBar.GetComponent<SpringDynamics>().React(1.0f);
		}
    }

	private IEnumerator PlayIsHitSFX() {
		isHitSFXPlayer.SetActive(true);
		isHitSFXPlayed = true;
		yield return new WaitForSeconds(isHitSFXDuration);
		isHitSFXPlayer.SetActive(false);
		isHitSFXPlayed = false;
	}
	#endregion

	#region Public Access Functions (Get/Set and Constructor functions)
	/// <summary>
	/// Returns whether or not the attached object is "dead" or "alive".
	/// </summary>
	/// <returns></returns>
	public bool GetDeathState() {
		return isDead;
	}

	/// <summary>
	/// Returns currentHealth.
	/// </summary>
	/// <returns></returns>
	public float GetCurrentHealth() {
		return currentHeatlh;
	}

	/// <summary>
	/// Sets health bar.
	/// </summary>
	/// <param name="newHealthBar"></param>
	public void SetHealthBar(GameObject newHealthBar) {
		healthBar = newHealthBar;
		UpdateHealthBar();
    }

	/// <summary>
	/// Takes away health.
	/// </summary>
	/// <param name="damageToAdd"></param>
	public void DamagePlayer(float damageToAdd) {
		hitsTaken += 1;
		currentHeatlh -= damageToAdd;
		ClampHealth();
		UpdateHealthBar();

		//If the hit SFX has not been played.
		if (!isHitSFXPlayed) {
			//Play it.
			StartCoroutine("PlayIsHitSFX");
		}
	}

	/// <summary>
	/// Takes away health, specifically for the pot health
	/// </summary>
	/// <param name="damageToAdd"></param>
	public void DamagePot(float damageToAdd) {
		currentHeatlh -= damageToAdd;
		ClampHealth();
	}

	/// <summary>
	/// Adds health.
	/// </summary>
	/// <param name="ammountToHeal"></param>
	public void HealPlayer(float ammountToHeal) {
		currentHeatlh += ammountToHeal;
		ClampHealth();
		UpdateHealthBar();
	}

	/// <summary>
	/// Sets the player health to max.
	/// </summary>
	public void HealPlayerToMax() {
		currentHeatlh = maxHealth;
	}
	#endregion
}
