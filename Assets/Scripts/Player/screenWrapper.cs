using UnityEngine;
using System.Collections;

public class screenWrapper : MonoBehaviour {

	//Ghost Prefab
	public GameObject ghostPrefab;
	public PlayerBehaviour myBehaviour;
	public LifeCount myLife;

	//Ghost Characters on wrap (left, right)
	private GameObject[] ghosts = new GameObject[2];
	private cameraInfo cam;

	//Instantiate Ghost Prefab and pass in referenced Object (this)
	void createGhosts(){
		for (int i = 0; i < ghosts.Length; i++) {
			ghosts[i] = Instantiate(ghostPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			ghosts[i].gameObject.GetComponent<GhostBehaviour>().referencedAnim = gameObject.GetComponent<Animator>();
			ghosts[i].gameObject.GetComponent<GhostBehaviour>().referencedLife = myLife;
			ghosts [i].gameObject.GetComponent<GhostBehaviour> ().referencedPlayer = myBehaviour;
		}
	}

	//Update position/rotation/localScale of ghosts
	void positionGhosts(){
		Vector3 ghostPosition = transform.position;

		//Left Ghost
		ghostPosition.x = transform.position.x - cam.ScreenWidth;
		ghostPosition.y = transform.position.y;
		ghosts[0].transform.position = ghostPosition;
		ghosts[0].transform.rotation = transform.rotation;
		ghosts[0].transform.localScale = transform.localScale;

		//Right Ghost
		ghostPosition.x = transform.position.x + cam.ScreenWidth;
		ghostPosition.y = transform.position.y;
		ghosts[1].transform.position = ghostPosition;
		ghosts[1].transform.rotation = transform.rotation;
		ghosts[1].transform.localScale = transform.localScale;
	}

	//Swap object with Ghost once screen wrapped
	void swapWithGhost(){
		foreach (GameObject ghost in ghosts) {
			if (ghost.transform.position.x >= cam.BottomLeft.x && 
				ghost.transform.position.x <= cam.TopRight.x) {
				transform.position = ghost.transform.position;
				break;
			}
		}
	}

	// Use this for initialization
	void Start () {
		cam = GameObject.Find ("Main Camera").GetComponent<cameraInfo> ();
		myBehaviour = gameObject.GetComponent<PlayerBehaviour>();
		myLife = gameObject.GetComponent<LifeCount>();
		createGhosts ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		swapWithGhost ();
		positionGhosts ();
	}

	//When GameObject is Destroyed, destroy children (ghosts)
	void OnDestroy(){
		for (int i = 0; i < ghosts.Length; i++) {
			Destroy (ghosts [i], 0.0f);
		}
	}
}
