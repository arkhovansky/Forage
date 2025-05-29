using System;

using Unity.Entities;
using UnityEngine;



namespace App.Game.ECS.Resource.Plant.Components {



public struct RipeBiomass : IComponentData
{
	public float _value;  // Public only for inspector


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


	public void Reset(float value)
	{
		Value = value;
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
