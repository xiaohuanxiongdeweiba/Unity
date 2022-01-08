using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private bool isMove = false;
    private Vector3 destPos;


    public bool IsMove
    {
        get
        {
            return isMove;
        }

        set
        {
            isMove = value;
        }
    }

    public Vector3 DestPos
    {
        get
        {
            return destPos;
        }

        set
        {
            destPos = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (IsMove)
        {
        gameObject.GetComponent<Transform>().Translate((DestPos-gameObject.GetComponent<Transform>().position)*0.1f);
            if (gameObject.GetComponent<Transform>().position == destPos)
            {

                this.IsMove = false;
            }
        }
    }
    public Vector3[] ExchangeSweet(Collider2D sweetSelect, Collider2D sweetExchange)
    {
        Vector3 sweet1;//1糖原来位置
        Vector3 sweet2 = new Vector3(0, 0, 0);//2糖原来位置
        Vector3 temp;
        sweet1 = sweetSelect.gameObject.GetComponent<Transform>().transform.position;
        sweet2 = sweetExchange.gameObject.GetComponent<Transform>().transform.position;
        temp = sweet1;
        sweet1 = sweet2;//1糖交换后位置
        sweet2 = temp;//2糖交换后位置
        Vector3[] pos = new Vector3[2];
        pos[0] = sweet1;
        pos[1] = sweet2;
        //位置交换，在sweets 里面的 放置也应该交换，sweet 里面记录的 坐标也应该交换

        //sweets里面的放置交换

        int[] s1p = new int[] { sweetSelect.gameObject.GetComponent<Sweet>().XPos, sweetSelect.gameObject.GetComponent<Sweet>().YPos };
        int[] s2p = new int[] { sweetExchange.gameObject.GetComponent<Sweet>().XPos, sweetExchange.gameObject.GetComponent<Sweet>().YPos };
        //找到sweet里面记录的坐标
        sweetSelect.gameObject.GetComponent<Sweet>().XPos = s2p[0];
        sweetSelect.gameObject.GetComponent<Sweet>().YPos = s2p[1];
        sweetExchange.gameObject.GetComponent<Sweet>().XPos = s1p[0];
        sweetExchange.gameObject.GetComponent<Sweet>().YPos = s1p[1];
        GameManager.Instance.Sweets[s1p[0], s1p[1]] = sweetExchange.gameObject.GetComponent<Sweet>();
        GameManager.Instance.Sweets[s2p[0], s2p[1]] = sweetSelect.gameObject.GetComponent<Sweet>();
        return pos;

    }


}
