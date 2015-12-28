using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Player : Character {
	
	public const int IDLE = 0;
	public const int CATCH = 1;
	public int mode;
	public float opacity = 0.2f;

	//public Transform totemSpawn;
	public float holdTimePara = 0.5f;
	public float joyStickSensitivity = 0.5f;
	public IntVector2 holdDir;
	
	public Totem caughtTotem;
	//private IntVector2 oldDir;
	private Joystick joyStick;
	private Animator anim;
	private float holdTime;
	private Color temp;
	public TotemSummoner summoner;

	public GameObject sweatEffect;
	public GameObject playerStone;
	public GameObject catchIcon;

	
	void Start () {
		mode = IDLE;
		//Map.mainMap [1, 1].Add (this);
		Map.Create (this);
		//oldDir = dir;
		anim = GetComponent<Animator>();
		joyStick = GameObject.FindWithTag ("JoyStick").GetComponent<Joystick> ();
		Rotate (dir);
		holdTime = 0f;
		characterId = 0;
		summoner = GetComponent<TotemSummoner> ();
		temp = catchIcon.GetComponent<Image>().color;
		temp.a = opacity;
		catchIcon.GetComponent<Image> ().color = temp;
	}
	
	void Update () {
		//float moveH = Input.GetAxis ("Horizontal"); //PC
		float moveH = CrossPlatformInputManager.GetAxis("Horizontal");
		//float moveV = Input.GetAxis ("Vertical"); //PC
		float moveV = CrossPlatformInputManager.GetAxis("Vertical");
		
		float length = Mathf.Sqrt (moveH * moveH + moveV * moveV);
		float angle = Mathf.Atan2 (moveV, moveH);
		//print ("len:"+length+" angle:"+angle*Mathf.Rad2Deg);
		//print (moveH + ", " + moveV + " angle: " + angle);
		
		if (!inMoveThread) {
			IntVector2 movement = new IntVector2(1,0);
			if(length > 0.0f) {
				float offset = Mathf.PI/20;
				if( Mathf.Abs (angle) <= Mathf.PI/8+offset) {
					movement = Direction.RIGHT;
				}
				else if( angle >= Mathf.PI/8+offset && angle < Mathf.PI*3/8-offset ) {
					movement = Direction.UP_RIGHT;
				}
				else if( angle >= Mathf.PI*3/8-offset && angle < Mathf.PI*5/8+offset ) {
					movement = Direction.UP;
				}
				else if( angle >= Mathf.PI*5/8+offset && angle < Mathf.PI*7/8-offset ) {
					movement = Direction.UP_LEFT;
				}
				else if(angle >= Mathf.PI*7/8-offset || angle < -Mathf.PI*7/8+offset) {
					movement = Direction.LEFT;
				}
				else if( angle >= -Mathf.PI*7/8+offset && angle < -Mathf.PI*5/8-offset ) {
					movement = Direction.DOWN_LEFT;
				}
				else if( angle >= -Mathf.PI*5/8-offset && angle < -Mathf.PI*3/8+offset ) {
					movement = Direction.DOWN;
				}
				else if( angle >= -Mathf.PI*3/8+offset && angle < Mathf.PI/8-offset ) {
					movement = Direction.DOWN_RIGHT;
				}
				else{
					Debug.Log ("No angle match!! "+angle);
				}
				
				if(length >= joyStickSensitivity) { // trigger move
					if (mode == CATCH) {
						if(Mathf.Abs(movement.x) == 1) {
							movement = movement.x==1 ? Direction.RIGHT : Direction.LEFT;
						}
						if(!isMoving) {
							isMoving  = true;
							ChangeAnimation(dir);
						}
						MoveByVector (movement);
					}
					else {
						if(movement!=dir || !isMoving){
							isMoving  = true;
							Rotate (movement);
						}
						
						MoveByVector (movement);
					}
				}
				else { // stop move animation or rotate only
					if(Mathf.Abs(movement.x) == 1) {
						// allow only four directions when IDLE
						movement = movement.x==1 ? Direction.RIGHT : Direction.LEFT;
					}
					
					if(isMoving) {
						isMoving = false;
						if(mode!=CATCH) {
							// change dir to only four directions
							
							Rotate(movement);
						} else{
							// don't change dir when CATCH
							ChangeAnimation(dir);
						}
					}
					else if(movement != dir && mode!=CATCH) {
						Rotate(movement);
					}
					
				}
			}
			else{
				if(Mathf.Abs(dir.x) == 1) {
					// allow only four directions when IDLE
					dir = dir.x==1 ? Direction.RIGHT : Direction.LEFT;
					
				}
				if(isMoving) {
					isMoving = false;
					ChangeAnimation(dir);
				}
			}
		}
		/*
		if (!(Mathf.Abs (moveH) == 0.0f && Mathf.Abs (moveV) == 0.0f) && !inMoveThread) {
			
			int unitH = moveH == 0.0f ? 0 : (moveH > 0.0f ? 1 : -1); //PC
			//int unitH = Mathf.Abs (moveH) < 0.5f ? 0 : (moveH > 0.0f ? 1 : -1);
			int unitV = moveV == 0.0f ? 0 : (moveV > 0.0f ? 1 : -1); //PC
			//int unitV = Mathf.Abs (moveV) < 0.5f ? 0 : (moveV > 0.0f ? 1 : -1);
			
			IntVector2 movement = new IntVector2 (unitH, unitV);
			
			if ((movement != dir && mode != CATCH) ) {
				Rotate (movement);
 			} else if ( movement==oldDir || (Mathf.Abs (moveH) >= 0.5f || Mathf.Abs (moveV) >= 0.5f) ) {

				// If move toward the same direction or key hold for long enough
				// move the player
				
				if (mode == CATCH) {
					// Slant move is not allowed when CATCH
					if (Mathf.Abs (movement.x + movement.y) == 1) {
						if(!isMoving) {
							isMoving  = true;
							ChangeAnimation(dir);
						}
						MoveByVector (movement);
					}
					
				} else {
					if(!isMoving) {
						isMoving  = true;
						Rotate (movement);
					}
					MoveByVector (movement);
				}
			}
		}
		
		else if( Mathf.Abs (moveH) == 0.0f && Mathf.Abs (moveV) == 0.0f  && !inMoveThread) {
			if(isMoving) {
				isMoving = false;

				Debug.Log ("Stop moving!"+" isMoving: "+isMoving.ToString());
				//anim.SetTrigger ("back_idle");
				ChangeAnimation(dir);

			}
			
			oldDir = dir;
		}*/
		
		// hold press a period of time
		/*if (joyStick.axisPressState && length <  joyStickSensitivity  && !inMoveThread && !isMoving) {
			if(holdTime == 0f)
				holdDir = dir;
			else if(holdDir != dir) {
				holdTime = 0f;
			}
			holdTime = holdTime + Time.deltaTime;
		} 
		else if(!joyStick.axisPressState) {
			holdTime = 0f;
		}*/

		//Catch Icon opacity
		if(mode == IDLE)
			temp.a = opacity;
		else
			temp.a = 1.0f;
		catchIcon.GetComponent<Image> ().color = temp;
		if (mode == IDLE && Mathf.Abs (dir.x + dir.y) == 1) {
			IntVector2 actionPos = pos + dir;
			List<Character> charList = Map.Seek (actionPos);
			foreach (Character c in charList) {
				// If there's a totem in front of Player
				if (c is Totem) {
					temp.a = 1.0f;
					catchIcon.GetComponent<Image> ().color = temp;
					break;
				}
			}
		} 
		// Catch or release the totem when hold press
		if (Input.GetKeyDown (KeyCode.Space)) {
			if (mode == IDLE && /*holdTime > holdTimePara &&*/ !inMoveThread && !isMoving && Mathf.Abs (dir.x + dir.y) == 1) {
				IntVector2 actionPos = pos + dir;
				List<Character> charList = Map.Seek (actionPos);
				foreach (Character c in charList) {
					// If there's a totem in front of Player
					if (c is Totem) {
						//Debug.Log("Caught");
						caughtTotem = (Totem)c;
						caughtTotem.CaughtByPlayer ();
						mode = CATCH;
						break;
					}
				}
			} else if (mode == CATCH && /*holdTime < holdTimePara &&*/ !inMoveThread && !isMoving) {
				//Debug.Log("Dismiss");
				caughtTotem.ReleasedByPlayer ();
				caughtTotem = null;
				mode = IDLE;
			}
		}
		// Testing the MoveByVectorArray function by pressing 'M'
		if (Input.GetKeyDown (KeyCode.M) && !isMoving && mode != CATCH) {
			List<IntVector2> vecList = new List<IntVector2>();
			// right, right, up, up, left
			vecList.Add(new IntVector2(1,0));
			vecList.Add(new IntVector2(1,0));
			vecList.Add(new IntVector2(0,1));
			vecList.Add(new IntVector2(0,1));
			vecList.Add(new IntVector2(-1,0));
			vecList.Add(new IntVector2(1,0));
			vecList.Add(new IntVector2(1,0));
			vecList.Add(new IntVector2(0,1));
			vecList.Add(new IntVector2(0,1));
			float newSpeed = 10.0f;
			StartCoroutine(MoveByVectorArray(vecList, newSpeed));
		}
		
		if (HP <= 0) {
			Debug.Log ("The Player is fukcing dead!");
			Die();

		}
	}

	private void Die(){
		Debug.Log (transform.position);
		Instantiate (playerStone, this.transform.position, this.transform.rotation);
		Map.Destroy (this);
		Destroy (gameObject);
	}

	public void MoveByVector(IntVector2 offset) {
		IntVector2 newPos = Map.BoundPos(pos+offset);
		if (mode == CATCH) {
			
			IntVector2 totemNewPos = Map.BoundPos (pos + dir + offset);
			if (offset == dir) { // forward
				if ( !Map.IsEmpty(totemNewPos) ) {
					return;
				}
			} else if (offset == -dir) { // backward
				if ( !Map.IsEmpty(newPos) )
					return;
			} else { // 平移
				if ( !Map.IsEmpty(totemNewPos) || !Map.IsEmpty(newPos) )
					return;
			}
			// Update the pos of the caught totem
			IntVector2 totemPre = new IntVector2(caughtTotem.pos.x, caughtTotem.pos.y);
			caughtTotem.pos = totemNewPos; 
			Map.UpdatePos (caughtTotem, totemPre);
			
		} else if (!Map.IsEmpty(newPos)) {
			return;
		}
		
		// Update the main-map position first
		IntVector2 pre = new IntVector2(pos.x, pos.y);
		pos = newPos; 
		Map.UpdatePos (this, pre);
		Vector3 next = Map.GetRealPosition(newPos, this.GetType());
		StartCoroutine(MoveThread (next));
	}
	
	public void Rotate(IntVector2 a) {
		
		dir = a;
		ChangeAnimation (a);
		
	}
	
	public void Summon(int totemType)
	{
		// Plant a totem when pressing left ctrl and not moving and not slant
		if (/*Input.GetKeyDown (KeyCode.LeftControl) &&*/ !inMoveThread &&
		    (Mathf.Abs (dir.x + dir.y) == 1)) {
			summoner.Summon (totemType, pos + dir, dir);
		}
	}
	
	public void Catch(){
		if (mode == IDLE && !inMoveThread &&!isMoving && Mathf.Abs(dir.x+dir.y)==1 ) {
			IntVector2 actionPos = pos + dir;
			List<Character> charList = Map.Seek (actionPos);
			foreach (Character c in charList) {
				// If there's a totem in front of Player
				if (c is Totem) {
					//Debug.Log("Caught");
					caughtTotem = (Totem)c;
					caughtTotem.CaughtByPlayer ();
					mode = CATCH;
					break;
				}
			}
		}
		else if(mode == CATCH && !inMoveThread && !isMoving) {
			//Debug.Log("Dismiss");
			caughtTotem.ReleasedByPlayer();
			caughtTotem = null;
			mode = IDLE;
		}
	}
	
	public void ChangeAnimation(IntVector2 a) {
		if (isMoving) {
			if(a == Direction.LEFT) {
				anim.SetTrigger ("left_run");
			} else if(a == Direction.RIGHT) {
				anim.SetTrigger ("right_run");
			} else if(a == Direction.DOWN) {
				anim.SetTrigger ("front_run");
			} else if(a == Direction.UP){
				anim.SetTrigger ("back_run");
			}
			else if(a == Direction.UP_LEFT){
				anim.SetTrigger ("left_run");
			}
			else if(a == Direction.UP_RIGHT) {
				anim.SetTrigger ("right_run");
			}
			else if(a == Direction.DOWN_LEFT) {
				anim.SetTrigger ("left_run");
			}
			else if(a == Direction.DOWN_RIGHT) {
				anim.SetTrigger ("right_run");
			}
			
		} else {
			if (a == Direction.LEFT) {
				anim.SetTrigger ("left_idle");
			} else if (a == Direction.RIGHT) {
				anim.SetTrigger ("right_idle");
			} else if (a == Direction.UP) {
				anim.SetTrigger ("back_idle");
			} else if (a == Direction.DOWN) {
				anim.SetTrigger ("front_idle");
			}
			else if(a == Direction.UP_LEFT){
				anim.SetTrigger ("left_idle");
			}
			else if(a == Direction.UP_RIGHT) {
				anim.SetTrigger ("right_idle");
			}
			else if(a == Direction.DOWN_LEFT) {
				anim.SetTrigger ("left_idle");
				
			}
			else if(a == Direction.DOWN_RIGHT) {
				anim.SetTrigger ("right_idle");
			}
		}
	}
	
	public void SetIdle(){
		this.mode = IDLE;
	}
	
	
	void OnTriggerEnter2D(Collider2D other) {
		// Destroy everything that leaves the trigger
		
		
	}
	
	protected IEnumerator MoveThread(Vector3 next) {
		bool playerCatch = false;
		if(mode==CATCH) {
			playerCatch = true;
			caughtTotem.inMoveThread = true;
		}
		
		// 往下走，z值先更新 （解決斜走重疊的問題）
		if (transform.position.y > next.y) {
			transform.position = new Vector3(transform.position.x, transform.position.y, next.z);
		}
		Vector3 nextXY = new Vector3 (next.x, next.y, transform.position.z); // 不要移動z
		inMoveThread = true;
		while (transform.position != nextXY) {
			transform.position = Vector3.MoveTowards(transform.position, nextXY, speed * Time.deltaTime);
			yield return null;
		}
		inMoveThread = false;
		transform.position = next;//new Vector3( next.x, next.y, next.z);
		
		if (playerCatch) {
			caughtTotem.inMoveThread = false;
		}
	}
	
	
	
}