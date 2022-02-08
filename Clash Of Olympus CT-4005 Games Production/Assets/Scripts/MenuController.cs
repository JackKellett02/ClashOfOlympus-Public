using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        Button sButton = startButton.GetComponent<Button>();

        sButton.onClick.AddListener(StartButton);
    }

	private void Update () {
		
	}

	void StartButton () {

    }
}
