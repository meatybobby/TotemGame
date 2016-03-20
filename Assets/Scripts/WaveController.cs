using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WaveInformation
{
	public float bornTime;
	public GameObject monster;
	public IntVector2 bornPos;
	public int waveItemType;
}

public class WaveController : MonoBehaviour
{
	public List<GameObject> monster;
	private List<WaveInformation[]> waveInfo;
	private int nowWave;
	private int nowMon;
	// Use this for initialization
	/*void Start () {
		
	}*/
	
	// Update is called once per frame
	/*void Update () {
		
	}*/

	public void ReadFile (string filename)
	{
		TextAsset textAsset = Resources.Load (filename) as TextAsset;
		StringReader reader = new StringReader (textAsset.text);
		waveInfo = new List<WaveInformation[]> ();
		string s;
		int monsterNum;
		WaveInformation[] wave;
		while (true) {
			s = reader.ReadLine ();
			if (s == null)
				break;
			monsterNum = int.Parse (s);
			wave = new WaveInformation[monsterNum];
			for (int i = 0; i < monsterNum; i++) {
				s = reader.ReadLine ();
				string[] sp = s.Split (new char[] { ' ' });
				wave [i] = new WaveInformation();
				wave [i].bornTime = float.Parse (sp [0]);
				wave [i].monster = monster [int.Parse (sp [1])];
				wave [i].waveItemType = int.Parse (sp [1]);
				wave [i].bornPos = new IntVector2 (int.Parse (sp [2]), int.Parse (sp [3]));
			}
			waveInfo.Add (wave);
		}
		nowWave = 0;
	}

	public WaveInformation Next ()
	{
		if (nowMon == waveInfo[nowWave].Length)
			return null;
		return waveInfo[nowWave][nowMon++];
	}

	public bool NextWave()
	{
		if (nowWave == waveInfo.Count)
			return false;
		nowWave++;
		nowMon = 0;
		return true;
	}
}
