using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyPrefab;
	public float width = 10f;
	public float hight = 5f;
	public float speed = 15f;
	public float spawnDelay = 0.5f;

	private bool movingRight = false;
	private float xMax;
	private float xMin;
	// Use this for initialization
	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint (new Vector3(0,0,distance));
		Vector3 RightMost = Camera.main.ViewportToWorldPoint (new Vector3(1,0,distance));
		xMin = leftMost.x + 0.5f;
		xMax = RightMost.x + 0.5f;
		SpawnUntilFull ();
	}

	public void OnDrawGizmos(){
		Gizmos.DrawWireCube (transform.position, new Vector3 (width, hight, 0));
	}

	// Update is called once per frame
	void Update () {
		if (movingRight) {
			transform.position += Vector3.right * speed * Time.deltaTime;
		} else {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}

		float rightEdgeOfFormation = transform.position.x + (0.5f * width);
		float leftEdgeOfFormation = transform.position.x - (0.5f * width);
		if (leftEdgeOfFormation <= xMin){
			movingRight = true;
		} else if (rightEdgeOfFormation >= xMax){
			movingRight = false;
		}

		if(AllMembersDead()){
			SpawnUntilFull ();
		}
	}

	#region Obsolete enemy spawner
	void SpawnEnemies(){
		//spawn enemy and child it to Enemy formation

		foreach (Transform child in transform) {
			GameObject enemy = Instantiate (enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;

		}
	}
	#endregion

	Transform NextFreePosition(){
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount <= 0) {
				return childPositionGameObject;
			}

		}
		return null;
	}

	bool AllMembersDead(){
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) {
				return false;
			}
	
		}
		return true;
	}

	void SpawnUntilFull(){
		Transform freePos = NextFreePosition ();
		if (freePos) {
			GameObject enemy = Instantiate (enemyPrefab, freePos.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePos;
		}
		if(NextFreePosition()){
			Invoke ("SpawnUntilFull",spawnDelay);
		}
	}

}
