using UnityEngine;
using System.Collections;

public class cameraInfo : MonoBehaviour {

	//Private
	private static float m_screenWidth;
	private static float m_screenHeight;
	private static Vector3 m_screenBottomLeft;
	private static Vector3 m_screenTopRight;
	private Camera cam;

	// Use this for initialization
	void Start () {
		cam = Camera.main; //Get the camera values
		m_screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
		m_screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

		//Find the width and height of screen based on world size
		m_screenWidth = m_screenTopRight.x - m_screenBottomLeft.x;	
		m_screenHeight = m_screenTopRight.y - m_screenBottomLeft.y;
	}

    //Getter Functions to obtain value of screen Width and Height
    #region Properties
    public float ScreenWidth
    {
		get
        {
            return m_screenWidth;
        }
	}
	public float ScreenHeight
    {
        get
        {
            return m_screenHeight;
        }
	}
	public Vector3 BottomLeft
    {
        get
        {
            return m_screenBottomLeft;
        }
		
	}
	public Vector3 TopRight
    {
        get
        {
            return m_screenTopRight;
        }
		
	}
    #endregion
}
