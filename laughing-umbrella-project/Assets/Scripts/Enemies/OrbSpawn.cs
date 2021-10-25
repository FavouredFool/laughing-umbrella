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

    public Vector3 GetOrbSpawnPos()
    {
		float xOffset;
		float yOffset;
		Vector3 spawnPos;
		//Collider2D collider;
		RaycastHit2D rayCollision;
		do
		{
			if (Random.Range(0, 2) == 0)
				xOffset = Random.Range(-OuterCircleRadius, -InnerCircleRadius);

			else
				xOffset = Random.Range(InnerCircleRadius, OuterCircleRadius);

			if (Random.Range(0, 2) == 0)
				yOffset = Random.Range(-OuterCircleRadius, -InnerCircleRadius);

			else
				yOffset = Random.Range(InnerCircleRadius, OuterCircleRadius);

			spawnPos = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, 0);

			// Check for wall between object and spawnpoint

			rayCollision = Physics2D.Raycast(gameObject.transform.position, spawnPos - gameObject.transform.position, Vector2.Distance(spawnPos, gameObject.transform.position), forbiddenCollisionLayers);
			//collider = Physics2D.OverlapPoint(new Vector2(spawnPos.x, spawnPos.y), forbiddenCollisionLayers);

		} while (rayCollision.collider != null);

		return spawnPos;
    }

	
	#endregion
}
