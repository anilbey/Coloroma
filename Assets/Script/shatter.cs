using UnityEngine;
using System.Collections;

public class shatter : MonoBehaviour {

	static Vector3[] newVerts = new Vector3[3];
	static Vector3[] newNormals = new Vector3[3];
	static Vector2[] newUvs = new Vector2[3];
	public static bool InCollision = false;
	public static bool stopIt  = false;
	void Start()
	{
		InCollision = false;
	}

		public IEnumerator SplitMesh ()
		{
			if(ColorBars.redAmount > 0)
				ColorBars.redAmount -= 0.1f;
			if(ColorBars.greenAmount > 0)
					ColorBars.greenAmount -= 0.1f;
			if(ColorBars.blueAmount > 0)
					ColorBars.blueAmount -= 0.1f;
			MeshFilter MF = this.GetComponent<MeshFilter>();
			MeshRenderer MR = GetComponent<MeshRenderer>();
			Mesh M = MF.mesh;
			Vector3[] verts = M.vertices;
			Vector3[] normals = M.normals;
			Vector2[] uvs = M.uv;
			int submesh = 0;
				int[] indices = M.GetTriangles(submesh);
				for (int i = 0; i < indices.Length; i += 20)
				{				
					for (int n = 0; n < 2; n++)
					{
						newVerts[n] = verts[indices[i + n]];
						newUvs[n] = uvs[indices[i + n]];
						newNormals[n] = normals[indices[i + n]];
					}
					Mesh mesh = new Mesh();
					mesh.vertices = newVerts;
					mesh.normals = newNormals;
					mesh.uv = newUvs;
					
					mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };
					
					GameObject GO = new GameObject("Triangle " + (i / 2));
					GO.transform.position = transform.position;
					GO.transform.rotation = transform.rotation;
					GO.AddComponent<MeshRenderer>().material = MR.materials[submesh];
					GO.AddComponent<MeshFilter>().mesh = mesh;
					GO.AddComponent<BoxCollider>();
					GO.AddComponent<Rigidbody>().AddExplosionForce(100, transform.position, 30);
					Destroy(GO, Random.Range(0.0f, 1.0f));
					stopIt = true;
				}

			MR.enabled = false;
			yield return new WaitForSeconds(0.1f);

			Destroy(gameObject);
		}

		void OnCollisionEnter(Collision otherCollision)
		{
			if(ColorBars.circularDummy + 9 == otherCollision.gameObject.layer)
				return;
			if(!InCollision)
			{
				InCollision = true;
				this.transform.GetComponent<Collider>().enabled = false;
				StartCoroutine("SplitMesh");
			}
		}
	}