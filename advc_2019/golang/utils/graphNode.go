package utils

import "fmt"

type GraphEdge struct {
	targetNode *GraphNode
	distance   int
}

type GraphNode struct {
	Name             string
	DistanceFromRoot int
	Value            int
	StrValue         string
	edges            map[string]GraphEdge
}

func NewGraphNode(name string) *GraphNode {
	node := GraphNode{}
	node.Name = name
	node.edges = map[string]GraphEdge{}
	return &node
}

func (n *GraphNode) String() string {
	return fmt.Sprintf("[%s(%d)]", n.Name, n.DistanceFromRoot)
}

func (n *GraphNode) AddEdge(other *GraphNode, distance int) {

	if _, exist := n.edges[other.Name]; exist {
		fmt.Printf("Warning! Node [%s] already has an edge to [%s]\n", n.Name, other.Name)
	}
	n.edges[other.Name] = GraphEdge{other, distance}
}
