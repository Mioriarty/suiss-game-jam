using UnityEngine;

public class AStarNode : MonoBehaviour
{

    public AStarNode cameFrom;

    public AStarNode[] connections;
    public float gScore;
    public float hScore;

    public float FScore => gScore + hScore;


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
        }
    }
}
