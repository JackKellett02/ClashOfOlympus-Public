using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// it has a parent object that has all the scripts in them
/// it has two sub objects that are the particle effect for when it breaks
/// and the model of the breakable pot obstacle. 
/// At the start the particle effect is off and the model is on 
/// and then when the health of the breakable pot reaches 0 
/// it makes the model of the pot inactive and starts the particle effect 
/// then it destroys the object after so many seconds 
/// (you can change the duration of the particle effect so how long the object exits for after it's been "killed")
/// oh and it also has a tag "Breakable" so that the attack script can access the health script of it.
/// </summary>
public class BreakablePotScript : MonoBehaviour {
	#region Variables To Assign via the unity inspector (SerializeFields)
	[SerializeField]
	private GameObject particleEfffectObject = null;

	[SerializeField]
	private GameObject obstacleModel = null;

	[SerializeField]
	private float durationOfParticleEffect = 1.0f;

	[SerializeField]
	private GameObject powerupGenerator;
	#endregion

	#region Private Functions
	// Start is called before the first frame update
	void Start() {
		particleEfffectObject.SetActive(false);
		obstacleModel.SetActive(true);
		//specialPU.SetActive(false);
		//healthPU.SetActive(false);
		//damagePU.SetActive(false);
	}

	// Update is called once per frame
	void Update() {
		//Check if the pot has been destroyed.
		if (gameObject.GetComponent<HealthScript>().GetDeathState()) {
			//TODO: BREAKING ANIMATION
			StartBreakingAnimation();
		}
		//gameObject.GetComponent<HealthScript>().DamagePlayer(1.0f * Time.deltaTime);
		//Debug.Log("Current Pot Health: " + gameObject.GetComponent<HealthScript>().GetCurrentHealth());
	}

	/// <summary>
	/// Turns the particle effect on and hides the model.
	/// </summary>
	private void StartBreakingAnimation() {
		//Set the particle Effect To Active
		particleEfffectObject.SetActive(true);

		//Disable the obstacle model
		obstacleModel.SetActive(false);

		//Start Destruction Timer
		StartCoroutine("ParticleEffectTimer");
	}

	/// <summary>
	/// Destroys the obstacle after so many seconds.
	/// </summary>
	/// <returns></returns>
	private IEnumerator ParticleEffectTimer() {
		yield return new WaitForSeconds(durationOfParticleEffect);
		gameObject.GetComponent<PowerupGenerator>().PickPowerUp();

		//Destroy parent game object.
		Destroy(gameObject);
	}


	#endregion
}
