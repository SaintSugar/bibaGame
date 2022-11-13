using UnityEngine;
using System;
 

public class PlayerFollow : MonoBehaviour {

    public Transform player;

    // Update is called once per frame
    void Update () {
        float xp, yp, xc, yc;

        xp = player.transform.position.x;
        yp = player.transform.position.y;

        xc = transform.position.x;
        yc = transform.position.y;

        Vector2 pointer = new Vector2((xc-xp), (yc-yp)).normalized;
        
        float xo, yo;   

        xo = pointer.x * 3 + xp;
        yo = pointer.y * 3 + yp;



        float dist;
        dist = Mathf.Sqrt(Mathf.Pow(xp-xc, 2) + Mathf.Pow(yp-yc, 2));   
        Vector2 V = GetComponent<Rigidbody2D>().velocity;
        if (dist >=3) {
            V = new Vector2((xo-xc)*10, (yo-yc)*10);
        }
        GetComponent<Rigidbody2D>().velocity = V;
    }
}
