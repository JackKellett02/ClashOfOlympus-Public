using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerNumberScript : MonoBehaviour
{
    #region Variables to assign in the inspector [SerializeFields]
    [SerializeField]
    private GameObject twoPlayerText;
    [SerializeField]
    private GameObject fourPlayerText;
	#endregion

	#region Variable Declarations.
	public bool twoPlayerToggle = false;
	#endregion

	#region Private Functions.
	private void Awake()
    {
        DontDestroyOnLoad(this);
    }
	#endregion

	#region Public Access Functions.
	public void TogglePlayerNo() {
        twoPlayerToggle = !twoPlayerToggle;
        //Color tempColour = fourPlayerText.GetComponent<Text>().color;
        //fourPlayerText.GetComponent<Text>().color = twoPlayerText.GetComponent<Text>().color;
        //twoPlayerText.GetComponent<Text>().color = tempColour;
    }

    public void DestroyGameModeObject() {
        Destroy(this.gameObject);
	}
	#endregion
}
