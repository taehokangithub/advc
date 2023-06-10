package day17

import (
	"taeho/advc19_go/utils"
	"testing"
)

func TestEliminatePath(t *testing.T) {
	p1 := NewPathSet()
	p1.AddPath(utils.DIR_RIGHT, 3)
	p1.AddPath(utils.DIR_RIGHT, 4)
	p1.AddPath(utils.DIR_RIGHT, 5)
	p1.AddPath(utils.DIR_LEFT, 1)
	p1.AddPath(utils.DIR_LEFT, 2)
	p1.AddPath(utils.DIR_RIGHT, 3)
	p1.AddPath(utils.DIR_RIGHT, 4)
	p1.AddPath(utils.DIR_RIGHT, 5)
	p1.AddPath(utils.DIR_LEFT, 3)
	p1.AddPath(utils.DIR_RIGHT, 3)
	p1.AddPath(utils.DIR_RIGHT, 4)
	p1.AddPath(utils.DIR_LEFT, 5)
	p1.AddPath(utils.DIR_RIGHT, 3)
	p1.AddPath(utils.DIR_RIGHT, 4)
	p1.AddPath(utils.DIR_RIGHT, 5)

	p2 := NewPathSet()
	p2.AddPath(utils.DIR_RIGHT, 3)
	p2.AddPath(utils.DIR_RIGHT, 4)
	p2.AddPath(utils.DIR_RIGHT, 5)

	p1.Eliminate(p2)

	if p1.GetActualLength() != 6 {
		t.Error("!!expected 6, but got", p1.GetActualLength(), "from ", p1)
	}

}

func TestDividingPaths(t *testing.T) {
	str := "R,8,R,8,R,4,R,4,R,8,L,6,L,2,R,4,R,4,R,8,R,8,R,8,L,6,L,2"
	p := ParsePathSet(str)

	if len(p.paths) != 14 {
		t.Error("len", len(p.paths), "expected 14")
		return
	}

	pathSets := p.dividePathSet()

	if len(pathSets) != 3 {
		t.Error("len(sets)", len(pathSets), "expected 3")
	}
}
