using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TotemSummoner : MonoBehaviour
{
	public int mana;
	public int manaMax;
	public int[] cost;
	public GameObject[] totem;
	public int totemNum;
	public const int maxTotemNum = 1000;
	public Player player;
	public Text manaDisplay;
	public GameObject buttonPanel;
	public GameObject manaView;
	public float opacity = 0.2f;
	private float reManaTime;
	private RectTransform maskRect,manaRect;
	private Vector2 originSize;
	private Vector2 originPos;
	private GameObject[] buttons;

	// Use this for initialization
	void Start ()
	{
		totemNum = 0;
		mana = manaMax;
		player = GetComponent<Player> ();
		reManaTime = 0;
		manaRect = manaView.GetComponent<RectTransform> ();
		maskRect = manaView.transform.parent.GetComponent<RectTransform> ();
		originSize = maskRect.sizeDelta;
		originPos = maskRect.anchoredPosition;
		buttons = new GameObject[totem.Length];
		int count = 0;
		foreach (Transform child in buttonPanel.transform){
			buttons[count] = child.gameObject;
			Color temp = buttons[count].GetComponent<Image>().color;
			temp.a = opacity;
			buttons[count].GetComponent<Image>().color = temp;
			count++;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		reManaTime += Time.deltaTime;
		if (reManaTime >= 3) {
			reManaTime = 0;
			addMana(1);
		}
		drawMana ();
		for(int i=0; i<cost.Length; i++){
			Color temp = buttons[i].GetComponent<Image>().color;
			if (mana >= cost [i])
				temp.a = 1.0f;
			else
				temp.a = opacity;
			buttons[i].GetComponent<Image>().color = temp;
		}
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
			case 3:
				totemObj = Instantiate (totem[3], totemRealPos, Quaternion.Euler (0f, 0f, 0f)) as GameObject;
				Totem004 newTotem4 = totemObj.GetComponent<Totem004> ();
				newTotem4.pos = pos;
				newTotem4.isCaught = false;
				newTotem4.playerRef = player;
				Map.Create (newTotem4);
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
		if (mana > manaMax)
			mana = manaMax;
	}

	private void drawMana() {
		float rate = (float)mana / manaMax;
		maskRect.sizeDelta = new Vector2 (originSize.x, originSize.y * rate);
		maskRect.anchoredPosition = new Vector2 (maskRect.anchoredPosition.x, originPos.y - originSize.y * (1 - rate));
		if(mana < 10)
			manaDisplay.text = " "+mana.ToString();
		else
			manaDisplay.text = mana.ToString();
	}
}

