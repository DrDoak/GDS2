using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {

	public PhysicsSS directionComponent;
    public GameObject player;

    public Vector3 upPosition;
    public Vector3 downPosition;
    public Vector3 leftPosition;
    public Vector3 rightPosition;

    private Quaternion upRotation, downRotation, leftRotation, rightRotation;

	private bool m_facingLeft;

    // Use this for initialization
    void Start () {
        if (directionComponent == null)
			directionComponent = GetComponent<PhysicsSS>();

		upRotation = Quaternion.Euler(-15f, 0f, 0f);
		downRotation = Quaternion.Euler(105f, 0f, 0f);
		leftRotation = Quaternion.Euler(20f, -70f, 90f);
		rightRotation = Quaternion.Euler(20f, 70f, -90f);


		m_facingLeft = directionComponent.FacingLeft;
        ChooseDirection();
    }
	void OnEnable()
    {
        ChooseDirection();
    }
	// Update is called once per frame
	void Update () {

		bool fl = directionComponent.FacingLeft;
		if (fl != m_facingLeft)
            ChooseDirection();
    }

    void ChooseDirection()
    {
		bool fl = directionComponent.FacingLeft;
		if (fl) {
			transform.position = player.transform.position + leftPosition;
			transform.rotation = leftRotation;
		} else {
			transform.position = player.transform.position + rightPosition;
			transform.rotation = rightRotation;
		}
		m_facingLeft = fl;
    }
}