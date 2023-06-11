package day18

import (
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
	c := NewCandidate()
	k := NewKeyGrid(m1)
	k.steps = 5
	k2 := k.Copy()
	k2.steps = 3
	k2.state.myLocs = append(k2.state.myLocs, utils.NewVector2D(5, 2))
	k3 := k2.Copy()
	k3.steps = 4
	k3.state.myLocs = append(k2.state.myLocs, utils.NewVector2D(2, 5))

	c.AddCandidate(k3)
	c.AddCandidate(k)
	c.AddCandidate(k2)

	expected := []int{3, 4, 5}
	for i := 0; i < len(expected); i++ {
		p := c.PopCandidate()
		if p == nil {
			t.Error("case", i, "popped no candidate")
		} else if p.steps != expected[i] {
			t.Error("case", i, "got steps", p.steps, "expected", expected[i])
		}
	}
}

func TestMinSteps(t *testing.T) {
	type Test struct {
		str   string
		steps int
	}
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
	}

	for i, c := range cases {
		k := NewKeyGrid(c.str)
		got := k.FindMinSteps()
		if got != c.steps {
			t.Error("case", i, "got", got, "expected", c.steps)
		}
	}
}
