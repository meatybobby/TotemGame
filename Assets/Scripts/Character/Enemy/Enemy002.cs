using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy002 : Enemy {
	
	public float fireRange = 3.0f;
	public float minRange = 0.05f;
	public float fireSpeed = 3.0f;
	public float firePeriod = 1.0f;
	public float attackWait;
	
	private GameObject tongue;
	private GameObject tongueBody;
	private bool fire;
	private bool collideSomething;
	private float countDown;
	private SpriteRenderer sprite;
	private Vector2 spriteLen;

	private Enemy002Anim anim;

	private bool targetFound = false;

	void Start () {
		Initialize ();
		shapeVector = new List<IntVector2> ();
		shapeVector.Add (new IntVector2(0,0));
		anim = GetComponent<Enemy002Anim> ();
		tongue = transform.FindChild ("Tongue").gameObject;
		tongueBody = tongue.transform.FindChild ("TongueBody").gameObject;
		fire = false;
		countDown = 0.0f;
		sprite = tongueBody.GetComponent<SpriteRenderer>();
		spriteLen = new Vector2 (sprite.bounds.extents.x*20f,sprite.bounds.extents.y);
	}
	
	void Update() {
		if (HP <= 0 && !isDead) {
			SetAnimation(DIE);
			Die ();
		}
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
			int idx;
			targetFound = false;
			//Debug.Log ("Start Raycast!");
			for(idx=0; idx<dirVec.Count; idx++) {
				Vector2 tonguePos = new Vector2(tongue.transform.position.x,tongue.transform.position.y);
				Vector2 hitDir = new Vector2(dirVec[idx].x, dirVec[idx].y);

				/*List<Character> charList = Map.MapRayCast(pos, dirVec[idx]);
				if(charList.Count > 0) {
					Debug.Log ("target: "+charList[0].GetType());
					targetFound = true;
					if(dir!=dirVec[idx]) Rotate (dirVec[idx]);
					fire = true;
					//tongue.SetActive(true);
					SetAnimation (ATTACK);
					attackWait = dir == Direction.DOWN ? 1.3f : 0.8f;
					Vector3 hitPos = Map.GetRealPosition(charList[0].pos, charList[0].GetType() );

					StartCoroutine( LongAttack (new Vector3(hitPos.x-tongue.transform.position.x,
					                                        hitPos.y-tongue.transform.position.y,
					                                        0.0f).normalized));
					break;
				}*/

				hit = Physics2D.Raycast(tonguePos, hitDir, fireRange);
				if (hit.collider != null && hit.collider.tag!="Enemy" && Map.isInBounded(pos)) {
					//Debug.Log(tongue.transform.position+" "+hit.point);
					Debug.Log("Target: "+hit.collider.transform.name);
					Debug.DrawLine(tongue.transform.position,hit.point);

					targetFound = true;

					if(dir!=dirVec[idx]) Rotate (dirVec[idx]);
					fire = true;

					SetAnimation (ATTACK);
					attackWait = dir == Direction.DOWN ? 1.3f : 0.8f;
					StartCoroutine( LongAttack (new Vector3(hit.point.x-tongue.transform.position.x,
					                                        hit.point.y-tongue.transform.position.y,
					                                        0.0f).normalized));
					break;
				}
			}

			if(!targetFound/*idx == dirVec.Count*/) {
				if(mapUpdated == true)
				{
					FindDirection(player.pos);
				}
				//Debug.Log(pace + "," + disMap[targetPos.x, targetPos.y] + "/" + guide[pace].x + "," + guide[pace].y);
				if (pace < disMap [targetPos.x, targetPos.y] - 1) {
					//Debug.Log("move like jagger");
					// Debug.Log("->"+guide[pace].x+" "+ guide[pace].y);

					if (guide [pace] != dir || !isMoving) {
						isMoving = true;
						Rotate (guide [pace]);
					}

					MoveByVector (guide [pace]);
					pace++;
				}
				else if(pace == disMap [targetPos.x, targetPos.y] - 1){
					isMoving = false;
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
		/*if (countDown > 0){
			yield return new WaitForSeconds (countDown);
		}*/
		yield return new WaitForSeconds (attackWait);
		if (dir == Direction.UP) {
			tongue.GetComponent<Enemy002Tongue> ().SetZPos (transform.position.z - 1.3f);
		} else if (dir == Direction.DOWN) {
			tongue.GetComponent<Enemy002Tongue> ().SetZPos (transform.position.z - 2.0f);
		} else {
			tongue.GetComponent<Enemy002Tongue> ().SetZPos (transform.position.z - 1.6f);
		}
		tongue.SetActive(true);
		//countDown = firePeriod;
		collideSomething = false;
		Vector3 initTonguePos = new Vector3 (tongue.transform.position.x,tongue.transform.position.y,tongue.transform.position.z);
		//Debug.Log(attackDir +" "+ fireSpeed +" "+ Time.deltaTime);
		//Vector3 attackVec = attackDir * fireSpeed * Time.deltaTime;
		//Debug.Log("attackVec( "+attackVec.x+", "+attackVec.y+")");
		//Debug.Log(tongue.transform.position == initTonguePos);
		while(Vector2.Distance(new Vector2(tongue.transform.position.x,tongue.transform.position.y),new Vector2(transform.position.x,transform.position.y)) < fireRange) {
			//countDown -= Time.deltaTime;
			tongue.transform.position += attackDir * fireSpeed * Time.deltaTime;
			if(attackDir.y == 0.0f){
				tongueBody.transform.position = new Vector3((tongue.transform.position.x + initTonguePos.x) / 2,
				                                            tongueBody.transform.position.y,
				                                            tongueBody.transform.position.z);
				tongueBody.transform.localScale = new Vector3(tongueBody.transform.localScale.x,
				                                              Mathf.Abs((tongue.transform.position.x - initTonguePos.x)/spriteLen.x),
				                                              tongueBody.transform.localScale.z);
			}
			else{
				tongueBody.transform.position = new Vector3(tongueBody.transform.position.x,
				                                            (tongue.transform.position.y + initTonguePos.y) / 2,
				                                            tongueBody.transform.position.z);
				tongueBody.transform.localScale = new Vector3(tongueBody.transform.localScale.x,
				                                              Mathf.Abs((tongue.transform.position.y - initTonguePos.y)/spriteLen.x),
				                                              tongueBody.transform.localScale.z);
			}
			if(collideSomething){
				break;
			}
			yield return null;
		}
		while(Vector3.Distance(tongue.transform.position,initTonguePos) > minRange){
			//countDown -= Time.deltaTime;
			tongue.transform.position -= attackDir * fireSpeed * Time.deltaTime;
			if(attackDir.y == 0.0f){
				tongueBody.transform.position = new Vector3((tongue.transform.position.x + initTonguePos.x) / 2,
				                                            tongueBody.transform.position.y,
				                                            tongueBody.transform.position.z);
				tongueBody.transform.localScale = new Vector3(tongueBody.transform.localScale.x,
				                                              Mathf.Abs((tongue.transform.position.x - initTonguePos.x)/spriteLen.x),
				                                              tongueBody.transform.localScale.z);
			}
			else{
				tongueBody.transform.position = new Vector3(tongueBody.transform.position.x,
				                                            (tongue.transform.position.y + initTonguePos.y) / 2,
				                                            tongueBody.transform.position.z);
				tongueBody.transform.localScale = new Vector3(tongueBody.transform.localScale.x,
				                                              Mathf.Abs((tongue.transform.position.y - initTonguePos.y)/spriteLen.x),
				                                              tongueBody.transform.localScale.z);
				
			}
			yield return null;
		}
		tongue.transform.position = initTonguePos;
		SetAnimation (IDLE);
		tongue.SetActive(false);
		yield return new WaitForSeconds (attackIntv);
		fire = false;


		//SetAnimation (WALK);
	}
	
	public void Rotate(IntVector2 a){
		dir = a;
		int angle;
		angle = a.x==0? (90*a.y):(90 - 90*a.x + 45*a.x*a.y);
		tongue.transform.rotation = Quaternion.Euler (0.0f, 0.0f, (float)angle+90.0f);
		SetAnimation (WALK);
	}
	
	public void SetAnimation(int mode) {
		// this will cause error because before start()
		if (anim != null) {
			anim.playAnim (dir, mode);
		}
	}

}
