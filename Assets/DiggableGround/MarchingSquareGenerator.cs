using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MarchingSquarePallete))]

public class MarchingSquareGenerator : MonoBehaviour
{
	[SerializeField] private int _width;
	[SerializeField] private int _height;

	// Leave it at 1, it does not work properly
	[SerializeField] private float _mouseRadius = 1;

	private MarchingSquarePallete _squarePallete;

	private int[,] _valueMatrix = new int[0, 0];
	private GameObject[,] _squares = new GameObject[0, 0];

	void Start()
	{
		_squarePallete = gameObject.GetComponent<MarchingSquarePallete>();

		_valueMatrix = new int[_width, _height];
		for (int i = 0; i < _width; i++)
		{
			for (int j = 0; j < _height; j++)
			{
				_valueMatrix[i, j] = 1;
			}
		}

		if (_width > 0 && _height > 0)
		{
			_squares = new GameObject[_width - 1, _height - 1];
			SpawnSquares();
		}
	}


	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Vector2 mouseGridPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

			if (mouseGridPosition.x - _mouseRadius > _width) return;
			if (mouseGridPosition.x + _mouseRadius < 0) return;
			if (mouseGridPosition.y - _mouseRadius > _height) return;
			if (mouseGridPosition.y + _mouseRadius < 0) return;

			ProcessMouseInput(mouseGridPosition);
		}
	}

	private void OnDrawGizmos()
	{
		GizmoDrawNodes();
	}

	void ProcessMouseInput(Vector2 mouseGridPosition)
	{
		int x = Mathf.RoundToInt(mouseGridPosition.x);
		int y = Mathf.RoundToInt(mouseGridPosition.y);

		if (x < 0 || y < 0) return;
		if (x >= _width || y >= _height) return;

		_valueMatrix[x, y] = 0;

		UpdateSquare(x, y);
	}

	void UpdateSquare(int x, int y)
	{
		if (x < _width - 1 && y < _height - 1) 
		{
			Destroy(_squares[x, y]);
			SpawnSquare(x, y, GetSquareState(x, y));
		}
		if(x > 0 && y < _height - 1) 
		{
			Destroy(_squares[x - 1, y]);
			SpawnSquare(x - 1, y, GetSquareState(x - 1, y));
		}

		if (y > 0 && x < _width - 1) 
		{
			Destroy(_squares[x, y - 1]);
			SpawnSquare(x, y - 1, GetSquareState(x, y - 1));
		}

		if (x > 0 && y > 0) 
		{
			Destroy(_squares[x - 1, y - 1]);
			SpawnSquare(x - 1, y - 1, GetSquareState(x - 1, y - 1));
		}
	}

	void SpawnSquares()
	{
		int width = _valueMatrix.GetLength(0);
		int height = _valueMatrix.GetLength(1);

		for (int i = 0; i < width - 1; i++)
		{
			for (int j = 0; j < height - 1; j++)
			{
				SpawnSquare(i, j, GetSquareState(i, j));
			}
		}
	}

	void SpawnSquare(int i, int j, GameObject square)
	{
		Vector3 pos = new Vector3(i + transform.position.x, j + transform.position.y, 0);
		_squares[i, j] = Instantiate(square, pos, Quaternion.identity, transform);
		//_squares[i, j].transform.position = pos;
	}

	void GizmoDrawNodes()
	{
		int width = _valueMatrix.GetLength(0);
		int height = _valueMatrix.GetLength(1);

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if (_valueMatrix[i, j] == 1)
				{
					var pos = transform.position;
					pos.x += i;
					pos.y += j;
					Gizmos.DrawSphere(pos, 0.2f);
				}
			}
		}
	}

	#region very_ugly_code_to_be_refactored
	GameObject GetSquareState(int i, int j)
	{
		if (_valueMatrix[i, j] == 1 && _valueMatrix[i, j + 1] == 1 && _valueMatrix[i + 1, j] == 1 && _valueMatrix[i + 1, j + 1] == 1)
		{
			return _squarePallete.FullSquare;
		}
		if (_valueMatrix[i, j] == 0 && _valueMatrix[i, j + 1] == 0 && _valueMatrix[i + 1, j] == 0 && _valueMatrix[i + 1, j + 1] == 0)
		{
			return _squarePallete.EmptySquare;

		}





		if (_valueMatrix[i, j] == 1 && _valueMatrix[i, j + 1] == 0 && _valueMatrix[i + 1, j] == 0 && _valueMatrix[i + 1, j + 1] == 0)
		{
			return _squarePallete.BLCorner;
		}
		if (_valueMatrix[i, j] == 0 && _valueMatrix[i, j + 1] == 0 && _valueMatrix[i + 1, j] == 1 && _valueMatrix[i + 1, j + 1] == 0)
		{
			return _squarePallete.BRCorner;
		}
		if (_valueMatrix[i, j] == 0 && _valueMatrix[i, j + 1] == 0 && _valueMatrix[i + 1, j] == 0 && _valueMatrix[i + 1, j + 1] == 1)
		{
			return _squarePallete.URCorner;
		}
		if (_valueMatrix[i, j] == 0 && _valueMatrix[i, j + 1] == 1 && _valueMatrix[i + 1, j] == 0 && _valueMatrix[i + 1, j + 1] == 0)
		{
			return _squarePallete.ULCorner;
		}






		if (_valueMatrix[i, j] == 1 && _valueMatrix[i, j + 1] == 1 && _valueMatrix[i + 1, j] == 0 && _valueMatrix[i + 1, j + 1] == 0)
		{
			return _squarePallete.LHalf;

		}
		if (_valueMatrix[i, j] == 1 && _valueMatrix[i, j + 1] == 0 && _valueMatrix[i + 1, j] == 1 && _valueMatrix[i + 1, j + 1] == 0)
		{
			return _squarePallete.BHalf;

		}
		if (_valueMatrix[i, j] == 0 && _valueMatrix[i, j + 1] == 0 && _valueMatrix[i + 1, j] == 1 && _valueMatrix[i + 1, j + 1] == 1)
		{
			return _squarePallete.RHalf;

		}
		if (_valueMatrix[i, j] == 0 && _valueMatrix[i, j + 1] == 1 && _valueMatrix[i + 1, j] == 0 && _valueMatrix[i + 1, j + 1] == 1)
		{
			return _squarePallete.UHalf;
		}





		if (_valueMatrix[i, j] == 0 && _valueMatrix[i, j + 1] == 1 && _valueMatrix[i + 1, j] == 1 && _valueMatrix[i + 1, j + 1] == 1)
		{
			return _squarePallete.BLEdge;
		}
		if (_valueMatrix[i, j] == 1 && _valueMatrix[i, j + 1] == 1 && _valueMatrix[i + 1, j] == 0 && _valueMatrix[i + 1, j + 1] == 1)
		{
			return _squarePallete.BREdge;
		}
		if (_valueMatrix[i, j] == 1 && _valueMatrix[i, j + 1] == 1 && _valueMatrix[i + 1, j] == 1 && _valueMatrix[i + 1, j + 1] == 0)
		{
			return _squarePallete.UREdge;
		}
		if (_valueMatrix[i, j] == 1 && _valueMatrix[i, j + 1] == 0 && _valueMatrix[i + 1, j] == 1 && _valueMatrix[i + 1, j + 1] == 1)
		{
			return _squarePallete.ULEdge;
		}






		if (_valueMatrix[i, j] == 1 && _valueMatrix[i, j + 1] == 0 && _valueMatrix[i + 1, j] == 0 && _valueMatrix[i + 1, j + 1] == 1)
		{
			return _squarePallete.LRDoubleCorner;
		}
		if (_valueMatrix[i, j] == 0 && _valueMatrix[i, j + 1] == 1 && _valueMatrix[i + 1, j] == 1 && _valueMatrix[i + 1, j + 1] == 0)
		{
			return _squarePallete.RLDoubleCorner;
		}

		return null;
	}
	#endregion
}
