using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	float xmin = -5;
	float xmax = 5;

	public float health = 100f;
	public float speed = 15f;
	public float projectileSpeed = 1f;
	public float padding = 1f;
	public GameObject shot;
	public float fireRate = 1f;

	public AudioClip fireSound;

	// Use this for initialization
	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint (new Vector3(0,0,distance));
		Vector3 RightMost = Camera.main.ViewportToWorldPoint (new Vector3(1,0,distance));
		xmin = leftMost.x + padding;
		xmax = RightMost.x - padding;
	}

	void Fire(){
		AudioSource.PlayClipAtPoint (fireSound, transform.position);
		Vector3 offset = new Vector3 (0, 1, 0);
		GameObject lazer = Instantiate (shot, transform.position+ offset, Quaternion.identity) as GameObject;
		lazer.GetComponent<Rigidbody2D> ().velocity = new Vector3(0, projectileSpeed,0);
	}

	// Update is called once per frame
	void Update () {

		//move ship
		if (Input.GetKey (KeyCode.LeftArrow)) {
			
			//transform.position += new Vector3 (-speed * Time.deltaTime, 0, 0);
			transform.position += Vector3.left * speed * Time.deltaTime;

		} else if (Input.GetKey (KeyCode.RightArrow)) {
			
			//transform.position += new Vector3 (speed* Time.deltaTime, 0, 0);
			transform.position += Vector3.right * speed * Time.deltaTime;

		}

		//Fire projectile
		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("Fire", 0.00001f, fireRate);
			}
		if (Input.GetKeyUp(KeyCode.Space)){
			CancelInvoke ("Fire");
		}

		//restrict player to playspace
		float newX = Mathf.Clamp (transform.position.x, xmin, xmax);
		transform.position = new Vector2 (newX, transform.position.y);
	}

	void OnTriggerEnter2D(Collider2D col){
		Shot missle = col.gameObject.GetComponent<Shot>();
		if (missle) {
			health -= missle.GetDamage ();
			missle.Hit ();
			if (health <= 0){
				Destroy (gameObject);
			}
		}
	}
}
