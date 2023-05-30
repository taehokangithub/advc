package tests

import (
	"taeho/advc19_go/utils"
	"testing"
)

func TestFreeMap(t *testing.T) {
	m := utils.NewFreeMap[int]()

	goodVec := utils.NewVector3D(3, 4, 5)
	badVec := utils.NewVector2D(2, 3)
	sampleVal := 25

	m.Set(goodVec, sampleVal)

	if v, ok := m.Get(badVec); ok {
		t.Errorf("Vector %v should not have value %v", badVec, v)
	}

	v, ok := m.Get(goodVec)
	if !ok {
		t.Errorf("Vector %v does not have a value!", goodVec)
	}
	if v != sampleVal {
		t.Errorf("Vector %v has a wrong value %v, not %v", goodVec, v, sampleVal)
	}
}

func TestGrid(t *testing.T) {
	g := utils.NewGrid[bool](utils.NewVector2D(10, 20))

	expectPanic(t, "GetPanicTest", func() {
		g.Get(utils.NewVector2D(30, 10))
	})

	expectPanic(t, "SetPanicTest", func() {
		g.Set(utils.NewVector2D(-10, 5), true)
	})

	goodVec := utils.NewVector2D(5, 3)
	badVec := utils.NewVector2D(2, 7)
	g.Set(goodVec, true)

	if g.Get(badVec) {
		t.Errorf("%v should not be true", badVec)
	}

	if !g.Get(goodVec) {
		t.Errorf("%v should be true", goodVec)
	}
}
