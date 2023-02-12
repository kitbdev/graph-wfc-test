using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Kutil;

public class GraphNodeHolder : MonoBehaviour {
    // hold mat
    public MaterialCon matCon;
    public float conRange = 3;
    public List<GraphNodeHolder> connections = new List<GraphNodeHolder>();

    [AddButton(nameof(ClearConnections))]
    [AddButton(nameof(FindConnections))]
    [AddButton(nameof(ConvertToGraph), buttonLayout = AddButtonAttribute.ButtonLayout.AFTER)]
    public VolGraph graph;

    [Space]
    [ReadOnly]
    public VolNode node;

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
        graph.UpdateNodes();
    }
    void ClearProp() {
        if (node == null || node.connectedNodes == null) return;
        node = null;
        foreach (var connode in connections) {
            connode.ClearProp();
        }
    }
    VolNode ToNode() {
        if (node != null) {
            return node;
        }
        node = new VolNode();
        node.pos = transform.position;
        node.radius = conRange;
        node.mat = matCon;
        var neis = GetNeighbors();
        foreach (var nei in neis) {
            node.ConnectTo(nei);
        }
        return node;
    }
    List<VolNode> GetNeighbors() {
        return connections.Select(g => g.ToNode()).ToList();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = new Color(0.3f, 0.9f, 0.7f, 0.5f);
        Gizmos.DrawSphere(transform.position, conRange);
        Gizmos.color = Color.white;
    }
}
