using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

	public float health = 150f;
	public float projectileSpeed = 15f;
	public GameObject projectile;
	public float shotsPerSecond = 0.5f;
	public int scoreValue = 100;

	public AudioClip fireSound;
	public AudioClip death;

	private ScoreKeeper scoreKeeper;

	void Start(){
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScoreKeeper> ();
	}

	void Die(){
		Destroy (gameObject);
		scoreKeeper.Score (scoreValue);
		AudioSource.PlayClipAtPoint (death, transform.position);
	}

	void OnTriggerEnter2D(Collider2D col){
		Shot missle = col.gameObject.GetComponent<Shot>();
		if (missle) {
			health -= missle.GetDamage ();
			missle.Hit ();
			if (health <= 0){
				Die ();
			}
		}
	}

	void Fire(){
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
		GameObject missle = Instantiate (projectile, transform.position, Quaternion.identity) as GameObject;
		missle.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -projectileSpeed);
	}

	void Update(){
		//fire based on probability
		float prob = Time.deltaTime * shotsPerSecond;
		if (Random.value < prob) {
			Fire ();
		}
	}

}
