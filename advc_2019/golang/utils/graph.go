package utils

import (
	"fmt"
	"math"
)

type Graph struct {
	Root  *GraphNode
	nodes map[string]*GraphNode
}

func NewGraph() *Graph {
	graph := Graph{}
	graph.nodes = map[string]*GraphNode{}
	return &graph
}

func (g *Graph) AddNode(name string) *GraphNode {
	node := NewGraphNode(name)
	g.nodes[name] = node

	if g.Root == nil {
		g.Root = node
	}
	return node
}

func (g *Graph) AddEdgeWithDistance(name1, name2 string, distance int) {
	node1 := g.addOrCreateNode(name1)
	node2 := g.addOrCreateNode(name2)
	node1.AddEdge(node2, distance)
	node2.AddEdge(node1, distance)
}

func (g *Graph) AddEdge(name1, name2 string) {
	g.AddEdgeWithDistance(name1, name2, 1)
}

func (g *Graph) SetRoot(name string) {
	if rootNode, ok := g.nodes[name]; ok {
		g.Root = rootNode
	} else {
		panic(fmt.Sprintf("SetRoot - node %s not found", name))
	}
}

func (g *Graph) GetNode(name string) *GraphNode {
	return g.nodes[name]
}

func (g *Graph) ForEachNodes(cb func(n *GraphNode)) {
	for _, n := range g.nodes {
		cb(n)
	}
}

// Dijkstra
func (g *Graph) SetAllDistances() {
	toVisit := make(map[*GraphNode]bool)

	for _, n := range g.nodes {
		n.DistanceFromRoot = math.MaxInt
		toVisit[n] = true
	}

	g.Root.DistanceFromRoot = 0

	for node := g.Root; len(toVisit) > 1; {

		delete(toVisit, node)

		for _, edge := range node.edges {
			target := edge.targetNode
			potentialDistance := node.DistanceFromRoot + edge.distance

			if target.DistanceFromRoot > potentialDistance {
				target.DistanceFromRoot = potentialDistance
			}
		}

		var nextVisit *GraphNode

		for n := range toVisit {
			if nextVisit == nil || nextVisit.DistanceFromRoot > n.DistanceFromRoot {
				nextVisit = n
			}
		}
		node = nextVisit
	}
}

//
// internal methods
//

func (g *Graph) addOrCreateNode(name string) *GraphNode {
	if n := g.GetNode(name); n != nil {
		return n
	}
	return g.AddNode(name)
}
