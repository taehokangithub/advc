package day18

import (
	"fmt"
	"os"
	"taeho/advc19_go/utils"
	"testing"
)

const m1 = `#########
#b.A.@.a#
#########`

const m2 = `########################
#f.D.E.e.C.b.A.@.a.B.c.#
######################.#
#d.....................#
########################`

const m3 = `########################
#...............b.C.D.f#
#.######################
#.....@.a.B.c.d.A.e.F.g#
########################`

const m4 = `#################
#i.G..c...e..H.p#
########.########
#j.A..b...f..D.o#
########@########
#k.E..a...g..B.n#
########.########
#l.F..d...h..C.m#
#################`

const m5 = `########################
#@..............ac.GI.b#
###d#e#f################
###A#B#C################
###g#h#i################
########################`

const m6 = `#######
#a.#Cd#
##@#@##
#######
##@#@##
#cB#.b#
#######`

const m7 = `###############
#d.ABC.#.....a#
######@#@######
###############
######@#@######
#b.....#.....c#
###############`

const m8 = `#############
#DcBa.#.GhKl#
#.###@#@#I###
#e#d#####j#k#
###C#@#@###J#
#fEbA.#.FgHi#
#############`

const m9 = `#############
#g#f.D#..h#l#
#F###e#E###.#
#dCba@#@BcIJ#
#############
#nK.L@#@G...#
#M###N#H###.#
#o#m..#i#jk.#
#############`

type searchQueueInterface interface {
	PushMove(*move)
	PopMove() *move
}

func testSearchQueueInterface(t *testing.T, sq searchQueueInterface) {
	samples := []int{3, 7, 5, 8, 9, 11, 2, 3, 5}
	expected := []int{2, 3, 3, 5, 5, 7, 8, 9, 11}
	locIncrement := 0
	for _, s := range samples {
		sq.PushMove(&move{
			loc:   utils.NewVector2D(locIncrement, 0),
			steps: s,
		})
		locIncrement++
	}

	for i := 0; i < len(expected); i++ {
		got := sq.PopMove().steps
		if got != expected[i] {
			t.Errorf("TestQueue at %d got %d expected %d", i, got, expected[i])
		}
	}
}

func TestSearchQueue(t *testing.T) {
	testSearchQueueInterface(t, newSearchQueue())
}

func TestSearchHeap(t *testing.T) {
	testSearchQueueInterface(t, newSearchMoveHeap())
}

func TestCopyGrid(t *testing.T) {
	k := NewKeyGrid(m2)
	k.steps = 123
	j := k.Copy()

	kstr := k.String()
	jstr := j.String()

	if kstr != jstr {
		t.Error("Copy failed, got", jstr)
		t.Error("expected", kstr)
	}
}

func TestCandidate(t *testing.T) {
	c := NewKeyGridHeap()
	k := NewKeyGrid(m1)
	k.steps = 5
	k2 := k.Copy()
	k2.steps = 3
	k2.state.myLocs = append(k2.state.myLocs, utils.NewVector2D(5, 2))
	k3 := k2.Copy()
	k3.steps = 4
	k3.state.myLocs = append(k2.state.myLocs, utils.NewVector2D(2, 5))
	k4 := k3.Copy()
	k4.state.keys['k'] = true
	k4.steps = 6
	k5 := k4.Copy()
	k5.state.keys['j'] = true
	k5.steps = 1

	c.PushKeyGrid(k)
	c.PushKeyGrid(k2)
	c.PushKeyGrid(k3)
	c.PushKeyGrid(k4)
	c.PushKeyGrid(k5)

	expected := []int{1, 3, 4, 5, 6}
	for i := 0; i < len(expected); i++ {
		p := c.PopKeyGrid()
		if p == nil {
			t.Error("case", i, "popped no candidate")
		} else if p.steps != expected[i] {
			t.Error("case", i, "got steps", p.steps, "expected", expected[i])
		}
	}
}

func TestKeyGraphBuild(t *testing.T) {
	strs := []string{m1, m2, m3, m4, m5, m6, m7, m8, m9}
	for i, str := range strs {
		k := NewKeyGrid(str)
		g := NewKeyGraph(k)
		g.BuildEdges()

		for _, vt := range g.vertexMap {
			for targetVertex, dist := range vt.edges {
				if targetDist, ok := targetVertex.edges[vt]; !ok {
					t.Errorf("Case [%d] %v target %v doesn't have a return edge", i, vt, targetVertex)
				} else if targetDist != dist {
					t.Errorf("Case [%d] %v - %v dist different : %d vs %d", i, vt, targetVertex, dist, targetDist)
				}
			}
		}
	}
}

func TestMinSteps(t *testing.T) {
	type Test struct {
		str   string
		steps int
	}
	file, err := os.ReadFile("../../data/input18.txt")
	if err != nil {
		panic(err)
	}
	mainCase := string(file)
	file, err = os.ReadFile("../../data/input18B.txt")
	if err != nil {
		panic(err)
	}

	mainCase2 := string(file)
	fmt.Println(mainCase[:1], mainCase2[:1]) // just to avoid "unused" error in case of commenting out

	cases := []Test{
		{m1, 8},
		{m2, 86},
		{m3, 132},
		{m4, 136},
		{m5, 81},

		{m6, 8},
		{m7, 24},
		{m8, 32},
		{m9, 72},

		{mainCase, 4510},
		{mainCase2, 1816},
	}

	for i, c := range cases {
		k := NewKeyGrid(c.str)
		got := k.FindMinSteps()
		if got != c.steps {
			t.Error("case", i, "got", got, "expected", c.steps)
		}
	}
}
