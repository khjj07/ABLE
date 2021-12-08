using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class MapManager : MonoBehaviour
{
	public GameObject wallPrefab;
	// Start is called before the first frame update
	public GameObject[,] Maze;
	public Vector3 Origin;
	public Vector3 Dest;
	public Transform MazeParent;
	private Board board;

	#region primitiveMap
	public enum TileType
	{
		Empty,
		Wall,
	}


	#endregion

	void Start()
	{
		board = new Board();
		//Origin = new Vector3(0.5f, 1.5f, 0.5f);
		board.Initialize(25);
		int destX = board.DestY;
		int destZ = board.DestX;
		Maze = new GameObject[25, 25];
		Dest = new Vector3(destX + Origin.x, 0, destZ + Origin.z);
		UnityMapRender();

	}

	// Update is called once per frame
	void Update()
	{

	}


	public void UnityMapRender()
	{
		for (int x = 0; x < board.Size; ++x) // 상하
		{
			for (int z = 0; z < board.Size; ++z)  // 좌우 
			{
				if (x == board.DestY && z == board.DestY)
					continue;

				if (board.Tile[x, z] != TileType.Empty)
				{
					Maze[x, z] = Instantiate(wallPrefab, new Vector3(Origin.x + (float)x, 0, Origin.z + (float)z), new Quaternion(0f, 0f, 0f, 0f));
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
			Tile = new TileType[size, size];
			Size = size;



			DestY = Size - 2;
			DestX = Size - 2;
			GenerateBySideWinder();
		}

		void GenerateBySideWinder()
		{
			// 일단 길을 다 막아버리는 작업
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

			// 랜덤으로 우측 혹은 아래로 길을 뚫는 작업
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
