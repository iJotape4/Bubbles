public class NextLevelButton : ChangeSceneButton
{
    void Start()
    {
        int currentLevelID = LevelSelectorManager.Instance.currentLevel.levelNumber;
        LevelSelectorManager.Instance.currentLevel = LevelSelectorManager.Instance.levels[currentLevelID];
    }
}