package utils

import (
	"testing"
)

func TestVector(t *testing.T) {
	v1 := NewVector4D(34, 27, 6, 3)
	v2 := NewVector3D(2, 3, 4)
	v3 := NewVector4D(36, 30, 10, 3)

	if v3 != v1.GetAdded(v2) {
		t.Errorf("Vector addition failed, %v + %v != %v (%v)", v1, v2, v3, v1.GetAdded(v2))
	}

	v1.Add(v2)

	if v3 != v1 {
		t.Errorf("Vector in-place addition failed, %v != %v", v1, v3)
	}

	vd := VECTOR_DOWN
	vu := VECTOR_UP
	vr := VECTOR_RIGHT
	vl := VECTOR_LEFT

	v2.Move(DIR_UP, 2)
	v2.Move(DIR_UP, 3)
	v2.Move(DIR_LEFT, 3)
	v2.Move(DIR_LEFT, 1)
	v2Moved := NewVector3D(-2, 8, 4)
	if v2 != v2Moved {
		t.Errorf("Vector movement 1 failed, %v != %v", v2, v2Moved)
	}

	v2.Move(DIR_RIGHT, 5)
	v2.Move(DIR_RIGHT, 2)
	v2.Move(DIR_DOWN, 3)
	v2.Move(DIR_DOWN, 11)
	v2Moved = NewVector3D(5, -6, 4)

	if v2 != v2Moved {
		t.Errorf("Vector movement 2 failed, %v != %v", v2, v2Moved)
	}

	if vd != VECTOR_DOWN ||
		vu != VECTOR_UP ||
		vl != VECTOR_LEFT ||
		vr != VECTOR_RIGHT {
		t.Errorf("Base vector compromised, %v %v %v %v", VECTOR_UP, VECTOR_DOWN, VECTOR_LEFT, VECTOR_RIGHT)
	}
}
