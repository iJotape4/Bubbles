using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    void OnMouseDown()
    {
        var levelScripts = GameObject.FindGameObjectWithTag("LevelScript");
        levelScripts.GetComponent<Game>().CreateLevel();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
