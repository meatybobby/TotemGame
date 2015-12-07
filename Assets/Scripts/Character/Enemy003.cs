using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy003 : Enemy {
	
	public float fireRange = 3.0f;
	public float minRange = 0.05f;
	public float fireSpeed = 3.0f;
	public float firePeriod = 1.0f;

	private GameObject tongue;
	private bool fire;
	private bool collideSomething;
	private float countDown;

	// Use this for initialization
	void Start () {
		Debug.Log ("enemy003 start!");
		player = GameObject.FindWithTag ("Player").GetComponent<Player> ();
		tongue = transform.FindChild ("Tongue").gameObject;
		disMap = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
		shapeVector = new List<IntVector2> ();
		shapeVector.Add (new IntVector2(0,0));
		mapUpdated = true;
		fire = false;
		pace = 0;
		countDown = 0.0f;
		attackPriority = new float[] {1,1,1};
		Map.Create(this);
		Rotate(dir);

	}

	void Update() {
		if (HP>0 && !inMoveThread && fire == false) {
			List<IntVector2> dirVec = new List<IntVector2>();
			RaycastHit2D hit;
			dirVec.Add(Direction.LEFT);
			dirVec.Add(Direction.RIGHT);
			dirVec.Add(Direction.UP);
			dirVec.Add(Direction.DOWN);
			for (int i = 0; i < dirVec.Count; i++) {
				IntVector2 temp = dirVec[i];
				int randomIndex = Random.Range(i, dirVec.Count);
				dirVec[i] = PathFinder.four_dir[randomIndex];
				dirVec[randomIndex] = temp;
			}

			foreach(IntVector2 dirTemp in dirVec){
				hit = Physics2D.Raycast(new Vector2(tongue.transform.position.x,tongue.transform.position.y),new Vector2(dirTemp.x,dirTemp.y) ,fireRange);
				//Debug.DrawLine(tongue.transform.position,tongue.transform.position+new Vector3(dirVec[i].x,dirVec[i].y,0.0f)*3);
				if (hit.collider != null) {
					//Debug.Log(tongue.transform.position+" "+hit.point);
					//Debug.Log(hit.collider.transform.name);
					//Debug.DrawLine(tongue.transform.position,hit.point);
					Rotate (dirTemp);
					fire = true;
					StartCoroutine( LongAttack (new Vector3(hit.collider.transform.position.x-tongue.transform.position.x,hit.collider.transform.position.y-tongue.transform.position.y,0.0f).normalized));
					break;
				}
			}
			if(fire == false){
				if(mapUpdated == true)
				{
					FindDirection(player.pos);
				}
				//Debug.Log(pace + "," + disMap[targetPos.x, targetPos.y] + "/" + guide[pace].x + "," + guide[pace].y);
				if (pace < disMap [targetPos.x, targetPos.y] - 1) {
					//Debug.Log("move like jagger");
					// Debug.Log("->"+guide[pace].x+" "+ guide[pace].y);
					if (guide [pace] != dir) {
						Rotate (guide [pace]);
					}
					MoveByVector (guide [pace]);
					pace++;
				}
				else if(pace == disMap [targetPos.x, targetPos.y] - 1){
					if (guide [pace] != dir) {
						Rotate (guide [pace]);
					}
				}
			}
		}
	}

	public void setCollideSomethingTrue(){
		collideSomething = true;
	}

	protected IEnumerator LongAttack(Vector3 attackDir){
		if (countDown > 0){
			yield return new WaitForSeconds (countDown);
		}
		countDown = firePeriod;
		collideSomething = false;
		Vector3 initTonguePos = new Vector3 (tongue.transform.position.x,tongue.transform.position.y,tongue.transform.position.z);
		//Debug.Log(attackDir +" "+ fireSpeed +" "+ Time.deltaTime);
		//Vector3 attackVec = attackDir * fireSpeed * Time.deltaTime;
		//Debug.Log("attackVec( "+attackVec.x+", "+attackVec.y+")");
		//Debug.Log(tongue.transform.position == initTonguePos);
		while(Vector2.Distance(new Vector2(tongue.transform.position.x,tongue.transform.position.y),new Vector2(transform.position.x,transform.position.y)) < fireRange) {
			countDown -= Time.deltaTime;
			tongue.transform.position += attackDir * fireSpeed * Time.deltaTime;
			if(collideSomething){
				break;
			}
			yield return null;
		}
		while(Vector3.Distance(tongue.transform.position,initTonguePos) > minRange){
			countDown -= Time.deltaTime;
			tongue.transform.position -= attackDir * fireSpeed * Time.deltaTime;
			yield return null;
		}
		tongue.transform.position = initTonguePos;
		fire = false;
	}

	public void Rotate(IntVector2 a){
		dir = a;
		int angle;
		angle = a.x==0? (90*a.y):(90 - 90*a.x + 45*a.x*a.y);
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, (float)angle+90.0f);
	}
}
