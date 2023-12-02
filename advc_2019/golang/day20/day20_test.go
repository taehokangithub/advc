package day20

import (
	"taeho/advc19_go/utils"
	"testing"	
)

const MZ1 string = `         A           
         A           
  #######.#########  
  #######.........#  
  #######.#######.#  
  #######.#######.#  
  #######.#######.#  
  #####  B    ###.#  
BC...##  C    ###.#  
  ##.##       ###.#  
  ##...DE  F  ###.#  
  #####    G  ###.#  
  #########.#####.#  
DE..#######...###.#  
  #.#########.###.#  
FG..#########.....#  
  ###########.#####  
             Z       
             Z       `

func TestPortalNameContainer(t *testing.T) {
	p := NewPortalNameContainer()

	p.AddPartialName('A', utils.NewVector2D(3, 3))
	p.AddPartialName('G', utils.NewVector2D(10, 11))
	p.AddPartialName('H', utils.NewVector2D(10, 10))
	p.AddPartialName('I', utils.NewVector2D(20, 20))
	p.AddPartialName('K', utils.NewVector2D(19, 20))
	p.AddPartialName('I', utils.NewVector2D(30, 20))
	p.AddPartialName('B', utils.NewVector2D(3, 4))
	p.AddPartialName('J', utils.NewVector2D(31, 20))

	expectedNames := map[string]bool{
		"AB": true,
		"GH": true,
		"IK": true,
		"IJ": true,
	}

	for _, pg := range p.portalGates {
		if _, ok := expectedNames[pg.name]; !ok {
			t.Error("Unexpected name", pg.name)
		}
	}
}

func TestPortalCreation(t *testing.T) {
	m := NewMaze(MZ1)

	if m.entrance.X != 9 || m.entrance.Y != 2 {
		t.Error("entrance loc is wrong", m.entrance, "expected 9,2")
	}
	if m.exit.X != 13 || m.exit.Y != 16 {
		t.Error("exit loc is wrong", m.exit, "expected 13, 16")
	}
	for _, portal := range m.portals {
		if m.IsOuter(&portal.locInner) {
			t.Error("loc", portal.locInner, "should be inner")
		}
		if !m.IsOuter(&portal.locOuter) {
			t.Error("loc", portal.locOuter, "should be outer")
		}
	}
	sampleLoc := utils.NewVector2D(11, 12)
	if _, ok := m.portals[sampleLoc]; !ok {
		t.Error("Sample loc", sampleLoc, "does not exist in portals")
	}
}

func TestFindExit(t *testing.T) {

	type Case struct {
		str      string
		expected int
	}
	cases := []Case{
		{MZ1, 23},
		//{MZ2, 58},
	}

	for i, c := range cases {
		mz := NewMaze(c.str)
		got := mz.FindShortestPath(false)

		if got != c.expected {
			t.Error("case", i, "got", got, "expected", c.expected)
		}
	}
}

func TestGraph(t *testing.T) {
	g := NewMageGraph(MZ1)

	expected := 8
	if len(g.nodes) != expected {
		t.Error("Got", len(g.nodes), "nodes, expected", expected, g.nodes)
	}
}

func TestMoveHeap(t *testing.T) {
	moves := []*GraphMove{
		{node: &Node{loc: utils.NewVector2D(1, 2)}, dist: 1},
		{node: &Node{loc: utils.NewVector2D(2, 3)}, dist: 5},
		{node: &Node{loc: utils.NewVector2D(1, 5)}, dist: 7},
		{node: &Node{loc: utils.NewVector2D(2, 3)}, dist: 3},
		{node: &Node{loc: utils.NewVector2D(1, 2)}, dist: 6},
		{node: &Node{loc: utils.NewVector2D(5, 5)}, dist: 10},
		{node: &Node{loc: utils.NewVector2D(1, 8)}, dist: 6},
	}

	expected := []*GraphMove{
		{node: &Node{loc: utils.NewVector2D(1, 2)}, dist: 1},
		{node: &Node{loc: utils.NewVector2D(2, 3)}, dist: 3},
		{node: &Node{loc: utils.NewVector2D(1, 8)}, dist: 6},
		{node: &Node{loc: utils.NewVector2D(1, 5)}, dist: 7},
		{node: &Node{loc: utils.NewVector2D(5, 5)}, dist: 10},
	}

	mvHeap := utils.NewHeap[*GraphMove]()

	for _, mv := range moves {
		mvHeap.Push(mv)
	}

	if mvHeap.Len() != len(expected) {
		t.Error("Contains", mvHeap.Len(), "expected", len(expected))
		return
	}

	for _, mv := range expected {
		heapmv := mvHeap.Pop()
		if mv.dist != heapmv.dist || mv.node.loc != heapmv.node.loc {
			t.Error("Popped", heapmv, "expected", mv)
		}
	}
}

func TestGraphSearch(t *testing.T) {
	g := NewMageGraph(MZ1)
	ans := g.FindShortestPathToExit()
	expected := 23
	if ans != expected {
		t.Error("Got", ans, "expected", expected)
	}
}
