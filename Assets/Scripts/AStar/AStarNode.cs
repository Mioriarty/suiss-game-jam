using UnityEngine;

public class AStarNode : MonoBehaviour
{

    public AStarNode cameFrom;

    public AStarNode[] connections;
    public float gScore;
    public float hScore;

    public float FScore => gScore + hScore;

    public bool onlyTarget = false;

    public AStarNode[] NonTargetConnections => System.Array.FindAll(connections, conn => !conn.onlyTarget);


    private void OnDrawGizmos()
    {
        if (connections != null)
        {
            Gizmos.color = Color.cyan;
            foreach (var connection in connections)
            {
                if (connection != null)
                {
                    Gizmos.DrawLine(transform.position, connection.transform.position);
                }
            }

            if (onlyTarget)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(transform.position, 0.5f);
            }
        }
    }
}
