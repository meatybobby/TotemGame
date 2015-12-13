using UnityEngine;
using System.Collections;

public class TotemSummoner : MonoBehaviour
{
	public int mana;
	private int[] cost = new int[4] {1,1,1,1};
	public GameObject[] totem;
	public int totemNum;
	public const int maxTotemNum = 1000;
	public Player player;
	// Use this for initialization
	void Start ()
	{
		totemNum = 0;
		mana = 10;
		player = GetComponent<Player> ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void Summon(int totemId, IntVector2 pos, IntVector2 dir) {
		Debug.Log (cost.Length);
		if (cost[totemId] > mana || totemNum == maxTotemNum)
			return;
		if (Map.isInBounded(pos) && Map.IsEmpty (pos)) {
			Vector3 totemRealPos = Map.GetRealPosition(pos, typeof(Totem));
			GameObject totemObj;
			switch(totemId){
			case 0:
				//Debug.Log ("create totem!");
				totemObj = Instantiate (totem[0], totemRealPos, Quaternion.Euler (0f, 0f, 0f)) as GameObject;
				Totem001 newTotem = totemObj.GetComponent<Totem001> ();
				newTotem.Rotate (dir);
				newTotem.pos = pos;
				newTotem.isCaught = false;
				newTotem.playerRef = player;
				Map.Create (newTotem);
				break;
			case 1:
				totemObj = Instantiate (totem[1], totemRealPos, Quaternion.Euler (0f, 0f, 0f)) as GameObject;
				Totem002 newTotem2 = totemObj.GetComponent<Totem002> ();
				newTotem2.pos = pos;
				newTotem2.isCaught = false;
				newTotem2.playerRef = player;
				Map.Create (newTotem2);
				break;
			case 2:
				totemObj = Instantiate (totem[2], totemRealPos, Quaternion.Euler (0f, 0f, 0f)) as GameObject;
				Totem003 newTotem3 = totemObj.GetComponent<Totem003> ();
				newTotem3.pos = pos;
				newTotem3.isCaught = false;
				newTotem3.playerRef = player;
				Map.Create (newTotem3);
				break;

			default:
				break;
			}

			totemNum++;
			mana -= cost[totemId];
		}
	}

	public void addMana(int add) {
		mana += add;
	}
}

