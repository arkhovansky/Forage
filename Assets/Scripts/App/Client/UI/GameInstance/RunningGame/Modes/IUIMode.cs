using Lib.Grid;



namespace App.Client.UI.GameInstance.RunningGame {



public interface IUIMode
{
	void Update(AxialPosition? oldHoveredTilePosition, AxialPosition? newHoveredTilePosition);
}



}
