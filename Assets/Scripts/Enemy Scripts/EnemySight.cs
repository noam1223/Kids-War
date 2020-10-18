using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{

    [SerializeField]
    private Enemy enemy;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ninja")
        {
            enemy.Target = collision.gameObject;
        }

        //if(collision.gameObject.tag == "Sword")
        //{
        //    return;
        //}

        //if(collision.gameObject.tag == "Kunai")
        //{
        //    return;
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ninja")
        {
            enemy.Target = null;
        }
    }

}
