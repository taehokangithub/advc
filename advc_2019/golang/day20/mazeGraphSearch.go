package day20

import (
	"fmt"
	"taeho/advc19_go/utils"
)

type GraphMove struct {
	node  *Node
	dist  int
	layer int
}

func (mv *GraphMove) GetUniqueKey() string {
	return fmt.Sprintf("[%v:%d]", mv.node.loc, mv.layer)
}

func (mv *GraphMove) GetCost() int {
	return mv.dist
}

func (g *MazeGraph) FindShortestPathToExit() int {
	mvHeap := utils.NewHeap[*GraphMove]()
	mvHeap.Push(&GraphMove{
		node: g.nodes[g.maze.entrance],
	})

	for mvHeap.Len() > 0 {
		mv := mvHeap.Pop()
		tile := g.maze.grid.GetFast(&mv.node.loc)
		if tile == TILE_EXIT {
			return mv.dist
		}

		for otherNode, dist := range mv.node.edges {
			mvHeap.Push(&GraphMove{
				node: otherNode,
				dist: dist + mv.dist,
			})
		}
	}
	panic("Coud not find answer")
}

func (g *MazeGraph) FindLayeredShortestPathToExit() int {
	mvHeap := utils.NewHeap[*GraphMove]()
	mvHeap.Push(&GraphMove{g.nodes[g.maze.entrance], 0, 0})
	visited := NewVisitMap()

	for mvHeap.Len() > 0 {
		mv := mvHeap.Pop()
		visited.SetVisited(mv.layer, &mv.node.loc)

		tile := g.maze.grid.GetFast(&mv.node.loc)
		if tile == TILE_EXIT {
			return mv.dist
		}

		for otherNode, dist := range mv.node.edges {
			if newLayer, ok := g.GetLayer(&otherNode.loc, mv); ok {
				if visited.GetVisited(newLayer, &otherNode.loc) {
					continue
				}
				mvHeap.Push(&GraphMove{otherNode, dist + mv.dist, newLayer})
			}
		}
	}
	panic("Coud not find answer")
}

func (g *MazeGraph) GetLayer(loc *utils.Vector, mv *GraphMove) (newLayer int, isPossible bool) {
	isPossible = false
	newLayer = mv.layer
	if newLayer > 0 && (*loc == g.maze.exit || *loc == g.maze.entrance) {
		return
	}
	if p, ok := g.maze.portals[*loc]; ok {
		isOuter := g.maze.IsOuter(loc)
		if *p.GetPortalExit(loc) == mv.node.loc {
			if isOuter {
				newLayer++
			} else {
				newLayer--
			}
		} else if mv.layer == 0 && isOuter {
			return
		}
	}

	isPossible = true
	return
}
