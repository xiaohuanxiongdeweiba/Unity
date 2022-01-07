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
        PINK
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
        sweets = new Sweet[_xColumn, _yRow];
        CreatGrid();
        CreatSweet();
    }

    // Update is called once per frame
    void Update()
    {
        //选择
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D raycastHit = Physics2D.Raycast(new Vector2(ray.origin.x, ray.origin.y), Vector2.down);
            //RaycastHit raycastHit;
            if (raycastHit.collider)
            {
                if (sweetChoosen == null)//选择对象
                {
                    GameObject ob = raycastHit.collider.gameObject;
                    ob.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                    print(ob.name);
                    sweetChoosen = ob.GetComponent<Collider2D>();
                }
                else if (sweetChoosen == raycastHit.collider.gameObject.GetComponent<Collider2D>())//取消选择
                {

                    GameObject ob = raycastHit.collider.gameObject;
                    ob.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                    sweetChoosen = null;
                }
                else if()//选择第二个对象
                {
                    sweetChoosen.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                    GameObject ob = raycastHit.collider.gameObject;
                    ob.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                    print(ob.name);
                    sweetChoosen = ob.GetComponent<Collider2D>();
                }


            }



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
    public void CreatSweet()
    {

        for (int i = 0; i < _xColumn; i++)
        {
            for (int j = 0; j < _yRow; j++)
            {
                GameObject newSweet = Instantiate(sweetPrefabDic[SweetType.NORMAL], new Vector3(i, j), Quaternion.identity, Instance.transform);
                sweets[i, j] = newSweet.GetComponent<Sweet>();
                //设置随机颜色

                int colorNum = random.Next(0, 6);
                SpriteRenderer sp = sweets[i, j].GetComponentInChildren<SpriteRenderer>();
                //SpriteRenderer sp = newSweet.GetComponentInChildren<SpriteRenderer>();
                switch (colorNum)
                {
                    case 0:
                        sp.sprite = sweetSpriteDic[SweetColor.RED];
                        sweets[i, j].IniSweet(i, j, SweetColor.RED, SweetType.NORMAL);
                        break;
                    case 1:
                        sp.sprite = sweetSpriteDic[SweetColor.YELLOW];
                        sweets[i, j].IniSweet(i, j, SweetColor.YELLOW, SweetType.NORMAL);
                        break;
                    case 2:
                        sp.sprite = sweetSpriteDic[SweetColor.GREAN];
                        sweets[i, j].IniSweet(i, j, SweetColor.GREAN, SweetType.NORMAL);
                        break;
                    case 3:
                        sp.sprite = sweetSpriteDic[SweetColor.BLUE];
                        sweets[i, j].IniSweet(i, j, SweetColor.BLUE, SweetType.NORMAL);
                        break;
                    case 4:
                        sp.sprite = sweetSpriteDic[SweetColor.PURPLE];
                        sweets[i, j].IniSweet(i, j, SweetColor.PURPLE, SweetType.NORMAL);
                        break;
                    case 5:
                        sp.sprite = sweetSpriteDic[SweetColor.PINK];
                        sweets[i, j].IniSweet(i, j, SweetColor.PINK, SweetType.NORMAL);
                        break;
                }
                CorrectPosition(newSweet);
            }

        }
    }
}
