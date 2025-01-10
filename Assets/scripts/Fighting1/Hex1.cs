using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Hex1
{
    public int q; // Kolumna
    public int r; // Wiersz
    public int s;

    // Definicje przesuniêæ dla siatki even-q top-flat
    private static readonly (int dq, int dr)[] EvenQDirections =
    {
        (1, 0), (0, -1), (-1, -1), (-1, 0), (-1, 1), (0, 1)
    };

    private static readonly (int dq, int dr)[] OddQDirections =
    {
        (1, 0), (1, -1), (0, -1), (-1, 0), (0, 1), (1, 1)
    };

    public Hex1(int q, int r)
    {
        this.q = q;
        this.r = r;
        this.s = -q - r;
    }

    public Hex1 GetNeighbor(int direction)
    {
        var directions = q % 2 == 0 ? EvenQDirections : OddQDirections;
        if (direction < 0 || direction >= directions.Length) return null;
        var (dq, dr) = directions[direction];
        return new Hex1(q + dq, r + dr);
    }

    public List<Hex1> GetAllNeighbors()
    {
        List<Hex1> neighbors = new List<Hex1>();
        var directions = q % 2 == 0 ? EvenQDirections : OddQDirections;

        foreach (var (dq, dr) in directions)
        {
            neighbors.Add(new Hex1(q + dq, r + dr));
        }

        return neighbors;
    }


    // Metoda obliczaj¹ca dystans miêdzy dwoma heksami
    public int HexDistance(Hex1 other)
    {
        return (Mathf.Abs(this.q - other.q) + Mathf.Abs(this.r - other.r) + Mathf.Abs(this.s - other.s)) / 2;
    }

}

