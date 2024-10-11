using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class SaveStars
{
    public int[] starsAchivement;

    public SaveStars(int Levels)
    {
        this.starsAchivement = new int[Levels];
    }
    public void addData(int pos,int data)
    {
        starsAchivement[pos] = data;
    }
}
