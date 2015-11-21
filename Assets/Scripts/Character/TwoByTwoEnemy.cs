using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwoByTwoEnemy : Enemy {

    // Use this for initialization
    void Start () {
        Debug.Log("enemy start!");
        mapUpdated = true;
        isAttack = false;
        pace = 0;
        shapeVector.Add(new IntVector2(0, 0));
        shapeVector.Add(new IntVector2(0, 1));
        shapeVector.Add(new IntVector2(1, 0));
        shapeVector.Add(new IntVector2(1, 1));
        frontVector[2, 1] = new List<IntVector2>();
        frontVector[1, 2] = new List<IntVector2>();
        frontVector[1, 0] = new List<IntVector2>();
        frontVector[0, 1] = new List<IntVector2>();
        frontVector[2, 1].Add(new IntVector2(1, 0));
        frontVector[2, 1].Add(new IntVector2(1, 1));
        frontVector[1, 2].Add(new IntVector2(0, 1));
        frontVector[1, 2].Add(new IntVector2(1, 1));
        frontVector[1, 0].Add(new IntVector2(0, 0));
        frontVector[1, 0].Add(new IntVector2(1, 0));
        frontVector[0, 1].Add(new IntVector2(0, 0));
        frontVector[0, 1].Add(new IntVector2(0, 1));
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        disMap = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
        Map.Create(this);
        disMap = FindPath.ShortestPath(pos, frontVector);
    }
}
