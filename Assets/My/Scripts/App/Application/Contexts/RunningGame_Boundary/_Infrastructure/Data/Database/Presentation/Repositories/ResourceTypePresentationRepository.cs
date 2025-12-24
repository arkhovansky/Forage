using System;
using System.Collections.Generic;

using UnityEngine;

using App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.ScriptableObjects;
using App.Game.Database;
using App.Infrastructure.Shared.Contracts.Database.Presentation;



namespace App.Application.Contexts.RunningGame_Boundary._Infrastructure.Data.Database.Presentation.Repositories {



public class ResourceTypePresentationRepository : IResourceTypePresentationRepository
{
	private record ResourceType_Data(
		string Name,
		ResourceTypePresentation GraphicalData);

	private readonly Dictionary<ResourceTypeId, ResourceType_Data> _resourceTypes = new();


	//----------------------------------------------------------------------------------------------


	public ResourceTypePresentationRepository(ResourceTypes_Presentation asset)
	{
		var quadMesh = CreateQuadMesh();

		foreach (ResourceTypeId typeId in Enum.GetValues(typeof(ResourceTypeId))) {
			var dbData = asset.GetResourceTypeData(typeId);
			_resourceTypes[typeId] = new ResourceType_Data(
				dbData.Name,
				new ResourceTypePresentation(quadMesh, dbData.Material));
		}
	}


	public string GetName(ResourceTypeId typeId)
	{
		return _resourceTypes[typeId].Name;
	}


	public ResourceTypePresentation Get(ResourceTypeId typeId)
	{
		return _resourceTypes[typeId].GraphicalData;
	}


	//----------------------------------------------------------------------------------------------
	// private

	private Mesh CreateQuadMesh()
	{
		return new Mesh {
			vertices = new Vector3[] {
				new (-0.5f, 0, -0.5f),
				new (0.5f, 0, -0.5f),
				new (-0.5f, 0, 0.5f),
				new (0.5f, 0, 0.5f)
			},
			triangles = new[] {
				// lower left triangle
				0, 2, 1,
				// upper right triangle
				2, 3, 1
			},
			normals = new[] {
				Vector3.up,
				Vector3.up,
				Vector3.up,
				Vector3.up
			},
			uv = new Vector2[] {
				new (0, 0),
				new (1, 0),
				new (0, 1),
				new (1, 1)
			}
		};
	}
}



}
