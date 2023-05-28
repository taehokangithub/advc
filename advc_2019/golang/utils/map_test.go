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
