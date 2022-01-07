using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweet : MonoBehaviour
{
    private int _xPos;
    private int _yPos;
    private GameManager.SweetColor color;
    private GameManager.SweetType type;

    public int XPos
    {
        get
        {
            return _xPos;
        }

        set
        {
            _xPos = value;
        }
    }

    public int YPos
    {
        get
        {
            return _yPos;
        }

        set
        {
            _yPos = value;
        }
    }

    public GameManager.SweetColor Color
    {
        get
        {
            return color;
        }

        set
        {
            color = value;
        }
    }

    public GameManager.SweetType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    public void IniSweet(int xpos,int ypos,GameManager.SweetColor color, GameManager.SweetType type)
    {
        this.XPos = xpos;
        this.YPos = ypos;
        this.Color = color;
        this.Type = type;
    }
}
