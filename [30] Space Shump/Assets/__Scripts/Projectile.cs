using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private WeaponType _type;

    // This public property makes the field _type take action when it is set
    public WeaponType Type
    {
        get { return _type; }
        set { SetType(value); }
    }

    void Awake()
    {
        // Test to see whether this has passed off screen every 2 seconds
        InvokeRepeating("CheckOffScreen", 2f, 2f);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // The set type function
    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition definition = Main.GetWeaponDefinition(eType);
        GetComponent<Renderer>().material.color = definition.projectileColor;
    }

    // Check whether the projectile is offscreen, if so destroy it
    void CheckOffScreen()
    {
        if (Utils.ScreenBoundsCheck(this.GetComponent<Collider>().bounds, BoundsTest.offScreen) != Vector3.zero)
        {
            Destroy(this.gameObject);
        }
    }
}
