using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

/// <summary>
/// This script allows the player to return to the lobby when they reach the podium scene.
/// Currently it only lets them exit the game as we haven't coded in the functionality to properly reset the other scripts yet.
/// </summary>
public class ReturnToLobbyScript : MonoBehaviour
{
    #region Variables to assign via the unity inspector (Serialize Fields)
    [SerializeField]
    private float countdownToMenu = 1.0f;
    #endregion

    #region Variable Declarations
    bool done = false;
    private bool scoreTwoPlayerToggle = false;
    private GameObject gameModeObject = null;
    #endregion

    #region Private Functions
    // Start is called before the first frame update
    void Start()
    {
        //Get the gamemode object.
        gameModeObject = GameObject.FindGameObjectsWithTag("ToggleGameModeObject")[0];
        scoreTwoPlayerToggle = gameModeObject.GetComponent<PlayerNumberScript>().twoPlayerToggle;
    }

    // Update is called once per frame
    void Update()
    {
        if (!done)
        {
            StartCoroutine("ReturnToMenu");
            done = true;
        }
    }

    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(countdownToMenu);

        //Reset Player ID stuff.
        PlayerIdentificationScript.ResetPlayerCounter();

        //Get the player game objects.
        GameObject playerOne = GameObject.FindGameObjectsWithTag("Player")[0];
        GameObject playerTwo = GameObject.FindGameObjectsWithTag("Player")[1];
        GameObject playerThree = null;
        GameObject playerFour = null;
        if (!scoreTwoPlayerToggle) {
            playerThree = GameObject.FindGameObjectsWithTag("Player")[2];
            playerFour = GameObject.FindGameObjectsWithTag("Player")[3];
        }

        //Get the tracking object.
        ScoreKeepingScript.ResetRoundStuff();
        GameObject scoreTracker = GameObject.FindGameObjectsWithTag("TournamentTracker")[0];

        if (scoreTracker)
        {
            //Delete them.
            Destroy(scoreTracker);
            gameModeObject.GetComponent<PlayerNumberScript>().DestroyGameModeObject();
            Destroy(playerOne);
            Destroy(playerTwo);
            if(playerThree != null || playerFour != null) {
                Destroy(playerThree);
                Destroy(playerFour);
            }
        }

        //Load the main menu.
        SceneManager.LoadScene(0);
    }
    #endregion

    #region Public Access Functions (Getters and Setters)

    #endregion
}
