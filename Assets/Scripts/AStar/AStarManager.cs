using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AStarManager : MonoBehaviour
{
    public static AStarManager instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Vector3> GeneratePath(Vector3 start, Vector3 end, bool tryConnectedStartNodes = true)
    {
        AStarNode orignialStartNode = FindNearestAStarNode(start);
        AStarNode endNode = FindNearestAStarNode(end);

        AStarNode[] startNodes;
        if (tryConnectedStartNodes)
        {
            List<AStarNode> tempStartNodes = new List<AStarNode> { orignialStartNode };
            tempStartNodes.AddRange(orignialStartNode.connections);
            startNodes = tempStartNodes.ToArray();
        }
        else
        {
            startNodes = new AStarNode[] { orignialStartNode };
        }

        List<AStarNode> shortestPath = null;
        foreach (AStarNode startNode in startNodes)
        {
            List<AStarNode> nodesPath = GeneratePath(startNode, endNode);
            Debug.Log(nodesPath.Count);
            if (nodesPath != null && (shortestPath == null || nodesPath.Count < shortestPath.Count))
            {
                shortestPath = nodesPath;
            }
        }

        List<Vector3> path = new List<Vector3>();

        if (shortestPath != null)
        {
            foreach (AStarNode n in shortestPath)
            {
                path.Add(n.transform.position);
            }
        }

        path.Add(end);

        return path;
    }

    public List<AStarNode> GeneratePath(AStarNode start, AStarNode end)
    {
        List<AStarNode> openSet = new List<AStarNode>();

        foreach(AStarNode n in FindObjectsByType<AStarNode>(FindObjectsSortMode.None))
        {
            n.gScore = float.MaxValue;
        }

        start.gScore = 0;
        start.hScore = Vector2.Distance(start.transform.position, end.transform.position);
        openSet.Add(start);

        while(openSet.Count > 0)
        {
            int lowestF = default;

            for(int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FScore < openSet[lowestF].FScore)
                {
                    lowestF = i;
                }
            }

            AStarNode currentAStarNode = openSet[lowestF];
            openSet.Remove(currentAStarNode);

            if(currentAStarNode == end)
            {
                List<AStarNode> path = new List<AStarNode>();

                path.Insert(0, end);

                while(currentAStarNode != start)
                {
                    currentAStarNode = currentAStarNode.cameFrom;
                    path.Add(currentAStarNode);
                }

                path.Reverse();
                return path;
            }

            foreach(AStarNode connectedAStarNode in currentAStarNode.connections)
            {
                float heldGScore = currentAStarNode.gScore + Vector2.Distance(currentAStarNode.transform.position, connectedAStarNode.transform.position);

                if(heldGScore < connectedAStarNode.gScore)
                {
                    connectedAStarNode.cameFrom = currentAStarNode;
                    connectedAStarNode.gScore = heldGScore;
                    connectedAStarNode.hScore = Vector2.Distance(connectedAStarNode.transform.position, end.transform.position);

                    if (!openSet.Contains(connectedAStarNode))
                    {
                        openSet.Add(connectedAStarNode);
                    }
                }
            }
        }

        return null;
    }

    public AStarNode FindNearestAStarNode(Vector2 pos)
    {
        AStarNode foundAStarNode = null;
        float minDistance = float.MaxValue;

        foreach(AStarNode AStarNode in FindObjectsByType<AStarNode>(FindObjectsSortMode.None))
        {
            float currentDistance = Vector2.Distance(pos, AStarNode.transform.position);

            if(currentDistance < minDistance)
            {
                minDistance = currentDistance;
                foundAStarNode = AStarNode;
            }
        }

        return foundAStarNode;
    }

    public AStarNode FindFurthestAStarNode(Vector2 pos)
    {
        AStarNode foundAStarNode = null;
        float maxDistance = default;

        foreach (AStarNode AStarNode in FindObjectsByType<AStarNode>(FindObjectsSortMode.None))
        {
            float currentDistance = Vector2.Distance(pos, AStarNode.transform.position);
            if(currentDistance > maxDistance)
            {
                maxDistance = currentDistance;
                foundAStarNode = AStarNode;
            }
        }

        return foundAStarNode;
    }

    public AStarNode[] AllAStarNodes()
    {
        return FindObjectsByType<AStarNode>(FindObjectsSortMode.None);
    }
}