using UnityEngine;

public class OrbSpawn : MonoBehaviour {

	#region Variables
	[Header("Spawnarea")]
	// Äußerer Kreis - Außerhalb dieses Kreises kann kein Orb gespawned werden.
	public float OuterCircleRadius;
	// Innerer Kreis - Innerhalb dieses Kreises kann kein Orb gespawned werden.
	public float InnerCircleRadius;

	[Header("Forbidden Spawn-Layers")]
	// In diesen Layern kann kein Orb gespawned werden.
	public LayerMask forbiddenCollisionLayers;
    #endregion


    #region UnityMethods

    void Start()
    {
		forbiddenCollisionLayers = LayerMask.GetMask("Obstacle");
    }

    public Vector3 GetOrbSpawnPos()
    {
		float xOffset;
		float yOffset;
		Vector3 spawnPos;
		Collider2D collider;
		do
		{
			if (Random.Range(0f, 1f) == 0)
				xOffset = Random.Range(-OuterCircleRadius, -InnerCircleRadius);

			else
				xOffset = Random.Range(InnerCircleRadius, OuterCircleRadius);

			if (Random.Range(0f, 1f) == 0)
				yOffset = Random.Range(-OuterCircleRadius, -InnerCircleRadius);

			else
				yOffset = Random.Range(InnerCircleRadius, OuterCircleRadius);


			spawnPos = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, 0);

			// Check for collision with wall
			collider = Physics2D.OverlapPoint(new Vector2(spawnPos.x, spawnPos.y), forbiddenCollisionLayers);

		} while (collider != null);

		return spawnPos;
    }

	
	#endregion
}
