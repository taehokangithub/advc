package utils

import "fmt"

type GraphEdge struct {
	targetNode *GraphNode
	distance   int
}

type GraphNode struct {
	Name             string
	DistanceFromRoot int
	edges            map[string]GraphEdge
}

type Graph struct {
	Root            *GraphNode
	HasCircularLink bool
	nodes           map[string]*GraphNode
}

func NewGraphNode(name string) *GraphNode {
	node := GraphNode{}
	node.Name = name
	node.edges = map[string]GraphEdge{}
	return &node
}

func NewGraph() *Graph {
	graph := Graph{}
	graph.nodes = map[string]*GraphNode{}
	graph.HasCircularLink = false
	return &graph
}

func (n *GraphNode) String() string {
	return fmt.Sprintf("[%s(%d)]", n.Name, n.DistanceFromRoot)
}

func (n *GraphNode) AddEdge(other *GraphNode, weight int) {
	n.edges[other.Name] = GraphEdge{other, weight}
}

func (g *Graph) AddNode(name string) *GraphNode {
	node := NewGraphNode(name)
	g.nodes[name] = node

	if g.Root == nil {
		g.Root = node
	}
	return node
}

func (g *Graph) AddEdgeWithWeight(name1, name2 string, weight int) {
	/* It doesn't work - it can happen with a tree, if the order by edges are random
	   Need some way to detect circular link

	if !g.HasCircularLink && g.GetNode(name1) != nil && g.GetNode(name2) != nil {
		fmt.Println("Found circular link", name1, name2)
		g.HasCircularLink = true
	}
	*/

	node1 := g.addOrCreateNode(name1)
	node2 := g.addOrCreateNode(name2)
	node1.AddEdge(node2, weight)
	node2.AddEdge(node1, weight)
}

func (g *Graph) AddEdge(name1, name2 string) {
	g.AddEdgeWithWeight(name1, name2, 1)
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
	if g.HasCircularLink {
		panic("Not implemented for circular link - requires Dijkstra")
	} else {
		g.Root.traverseInternal(map[*GraphNode]bool{})
	}
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
	for _, edge := range node.edges {
		if _, ok := visitied[edge.targetNode]; !ok {
			edge.targetNode.DistanceFromRoot = node.DistanceFromRoot + edge.distance
			edge.targetNode.traverseInternal(visitied)
		}
	}
}

func (g *Graph) addOrCreateNode(name string) *GraphNode {
	if n := g.GetNode(name); n != nil {
		return n
	}
	return g.AddNode(name)
}
