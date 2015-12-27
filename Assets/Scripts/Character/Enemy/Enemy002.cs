using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy002 : Enemy {
	
	public float fireRange = 3.0f;
	public float minRange = 0.1f;
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
		attackPriority = new float[] {1,1,1,1,1};
		defaultPriority = new float[] {1,1,1,1,1};
		shapeVector = new List<IntVector2> ();
		shapeVector.Add (new IntVector2(0,0));
		anim = GetComponent<Enemy002Anim> ();
		tongue = transform.FindChild ("Tongue").gameObject;
		tongueBody = tongue.transform.FindChild ("TongueBody").gameObject;
		fire = false;
		countDown = 0.0f;
		sprite = tongueBody.GetComponent<SpriteRenderer>();
		spriteLen = new Vector2 (sprite.bounds.extents.x*13f,sprite.bounds.extents.y);
	}
	
	void Update() {
		//Hp GUI
		HpUpdate ();
		if (HP <= 0 && !isDead) {
			SetAnimation(DIE);
			Die ();
		}
		if (HP>0 && !inMoveThread && fire == false && player!=null) {

			targetFound = false;
			bool toHitTotem002 = false;
			IntVector2 targetDir = new IntVector2(0,0);

			// if angy, hit only totem002
			if(angryNum > 0) { 
				targetDir = Map.FindTotem002ForEnemy002(pos);
				if(!(targetDir.x==0 && targetDir.y==0)){
					toHitTotem002 = true;
					targetFound = true;
				}

			}

			Vector2 tonguePos = new Vector2(tongue.transform.position.x,tongue.transform.position.y);
			if(toHitTotem002) {
				if(dir!=targetDir) Rotate (targetDir);
				fire = true;
				isMoving = false;
				SetAnimation (ATTACK);
				attackWait = dir == Direction.DOWN ? 1.3f : 0.8f;
				RaycastHit2D hit = Physics2D.Raycast(tonguePos, new Vector2(targetDir.x, targetDir.y), fireRange);
				StartCoroutine( LongAttack (new Vector3(hit.point.x-tongue.transform.position.x,
				                                        hit.point.y-tongue.transform.position.y,
				                                        0.0f).normalized));
			}
			else {
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
				
				//Debug.Log ("Start Raycast!");
				for(idx=0; idx<dirVec.Count; idx++) {

					Vector2 hitDir = new Vector2(dirVec[idx].x, dirVec[idx].y);
					
					//List<Character> charList = Map.MapRayCast(pos, dirVec[idx]);
					/*Character attackTarget = Map.MapRayCast(pos, dirVec[idx]);
				if(attackTarget!=null) {
					Debug.Log ("target: "+attackTarget.GetType());
					targetFound = true;
					if(dir!=dirVec[idx]) Rotate (dirVec[idx]);
					fire = true;
					//tongue.SetActive(true);
					SetAnimation (ATTACK);
					attackWait = dir == Direction.DOWN ? 1.3f : 0.8f;
					Vector3 hitPos = Map.GetRealPosition(attackTarget.pos, attackTarget.GetType() );

					StartCoroutine( LongAttack (new Vector3(hitPos.x-tongue.transform.position.x,
					                                        hitPos.y-tongue.transform.position.y,
					                                        0.0f).normalized));
					break;
				}*/
					
					hit = Physics2D.Raycast(tonguePos, hitDir, fireRange);
					if (hit.collider != null 
					    && (hit.collider.tag=="Player" || hit.collider.tag=="Totem")
					    && Map.isInBounded(pos)) {
						//Debug.Log(tongue.transform.position+" "+hit.point);
						//Debug.Log("Target: "+hit.collider.transform.name);
						Debug.DrawLine(tongue.transform.position,hit.point);
						
						targetFound = true;
						
						if(dir!=dirVec[idx]) Rotate (dirVec[idx]);
						fire = true;
						isMoving = false;
						SetAnimation (ATTACK);
						attackWait = dir == Direction.DOWN ? 1.3f : 0.8f;
						StartCoroutine( LongAttack (new Vector3(hit.point.x-tongue.transform.position.x,
						                                        hit.point.y-tongue.transform.position.y,
						                                        0.0f).normalized));
						break;
					}
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
		Vector3 initTonguePos = new Vector3 (tongue.transform.position.x,
		                                     tongue.transform.position.y,
		                                     tongue.transform.position.z);
		Vector3 initTongueBodyScale = new Vector3 (tongueBody.transform.localScale.x, 
		                                          tongueBody.transform.localScale.y,
		                                          tongueBody.transform.localScale.z);
		//Debug.Log(attackDir +" "+ fireSpeed +" "+ Time.deltaTime);
		//Vector3 attackVec = attackDir * fireSpeed * Time.deltaTime;
		//Debug.Log("attackVec( "+attackVec.x+", "+attackVec.y+")");
		//Debug.Log(tongue.transform.position == initTonguePos);
		Vector2 tongue2D = new Vector2(tongue.transform.position.x,tongue.transform.position.y);
		Vector2 transform2D = new Vector2 (transform.position.x, transform.position.y);
		while(Vector2.Distance(tongue2D,transform2D) < fireRange) {
			//countDown -= Time.deltaTime;
			if (isDead) {
				Destroy (tongue);
				yield break;
			}
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
			tongue2D = new Vector2(tongue.transform.position.x,tongue.transform.position.y);
			if(collideSomething){
				break;
			}
			yield return null;
		}
		while(Vector3.Distance(tongue.transform.position,initTonguePos) > minRange){
			//countDown -= Time.deltaTime;
			if (isDead) {
				Destroy (tongue);
				yield break;
			}
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
		tongueBody.transform.localScale = initTongueBodyScale;
		SetAnimation (IDLE);
		tongue.SetActive(false);
		yield return new WaitForSeconds (attackIntv);
		fire = false;
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
