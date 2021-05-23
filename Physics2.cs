using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics2 : MonoBehaviour
{
    public float gravitation = -9.82f;
    public float stiffness = 14f;
    public float damping = 20f;

    public Transform[] nodes;
    private Vector2[] velocities;
    private Vector2[] accelerations;

    public Vector2Int[] connections;
    private float[] defaultDistances;
    private float[] prevDistances;

    private void Start()
	{
        velocities = new Vector2[nodes.Length];
        accelerations = new Vector2[nodes.Length];

        defaultDistances = new float[connections.Length];
        for (int i = 0; i < connections.Length; i++) {
            defaultDistances[i] = Vector3.Distance(nodes[connections[i].x].position, nodes[connections[i].y].position);
        }
        prevDistances = new float[defaultDistances.Length];
        for (int i = 0; i < defaultDistances.Length; i++)
        {
            prevDistances[i] = defaultDistances[i];
        }
    }

	private void FixedUpdate()
	{
        for (int i = 0; i < nodes.Length; i++) {
            accelerations[i] = new Vector2(0, gravitation);
        }

        for (int i = 0; i < connections.Length; i++) {
            float dist = Vector3.Distance(nodes[connections[i].x].position, nodes[connections[i].y].position);
            float forceMagS = stiffness * (dist - defaultDistances[i]);
            float forceMagD = ((dist - prevDistances[i]) / Time.fixedDeltaTime) * damping;
            Vector3 dir = (nodes[connections[i].x].position - nodes[connections[i].y].position).normalized;
            Vector2 forceDir = new Vector2(dir.x,dir.y);
            accelerations[connections[i].x] += -forceDir * forceMagS;
            accelerations[connections[i].x] += forceDir * forceMagD;
            accelerations[connections[i].y] += forceDir * forceMagS;
            accelerations[connections[i].y] += -forceDir * forceMagD;
            prevDistances[i] = dist;
        }

        for (int i = 0; i < nodes.Length; i++)
        {
            Vector2[] results = WallStop(nodes[i].position,velocities[i],accelerations[i]);
            nodes[i].position = new Vector3(results[0].x,results[0].y,0);
            velocities[i] = results[1];
            accelerations[i] = results[2];

            velocities[i] += accelerations[i] * Time.fixedDeltaTime;
            nodes[i].position += new Vector3(velocities[i].x,velocities[i].y,0) * Time.fixedDeltaTime;
        }
    }

    private Vector2[] WallStop(Vector3 position, Vector2 velocity, Vector2 acceleration) {
        if (position.y >= 8) {
            position = new Vector3(position.x, 8, 0);
            if (velocity.y > 0) { velocity.y = 0; }
            if (acceleration.y > 0) { acceleration.y = 0; }
        }
        else if (position.y <= -20) {
            position = new Vector3(position.x, -20, 0);
            if (velocity.y < 0) { velocity.y = 0; }
            if (acceleration.y < 0) { acceleration.y = 0; }
        }
        if (position.x >= 30)
        {
            position = new Vector3(30, position.y, 0);
            if (velocity.x > 0) { velocity.x = 0; }
            if (acceleration.x > 0) { acceleration.x = 0; }
        }
        else if (position.x <= -30)
        {
            position = new Vector3(-30, position.y, 0);
            if (velocity.x < 0) { velocity.x = 0; }
            if (acceleration.x < 0) { acceleration.x = 0; }
        }

        Vector2[] result = { new Vector2(position.x, position.y), velocity, acceleration };
        return result;
    }
}
