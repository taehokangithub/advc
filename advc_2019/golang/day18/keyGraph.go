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

func newVertext(loc *utils.Vector) *vertex {
	return &vertex{
		loc:   *loc,
		edges: make([]edge, 0, 60),
	}
}

type keyGraph struct {
	vertices []*vertex
}

func newKeyGraph(k *keyGrid) *keyGraph {
	g := &keyGraph{
		vertices: make([]*vertex, 0, len(k.state.keys)*2+len(k.state.myLocs)+10),
	}

	k.grid.Foreach(func(loc utils.Vector, val rune) {
		if k.IsKey(val) || k.IsDoor(val) {
			g.vertices = append(g.vertices, newVertext(&loc))
		}
	})

	for _, l := range k.state.myLocs {
		g.vertices = append(g.vertices, newVertext(&l))
	}

	return g
}
