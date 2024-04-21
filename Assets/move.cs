using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class move : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsOwner == false)
        {
            return;
        }


        Vector2 direction = new Vector2()
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        };
        float moveSpeed = 3f;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
