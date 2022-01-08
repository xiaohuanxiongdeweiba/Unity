using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public enum SweetType
    {
        NORMAL,
        BARRIR,
        EMPTY,
        RAINBOW,
        COLUMNCLEAR,
        ROWCLEAR
    }

    public enum SweetColor
    {
        RED,
        YELLOW,
        GREAN,
        PURPLE,
        BLUE,
        PINK,
        BISCATE
    }
    [Serializable]
    public struct SweetTypePrefab
    {
        public SweetType type;
        public GameObject prefab;
    }
    [Serializable]
    public struct SweetColorSprite
    {
        public SweetColor color;
        public Sprite sprite;
    }
    //单例
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    public Sweet[,] Sweets
    {
        get
        {
            return sweets;
        }

        set
        {
            sweets = value;
        }
    }

    public int _yRow;
    public int _xColumn;

    public GameObject gridPrefab;
    private Collider2D sweetChoosen = null;
    private Sweet[,] sweets;

    public SweetTypePrefab[] sweetTypePrefabs;
    public SweetColorSprite[] sweetColorSprites;
    public Dictionary<SweetType, GameObject> sweetPrefabDic = new Dictionary<SweetType, GameObject>();
    public Dictionary<SweetColor, Sprite> sweetSpriteDic = new Dictionary<SweetColor, Sprite>();

    // Start is called before the first frame update
    public void Awake()
    {
        Instance = this;
        for (int i = 0; i < sweetTypePrefabs.Length; i++)
        {
            sweetPrefabDic.Add(sweetTypePrefabs[i].type, sweetTypePrefabs[i].prefab);
        }
        for (int i = 0; i < sweetColorSprites.Length; i++)
        {
            sweetSpriteDic.Add(sweetColorSprites[i].color, sweetColorSprites[i].sprite);
        }

    }
    void Start()
    {
        Sweets = new Sweet[_xColumn, _yRow];
        CreatGrid();

        //for (int i = 0; i < _xColumn; i++)
        //{
        //    for (int j = 0; j < _yRow; j++)
        //    {
        //        CreatSweet(i,j,SweetType.NORMAL);
        //    }
        //}
        //测试掉落
        for (int i = 0; i < _xColumn; i++)
        {
            for (int j = 0; j < _yRow; j++)
            {
                CreatSweet(i, j, SweetType.EMPTY);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChoosenSweet();
        //选择
        if (Input.GetMouseButtonDown(0))
        {
            ClickSweet();
        }
        IsDownEmptySweet();
    }
    /// <summary>
    /// 选择第一个糖果和第二个糖果交换
    /// </summary>
    private void ClickSweet()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(ray.origin.x, ray.origin.y), Vector2.down);
        // raycastHit 是被选中的物体的collider
        if (raycastHit.collider)
        {
            if (sweetChoosen == null)//选中的为空，则添加选择对象
            {
                GameObject ob = raycastHit.collider.gameObject;
                sweetChoosen = ob.GetComponent<Collider2D>();
            }
            else//选择的不为空
            {
                float scx = sweetChoosen.gameObject.GetComponent<Sweet>().XPos;
                float scy = sweetChoosen.gameObject.GetComponent<Sweet>().YPos;
                float sex = raycastHit.collider.gameObject.GetComponent<Sweet>().XPos;
                float sey = raycastHit.collider.gameObject.GetComponent<Sweet>().YPos;
                if (sweetChoosen == raycastHit.collider.gameObject.GetComponent<Collider2D>())//第二次点击的同一个目标，取消选择
                {

                    //GameObject ob = raycastHit.collider.gameObject;
                    //ob.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                    sweetChoosen = null;
                }
                else if (((scx == sex) && (scy - 1 == sey || scy + 1 == sey)) || ((scy == sey) && (scx - 1 == sex || scx + 1 == sex)))//第二个选择的能交换
                {
                    //进行位置交换
                    print("相邻");
                    //sweetChoosen目标位置为pos[0]
                    Vector3[] pos = sweetChoosen.gameObject.GetComponent<Move>().ExchangeSweet(sweetChoosen, raycastHit.collider);
                    //sweetChoosen.gameObject.GetComponent<Transform>().Translate(new Vector3 ((pos[0].x - sweetChoosen.gameObject.GetComponent<Transform>().position.x)*10f, (pos[0].y - sweetChoosen.gameObject.GetComponent<Transform>().position.y))*10f);
                    //raycastHit.collider.gameObject.GetComponent<Transform>().Translate(new Vector3( pos[1].x - raycastHit.collider.gameObject.GetComponent<Transform>().position.x, pos[1].y - raycastHit.collider.gameObject.GetComponent<Transform>().position.y));
                    sweetChoosen.gameObject.GetComponent<Move>().DestPos = pos[0];
                    raycastHit.collider.gameObject.GetComponent<Move>().DestPos = pos[1];
                    sweetChoosen.gameObject.GetComponent<Move>().IsMove = true;
                    raycastHit.collider.gameObject.GetComponent<Move>().IsMove = true;
                    //sweetChoosen.gameObject.GetComponent<Transform>().position = pos[0];
                    //raycastHit.collider.gameObject.GetComponent<Transform>().position = pos[1];
                    sweetChoosen = null;
                    print("交换成功");

                }
                else//第二个选择的为不相关，变换被选中的目标
                {
                    GameObject ob = raycastHit.collider.gameObject;
                    sweetChoosen = ob.GetComponent<Collider2D>();
                }
            }

        }
    }

    public void ChoosenSweet()
    {
        for (int i = 0; i < _xColumn; i++)
        {
            for (int j = 0; j < _yRow; j++)
            {
                if (sweetChoosen == Sweets[i, j].GetComponent<Collider2D>())
                {
                    sweetChoosen.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                }
                else if (Sweets[i, j].GetComponent<Collider2D>() != sweetChoosen)
                {
                    Sweets[i, j].GetComponentInChildren<SpriteRenderer>().color = Color.white;
                }
            }

        }
        if (sweetChoosen != null)
        {
            print(sweetChoosen.gameObject.GetComponent<Sweet>().XPos + "," + sweetChoosen.gameObject.GetComponent<Sweet>().YPos);

        }


    }
    /// <summary>
    /// 生成巧克力网格
    /// </summary>
    public void CreatGrid()
    {
        for (int i = 0; i < _xColumn; i++)
        {
            for (int j = 0; j < _yRow; j++)
            {
                GameObject newgrid = Instantiate(gridPrefab, new Vector3(i, j), Quaternion.identity, Instance.transform);
                CorrectPosition(newgrid);
            }

        }
    }
    /// <summary>
    /// 纠正位置
    /// </summary>
    /// <param name="gameObject"></param>
    public void CorrectPosition(GameObject gameObject)
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x - 4f, gameObject.transform.position.y - 4f);
    }

    Random random = new Random();
    /// <summary>
    /// 生成糖果
    /// </summary>
    public void CreatSweet(int i, int j, SweetType type)
    {


        GameObject newSweet = Instantiate(sweetPrefabDic[type], new Vector3(i, j), Quaternion.identity, Instance.transform);
        Sweets[i, j] = newSweet.GetComponent<Sweet>();
        if (type == SweetType.EMPTY)
        {
            Sweets[i, j].IniSweet(i, j, type);

        }
        //设置随机颜色

        if (type == SweetType.NORMAL)
        {
            int colorNum = random.Next(0, 6);
            SpriteRenderer sp = Sweets[i, j].GetComponentInChildren<SpriteRenderer>();
            //SpriteRenderer sp = newSweet.GetComponentInChildren<SpriteRenderer>();
            switch (colorNum)
            {
                case 0:
                    sp.sprite = sweetSpriteDic[SweetColor.RED];
                    Sweets[i, j].IniSweet(i, j, SweetColor.RED, type);
                    break;
                case 1:
                    sp.sprite = sweetSpriteDic[SweetColor.YELLOW];
                    Sweets[i, j].IniSweet(i, j, SweetColor.YELLOW, type);
                    break;
                case 2:
                    sp.sprite = sweetSpriteDic[SweetColor.GREAN];
                    Sweets[i, j].IniSweet(i, j, SweetColor.GREAN, type);
                    break;
                case 3:
                    sp.sprite = sweetSpriteDic[SweetColor.BLUE];
                    Sweets[i, j].IniSweet(i, j, SweetColor.BLUE, type);
                    break;
                case 4:
                    sp.sprite = sweetSpriteDic[SweetColor.PURPLE];
                    Sweets[i, j].IniSweet(i, j, SweetColor.PURPLE, type);
                    break;
                case 5:
                    sp.sprite = sweetSpriteDic[SweetColor.PINK];
                    Sweets[i, j].IniSweet(i, j, SweetColor.PINK, type);
                    break;
            }
        }

        CorrectPosition(newSweet);

    }

    public void IsDownEmptySweet()
    {
        for (int i = 0; i < _xColumn; i++)
        {
            for (int j = 0; j < _yRow; j++)
            {
                if (Sweets[i, j].Type == SweetType.EMPTY)
                {
                    print("检测到空糖果");
                    //如果不是嘴上面的糖果，则上面的填充下来
                    if (j != _yRow - 1)
                    {
                        int[] sweet1 = new int[2] { i, j };
                        int[] sweet2 = new int[2] { i, j + 1 };
                        sweets[i, j].GetComponent<Move>().ExchangeSweet(sweet1, sweet2);
                    }
                    else if (j == _yRow - 1)
                    {
                        Destroy(Sweets[i, j].gameObject);

                        CreatSweet(i, j, SweetType.NORMAL);
                        //Sweets[i, j].GetComponentInChildren<SpriteRenderer>().sprite = sweetSpriteDic[SweetColor.BISCATE];
                    }


                    //如果是嘴上面的糖果，则在该位置生成一个糖果
                }

            }

        }
    }
}
