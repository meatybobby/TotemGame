using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WaveInformation
{
	public float bornTime;
	public GameObject monster;
	public IntVector2 bornPos;
}

public class WaveController : MonoBehaviour {
	public List<GameObject> monster;
	private List<WaveInformation> waveInfo;
	private int now;
	// Use this for initialization
	/*void Start () {
		
	}*/
	
	// Update is called once per frame
	/*void Update () {
		
	}*/

	public void ReadFile(string filename) {
		TextAsset textAsset = Resources.Load (filename) as TextAsset;
		StringReader reader = new StringReader (textAsset.text);
		waveInfo = new List<WaveInformation> ();
		string s;
		while (true) {
			s = reader.ReadLine();
			if(s == null) break;
			string[] sp = s.Split(new char[] {' '});
			WaveInformation temp = new WaveInformation();
			temp.bornTime = float.Parse(sp[0]);
			temp.monster = monster[int.Parse(sp[1])] ;
			temp.bornPos = new IntVector2(int.Parse(sp[2]),int.Parse(sp[3]));
			waveInfo.Add(temp);
		}
		now = 0;
	}

	public WaveInformation Next() {
		if (now == waveInfo.Count)
			return null;
		return waveInfo [now++];
	}
}
