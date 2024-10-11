using Events;
using MyBox;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChangeSceneButton : UIButton
{
    [SerializeField,Scene] private string sceneName;
    [SerializeField] private LoadSceneMode loadSceneMode = LoadSceneMode.Single;

    private Scene currentScene; 

    GameObject audioController;
    public SongController controller;

    private void Start()
    {
        audioController = GameObject.FindGameObjectWithTag("GameSounds");
        controller = audioController.GetComponent<SongController>();
        currentScene = SceneManager.GetActiveScene();
    }

    protected override void ClickButtonMethod() 
    {
        Debug.Log("Revisando sonido");
        if (currentScene.name == "LevelScene" && (sceneName == "Level Selector" || sceneName == "MainMenu") && controller.MusicOn)
        {
            EventManager.Dispatch(SongsEvents.StopSong);
            EventManager.Dispatch(SongsEvents.PlayMusic, songs.BackgroundMusic);
        }
        SceneManager.LoadScene(sceneName, loadSceneMode);
    }
    
}