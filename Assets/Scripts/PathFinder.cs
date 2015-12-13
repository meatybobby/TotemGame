using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour{

	public static List<IntVector2> four_dir;

	static PathFinder()
	{
		four_dir = new List<IntVector2>();
		four_dir.Add(new IntVector2(-1,0));
		four_dir.Add(new IntVector2(1,0));
		four_dir.Add(new IntVector2(0,-1));
		four_dir.Add(new IntVector2(0,1));
	}

	public static int[,] ShortestPath(IntVector2 pos)
	{
		int[,] calc = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
		for (int i = 0; i < Map.MAP_WIDTH+2; i++)
			for (int j = 0; j < Map.MAP_HEIGHT+2; j++)
				calc[i,j] = -1;
		calc [pos.x, pos.y] = 0;
		Queue<IntVector2> que = new Queue<IntVector2> ();
		que.Enqueue (pos);
		while (que.Count != 0) {
			IntVector2 temp = que.Dequeue();
			foreach (IntVector2 dir in four_dir) {
				IntVector2 newPos = temp + dir;
				if(Map.isInBounded(newPos)) {
					if(calc[newPos.x, newPos.y] == -1 && (Map.IsEmpty(newPos) || Map.Seek(newPos)[0] is Enemy)) {
						calc[newPos.x, newPos.y] = calc[temp.x, temp.y] + 1;
						que.Enqueue(newPos);
					}
				}
			}
		}
		return calc;
	}

	// ShortestPath(lower,upper)
	// The parameters is the lower point and the upper point of the rectangle.
	// We use the lower as the pivot. So the queue and the calc map would use this pivot.
	public static int[,] ShortestPath(IntVector2 lower,IntVector2 upper) {
		int[,] calc = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
		int width = upper.x - lower.x, height = upper.y - lower.y;
		for (int i = 0; i < Map.MAP_WIDTH+2; i++)
			for (int j = 0; j < Map.MAP_HEIGHT+2; j++)
				calc[i,j] = -1;
		calc [lower.x, lower.y] = 0;
		Queue<IntVector2> que = new Queue<IntVector2> ();
		que.Enqueue (lower);
		while (que.Count != 0) {
			IntVector2 temp = que.Dequeue();
			if(temp.x+width+1 <= Map.MAP_WIDTH && calc[temp.x+1, temp.y] == -1) {
				IntVector2 newPos = new IntVector2(temp.x+1, temp.y);
				IntVector2 lowerY = new IntVector2(temp.x+width+1, temp.y);
				IntVector2 upperY = new IntVector2(temp.x+width+1, temp.y + height);
				if(IsNoBarrier(lowerY, upperY)) {
					calc[newPos.x, newPos.y] = calc[temp.x, temp.y] + 1;
					que.Enqueue(newPos);
				}
			}
			if(temp.x-1 > 0 && calc[temp.x-1, temp.y] == -1) {
				IntVector2 newPos = new IntVector2(temp.x-1, temp.y);
				IntVector2 upperY = new IntVector2(temp.x-1, temp.y + height);
				if(IsNoBarrier(newPos, upperY)) {
					calc[newPos.x, newPos.y] = calc[temp.x, temp.y] + 1;
					que.Enqueue(newPos);
				}
			}
			if(temp.y+height+1 <= Map.MAP_HEIGHT && calc[temp.x, temp.y+1] == -1) {
				IntVector2 newPos = new IntVector2(temp.x, temp.y+1);
				IntVector2 lowerX = new IntVector2(temp.x, temp.y+height+1);
				IntVector2 upperX = new IntVector2(temp.x+width, temp.y+height+1);
				if(IsNoBarrier(lowerX, upperX)) {
					calc[newPos.x, newPos.y] = calc[temp.x, temp.y] + 1;
					que.Enqueue(newPos);
				}
			}
			if(temp.y-1 > 0 && calc[temp.x, temp.y-1] == -1) {
				IntVector2 newPos = new IntVector2(temp.x, temp.y-1);
				IntVector2 upperX = new IntVector2(temp.x+width, temp.y-1);
				if(IsNoBarrier(newPos, upperX)) {
					calc[newPos.x, newPos.y] = calc[temp.x, temp.y] + 1;
					que.Enqueue(newPos);
				}
			}
		}
		return calc;
	}

	private static bool IsNoBarrier(IntVector2 begin, IntVector2 end)
	{
		if (begin.x == end.x) {
			int lower = begin.y, upper = end.y;
			for(int i = lower; i <= upper; i++) {
				IntVector2 temp = new IntVector2(begin.x,i);
				if(!Map.IsEmpty(temp) && !(Map.Seek(temp)[0] is Enemy))
					return false;
			}
			return true;
		} else {
			int lower = begin.x, upper = end.x;
			for(int i = lower; i <= upper; i++) {
				IntVector2 temp = new IntVector2(i,begin.y);
				if(!Map.IsEmpty(temp) && !(Map.Seek(temp)[0] is Enemy))
					return false;
			}
			return true;
		}
	}

	public static IntVector2[] TracePath(IntVector2 pos,int[,] map)
	{
		IntVector2 tmpPos = pos;
		IntVector2[] path = new IntVector2[(Map.MAP_WIDTH + 2)* (Map.MAP_HEIGHT + 2)];
		while (map[tmpPos.x, tmpPos.y] > 0)
		{
			List<IntVector2> fourdir = new List<IntVector2>();
			fourdir.Add(new IntVector2(-1,0));
			fourdir.Add(new IntVector2(1,0));
			fourdir.Add(new IntVector2(0,-1));
			fourdir.Add(new IntVector2(0,1));

			// Shuffle the directions
			for (int i = 0; i < fourdir.Count; i++) {
				IntVector2 temp = fourdir[i];
				int randomIndex = Random.Range(i, fourdir.Count);
				fourdir[i] = four_dir[randomIndex];
				fourdir[randomIndex] = temp;
			}

			foreach(IntVector2 dir in fourdir) {
				if (map[tmpPos.x + dir.x, tmpPos.y + dir.y] == map[tmpPos.x, tmpPos.y] - 1)
				{
					tmpPos += dir;
					path[map[tmpPos.x, tmpPos.y]] = new IntVector2(-dir.x, -dir.y);
					break;
				}
			}
		}
		return path;
	}

	public static IntVector2[] TracePathNoRandom(IntVector2 pos,int[,] map)
	{
		IntVector2 tmpPos = pos;
		IntVector2[] path = new IntVector2[(Map.MAP_WIDTH + 2)* (Map.MAP_HEIGHT + 2)];
		while (map[tmpPos.x, tmpPos.y] > 0)
		{
			List<IntVector2> fourdir = new List<IntVector2>();
			fourdir.Add(new IntVector2(-1,0));
			fourdir.Add(new IntVector2(1,0));
			fourdir.Add(new IntVector2(0,-1));
			fourdir.Add(new IntVector2(0,1));

			foreach(IntVector2 dir in fourdir) {
				if (map[tmpPos.x + dir.x, tmpPos.y + dir.y] == map[tmpPos.x, tmpPos.y] - 1)
				{
					tmpPos += dir;
					path[map[tmpPos.x, tmpPos.y]] = new IntVector2(-dir.x, -dir.y);
					break;
				}
			}
		}
		return path;
	}

	// Get the nearest position of player that enemy can arrive first
	public static IntVector2 RetrievePlayer(IntVector2 playerPos, int[,] map)
	{
		int[,] cal = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
		int min = 100000, depth = 100000;
		IntVector2 target = new IntVector2(0,0);
		Queue<IntVector2> queue = new Queue<IntVector2>();
		for (int i = 0; i < Map.MAP_WIDTH + 2; i++)
			for (int j = 0; j < Map.MAP_HEIGHT + 2; j++)
				cal[i, j] = -1;
		cal [playerPos.x, playerPos.y] = 0;
		queue.Enqueue (playerPos);
		while (queue.Count != 0)
		{
			IntVector2 tmpPos = queue.Dequeue();
			if(cal[tmpPos.x, tmpPos.y] >= depth) break;
			foreach (IntVector2 dir in four_dir)
			{
				IntVector2 newPos = tmpPos + dir;
				if (Map.isInBounded(newPos))
				{
					if(cal[newPos.x, newPos.y] == -1) {
						cal[newPos.x, newPos.y] = cal[tmpPos.x, tmpPos.y] + 1;
						if(map[newPos.x, newPos.y] != -1) {
							depth = cal[newPos.x, newPos.y];
							if(min > map[newPos.x, newPos.y]) {
								map[tmpPos.x, tmpPos.y] = map[newPos.x, newPos.y] + 1;
								min = map[newPos.x, newPos.y];
								target = tmpPos;
							}
						}
						else queue.Enqueue(newPos);
					}
				}
			}
		}
		return target;
	}

	public static IntVector2 RetrievePlayer(IntVector2 playerPos, int[,] map, int width, int height)
	{
		int[,] cal = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
		int min = 100000, depth = 100000;
		IntVector2 target = new IntVector2(0,0);
		Queue<IntVector2> queue = new Queue<IntVector2>();
		for (int i = 0; i < Map.MAP_WIDTH + 2; i++)
			for (int j = 0; j < Map.MAP_HEIGHT + 2; j++)
				cal[i, j] = -1;
		cal [playerPos.x, playerPos.y] = 0;
		List<IntVector2> check = new List<IntVector2> ();
		for (int i = 0; i < width; i++) {
			IntVector2 newPos = new IntVector2 (playerPos.x - i, playerPos.y - height);
			check.Add (newPos);
		}
		for (int i = 0; i < width; i++) {
			IntVector2 newPos = new IntVector2 (playerPos.x - i, playerPos.y + 1);
			check.Add (newPos);
		}
		for (int i = 0; i < height; i++) {
			IntVector2 newPos = new IntVector2 (playerPos.x - width, playerPos.y - i);
			check.Add (newPos);
		}
		for (int i = 0; i < height; i++) {
			IntVector2 newPos = new IntVector2 (playerPos.x + 1, playerPos.y - i);
			check.Add (newPos);
		}
		foreach (IntVector2 newPos in check) {
			if (Map.isInBounded (newPos)) {
				cal[newPos.x, newPos.y] = 1;
				if(map[newPos.x, newPos.y] != -1) {
					depth = cal[newPos.x, newPos.y];
					if(min > map[newPos.x, newPos.y]) {
						min = map[newPos.x, newPos.y];
						target = newPos;
						//Debug.Log (newPos);
					}
				}
				else queue.Enqueue(newPos);
			}
		}
		while (queue.Count != 0)
		{
			IntVector2 tmpPos = queue.Dequeue();
			if(cal[tmpPos.x, tmpPos.y] >= depth) break;
			foreach (IntVector2 dir in four_dir)
			{
				IntVector2 newPos = tmpPos + dir;
				if (Map.isInBounded(newPos))
				{
					if(cal[newPos.x, newPos.y] == -1) {
						cal[newPos.x, newPos.y] = cal[tmpPos.x, tmpPos.y] + 1;
						if(map[newPos.x, newPos.y] != -1) {
							depth = cal[newPos.x, newPos.y];
							if(min > map[newPos.x, newPos.y]) {
								map[tmpPos.x, tmpPos.y] = map[newPos.x, newPos.y] + 1;
								min = map[newPos.x, newPos.y];
								target = tmpPos;
							}
						}
						else queue.Enqueue(newPos);
					}
				}
			}
		}
		return target;
	}
}

