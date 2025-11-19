using System;
using System.Collections.Generic;

using UnityEngine;

using App.Application.PresentationDatabase;
using App.Game.Database;



namespace App.Infrastructure.External.Database.PresentationDatabase_Impl.Repositories {



public class ResourceTypePresentationRepository : IResourceTypePresentationRepository
{
	private readonly Dictionary<ResourceTypeId, ResourceTypePresentation> _resourceTypes = new();


	//----------------------------------------------------------------------------------------------


	public ResourceTypePresentationRepository()
	{
		var quadMesh = CreateQuadMesh();

		var resourceTypesDB = GameDatabase.Instance.Presentation.ResourceTypes;

		foreach (ResourceTypeId typeId in Enum.GetValues(typeof(ResourceTypeId))) {
			_resourceTypes[typeId] = new ResourceTypePresentation(
				quadMesh,
				resourceTypesDB.GetResourceTypeData(typeId).Material);
		}
	}


	public ResourceTypePresentation Get(ResourceTypeId resourceTypeId)
	{
		return _resourceTypes[resourceTypeId];
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
