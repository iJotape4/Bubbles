using Events;
using MyBox;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : UIButton
{
    [SerializeField, Scene] protected string sceneToLoad, levelSelectorScene;
    [SerializeField] protected int level;
    [SerializeField] GameObject stars;
    [SerializeField] Star[] starsUI;

    [SerializeField] SongController controller;

    protected override void ClickButtonMethod()
    {
        StartCoroutine(LoadSceneAndExecuteScript());
    }
    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameSounds").GetComponent<SongController>();

        SaveStars stars = JsonReader.loadData();
        SetStars(stars.starsAchivement[level-1]);
    }
    // Start is called before the first frame update
    protected IEnumerator LoadSceneAndExecuteScript()
    {
        // Load the scene asynchronously
        LevelSelectorManager.Instance.currentLevel = LevelSelectorManager.Instance.levels[level - 1];
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.UnloadSceneAsync(levelSelectorScene);

        if (controller.MusicOn)
        {
            EventManager.Dispatch(SongsEvents.StopSong);
            EventManager.Dispatch(SongsEvents.PlayMusic, songs.InGameMusic);
            //EventManager.Dispatch(SongsEvents.LoopSong, songs.InGameMusic);
        }
        
    }
    public void SetStars(int stars)
    {

        if (stars > 0)
        {
            this.stars.SetActive(true);
            for(int i=0;i<starsUI.Length;i++){
                if (i < stars)
                {
                    starsUI[i].setStar(true);
                }
                else { starsUI[i].setStar(false); }
               
            }
           
        }
        else
        {
            this.stars.SetActive(false);
        }
        
    }
    public void SetLevel(int _level)
    {
        level = _level;
        GetComponentInChildren<TextMeshProUGUI>().text = level.ToString();
    }
}