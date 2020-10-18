using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class KunaiScript : MonoBehaviour
{

    [SerializeField]
    private float speed;

    private Rigidbody2D myRigidbody;

    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 0.35f);
    }

    private void FixedUpdate()
    {
        //MyRigidbody.velocity = direction * speed;
        myRigidbody.MovePosition(myRigidbody.position + (direction * speed * Time.deltaTime));

    }

    public void getDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    // Update is called once per frame
    void Update()
    {
           
    }
}
