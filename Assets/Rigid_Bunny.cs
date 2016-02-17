using UnityEngine;
using System.Collections;

public class Rigid_Bunny : MonoBehaviour 
{
	public bool launched = false;
	
	public Vector3 x;							// position
	public Vector3 v    = new Vector3(0, 0, 0);	// velocity
	public Quaternion q = Quaternion.identity;	// quaternion
	public Vector3 w    = new Vector3(2, 0, 0);	// angular velocity
	
	public float m;
	public float mass;							// mass
	public Matrix4x4 I_body;					// body inertia

	public float linear_damping;				// for damping
	public float angular_damping;
	public float restitution;					// for collision
	
	
	// Use this for initialization
	void Start () 
	{
		//Initialize coefficients
		w = new Vector3 (0, 0, 2);
		x = new Vector3 (0, 0.6f, 0);
		v = new Vector3 (4, 2, 0);
		q = Quaternion.identity;
		linear_damping  = 0.999f;
		angular_damping = 0.98f;
		restitution 	= 0.5f;		// elastic collision
		m 				= 1;
		mass 			= 0; 
		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		for (int i=0; i<vertices.Length; i++) 
		{
			mass += m;
			float diag=m*vertices[i].sqrMagnitude;
			I_body[0, 0]+=diag;
			I_body[1, 1]+=diag;
			I_body[2, 2]+=diag;
			I_body[0, 0]-=m*vertices[i][0]*vertices[i][0];
			I_body[0, 1]-=m*vertices[i][0]*vertices[i][1];
			I_body[0, 2]-=m*vertices[i][0]*vertices[i][2];
			I_body[1, 0]-=m*vertices[i][1]*vertices[i][0];
			I_body[1, 1]-=m*vertices[i][1]*vertices[i][1];
			I_body[1, 2]-=m*vertices[i][1]*vertices[i][2];
			I_body[2, 0]-=m*vertices[i][2]*vertices[i][0];
			I_body[2, 1]-=m*vertices[i][2]*vertices[i][1];
			I_body[2, 2]-=m*vertices[i][2]*vertices[i][2];
		}
		I_body [3, 3] = 1;
	}
	
	Matrix4x4 Get_Cross_Matrix(Vector3 a)
	{
		// Get the cross product matrix of vector a
		Matrix4x4 A = Matrix4x4.zero;
		A [0, 0] = 0; 
		A [0, 1] = -a [2]; 
		A [0, 2] = a [1]; 
		A [1, 0] = a [2]; 
		A [1, 1] = 0; 
		A [1, 2] = -a [0]; 
		A [2, 0] = -a [1]; 
		A [2, 1] = a [0]; 
		A [2, 2] = 0; 
		A [3, 3] = 1;
		return A;
	}
	
	Matrix4x4 Quaternion_2_Matrix(Quaternion q)
	{
		// Get the rotation matrix R from quaternion q
		Matrix4x4 R = Matrix4x4.zero;
		R[0, 0]=q[3]*q[3]+q[0]*q[0]-q[1]*q[1]-q[2]*q[2];
		R[0, 1]=2*(q[0]*q[1]-q[3]*q[2]);
		R[0, 2]=2*(q[0]*q[2]+q[3]*q[1]);
		R[1, 0]=2*(q[0]*q[1]+q[3]*q[2]);
		R[1, 1]=q[3]*q[3]-q[0]*q[0]+q[1]*q[1]-q[2]*q[2];
		R[1, 2]=2*(q[1]*q[2]-q[3]*q[0]);
		R[2, 0]=2*(q[0]*q[2]-q[3]*q[1]);
		R[2, 1]=2*(q[1]*q[2]+q[3]*q[0]);
		R[2, 2]=q[3]*q[3]-q[0]*q[0]-q[1]*q[1]+q[2]*q[2];
		R[3, 3]=1;
		return R;
	}

	Quaternion Normalize_Quaternion(Quaternion q)
	{
		Quaternion result;
		float q_length=Mathf.Sqrt(q.x*q.x+q.y*q.y+q.z*q.z+q.w*q.w);
		result.x=q.x/q_length;
		result.y=q.y/q_length;
		result.z=q.z/q_length;
		result.w=q.w/q_length;
		return result;
	}

	Quaternion Quaternion_Multiply(Quaternion q1, Quaternion q2)
	{
		Vector3 v1 = new Vector3(q1.x, q1.y, q1.z);
		Vector3 v2 = new Vector3(q2.x, q2.y, q2.z); 
		float s1   = q1.w;
		float s2   = q2.w;
		Vector3 v = s1 * v2 + s2 * v1 + Vector3.Cross (v1, v2);
		float s   = s1 * s2 - Vector3.Dot (v1, v2);
		Quaternion q = new Quaternion (v.x, v.y, v.z, s);
		return q;
	}


	void Collision_Handler(float dt)
	{
        //TODO
    }


	// Update is called once per frame
	void Update () 
	{
		float dt = 0.02f;

		if(Input.GetKey("r"))
		{
			x = new Vector3 (0, 0.6f, 0);
			restitution = 0.5f;
			launched=false;
		}
		if(Input.GetKey("l"))
		{
			v = new Vector3 (4, 2, 0);
			launched=true;
		}

		// Part I: Update velocities
		

		// Part II: Collision Handler
		Collision_Handler (dt);

		// Part III: Update position & orientation
		// Update linear status
		
		// Update angular status
		

		// Part IV: Assign to the bunny object
		transform.position = x;
		transform.rotation = q;
	}
}
