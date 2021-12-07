using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class MapManager : MonoBehaviour
{
	Transform target;
	public GameObject wallPrefab;
	// Start is called before the first frame update
	public GameObject[,] Maze;
	public Vector3 Origin;
	private Vector3 Dest;
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
		Origin = new Vector3(0.5f, 1.5f, 0.5f);
		target = FindObjectOfType<Player>().transform;
		board.Initialize(10);
		int destX = board.DestY;
		int destZ = board.DestX;

		Dest = Origin + new Vector3(destX, 0, destZ);
		UnityMapRender();

	}

	// Update is called once per frame
	void Update()
    {
        
    }

	public void MissionClear()
	{
		if (target.transform.position == Dest)
		{
			Debug.Log("�̼ǿϷ�");
		}
	}

	public void UnityMapRender()
	{
		for (int x = 0; x < board.Size; ++x) // ����
		{
			for (int z = 0; z < board.Size; ++z)  // �¿� 
			{
				if (x == board.DestY || z == board.DestY)
					continue;

				if (board.Tile[x, z] != TileType.Empty)
				{
					Maze[x, z] = Instantiate(wallPrefab, Origin + new Vector3(x, 0, z), transform.rotation);
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
			// �ϴ� ���� �� ���ƹ����� �۾�
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

			// �������� ���� Ȥ�� �Ʒ��� ���� �մ� �۾�
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
