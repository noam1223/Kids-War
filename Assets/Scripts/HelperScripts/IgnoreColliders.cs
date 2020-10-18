    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreColliders : MonoBehaviour
{

    [SerializeField]
    private Collider2D[] gameObjects;


    private void Start()
    {

        GameObject[] ignorObjects = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < ignorObjects.Length; i++)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ignorObjects[i].GetComponent<Collider2D>(), true);
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Ninja");
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerObject.GetComponent<Collider2D>(), true);
        //playerObject.GetComponent<PlayerScript>().IgnoreSightCollision(playerObject);
    }

}
