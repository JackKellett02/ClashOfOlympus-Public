using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionScript : MonoBehaviour
{
    #region Variables to assign via the unity inspector (Serialize Fields)

    #endregion

    #region Variable Declarations
    private GameObject spawnPoint = null;
    private bool moved = false;
    #endregion

    #region Private Functions
    private void Awake()
    {
        moved = true;
        Debug.Log("Players placed in lobby");
        spawnPoint = GameObject.FindGameObjectWithTag("Spawn");
        StartCoroutine("WaitForMove");
    }

    private void Update()
    {
     
    }
    private IEnumerator WaitForMove()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position = spawnPoint.transform.position;
        this.GetComponent<PlayerMovement>().SetBool(true);
        moved = true;
    }
    #endregion

    #region Public Access Functions (Getters and setters)

    #endregion
}
