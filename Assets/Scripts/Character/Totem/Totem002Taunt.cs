using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//嘲諷區域
public class Totem002Taunt : MonoBehaviour {
	
	private Dictionary<System.Type,float> TauntList;
	public Totem002 totem002;


	void Start () {
		TauntList = new Dictionary<System.Type,float> ();
		totem002 = GetComponentInParent<Totem002> ();
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log (other.tag);
		if (other.tag == "Enemy") {
			//Debug.Log("Taunt");
			totem002.anim.SetTrigger("isTaunt");
			Enemy enemy = other.gameObject.GetComponent<Enemy> ();
			if(!TauntList.ContainsKey(enemy.GetType()))
			   TauntList.Add(enemy.GetType(),enemy.GetPriority(2));
			enemy.SetPriority (2,0);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Enemy") {
			//Debug.Log ("Enenmy exit collider!");
			totem002.anim.SetTrigger("isIdle");
			Enemy enemy = other.gameObject.GetComponent<Enemy> ();
			if(TauntList.ContainsKey(enemy.GetType())){
				enemy.SetPriority (2,TauntList[enemy.GetType()]);
			}
		}
	}
}
