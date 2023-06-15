package day18

import "taeho/advc19_go/utils"

type edge struct {
	dist     int
	vertices [2]*vertex
}

type vertex struct {
	loc   utils.Vector
	edges []edge
}

func newVertex(loc *utils.Vector) *vertex {
	return &vertex{
		loc:   *loc,
		edges: make([]edge, 0, 60),
	}
}

type keyGraph struct {
	vertexMap map[string]*vertex
	k         *keyGrid
}

func newKeyGraph(k *keyGrid) *keyGraph {
	g := &keyGraph{
		vertexMap: make(map[string]*vertex),
		k:         k,
	}

	k.grid.Foreach(func(loc utils.Vector, val rune) {
		if k.IsKey(val) || k.IsDoor(val) {
			g.vertexMap[loc.String()] = newVertex(&loc)
		}
	})

	for _, loc := range k.state.myLocs {
		g.vertexMap[loc.String()] = newVertex(&loc)
	}

	return g
}

/*
func (g *keyGraph) buildEdges() {
	type gmove struct {
		origin *vertex
		loc    *utils.Vector
		dist   int
	}
	sq := utils.NewRingQueue[gmove](g.k.grid.Size.X * g.k.grid.Size.Y)

}
*/
