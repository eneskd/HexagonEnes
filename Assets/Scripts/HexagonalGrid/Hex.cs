using System;
using HexagonalGrid;
using UnityEngine;

public enum HexNeighbourDirection
{
    SE = 0,
    NE = 1,
    N = 2,
    NW = 3,
    SW = 4,
    S = 5
}

[Serializable]
public class Hex
{
    public int q; // 60 degree cw to r
    public int r; // Vertical axis
    public int s; // Not necessary = - q - r 
    
    public HexTile parent;


    public int X => q;
    public int Y => r + q / 2;

    // From SE in ccw direction
    public static Hex[] NeighborDirections { get; } =
    {
        new Hex(1, -1), new Hex(1, 0), new Hex(0, 1),
        new Hex(-1, 1), new Hex(-1, 0), new Hex(0, -1)
    };


    public static float XScale = 1;
    public static float YScale = 1;

    public static Vector3 Origin = Vector3.zero;

    #region Constructors

    public Hex(int q, int r, int s)
    {
        if (q + r + s == 0)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }
        else
        {
            Debug.LogError("Hex could not created");
        }
    }

    public Hex(int q, int r)
    {
        this.q = q;
        this.r = r;
        this.s = -q - r;
    }
    
    public Hex(int q, int r, HexTile parent)
    {
        this.q = q;
        this.r = r;
        this.s = -q - r;
        this.parent = parent;
    }

    public static Hex FromXY(int x, int y)
    {
        var q = x;
        var r = y - x / 2;
        return new Hex(q, r);
    }
    
    public static Hex FromXY(int x, int y, HexTile parent)
    {
        var q = x;
        var r = y - x / 2;
        return new Hex(q, r, parent);
    }


    #endregion

    public void SetCoord(int q, int r)
    {
        this.q = q;
        this.r = r;
        this.s = -q - r;
    }

    public Vector2 GetRelativePosition()
    {
        var matrix = HexTransform.Transform;
        var x = matrix[0][0] * q + matrix[1][0] * r;
        var y = matrix[0][1] * q + matrix[1][1] * r;
        x *= XScale;
        y *= YScale;
        return new Vector2(x, y);
    }

    public Vector3 GetPosition()
    {
        var v = GetRelativePosition();
        return Origin + new Vector3(v.x, v.y);
    }

    public Vector3 GetCornerPosition(HexCornerDirections direction)
    {
        return GetCornerPosition((int) direction);
    }

    public Vector3 GetCornerPosition(int direction)
    {
        direction %= 6;
        var angle = direction * Mathf.PI / 3;
        var length = HexTransform.EdgeLength;
        var x = Mathf.Cos(angle) * length * XScale;
        var y = Mathf.Sin(angle) * length * YScale;
        return GetPosition() + new Vector3(x, y);
    }

    public Hex GetNeighbour(HexNeighbourDirection direction)
    {
        return GetNeighbour((int) direction);
    }

    public Hex GetNeighbour(int direction)
    {
        return this + NeighborDirections[direction % 6];
    }
    
    public void MoveHex(HexNeighbourDirection direction, int count)
    {
        for (int i = 0; i < count; i++)
        {
            MoveHex((int) direction);
        }
    }

    public void MoveHex(HexNeighbourDirection direction)
    {
        MoveHex((int) direction);
    }
    
    

    public void MoveHex(int direction)
    {
        var hex = GetNeighbour(direction);
        q = hex.q;
        r = hex.r;
        s = hex.s;
    }

    #region Operations

    public Hex Add(Hex h)
    {
        return this + h;
    }

    public Hex Subtract(Hex h)
    {
        return this - h;
    }

    public Hex Multiply(int o)
    {
        return this * o;
    }

    public int Length()
    {
        return (Math.Abs(q) + Math.Abs(r) + Math.Abs(s)) / 2;
    }

    public int Distance(Hex h)
    {
        return this.Subtract(h).Length();
    }

    public static int Distance(Hex a, Hex b)
    {
        return (a - b).Length();
    }

    #endregion

    #region Operators

    public static Hex operator +(Hex h, Hex o)
    {
        return new Hex(h.q + o.q, h.r + o.r, h.s + o.s);
    }

    public static Hex operator -(Hex h, Hex o)
    {
        return new Hex(h.q - o.q, h.r - o.r, h.s - o.s);
    }

    public static Hex operator *(Hex h, int o)
    {
        return new Hex(h.q * o, h.r * o, h.s * o);
    }

    #endregion
}