using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBubble : StaticBubble
{
    [SerializeField] public float speed;
    [SerializeField] private Vector2 destinationPos, InitialPos;
    float time;
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        InitialPos = transform.position;
    }
    public override void setData(ObstacleData data)
    {
        speed = data.speed;
        destinationPos = data.targetPosition;
    }

    private void Update()
    {
        //GetComponent<Rigidbody2D>().velocity = (destinationPos - InitialPos) * speed;
        time += Time.deltaTime * speed;
        Vector2 CurrentPos = Vector2.Lerp(InitialPos, destinationPos, time);
        transform.position = CurrentPos;
        if (time >= 1)
        {
            destinationPos = InitialPos;
            InitialPos = transform.position;
            time = 0;
        }
    }
}
