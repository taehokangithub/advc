package day18

import (
	"fmt"
	"strings"
	"taeho/advc19_go/utils"
)

type gmove struct {
	loc  utils.Vector
	dist int
}

type vertex struct {
	tile  rune
	loc   utils.Vector
	edges map[*vertex]int // distance to the vertex
}

func (v *vertex) String() string {
	return fmt.Sprintf("[%v:%v]", string(v.tile), v.loc)
}

func newVertex(loc *utils.Vector, tile rune) *vertex {
	return &vertex{
		loc:   *loc,
		tile:  tile,
		edges: map[*vertex]int{},
	}
}

type keyGraph struct {
	vertexMap   map[string]*vertex
	k           *keyGrid
	searchQueue *utils.RingQueue[gmove]
}

func NewKeyGraph(k *keyGrid) *keyGraph {
	g := &keyGraph{
		vertexMap:   make(map[string]*vertex),
		k:           k,
		searchQueue: utils.NewRingQueue[gmove](k.grid.Size.X * k.grid.Size.Y),
	}

	k.grid.Foreach(func(loc utils.Vector, val rune) {
		if k.IsKey(val) || k.IsDoor(val) {
			g.vertexMap[loc.String()] = newVertex(&loc, val)
		}
	})

	for _, loc := range k.state.myLocs {
		g.vertexMap[loc.String()] = newVertex(&loc, TILE_ME)
	}

	return g
}

func (g *keyGraph) BuildEdges() {
	for _, v := range g.vertexMap {
		g.buildEdgesInternal(v)
	}
}

func (g *keyGraph) buildEdgesInternal(vt *vertex) {
	g.searchQueue.Clear()
	g.searchQueue.Push(gmove{loc: vt.loc, dist: 0})
	visited := map[string]bool{}

	for g.searchQueue.Len() > 0 {
		move := g.searchQueue.Pop()
		move.dist++
		visited[move.loc.String()] = true

		for _, dvec := range utils.DIR_VECTORS {
			movedVec := move.loc.GetAdded(dvec)
			if visited[movedVec.String()] {
				continue
			}
			tile := g.k.grid.GetFast(&movedVec)
			if tile == TILE_WALL {
				continue
			} else if tile == TILE_EMPTY && !g.k.isMyLoc(movedVec) {
				g.searchQueue.Push(gmove{loc: movedVec, dist: move.dist})
			} else {
				targetVertex, ok := g.vertexMap[movedVec.String()]
				if !ok {
					panic(fmt.Sprint("There is nothing at ", movedVec.String(), " TILE ", tile))
				}

				vt.edges[targetVertex] = move.dist
			}
		}
	}
}

func (g *keyGraph) String() string {
	builder := strings.Builder{}
	builder.WriteString(fmt.Sprintf("[Key Graph : %d vertices]", len(g.vertexMap)))
	for locstr, vt := range g.vertexMap {
		builder.WriteString(fmt.Sprintf("[Vertex %v] at %v", string(vt.tile), locstr))
		for targetvt, dist := range vt.edges {
			builder.WriteString(fmt.Sprintf("       %v, distance %d", targetvt.String(), dist))
		}
	}
	return builder.String()
}
