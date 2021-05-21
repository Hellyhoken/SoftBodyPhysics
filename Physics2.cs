using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics2 : MonoBehaviour
{
    public float gravitation = -9.82f;
    public float stiffness = 14f;

    public Transform[] nodes;
    private Vector2[] velocities;
    private Vector2[] accelerations;

    public int[][] connections;
    private float[] defaultDistances;

	private void Start()
	{
        velocities = new Vector2[nodes.Length];
        accelerations = new Vector2[nodes.Length];

        defaultDistances = new float[connections.Length];
        for (int i = 0; i < connections.Length; i++) {
            defaultDistances[i] = Vector3.Distance(nodes[connections[i][0]].position, nodes[connections[i][1]].position);
        }
    }

	private void FixedUpdate()
	{
        for (int i = 0; i < nodes.Length; i++) {
            accelerations[i] = new Vector2(0, gravitation);
        }

        for (int i = 0; i < connections.Length; i++) {
            float forceMag = stiffness * (Vector3.Distance(nodes[connections[i][0]].position, nodes[connections[i][1]].position) - defaultDistances[i]);
            accelerations[connections[i][0]] = (nodes[connections[i][1]].position - nodes[connections[i][0]].position).normalized * forceMag;
            accelerations[connections[i][1]] = (nodes[connections[i][0]].position - nodes[connections[i][1]].position).normalized * forceMag;
        }
	}
}
