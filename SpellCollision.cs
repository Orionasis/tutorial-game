﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCollision : MonoBehaviour
{
    public float spellDammage;

    // Use this for initialization 
    void Start()
    {
        Destroy(gameObject, 10);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<enemyAI>().ApplyDammage(spellDammage);
        }

        if (col.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}