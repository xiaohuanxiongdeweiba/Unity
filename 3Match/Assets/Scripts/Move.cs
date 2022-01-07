using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ExchangeSweet( Collider2D sweetSelect, Collider2D sweetExchange)
    {
        Vector3 sweet1;
        Vector3 sweet2;
        Vector3 temp;
        sweet1= sweetSelect.gameObject.GetComponent<Transform>().transform.position;
        sweet2 = sweetExchange.gameObject.GetComponent<Transform>().transform.position;
        temp = sweet1;
        sweet1 = sweet2;
        sweet2 = temp;
    }

    
}
