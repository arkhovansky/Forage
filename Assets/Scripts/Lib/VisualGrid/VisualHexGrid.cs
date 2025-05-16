using System;

using UnityEngine;

using Lib.Grid;



namespace Lib.VisualGrid {



 public class VisualHexGrid
 	: HexGrid
 {
 	public HexLayout Layout { get; }


    //----------------------------------------------------------------------------------------------


 	public VisualHexGrid(HexLayout layout,
                         uint width, uint height,
                         HexGridLineOffset lineOffset)
 		: base(width, height, layout.Orientation, lineOffset)
 	{
	    Layout = layout;
 	}


 	public VisualHexGrid(HexLayout layout, HexGrid grid)
 		: base(grid.Width, grid.Height, layout.Orientation, grid.LineOffset)
    {
	    if (layout.Orientation != grid.Orientation)
		    throw new ArgumentException();
	    
	    Layout = layout;
 	}



    public AxialPosition? GetAxialPosition(Vector2 point)
    {
	    AxialPosition freePos = Layout.GetAxialPosition(point);
	    return IsInside(freePos) ? freePos : null;
    }
 }



}
