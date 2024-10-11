using MyBox;
using UnityEngine;

public class LevelsLayout : MonoBehaviour
{
    [SerializeField] GameObject levelButtonprefab,rowsPrefab;
    //[SerializeField, ReadOnly] GameObject[] levelsList;
    [SerializeField] int rows;
    [SerializeField] float heightBt;
    [SerializeField] RectTransform sizeScroll;
    void Start()
    {
        LevelData[] levelsList = LevelSelectorManager.Instance.levels;
        InstantiateLevelsButtons(levelsList);
    }
    private void InstantiateLevelsButtons(LevelData[] levelsList)
    {
        int horizontalLayouts=0;
       for (int i = 0; i < levelsList.Length; i++)
        {
            GameObject rowsObject = Instantiate(rowsPrefab, transform);
            horizontalLayouts++;
            for(int j = 0; j < rows; j++)
            {
                GameObject levelButton = Instantiate(levelButtonprefab, rowsObject.transform);
                levelButton.GetComponent<LevelButton>().SetLevel(i + 1);
                levelButton.GetComponent<LevelButton>().SetStars(levelsList[i].starsAchieved);
                if(!(j==rows-1))i++;
                if (i >= levelsList.Length) { break; }
            }         
        }
        sizeScroll.SetHeight((heightBt * horizontalLayouts)+heightBt);
        transform.position = new Vector2(transform.position.x, sizeScroll.transform.position.y);
    }
}