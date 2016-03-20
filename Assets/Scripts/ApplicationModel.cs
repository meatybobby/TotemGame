using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Levelup;

// A class to store levels' information
public class ApplicationModel {
	// storing varibles reachable from every scene
	private static int levelNum = 1;
	public static readonly int totalLevelNum = 8;
	public static bool[] levelLockMap = { true, true, true, true, true, false, false, false};
	public static List<Level> levelList;

	public static int currentEnemyNum = 0;


	static ApplicationModel() {
		levelList = new List<Level>();
		Level level;

		for(int i=1; i<=totalLevelNum; i++) {
			level = new Level("level" + i);
			levelList.Add(level);
		}
	}

	public static Level GetCurrentLevel () {
		return levelList[levelNum-1];
	}

	public static void SetCurrentLevel(int value) {
		levelNum = value;
	}

}
