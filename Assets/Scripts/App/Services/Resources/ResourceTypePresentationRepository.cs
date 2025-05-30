﻿using System.Collections.Generic;

using UnityEngine;



namespace App.Services.Resources {



public class ResourceTypePresentationRepository : IResourceTypePresentationRepository
{
	private readonly Dictionary<uint, ResourceTypePresentation> _resourceTypes = new();



	//----------------------------------------------------------------------------------------------
	// public

	public ResourceTypePresentationRepository()
	{
		var quadMesh = CreateQuadMesh();

		var appleMaterial = UnityEngine.Resources.Load<Material>("Materials/Resources/Apple");
		var bananasMaterial = UnityEngine.Resources.Load<Material>("Materials/Resources/Bananas");
		var citrusMaterial = UnityEngine.Resources.Load<Material>("Materials/Resources/Citrus");
		var wheatMaterial = UnityEngine.Resources.Load<Material>("Materials/Resources/Wheat");


		_resourceTypes[0] = new ResourceTypePresentation { Mesh = quadMesh, Material = appleMaterial };
		_resourceTypes[1] = new ResourceTypePresentation { Mesh = quadMesh, Material = bananasMaterial };
		_resourceTypes[2] = new ResourceTypePresentation { Mesh = quadMesh, Material = citrusMaterial };
		_resourceTypes[3] = new ResourceTypePresentation { Mesh = quadMesh, Material = wheatMaterial };
	}


	public ResourceTypePresentation Get(uint resourceTypeId)
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
