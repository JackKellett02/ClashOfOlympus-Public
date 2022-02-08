using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : MonoBehaviour
{

    #region Variables to assign via the unity inspector [SerializeFields]
    [SerializeField]
    private GameObject transitionShade = null;
    #endregion

    #region Public Access Functions (Getters and Setters)
    public void StartGame()
    {
        transitionShade.GetComponent<SpringDynamics>().SwitchPos();
        StartCoroutine("StartGameTransition");
    }

    IEnumerator StartGameTransition(){
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
