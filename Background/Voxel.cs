using UnityEngine;
using System;

[Serializable]
public class Voxel 
{
	public bool state;

	public Vector2 position, xEdgePosition, yEdgePosition;

    public Voxel (int x, int y, float size) 
    {
		position.x = (x + 0.5f) * size;
		position.y = (y + 0.5f) * size;

		xEdgePosition = position;
		xEdgePosition.x += size * 0.5f;
		yEdgePosition = position;
		yEdgePosition.y += size * 0.5f;
	}
}