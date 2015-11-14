using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FindPath :MonoBehaviour{

	public static List<IntVector2> four_dir;

	static FindPath(){
		four_dir = new List<IntVector2>();
		four_dir.Add(new IntVector2(-1,0));
		four_dir.Add(new IntVector2(1,0));
		four_dir.Add(new IntVector2(0,-1));
		four_dir.Add(new IntVector2(0,1));
	}
	public static int[,] ShortestPath(IntVector2 pos)
    {
        int[,] calc = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
        bool[,] visited = new bool[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
        for (int i = 0; i < Map.MAP_WIDTH+2; i++)
			for (int j = 0; j < Map.MAP_HEIGHT+2; j++) {
				visited[i,j] = false;
				calc[i,j] = -1;
			}
                
        visited[pos.x, pos.y] = true;
        calc[pos.x, pos.y] = 0;
        Queue<IntVector2> queue = new Queue<IntVector2>();
		for (int i = -1; i <= 1; i++)
			for (int j = -1; j <= 1; j++)
				if (Mathf.Abs(i) + Mathf.Abs(j) == 1)
			{
				IntVector2 newPos = new IntVector2(pos.x+i, pos.y+j);
				if (newPos.x > 0 && newPos.x <= Map.MAP_WIDTH && newPos.y > 0 && newPos.y <= Map.MAP_HEIGHT)
				{
					if (!visited[newPos.x, newPos.y] && ( Map.IsEmpty(newPos) 
					                                     /*|| ( !Map.IsEmpty(newPos) && Map.Seek(newPos)[0].GetType() == typeof(Player) )  */))
					{
						/*if (i == 0 || j == 0)
                                {*/
						queue.Enqueue(newPos);
						visited[newPos.x, newPos.y] = true;
						calc[newPos.x, newPos.y] = calc[pos.x, pos.y] + 1;
						//Debug.Log((tmpPos.x + i) + "," + (tmpPos.y + j));
						//}
						/*else if (Map.Seek(new IntVector2(tmpPos.x + i, tmpPos.y)).Count == 0   && Map.Seek(new IntVector2(tmpPos.x, tmpPos.y + j)).Count == 0)
                                {
                                    queue.Enqueue(new IntVector2(tmpPos.x + i, tmpPos.y + j));
                                    visited[tmpPos.x + i, tmpPos.y + j] = true;
                                    calc[tmpPos.x + i, tmpPos.y + j] = calc[tmpPos.x, tmpPos.y] + 1;
                                    //Debug.Log((tmpPos.x + i) + "," + (tmpPos.y + j));
                                }*/
					}
				}
			}
        while (queue.Count != 0)
        {
            IntVector2 tmpPos = queue.Dequeue();
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                    if (Mathf.Abs(i) + Mathf.Abs(j) == 1)
                    {
						IntVector2 newPos = new IntVector2(tmpPos.x+i, tmpPos.y+j);
                        if (newPos.x > 0 && newPos.x <= Map.MAP_WIDTH && newPos.y > 0 && newPos.y <= Map.MAP_HEIGHT)
                        {
                            if (!visited[newPos.x, newPos.y] && ( Map.IsEmpty(newPos) 
                                || ( !Map.IsEmpty(newPos) && Map.Seek(newPos)[0].GetType() == typeof(Player) )  ))
                            {
                                /*if (i == 0 || j == 0)
                                {*/
                                    queue.Enqueue(newPos);
                                    visited[newPos.x, newPos.y] = true;
                                    calc[newPos.x, newPos.y] = calc[tmpPos.x, tmpPos.y] + 1;
                                    //Debug.Log((tmpPos.x + i) + "," + (tmpPos.y + j));
                                //}
                                /*else if (Map.Seek(new IntVector2(tmpPos.x + i, tmpPos.y)).Count == 0   && Map.Seek(new IntVector2(tmpPos.x, tmpPos.y + j)).Count == 0)
                                {
                                    queue.Enqueue(new IntVector2(tmpPos.x + i, tmpPos.y + j));
                                    visited[tmpPos.x + i, tmpPos.y + j] = true;
                                    calc[tmpPos.x + i, tmpPos.y + j] = calc[tmpPos.x, tmpPos.y] + 1;
                                    //Debug.Log((tmpPos.x + i) + "," + (tmpPos.y + j));
                                }*/
                            }
                        }
                    }
        }
       /* for (int j = Map.MAP_HEIGHT + 1; j >= 0; j--)
            Debug.Log(calc[0, j] + " " + calc[1, j] + " " + calc[2, j] + " " + calc[3, j] + " " +
                      calc[4, j] + " " + calc[5, j] + " " + calc[6, j] + " " + calc[7, j] + " " +
                      calc[8, j] + " " + calc[9, j] + " " + calc[10, j] + " " + calc[11, j]);*/
        return calc;
    }

    public static IntVector2[] TracePath(IntVector2 pos,int[,] map)
    {
        IntVector2 tmpPos = new IntVector2(pos.x, pos.y);
        IntVector2[] path = new IntVector2[(Map.MAP_WIDTH + 2) * (Map.MAP_HEIGHT + 2)];
        while (map[tmpPos.x, tmpPos.y] > 0)
        {

            bool check = false;
		 	
			// Shuffle the directions
			for (int i = 0; i < four_dir.Count; i++) {
				IntVector2 temp = four_dir[i];
				int randomIndex = Random.Range(i, four_dir.Count);
				four_dir[i] = four_dir[randomIndex];
				four_dir[randomIndex] = temp;
			}
			
			foreach(IntVector2 dir in four_dir) {
				if (map[tmpPos.x + dir.x, tmpPos.y + dir.y] == map[tmpPos.x, tmpPos.y] - 1)
				{
					tmpPos.x = tmpPos.x + dir.x;
					tmpPos.y = tmpPos.y + dir.y;
					path[map[tmpPos.x, tmpPos.y]] = new IntVector2(-dir.x, -dir.y);
					//Debug.Log(map[tmpPos.x, tmpPos.y]);
					//Debug.Log(map[tmpPos.x, tmpPos.y] + " " + path[map[tmpPos.x, tmpPos.y]].x+","+ path[map[tmpPos.x, tmpPos.y]].y);
					check = true;
					break;
				}
			}
			
			/*for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					if (Mathf.Abs(i) + Mathf.Abs(j) == 1)
					{
						if (map[tmpPos.x + i, tmpPos.y + j] == map[tmpPos.x, tmpPos.y] - 1)
                        {
                            tmpPos.x = tmpPos.x + i;
                            tmpPos.y = tmpPos.y + j;
                            path[map[tmpPos.x, tmpPos.y]] = new IntVector2(-i, -j);
                            //Debug.Log(map[tmpPos.x, tmpPos.y]);
                            //Debug.Log(map[tmpPos.x, tmpPos.y] + " " + path[map[tmpPos.x, tmpPos.y]].x+","+ path[map[tmpPos.x, tmpPos.y]].y);
                            check = true;
                            break;
                        }
                    }
                }
                if (check == true)
                {
                    break;
                }
            }*/
        }
        return path;
    }
}
