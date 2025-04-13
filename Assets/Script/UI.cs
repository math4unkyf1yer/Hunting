using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{

    [Header("Main Menu")]
    // For Main Menu
    public GameObject SettingsButton;
    public GameObject StartButton;

    public GameObject ExitButton;
    public GameObject Settings;
    public GameObject MainMenu;


    [Header("Pause Menu")]
    //Pause Menu 
    public GameObject pauseMenu;
    public GameObject ContinueButton;
    public GameObject SettingsPM;
    public GameObject quitButton;

    public Shoot shootScript;

    // For getting timer data to show in pause menu
    public TimeScript timeScript;

    void Start() {
        Time.timeScale = 1;
    }
    void Update() {
        if(Input.GetButtonDown("PauseButton")) {
            Pause();
        }
    }



    // Main Menu Methods
    public void StartGame() {
        SceneManager.LoadScene("Movement");
    }

    public void LoadSettings() {
        MainMenu.SetActive(false);
        Settings.SetActive(true);


    }
    public void LoadMainMenu() {
        // SceneManager.LoadScene("");

        MainMenu.SetActive(true);
        Settings.SetActive(false);
    }
    public void Quit () {
        Application.Quit();
    }


    // Pause Menu Methods

    public void LoadPauseSettings() {
        SettingsPM.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void LoadPauseMenu() {
        SettingsPM.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void Pause() {

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
    }
    public void LoadMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
