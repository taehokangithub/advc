package utils

import "fmt"

type GraphNode struct {
	Name             string
	DistanceFromRoot int
	edges            map[string]*GraphNode
}

type Graph struct {
	Root  *GraphNode
	nodes map[string]*GraphNode
}

func NewGraphNode(name string) *GraphNode {
	node := GraphNode{}
	node.Name = name
	node.edges = map[string]*GraphNode{}
	return &node
}

func NewGraph() *Graph {
	graph := Graph{}
	graph.nodes = map[string]*GraphNode{}
	return &graph
}

func (n *GraphNode) String() string {
	return fmt.Sprintf("[%s(%d)]", n.Name, n.DistanceFromRoot)
}

func (n *GraphNode) AddEdge(other *GraphNode) {
	n.edges[other.Name] = other
}

func (g *Graph) AddNode(name string) *GraphNode {
	node := NewGraphNode(name)
	g.nodes[name] = node

	if g.Root == nil {
		g.Root = node
	}
	return node
}

func (g *Graph) AddEdge(name1, name2 string) {
	node1 := g.addOrCreateNode(name1)
	node2 := g.addOrCreateNode(name2)
	node1.AddEdge(node2)
	node2.AddEdge(node1)
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

func (g *Graph) SetAllDistances() {
	visited := make(map[*GraphNode]bool)
	g.Root.traverseInternal(visited)
}

func (g *Graph) ForEachNodes(cb func(n *GraphNode)) {
	for _, n := range g.nodes {
		cb(n)
	}
}

//
// private methods
//

func (node *GraphNode) traverseInternal(visitied map[*GraphNode]bool) {
	visitied[node] = true
	for _, n := range node.edges {
		if _, ok := visitied[n]; !ok {
			n.DistanceFromRoot = node.DistanceFromRoot + 1
			n.traverseInternal(visitied)
		}
	}
}

func (g *Graph) addOrCreateNode(name string) *GraphNode {
	if n, ok := g.nodes[name]; ok {
		return n
	}
	return g.AddNode(name)
}
