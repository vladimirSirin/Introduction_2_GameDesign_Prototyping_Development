using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an enum of the weapon types in the game
// Includes a shield type to raise the shield
// Marked with [NI] below are not implemented in this book
public enum WeaponType
{
    none, // The default , no weapon
    blaster, // A simple blaster
    spread, // Two shots simultaneously
    missile, // Homing Missile [NI]
    phaser, // shots that move in waves [NI]
    laser, // Damge over time [NI]
    shield // Raise shieldlevel
}


// The weapon definition class allows you to set the properties
// of a specific weapon in the Inspector. Main has an array of WeaponDefinitions that makes it possible
// [System.Serializable] tells unity to try to view WeaponDefinition in the Inspector pane. It does not work
// for everything, but it will work for simple classes like this!
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;                       // the letter to show on the power-up
    public Color color = Color.white;           // The Color of collar and power-up
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;               // Amount of damage caused
    public float continuousDamage = 0;          // Damage Per second, for lasers
    public float delayBetweenShots = 0;
    public float velocity = 20;                 // Speed of projectiles
}

// Note: weapon prefabs, colors, and so on. Are set in the class 


public class Weapon : MonoBehaviour {

    // Initialize the variables for weapon fire delegate
    public static Transform PROJECTILE_ANCHOR;

    [SerializeField]
    private WeaponType _type;

    public GameObject collar;
    public WeaponDefinition def;
    public float lastShot; // Time last shot was fired



    // Use this for initialization
    void Start () {
        // Prepare the PROJECTILE_ANCHOR as parent game object, and Weapon Collar position as pos for projectiles made
        collar = GameObject.Find("Collar");
        if (PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_projectile_anchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        // Search for the parent of the weapon, if it is the hero, add Fire function of the weapon to the delegate of it.
        GameObject parentGo = this.transform.parent.gameObject;
        if (parentGo.tag == "Hero")
        {
            Hero.S.FireDelegate += Fire;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    // Mask the type for the private field _type, get and set
    public WeaponType Type
    {
        get { return _type; }
        set { SetType(value); }
    }

    // Define the SetType function, to set the type of the weapon
    void SetType(WeaponType value)
    {
        _type = value;

        // If it is none, then the weapon is not active at all
        if (value == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }

        // Set the collar color as the weapon color
        def = Main.GetWeaponDefinition(Type);
        collar.GetComponent<Renderer>().material.color = def.color;
        lastShot = 0; // You can always fire immediately after _type is set

    }

    // Define the Fire function, basically it switches based on the weapon type
    public void Fire()
    {
        // If the weapon is inactive, do not fire
        if (!gameObject.activeInHierarchy) return;

        // If it hasn't been enough time to shoot again, return
        if (Time.time - lastShot < def.delayBetweenShots) return;

        Projectile projectile;

        switch (Type)
        {
            case WeaponType.blaster:
                projectile = MakeProjectile();
                projectile.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;
                break;

            case WeaponType.spread:
                projectile = MakeProjectile();
                projectile.GetComponent<Rigidbody>().velocity = Vector3.up * def.velocity;
                projectile = MakeProjectile();
                projectile.GetComponent<Rigidbody>().velocity = new Vector3(-.2f, 0.9f, 0) * def.velocity;
                projectile = MakeProjectile();
                projectile.GetComponent<Rigidbody>().velocity = new Vector3(.2f, 0.9f, 0) * def.velocity;
                break;
        }


    }

    // Define the MakeProjectile function, which is used in Fire function to instantiate the prefabs and return to Fire
    public Projectile MakeProjectile()
    {
        GameObject projectileGo = Instantiate(def.projectilePrefab) as GameObject;
        if (transform.gameObject.tag == "Hero")
        {
            projectileGo.tag = "ProjectileHero";
            projectileGo.layer = LayerMask.NameToLayer("ProjectileHero");

        }
        else
        {
            projectileGo.tag = "ProjectileEnemy";
            projectileGo.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        projectileGo.transform.position = collar.transform.position;
        projectileGo.transform.parent = PROJECTILE_ANCHOR;

        Projectile proj = projectileGo.GetComponent<Projectile>();
        proj.Type = Type;
        lastShot = Time.time;
        return proj;
    }
}
