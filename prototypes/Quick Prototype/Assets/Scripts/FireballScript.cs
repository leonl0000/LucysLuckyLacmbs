﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    public GameObject explosion;
    public float fireballDamage = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        // Spawn explosion
        GameObject thisExplosion = Instantiate(explosion, this.gameObject.transform.position, this.gameObject.transform.rotation);

        // And call damage-inflicting function on sheep and wolves, etc.
        if (collision.collider.tag == "sheep")
            collision.gameObject.GetComponent<sheepScript>().wound(fireballDamage, gameObject.transform);
        else if (collision.collider.tag == "Predator")
            collision.gameObject.GetComponent<PreditorScript>().wound(fireballDamage, gameObject.transform);

        // Destroy fireball
        Destroy(this.gameObject);
    }
}