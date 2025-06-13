using System;



namespace Lib.Grid {



public enum HexMapLineOffset
{
	Even = 1,  // 0, 2, ...
	Odd = -1
}



public struct RectangularHexMap
{
	public uint Width { get; }
	public uint Height { get; }

	public HexOrientation Orientation { get; }

	public HexMapLineOffset LineOffset { get; }


	//----------------------------------------------------------------------------------------------


	private struct FreeOffsetPosition
	{
		public int Col, Row;

		public FreeOffsetPosition(int col, int row) {
			Col = col;
			Row = row;
		}
	}


	//----------------------------------------------------------------------------------------------


	public RectangularHexMap(uint width, uint height,
	                         HexOrientation orientation,
	                         HexMapLineOffset lineOffset)
	{
		Width = width;
		Height = height;
		Orientation = orientation;
		LineOffset = lineOffset;
	}


	public OffsetPosition OffsetPositionFromCellIndex(uint cellIndex)
		=> new(cellIndex % Width, cellIndex / Width);


	public uint CellIndexFrom(OffsetPosition offsetPosition)
		=> offsetPosition.Row * Height + offsetPosition.Col;

	public uint CellIndexFrom(AxialPosition axial)
		=> CellIndexFrom(OffsetPositionFrom(axial));


	public OffsetPosition OffsetPositionFrom(AxialPosition axial)
	{
		var freeOffset = FreeOffsetPositionFrom(axial);

		if (freeOffset.Col < 0 || freeOffset.Row < 0)
			throw new ArgumentOutOfRangeException();

		return new OffsetPosition((uint)freeOffset.Col, (uint)freeOffset.Row);
	}


	private FreeOffsetPosition FreeOffsetPositionFrom(AxialPosition axial)
	{
		int col, row;

		switch (Orientation) {
			case HexOrientation.FlatTop:
				col = axial.Q;
				row = axial.R + (axial.Q + (int)LineOffset * (axial.Q & 1)) / 2;

				break;

			case HexOrientation.PointyTop:
				col = axial.Q + (axial.R + (int)LineOffset * (axial.R & 1)) / 2;
				row = axial.R;

				break;

			default:
				throw new ArgumentOutOfRangeException();
		}

		return new FreeOffsetPosition(col, row);
	}


	public AxialPosition AxialPositionFromCellIndex(uint cellIndex)
		=> AxialPositionFrom(OffsetPositionFromCellIndex(cellIndex));


	public AxialPosition AxialPositionFrom(OffsetPosition offsetPosition)
	{
		int col = (int) offsetPosition.Col;
		int row = (int) offsetPosition.Row;

		int q, r;

		switch (Orientation) {
			case HexOrientation.FlatTop:
				q = col;
				r = row - (col + (int)LineOffset * (col & 1)) / 2;
				return new AxialPosition(q, r);

			case HexOrientation.PointyTop:
				q = col - (row + (int)LineOffset * (row & 1)) / 2;
				r = row;
				return new AxialPosition(q, r);

			default:
				throw new ArgumentOutOfRangeException();
		}
	}


	public bool Contains(AxialPosition position)
	{
		var freeOffset = FreeOffsetPositionFrom(position);
		return freeOffset.Col >= 0 && freeOffset.Col < Width &&
		       freeOffset.Row >= 0 && freeOffset.Row < Height;
	}
}



}
