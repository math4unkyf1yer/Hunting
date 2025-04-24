using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class UI : MonoBehaviour
{

    [Header("Main Menu")]
    // For Main Menu
    public GameObject SettingsButton;
    public GameObject StartButton;

    public GameObject ExitButton;
    public GameObject Settings;
    public GameObject MainMenu;
    public GameObject controlMenu;
    public GameObject backButton;


    [Header("Pause Menu")]
    //Pause Menu 
    public GameObject pauseMenu;
    public GameObject ContinueButton;
    public GameObject SettingsPM;
    public GameObject quitButton;

    public Shoot shootScript;
    private bool isPause = false;

    // For getting timer data to show in pause menu
    public TimeScript timeScript;

    void Start() {
        Time.timeScale = 1;
    }
    void Update() {

        // Need to make it so that this input isnt read in main menu
        if(Input.GetButtonDown("PauseButton")) {
            if(isPause == false) {
                Pause();
                isPause = true;
            }
            
        }
    }



    // Main Menu Methods
    public void StartGame() {

        MainMenu.SetActive(false);
        controlMenu.SetActive(true);
        StartCoroutine(WaitStart());
    }

    public void LoadSettings() {
        EventSystem eventSystem = EventSystem.current;

        eventSystem.SetSelectedGameObject(backButton);
        MainMenu.SetActive(false);
        Settings.SetActive(true);
    }
    public void LoadMainMenu() {
        // SceneManager.LoadScene("");
        EventSystem eventSystem = EventSystem.current;

        eventSystem.SetSelectedGameObject(StartButton);
        MainMenu.SetActive(true);
        Settings.SetActive(false);
    }
    public void Quit () {
        Application.Quit();
    }

    public void Continue() {
        SceneManager.LoadScene("Area1Hub");
    }


    // Pause Menu Methods

    public void LoadPauseSettings() {
        SettingsPM.SetActive(true);
        pauseMenu.SetActive(false);
        EventSystem eventSystem = EventSystem.current;

        eventSystem.SetSelectedGameObject(backButton);
    }

    public void LoadPauseMenu() {
        
        SettingsPM.SetActive(false);
        pauseMenu.SetActive(true);
        
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(ContinueButton);
    }

    public void Pause() {
        EventSystem eventSystem = EventSystem.current;

        eventSystem.SetSelectedGameObject(ContinueButton);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        shootScript.canShoot = false;
    }

    public void Resume() {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        shootScript.canShoot = true;
        isPause = false;
    }
    public void LoadMenu() {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator WaitStart() {
        yield return new WaitForSeconds(5f);
        controlMenu.SetActive(false);
        MainMenu.SetActive(true);
        SceneManager.LoadScene("Area1Hub");
        
    }
}
