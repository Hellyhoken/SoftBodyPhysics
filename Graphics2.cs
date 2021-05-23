using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics2 : MonoBehaviour
{
    public Physics2 physics = null;
    private LineRenderer[] lines = null;

	public GameObject linePrefab = null;

	private void Start()
	{
		lines = new LineRenderer[physics.connections.Length];
		for (int i = 0; i < physics.connections.Length; i++) {
			lines[i] = Instantiate(linePrefab, transform.root).GetComponent<LineRenderer>();
			lines[i].positionCount = 2;
		}
	}

	private void FixedUpdate()
	{
		for (int i = 0; i < lines.Length; i++) {
			Vector3[] ends = { physics.nodes[physics.connections[i].x].position, physics.nodes[physics.connections[i].y].position };
			lines[i].SetPositions(ends);
		}
	}
}
