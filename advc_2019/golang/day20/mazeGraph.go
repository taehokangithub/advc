package day20

import (
	"fmt"
	"strings"
	"taeho/advc19_go/utils"
)

type Node struct {
	loc   utils.Vector
	edges map[*Node]int
}

type MazeGraph struct {
	maze  *Maze
	nodes map[utils.Vector]*Node
}

var mqGraph *utils.RingQueue[*Move] = utils.NewRingQueue[*Move](1000)

func NewNode(loc *utils.Vector) *Node {
	return &Node{
		loc:   *loc,
		edges: make(map[*Node]int),
	}
}

func (n *Node) String() string {
	return fmt.Sprint("[", n.loc, "/", len(n.edges), "edges]")
}

func (n *Node) Name(m *Maze) string {
	builder := &strings.Builder{}
	builder.WriteString(fmt.Sprint("[", n.loc, "/"))
	if m.entrance == n.loc {
		builder.WriteString("Entrance")
	} else if m.exit == n.loc {
		builder.WriteString("Exit")
	} else if p, ok := m.portals[n.loc]; ok {
		builder.WriteString(p.name)
		if m.IsOuter(&n.loc) {
			builder.WriteString(" outer")
		} else {
			builder.WriteString(" inner")
		}
	}
	builder.WriteString("]")

	return builder.String()
}

func (n *Node) addEdge(g *MazeGraph, loc *utils.Vector, dist int) (nodeToExplore *Node) {
	var other *Node
	if v, ok := g.nodes[*loc]; ok {
		other = v
	} else {
		other = NewNode(loc)
		g.nodes[*loc] = other
		nodeToExplore = other
	}
	n.edges[other] = dist
	return
}

func NewMageGraph(str string) *MazeGraph {
	maze := NewMaze(str)
	g := &MazeGraph{
		maze:  maze,
		nodes: make(map[utils.Vector]*Node),
	}

	headNode := g.addNewNode(&g.maze.entrance)
	g.findReachableNodes(headNode)
	return g
}

func (g *MazeGraph) addNewNode(loc *utils.Vector) *Node {
	newNode := NewNode(loc)
	g.nodes[*loc] = newNode
	return newNode
}

func (g *MazeGraph) findReachableNodes(node *Node) {
	mqGraph.Clear()
	visited := map[utils.Vector]bool{}
	mv := &Move{
		loc:  node.loc,
		dist: 0,
	}
	mqGraph.Push(mv)
	nodesToExplore := []*Node{}

	addEdge := func(loc *utils.Vector, dist int) {
		if newNode := node.addEdge(g, loc, dist); newNode != nil {
			nodesToExplore = append(nodesToExplore, newNode)
		}
	}

	for !mqGraph.IsEmpty() {
		mv = mqGraph.Pop()
		dist := mv.dist + 1
		visited[mv.loc] = true

		if p, ok := g.maze.portals[mv.loc]; ok {
			portalExist := p.GetPortalExit(&mv.loc)
			if _, ok := visited[*portalExist]; ok {
				continue
			}
			addEdge(portalExist, dist)
		}

		for _, dvec := range utils.DIR_VECTORS {
			movedLoc := dvec.GetAdded(mv.loc)
			if _, ok := visited[movedLoc]; ok {
				continue
			}
			if _, ok := g.maze.portals[movedLoc]; ok {
				addEdge(&movedLoc, dist)
			} else {
				tile := g.maze.grid.GetFast(&movedLoc)
				if tile == TILE_ROAD {
					mqGraph.Push(&Move{
						loc:  movedLoc,
						dist: dist,
					})
				} else if tile == TILE_ENTRANCE || tile == TILE_EXIT {
					addEdge(&movedLoc, dist)
				}
			}

		}
	}

	for _, newNode := range nodesToExplore {
		g.findReachableNodes(newNode)
	}
}
