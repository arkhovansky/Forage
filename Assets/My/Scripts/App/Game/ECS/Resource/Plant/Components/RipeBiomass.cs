using System;

using Unity.Entities;
using UnityEngine;



namespace App.Game.ECS.Resource.Plant.Components {



/// <summary>
/// Biomass of a ripe plant resource, kg
/// </summary>
/// <remarks>
/// Present when the resource is ripe. Can be zero if exhausted.
/// </remarks>
public struct RipeBiomass : IComponentData
{
	[SerializeField] private float _value;


	public float Value {
		readonly get => _value;
		private set => _value = value;
	}


	public readonly bool IsZero
		=> Value == 0f;



	public RipeBiomass(float value)
	{
		_value = value;
	}


	public float Decrease(float decrement)
	{
		if (Mathf.Approximately(decrement, Value))
			Value = 0f;
		else {
			if (decrement > Value)
				throw new ArgumentOutOfRangeException();

			Value -= decrement;
		}

		return Value;
	}
}



}
