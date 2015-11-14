using UnityEngine;
using System.Collections;

public class Enemy : Character {

    public GameObject target;
    public int[,] disMap = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
    public bool mapUpdated;

    private Player player;
    private IntVector2[] guide = new IntVector2[(Map.MAP_WIDTH+2) * (Map.MAP_HEIGHT+2)];
    private FindPath findPath;
    private int pace;
    private IntVector2 targetPos;

    // Use this for initialization
    void Start () {
        mapUpdated = true;
        pace = 0;
        player = target.GetComponent<Player>();
        pos.x = 10;
        pos.y = 10;
        Map.Create(this);
        FindPath.ShortestPath(pos);
    }

    // Update is called once per frame
    void Update() {
        if(mapUpdated == true)
        {
            FindDirection();
            pace = 0;
        }
        if (!isMoving /*&& pace < disMap[targetPos.x , targetPos.y]*/)
        {
            Debug.Log("move like jagger");
            // Debug.Log("->"+guide[pace].x+" "+ guide[pace].y);
            MoveByVector(guide[pace]);
            pace++;
        }
    }
    private void FindDirection()
    {
        guide = FindPath.TracePath(player.pos, disMap);
        targetPos = new IntVector2(player.pos.x,player.pos.y);
        mapUpdated = false;
    }
}

