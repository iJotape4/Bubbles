using Events;
using System.Collections;
using UnityEngine;
public enum Direction
{
    LEFT_TO_RIGHT, RIGHT_TO_LEFT, TOP_TO_BOTTOM, BOTTOM_TO_TOP, RANDOM
}
public class BubbleWinds : MonoBehaviour, Obstacles
{
    [SerializeField] private int time,minCount,maxCount;
    [SerializeField] float speedBubbles,withLayout,heightLayout,durationWarning;
    Direction direction;
    [SerializeField] GameObject bubble;

    public void setData(ObstacleData data)
    {
        time = data.time;
        direction = data.direction;
        StartCoroutine(ActiveWild());
    }
    IEnumerator ActiveWild()
    {
        //time for active the wild
        yield return new WaitForSeconds(time);
        Direction newDirection;
        if (direction == Direction.RANDOM)//condition for random direction
        {
            newDirection = (Direction)Random.Range(0, 4);//assign a random direction
        }
        else newDirection = direction;

            BubblesManager.Instance.ActiveWarning(newDirection, durationWarning);
          EventManager.Dispatch(SongsEvents.LoopSong,songs.Warning); 
        //time for the warning duration
        yield return new WaitForSeconds(durationWarning);
        EventManager.Dispatch(SongsEvents.StopSong);
        StartCoroutine(spawnWild(Random.Range(minCount, maxCount), newDirection));  
    }
    IEnumerator spawnWild(int count,Direction direction)
    {
        Vector2 pos = BubblesManager.Instance.getPosDirection(direction);
        Vector2 directionMove=Vector2.zero;
        float x = withLayout / 2;
        float y = heightLayout / 2; 
        if (direction == Direction.LEFT_TO_RIGHT) { pos.y = Random.Range(y, -y); directionMove = Vector2.right; }
        if (direction == Direction.RIGHT_TO_LEFT) {pos.y = Random.Range(y, -y); directionMove = Vector2.left; }
        if (direction == Direction.TOP_TO_BOTTOM) {pos.x = Random.Range(-x, x); directionMove = Vector2.down;}
        if (direction == Direction.BOTTOM_TO_TOP){ pos.x = Random.Range(-x, x); directionMove = Vector2.up;   }
        GameObject bubble = Instantiate(this.bubble,pos,transform.rotation);
        bubble.GetComponent<Rigidbody2D>().velocity = directionMove*speedBubbles;
        bubble.GetComponent<WildBubble>().Init(Random.Range(-5,-1));
        Destroy(bubble, 5f);
        yield return new WaitForSeconds(Random.Range(0.1f,0.9f));
       if(count>0) StartCoroutine(spawnWild((count - 1),direction));
        else StartCoroutine(ActiveWild());
    }
}
