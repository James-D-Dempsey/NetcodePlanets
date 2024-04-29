using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : NetworkBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject PauseMenuUi;

    private int currentSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Tab) && IsHost)
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }       

    }

    public void Resume()
    {
        PauseMenuUi.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        PauseMenuUi.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void goEarth()
    {
        Debug.Log("changed to earth");
        Resume();
        GoToScene(0);
    }

    public void goMoon()
    {
        Debug.Log("changed to moon");
        Resume();
        GoToScene(1);
    }

    public void goJupiter()
    {
        Debug.Log("changed to jupiter");
        Resume();
        GoToScene(2);
    }

    [ClientRpc]
    void ChangeSceneClientRpc(int sceneIndex)
    {
        ChangeScene(sceneIndex);
    }

    public void GoToScene(int sceneIndex)
    {
        if (IsHost)
        {
            ChangeSceneClientRpc(sceneIndex);
            ChangeScene(sceneIndex);
        }
    }

    void ChangeScene(int sceneIndex)
    {
        if (sceneIndex != currentSceneIndex)
        {
            Debug.Log("Changing to scene index: " + sceneIndex);
            SceneManager.LoadScene(sceneIndex);
            currentSceneIndex = sceneIndex;
        }
    }
}
