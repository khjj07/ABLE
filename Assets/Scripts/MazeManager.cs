using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class MazeManager : Singleton<MazeManager>
{
	public GameObject wallPrefab;
	public GameObject endPrefab;
	// Start is called before the first frame update
	private GameObject[,] Maze;
	private GameObject endPoint;
	public Vector3 Origin;
	public Vector3 Dest;	
	public Transform MazeParent;
	private Board board;
	public int Size { get; set; }

	#region primitiveMap
	public enum TileType
	{
		Empty,
		Wall,
	}


	#endregion

	void Start()
	{
		Size = 25;
		board = new Board();
		board.Initialize(Size);
		Maze = new GameObject[Size, Size];
		Dest = new Vector3(board.DestY + Origin.x, 0, board.DestX + Origin.z);
		UnityMapRender();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void UnityMapRender()
	{
		for (int x = 0; x < board.Size; ++x) // »óÇÏ
		{
			for (int z = 0; z < board.Size; ++z)  // ÁÂ¿ì 
			{

				if (x == 23 && z == 23)
                {
					endPoint = Instantiate(endPrefab, new Vector3(Origin.x + (float)x, 0, Origin.z + (float)z), new Quaternion(0f, 0f, 0f, 0f));
					endPoint.transform.parent = MazeParent.transform;
                }
                if (board.Tile[x, z] != TileType.Empty)
				{
					Maze[x, z] = Instantiate(wallPrefab, new Vector3(Origin.x + (float)x, 0f, Origin.z + (float)z), new Quaternion(0f, 0f, 0f, 0f));
					Maze[x, z].transform.parent = MazeParent.transform;
					
				}
			}
		}
	}

	class Board
	{
		public int Size { get; private set; }

		public int DestY { get; private set; }
		public int DestX { get; private set; }

		public TileType[,] Tile { get; private set; }



		public void Initialize(int size)
		{

			if (size % 2 == 0)
				return;

			DestY = Size - 2;
			DestX = Size - 2;

			Tile = new TileType[size, size];
			Size = size;


			GenerateBySideWinder();
		}

		void GenerateBySideWinder()
		{
			for (int y = 0; y < Size; y++)
			{
				for (int x = 0; x < Size; x++)
				{
					if (x % 2 == 0 || y % 2 == 0)
						Tile[y, x] = TileType.Wall;
					else
						Tile[y, x] = TileType.Empty;
				}
			}
			System.Random rand = new System.Random();
			for (int y = 0; y < Size; y++)
			{
				int count = 1;
				for (int x = 0; x < Size; x++)
				{
					if (x % 2 == 0 || y % 2 == 0)
						continue;

					if (y == Size - 2 && x == Size - 2)
						continue;

					if (y == Size - 2)
					{
						Tile[y, x + 1] = TileType.Empty;
						continue;
					}

					if (x == Size - 2)
					{
						Tile[y + 1, x] = TileType.Empty;
						continue;
					}

					if (rand.Next(0, 2) == 0)
					{
						Tile[y, x + 1] = TileType.Empty;
						count++;
					}
					else
					{
						int randomIndex = rand.Next(0, count);
						Tile[y + 1, x - randomIndex * 2] = TileType.Empty;
						count = 1;
					}
				}
			}
		}

	}

}
