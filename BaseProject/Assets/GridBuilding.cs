using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilding : MonoBehaviour {

    public Vector2 startPos;
    public Vector2 size;
    public Vector2 gridSectionSize;
	public List<gridSection> grid = new List<gridSection> ();

	// Use this for initialization
	void Start () {
		for (int a = 0; a < size.x; a++)
		{
			for (int b = 0; b < size.y; b++)
			{
				grid.Add (new gridSection (new Vector2 (a, b)));
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmos()
    {
        for (int a = 0; a < size.x; a++)
        {
            for (int b = 0; b < size.y; b++)
            {
				Gizmos.DrawWireCube (new Vector3 (startPos.x + (a * (size.x / 2)), startPos.y + (b * (size.y / 2)), 0), new Vector3 (gridSectionSize.x, gridSectionSize.y, 1));
            }
        }
    }
}
	
[System.Serializable]
public struct gridSection 
{
	public Vector2 xy;
	public List<GameObject> obj;

	public gridSection(Vector2 _xy) {
		xy = _xy;
		obj = new List<GameObject> ();
	}
}