using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGravityWell : MonoBehaviour
{
    [SerializeField] float timeToActive;
    [SerializeField] GameObject gravityWell;
        private void Start()
        {
            StartCoroutine(Active());
        }
    
    IEnumerator Active()
    {
        yield return new WaitForSeconds(timeToActive);

    }
}