using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    [SerializeField] Sprite on, off;
    public void setStar(bool on)
    {
        if (on) { GetComponent<Image>().sprite = this.on; }
        else { GetComponent<Image>().sprite = this.off; }
    }
}
