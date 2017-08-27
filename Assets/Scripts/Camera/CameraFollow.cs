using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    #region Public Members
    public GameObject Reference;
    public float CameraSpeed;
    public Vector3 Offset;
    public Vector3 CharacterOffset;
    Camera Camera;

    bool MoveLeft;
    bool MoveRight;
    bool MoveUp;
    bool MoveDown;
    #endregion

    // Use this for initialization
    void Start () {
        Camera = GetComponent<Camera>();
        MoveLeft = MoveRight = MoveUp = MoveDown = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Reference)
        {
            float horizontalDir = Reference.transform.position.x - transform.position.x + CharacterOffset.x;
            float verticalDir = Reference.transform.position.y - transform.position.y + CharacterOffset.y;

            Vector3 target = Reference.transform.position;

            //Camera Wont move horizontally under these conditions
            if (!MoveLeft && horizontalDir < 0 || !MoveRight && horizontalDir > 0)
            {
                target = new Vector3(transform.position.x, target.y, target.z);
            }

            if(!MoveUp && verticalDir > 0 || !MoveDown && verticalDir < 0)
            {
                target = new Vector3(target.x, transform.position.y - CharacterOffset.y, target.z);
            }

            transform.position = Vector3.Lerp(transform.position, target, CameraSpeed) + new Vector3(0, 0, -10) + Offset;
        }
	}

    //Trigger Handlers
    #region Trigger Handlers
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("CameraLeft")))
        {
            MoveLeft = false;
        }
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("CameraRight")))
        {
            MoveRight = false;
        }
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("CameraTop")))
        {
            MoveUp = false;
        }
        else if (other.gameObject.layer.Equals(LayerMask.NameToLayer("CameraBottom")))
        {
            MoveDown = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("CameraLeft")))
        {
            MoveLeft = true;
        }
        else if(other.gameObject.layer.Equals(LayerMask.NameToLayer("CameraRight")))
        {
            MoveRight = true;
        }
        else if(other.gameObject.layer.Equals(LayerMask.NameToLayer("CameraTop")))
        {
            MoveUp = true;
        }
        else if(other.gameObject.layer.Equals(LayerMask.NameToLayer("CameraBottom")))
        {
            MoveDown = true;
        }
    }
    #endregion
}
