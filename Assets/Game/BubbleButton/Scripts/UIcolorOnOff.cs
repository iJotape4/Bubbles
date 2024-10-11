using Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIcolorOnOff : MonoBehaviour
{
    [SerializeField] UIcolorOnOff otherButton;
    TextMeshProUGUI tx;

    //This bool only checks if the button is one of the ON buttons.
    [SerializeField] public bool IsOn;
    //On the other, this bool checks if whatever the buttons is, what should be the Audio activity.
    [SerializeField] public bool buttonON;
    // works as the sounds manager.
    [SerializeField] private SongController controller;

    [SerializeField] Scene currentScene;

    private void Start()
    {
        tx = transform.GetComponentInChildren<TextMeshProUGUI>();
        GetComponent<Button>().onClick.AddListener(Enable);
        controller = GameObject.FindGameObjectWithTag("GameSounds").GetComponent<SongController>();
        currentScene = SceneManager.GetActiveScene();

        //Here the code finds if the current button is the ON button.
        if (gameObject.CompareTag("Settings"))
        {
            IsOn = true;
            //At the star the script will asign the state saved at the SongController.
            if (gameObject.name == "S ON Button") buttonON = controller.SoundsOn;
            if (gameObject.name == "M ON Button") buttonON = controller.MusicOn;
            if (gameObject.name == "V ON Button") buttonON = controller.VibrOn;
        }
        if (!gameObject.CompareTag("Settings"))
        {
            //This will assing the buttonON to the off button if the case.
            if (!otherButton.buttonON)
            {
                buttonON = true;
            }
        }

        // Checks the Sound state.
        //CheckMusic();
        //CheckSound();

    }

    private void OnDestroy()
    {
        //At the end of the scene, the script will save the state of audio in the SongController.
        if (gameObject.name == "S ON Button") controller.SoundsOn = buttonON;
        if (gameObject.name == "M ON Button") controller.MusicOn = buttonON;
        if (gameObject.name == "V ON Button") controller.VibrOn = buttonON;
    }

    private void Update()
    {
        if (buttonON) Enable();
    }


    public void CheckSound()
    {
        if (!IsOn && buttonON)
        {
            if (gameObject.name == "S OFF Button")
            {
                SoundsOff();
            }
        }
        else if (gameObject.name == "S ON Button" && buttonON)
        {
            SoundsOn();
        }
    }

    public void CheckMusic()
    {
        if (!IsOn && buttonON)
        {
            controller.MusicSource.Stop();
        }
        else if (IsOn && buttonON)
        {
            if (currentScene.name == "LevelScene")
            {
                EventManager.Dispatch(SongsEvents.PlayMusic, songs.InGameMusic);
            }

            if(!(currentScene.name == "LevelScene"))
            {
                EventManager.Dispatch(SongsEvents.PlayMusic, songs.BackgroundMusic);
            }
            //EventManager.Dispatch(SongsEvents.LoopSong, songs.BackgroundMusic);
        }

    }

    public void Enable()
    {
        otherButton.Disable();
        //EventManager.Dispatch(LevelFlowEvents.levelSongState, IsOn);
        tx.color = Color.green;
    }
    public void Disable()
    {
        tx.color = Color.red;
    }

    public void TurnColorOn()
    {
        buttonON = true;
    }

    public void TurnColorOff() 
    {
        buttonON = false;
    }

    public void MainThemeOn()
    {
        EventManager.Dispatch(SongsEvents.PlayMusic, songs.BackgroundMusic);
        EventManager.Dispatch(SongsEvents.LoopSong, songs.BackgroundMusic);
    }

    public void MainThemeOff()
    {
        EventManager.Dispatch(SongsEvents.StopSong);
    }

    public void SoundsOn()
    {
        EventManager.Dispatch(SongsEvents.EnableSounds);
    }

    public void SoundsOff()
    {
        EventManager.Dispatch(SongsEvents.StopSounds);
    }
}
