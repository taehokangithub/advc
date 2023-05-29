package utils

import (
	"testing"
)

func TestFreeMap(t *testing.T) {
	m := NewFreeMap[int]()

	goodVec := NewVector3D(3, 4, 5)
	badVec := NewVector2D(2, 3)
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
	g := NewGrid[bool](NewVector2D(10, 20))

	ExpectPanic(t, "GetPanicTest", func() {
		g.Get(NewVector2D(30, 10))
	})

	ExpectPanic(t, "SetPanicTest", func() {
		g.Set(NewVector2D(-10, 5), true)
	})

	goodVec := NewVector2D(5, 3)
	badVec := NewVector2D(2, 7)
	g.Set(goodVec, true)

	if g.Get(badVec) {
		t.Errorf("%v should not be true", badVec)
	}

	if !g.Get(goodVec) {
		t.Errorf("%v should be true", goodVec)
	}
}
