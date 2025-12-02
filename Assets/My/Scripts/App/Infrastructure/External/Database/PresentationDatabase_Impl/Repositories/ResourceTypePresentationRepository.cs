using System;
using System.Collections.Generic;

using UnityEngine;

using App.Application.PresentationDatabase;
using App.Game.Database;



namespace App.Infrastructure.External.Database.PresentationDatabase_Impl.Repositories {



public class ResourceTypePresentationRepository : IResourceTypePresentationRepository
{
	private record ResourceType_Data(
		string Name,
		ResourceTypePresentation GraphicalData);

	private readonly Dictionary<ResourceTypeId, ResourceType_Data> _resourceTypes = new();


	//----------------------------------------------------------------------------------------------


	public ResourceTypePresentationRepository()
	{
		var quadMesh = CreateQuadMesh();

		var resourceTypesDB = GameDatabase.Instance.Presentation.ResourceTypes;

		foreach (ResourceTypeId typeId in Enum.GetValues(typeof(ResourceTypeId))) {
			var dbData = resourceTypesDB.GetResourceTypeData(typeId);
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
			vertices = new Vector3[4] {
				new (-0.5f, 0, -0.5f),
				new (0.5f, 0, -0.5f),
				new (-0.5f, 0, 0.5f),
				new (0.5f, 0, 0.5f)
			},
			triangles = new int[6] {
				// lower left triangle
				0, 2, 1,
				// upper right triangle
				2, 3, 1
			},
			normals = new Vector3[4] {
				Vector3.up,
				Vector3.up,
				Vector3.up,
				Vector3.up
			},
			uv = new Vector2[4] {
				new (0, 0),
				new (1, 0),
				new (0, 1),
				new (1, 1)
			}
		};
	}
}



}
