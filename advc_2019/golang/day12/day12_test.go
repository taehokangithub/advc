package day12

import (
	"fmt"
	"taeho/advc19_go/utils"
	"testing"
)

func TestParser(t *testing.T) {
	str := `<x=-14, y=-4, z=-11>
	<x=-9, y=6, z=-7>
	<x=4, y=1, z=4>
	<x=2, y=-14, z=-9>`

	expected := "[<[pos:[-14:-4:-11]][vel:[0:0:0]]> <[pos:[-9:6:-7]][vel:[0:0:0]]> <[pos:[4:1:4]][vel:[0:0:0]]> <[pos:[2:-14:-9]][vel:[0:0:0]]>]"
	m := NewBodies(str)

	ret := fmt.Sprint(m.data)

	if ret != expected {
		t.Errorf("Parser - got %s expected %s", ret, expected)
	}
}

func checkMoon(t *testing.T, m *Moon, pos, vel utils.Vector) {
	if m.pos != pos {
		t.Errorf("checkMoon : got pos %v expected %v", m.pos, pos)
	}
	if m.vel != vel {
		t.Errorf("CheckMoon : got vel %v expected %v", m.vel, vel)
	}
}

func TestSim(t *testing.T) {
	str := `<x=-1, y=0, z=2>
	<x=2, y=-10, z=-7>
	<x=4, y=-8, z=8>
	<x=3, y=5, z=-1>`

	b := NewBodies(str)

	for i := 0; i < 10; i++ {
		b.SimulateOneTurn()
		fmt.Println(b)
	}

	checkMoon(t, b.data[0], utils.NewVector3D(2, 1, -3), utils.NewVector3D(-3, -2, 1))
	checkMoon(t, b.data[1], utils.NewVector3D(1, -8, 0), utils.NewVector3D(-1, 1, 3))
	checkMoon(t, b.data[2], utils.NewVector3D(3, -6, 1), utils.NewVector3D(3, 2, -3))
	checkMoon(t, b.data[3], utils.NewVector3D(2, 0, 4), utils.NewVector3D(1, -1, -1))

	energy := b.TotalEnergy()
	expected := 179
	if energy != expected {
		t.Errorf("Total energy got %d expected %d", energy, expected)
	}

	b = NewBodies(str)
	got := b.GetReturnToBase()
	expected = 2772
	if got != int64(expected) {
		t.Errorf("Return to base got %d, expected %d", got, expected)
	}
}
