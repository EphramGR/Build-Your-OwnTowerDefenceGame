using UnityEngine;
using System.Collections.Generic;

[SelectionBase]

public class VoxelGrid : MonoBehaviour 
{
	private Mesh mesh;

	private List<Vector3> vertices;
	private List<int> triangles;

    public GameObject voxelPrefab;

	public int resolution;

	public int percentOfHill;

	public bool clearSolo;

	public bool groupChances;

	public int percentOfGroupedHill;

	public bool overflowWork;

	private float voxelSize;

	private Material[] voxelMaterials;

	private Voxel[] voxels;

	private bool trueCount;

	public bool firstTwoFalse;

	public float redoPercent;

	public int redoAttempts;

	public bool moreGrass;

	public bool moreMountian;

	public bool mountians;

	public bool hills;

	public bool rivers;

	public Color backgroundColor;

	public Color custom;

	private bool redoSuccess;

	/*private void wasAwake () 
    {
		voxelSize = 100;
		voxels = new Voxel[resolution * resolution];

        for (int i = 0, y = 0; y < resolution; y++) 
        {
			for (int x = 0; x < resolution; x++, i ++) 
            {
				CreateVoxel(i, x, y);
			}
		}
	}*/

    private void CreateVoxel (int i, int x, int y) 
    {
        GameObject o = Instantiate(voxelPrefab) as GameObject;
        o.transform.parent = transform;
        o.transform.localPosition = new Vector3((x + 300f + 0.5f) * voxelSize, (y + 300f + 0.5f) * voxelSize, -0.01f);
		o.transform.localScale = Vector3.one * voxelSize * 0.1f;
		//Debug.Log(o);
		voxelMaterials[i] = o.GetComponent<MeshRenderer>().material; //
		voxels[i] = new Voxel(x, y, voxelSize);
		assignState(i);
    }

	private void assignState(int i)
	{
		/*if ( i == 0)
		{
			Debug.Log("starting");
		}*/
		if (firstTwoFalse && (i == 0 | i == 1))
		{
			voxels[i].state = false;
		}
		else if (trueCount && groupChances)
		{
			if (connect(i))
			{
				if(Random.Range(0, 100) <= percentOfGroupedHill)
				{
					voxels[i].state = true;
				}
				else
				{
					voxels[i].state = false;
				}
			}
			else if (Random.Range(0, 100) <= percentOfHill)
			{
				voxels[i].state = true;
			}
			else
			{
				voxels[i].state = false;
			}
		}
		else if (Random.Range(0, 100) <= percentOfHill)
		{
			voxels[i].state = true;
			trueCount = true;
		}
		else
		{
			voxels[i].state = false;
		}

	}

	public void redraw()
	{
		int counter = 0;

		for (int i = 0; i <= resolution*resolution - 1; i++)
		{
			if(voxels[i].state == true)
			{
				counter += 1;
			}
		}

		//Debug.Log(((float)counter)/((float)(resolution*resolution)) * 100f);

		if (((float)counter)/((float)(resolution*resolution)) * 100f >= redoPercent)
		{
			//Debug.Log("Redrawing");
			trueCount = false;
			for (int i = 0; i <= resolution*resolution - 1; i++)
			{
				assignState(i);
			}

			if (clearSolo)
			{
				touchup();
			}
		}
		else
		{
			redoSuccess = true;
		}
	}

    public void Initialize (int resolution, float size) 
    {
		/*this.resolution = resolution;
		voxelSize = size / resolution;
		voxels = new Voxel[resolution * resolution];
		voxelMaterials = new Material[voxels.Length];

		for (int i = 0, y = 0; y < resolution; y++) 
		{
			for (int x = 0; x < resolution; x++, i++) 
			{
				CreateVoxel(i, x, y);
			}
		}

		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "VoxelGrid Mesh";
		vertices = new List<Vector3>();
		triangles = new List<int>();
		Refresh();*/
	}

    public void Awake () 
    {
		redoSuccess = false;
		Color mud = new Color (144f/255f, 90f/255f, 39f/255f);
		Color grass = new Color (41f/255f, 144f/255f, 39f/255f);
		Color lightgrass = new Color (42f/255f, 182f/255f, 37f/255f);
		Color water = new Color (49f/255f, 77f/255f, 121f/255f);
		Color mountian = new Color (73f/255f, 95f/255f, 110f/255f);
		
		if(hills)
		{
			transform.GetComponent<Renderer>().material.color = grass;
			backgroundColor = lightgrass;
		}
		else if (mountians)
		{
			transform.GetComponent<Renderer>().material.color = mountian;
			backgroundColor = grass;
		}
		else if (rivers)
		{
			transform.GetComponent<Renderer>().material.color = water;
			backgroundColor = grass;
		}
		else
		{
			transform.GetComponent<Renderer>().material.color = custom;
		}


		trueCount = false;
		voxelSize = 100;
		voxels = new Voxel[resolution * resolution];
		voxelMaterials = new Material[voxels.Length];

		for (int i = 0, y = 0; y < resolution; y++) 
		{
			for (int x = 0; x < resolution; x++, i++) 
			{
				CreateVoxel(i, x, y);
			}
		}

		if (clearSolo)
		{
			touchup();
		}

		for (int i = 0; i < redoAttempts; i++)
		{
			redraw();
			if (redoSuccess)
			{
				break;
			}
		}

		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "VoxelGrid Mesh";
		vertices = new List<Vector3>();
		triangles = new List<int>();

		Refresh();

		var l = GetComponent<Renderer>(); 

		transform.RotateAround(l.bounds.center, Vector3.forward, Random.Range(1, 5)*90);
	}

	private void SetVoxelColors () 
	{

		for (int i = 0; i < voxels.Length; i++) 
		{
			//voxelMaterials[i].color = Color.black;
		}
	}

	private void Refresh () 
	{
		SetVoxelColors();
		Triangulate();
	}

	private void Triangulate () 
	{
		vertices.Clear();
		triangles.Clear();
		mesh.Clear();

		TriangulateCellRows();

		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();

		/*Color32[] colors = new Color32[mesh.vertices.Length];
		for (int i = 0; i < mesh.vertices.Length; i++)
		{
			colors[i] = new Color(Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), 1.0f);
		}
		mesh.colors32 = colors;*/
	}

	private void TriangulateCellRows () 
	{
		int cells = resolution - 1;
		for (int i = 0, y = 0; y < cells; y++, i++) 
		{
			for (int x = 0; x < cells; x++, i++) 
			{
				TriangulateCell(voxels[i],
					voxels[i + 1],
					voxels[i + resolution],
					voxels[i + resolution + 1]);
			}
		}
	}

	private void TriangulateCell (Voxel a, Voxel b, Voxel c, Voxel d) 
	{
		int cellType = 0;
		if (a.state) 
		{
			cellType |= 1;
		}
		if (b.state) 
		{
			cellType |= 2;
		}
		if (c.state) 
		{
			cellType |= 4;
		}
		if (d.state) 
		{
			cellType |= 8;
		}

		switch (cellType) 
		{
		case 0:
			return;
		case 1:
			AddTriangle(a.position, a.yEdgePosition, a.xEdgePosition);
			break;
		case 2:
			AddTriangle(b.position, a.xEdgePosition, b.yEdgePosition);
			break;
		case 4:
			AddTriangle(c.position, c.xEdgePosition, a.yEdgePosition);
			break;
		case 8:
			AddTriangle(d.position, b.yEdgePosition, c.xEdgePosition);
			break;
		case 3:
			AddQuad(a.position, a.yEdgePosition, b.yEdgePosition, b.position);
			break;
		case 5:
			AddQuad(a.position, c.position, c.xEdgePosition, a.xEdgePosition);
			break;
		case 10:
			AddQuad(a.xEdgePosition, c.xEdgePosition, d.position, b.position);
			break;
		case 12:
			AddQuad(a.yEdgePosition, c.position, d.position, b.yEdgePosition);
			break;
		case 15:
			AddQuad(a.position, c.position, d.position, b.position);
			break;
		case 7:
			AddPentagon(a.position, c.position, c.xEdgePosition, b.yEdgePosition, b.position);
			break;
		case 11:
			AddPentagon(b.position, a.position, a.yEdgePosition, c.xEdgePosition, d.position);
			break;
		case 13:
			AddPentagon(c.position, d.position, b.yEdgePosition, a.xEdgePosition, a.position);
			break;
		case 14:
			AddPentagon(d.position, b.position, a.xEdgePosition, a.yEdgePosition, c.position);
			break;
		case 6:
			AddTriangle(b.position, a.xEdgePosition, b.yEdgePosition);
			AddTriangle(c.position, c.xEdgePosition, a.yEdgePosition);
			break;
		case 9:
			AddTriangle(a.position, a.yEdgePosition, a.xEdgePosition);
			AddTriangle(d.position, b.yEdgePosition, c.xEdgePosition);
			break;
		}
	}

	private void AddTriangle (Vector3 a, Vector3 b, Vector3 c) 
	{
		int vertexIndex = vertices.Count;
		vertices.Add(a);
		vertices.Add(b);
		vertices.Add(c);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
	}

	private void AddQuad (Vector3 a, Vector3 b, Vector3 c, Vector3 d) 
	{
		int vertexIndex = vertices.Count;
		vertices.Add(a);
		vertices.Add(b);
		vertices.Add(c);
		vertices.Add(d);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 3);
	}

	private void AddPentagon (Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 e) 
	{
		int vertexIndex = vertices.Count;
		vertices.Add(a);
		vertices.Add(b);
		vertices.Add(c);
		vertices.Add(d);
		vertices.Add(e);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 3);
		triangles.Add(vertexIndex + 4);
	}

	private void touchup ()
	{
		for (int i = 0; i <= (resolution*resolution - 1); i++)
		{
			if (i == 0)
			{
				if (voxels[i].state)
				{
					if (voxels[i + 1].state == false && voxels[resolution].state == false)
					{
						voxels[i].state = false;
					}
				}
				else
				{
					if (voxels[i + 1].state == true && voxels[resolution].state == true)
					{
						voxels[i].state = true;
					}
				}
			}
			else if (i == resolution - 1)
			{
				if (voxels[i].state)
				{
					if (voxels[i - 1].state == false && voxels[i + resolution].state == false)
					{
						voxels[i].state = false;
					}
				}
				else
				{
					if (voxels[i - 1].state == true && voxels[i + resolution].state == true)
					{
						voxels[i].state = true;
					}
				}
			}
			else if (i == resolution*resolution - 1)
			{
				if (voxels[i].state)
				{
					if (voxels[i - 1].state == false && voxels[i - resolution].state == false)
					{
						voxels[i].state = false;
					}
				}
				else
				{
					if (voxels[i - 1].state == true && voxels[i - resolution].state == true)
					{
						voxels[i].state = true;
					}
				}
			}
			else if (i == resolution*resolution - resolution)
			{
				if (voxels[i].state)
				{
					if (voxels[i + 1].state == false && voxels[i - resolution].state == false)
					{
						voxels[i].state = false;
					}
				}
				else
				{
					if (voxels[i + 1].state == true && voxels[i - resolution].state == true)
					{
						voxels[i].state = true;
					}
				}
			}
			else if ((i + 1) % resolution == 0) //right side
			{
				if (voxels[i].state)
				{
					if (voxels[i - 1].state == false && voxels[i + resolution].state == false && voxels[i - resolution].state == false)
					{
						voxels[i].state = false;
					}
				}
				else
				{
					if (voxels[i - 1].state == true && voxels[i + resolution].state == true && voxels[i - resolution].state == true)
					{
						voxels[i].state = true;
					}
				}
			}
			else if ((i + 1) % resolution == 1) //left side
			{
				if (voxels[i].state)
				{
					if (voxels[i + 1].state == false && voxels[i + resolution].state == false && voxels[i - resolution].state == false)
					{
						voxels[i].state = false;
					}
				}
				else
				{
					if (voxels[i + 1].state == true && voxels[i + resolution].state == true && voxels[i - resolution].state == true)
					{
						voxels[i].state = true;
					}
				}
			}
			else if (0 < i && i < resolution)
			{
				if (voxels[i].state)
				{
					if (voxels[i + 1].state == false && voxels[i - 1].state == false && voxels[i + resolution].state == false)
					{
						voxels[i].state = false;
					}
				}
				else
				{
					if (voxels[i + 1].state == true && voxels[i - 1].state == true && voxels[i + resolution].state == true)
					{
						voxels[i].state = true;
					}
				}
			}
			else if (resolution*resolution - resolution < i && i < resolution*resolution)
			{
				if (voxels[i].state)
				{
					if (voxels[i + 1].state == false && voxels[i - 1].state == false && voxels[i - resolution].state == false)
					{
						voxels[i].state = false;
					}
				}
				else
				{
					if (voxels[i + 1].state == true && voxels[i - 1].state == true && voxels[i - resolution].state == true)
					{
						voxels[i].state = true;
					}
				}
			}
			else
			{
				if (voxels[i].state)
				{
					if (voxels[i + 1].state == false && voxels[i - 1].state == false && voxels[i + resolution].state == false && voxels[i - resolution].state == false)
					{
						voxels[i].state = false;
					}
				}
				else
				{
					if (voxels[i + 1].state == true && voxels[i - 1].state == true && voxels[i + resolution].state == true && voxels[i - resolution].state == true)
					{
						voxels[i].state = true;
					}
				}
			}
		}
	}

	private bool connect(int i)
	{

		if (0 < i && i < resolution)
		{
			if (voxels[i - 1].state == true)
			{
				return true;
			}
		}
		else if ((i + 1) % resolution == 1)
		{
			if (voxels[i - resolution].state == true)
			{
				return true;
			}
		}
		else
		{
			if (overflowWork)
			{
				if (voxels[i - 1].state == true | voxels[i - resolution].state == true)
				{
					return true;
				}
			}
			else
			{
				if (voxels[i - 1].state == true && voxels[i - resolution].state == true)
				{
					return true;
				}
			}
		}
		return false;
	}
}