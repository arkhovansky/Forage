using System;

using Unity.Entities;
using UnityEngine;

using Lib.Grid;



namespace App.Game.ECS.BandMember.Movement.Components {



public struct IntraCellMovement : IComponentData, IEnableableComponent
{
	/// <summary>
	/// Coordinate along intra-cell path in range [0, 1]
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public float _x;  // Public only for inspector

	public AxialPosition PreviousPosition;



	private const float CenterX = 0;

	private const float Radius = 0.5f;

	private const float FinalEdgeX = CenterX + Radius;

	/// <summary>
	/// Origin of coordinate system starting from edge
	/// </summary>
	private const float EdgeCoordSystemOrigin = CenterX - Radius;



	private float X {
		get => _x;
		set => _x = value;
	}


	public bool IsAtCenter
		// ReSharper disable once CompareOfFloatsByEqualityOperator
		=> X == CenterX;

	public bool IsBeforeCenter
		=> X < CenterX;

	public bool IsAfterCenter
		=> X > CenterX;


	public float DistanceToCenter {
		get {
			if (IsAfterCenter)
				throw new InvalidOperationException("Intra-cell position is after center");

			return CenterX - X;
		}
	}

	public float DistanceToFinalEdge
		// 2R - (x - EdgeCoordSystemOrigin) = 2R - (x - (C - R)) = C + R - x
		=> FinalEdgeX - X;


	public float PositionLerpParameter
		=> IsBeforeCenter
			? (Radius + (X - EdgeCoordSystemOrigin)) / (2 * Radius)
			: (X - CenterX) / (2 * Radius);



	public void SetAtCenter()
		=> X = CenterX;

	public void SetAtStartEdge(AxialPosition previousPosition)
	{
		X = EdgeCoordSystemOrigin;
		PreviousPosition = previousPosition;
	}


	public float Advance(float distance)
	{
		if (Mathf.Approximately(distance, DistanceToFinalEdge))
			X = FinalEdgeX;
		else if (distance > DistanceToFinalEdge)
			throw new ArgumentOutOfRangeException(nameof(distance), "Distance is beyond cell edge");
		else
			X += distance;

		return X;
	}
}



}
