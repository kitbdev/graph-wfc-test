using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class GraphNodeHolder : MonoBehaviour {
    // hold mat
    public MaterialCon matCon;
    public float conRange = 3;
    public List<GraphNodeHolder> connections = new List<GraphNodeHolder>();

    // [Kutil.AddButton(nameof(FindConnections))]
    // public int dummyCon;

    [Kutil.AddButton(nameof(ConvertToGraph), buttonLayout = Kutil.AddButtonAttribute.ButtonLayout.AFTER)]
    public VolGraph graph;

    [Space]
    [Kutil.ReadOnly]
    public Node node;

    [ContextMenu("clear connections")]
    public void ClearConnections() {
        connections.Clear();
    }
    [ContextMenu("find connections")]
    public void FindConnections() {
        ClearConnections();

        Collider[] colliders = Physics.OverlapSphere(transform.position, conRange);
        List<GraphNodeHolder> matches = colliders
                                .Select(c => c.gameObject.GetComponent<GraphNodeHolder>())
                                .Except(new GraphNodeHolder[] { this })
                                .ToList();
        connections.AddRange(matches);
    }

    public void ConvertToGraph() {
        if (graph == null) return;
        ClearProp();
        graph.rootNode = ToNode();
    }
    void ClearProp() {
        if (node == null || node.connectedNodes == null) return;
        node.connectedNodes = null;
        foreach (var connode in connections) {
            connode.ClearProp();
        }
    }
    Node ToNode() {
        if (node == null) {
            node = new Node();
            // return node;
        }
        node.pos = transform.position;
        node.radius = conRange;
        node.mat = matCon;
        if (node.connectedNodes == null) {
            node.connectedNodes = new();
            node.connectedNodes = GetNeighbors();
        }
        return node;
    }
    List<Node> GetNeighbors() {
        return connections.Select(g => g.ToNode()).ToList();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = new Color(0.3f, 0.9f, 0.7f, 0.5f);
        Gizmos.DrawSphere(transform.position, conRange);
        Gizmos.color = Color.white;
    }
}
