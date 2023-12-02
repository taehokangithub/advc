package utils

type Tree struct {
	Graph
}

func NewTree() *Tree {
	tree := Tree{}
	tree.Graph = *NewGraph()
	return &tree
}

func (t *Tree) SetAllDistances() {
	t.traverse(t.Root, nil)
}

func (t *Tree) traverse(node *GraphNode, parent *GraphNode) {
	for _, edge := range node.edges {
		if parent != edge.targetNode {
			edge.targetNode.DistanceFromRoot = node.DistanceFromRoot + edge.distance
			t.traverse(edge.targetNode, node)
		}
	}
}
