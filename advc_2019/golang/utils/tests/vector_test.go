package tests

import (
	"taeho/advc19_go/utils"
	"testing"
)

func TestVector(t *testing.T) {
	v1 := utils.NewVector4D(34, 27, 6, 3)
	v2 := utils.NewVector3D(2, 3, 4)
	v3 := utils.NewVector4D(36, 30, 10, 3)

	if v3 != v1.GetAdded(v2) {
		t.Errorf("Vector addition failed, %v + %v != %v (%v)", v1, v2, v3, v1.GetAdded(v2))
	}

	v1.Add(v2)

	if v3 != v1 {
		t.Errorf("Vector in-place addition failed, %v != %v", v1, v3)
	}

	vd := utils.VECTOR_DOWN
	vu := utils.VECTOR_UP
	vr := utils.VECTOR_RIGHT
	vl := utils.VECTOR_LEFT

	v2.Move(utils.DIR_UP, 2)
	v2.Move(utils.DIR_UP, 3)
	v2.Move(utils.DIR_LEFT, 3)
	v2.Move(utils.DIR_LEFT, 1)
	v2Moved := utils.NewVector3D(-2, 8, 4)
	if v2 != v2Moved {
		t.Errorf("Vector movement 1 failed, %v != %v", v2, v2Moved)
	}

	v2.Move(utils.DIR_RIGHT, 5)
	v2.Move(utils.DIR_RIGHT, 2)
	v2.Move(utils.DIR_DOWN, 3)
	v2.Move(utils.DIR_DOWN, 11)
	v2Moved = utils.NewVector3D(5, -6, 4)

	if v2 != v2Moved {
		t.Errorf("Vector movement 2 failed, %v != %v", v2, v2Moved)
	}

	if vd != utils.VECTOR_DOWN ||
		vu != utils.VECTOR_UP ||
		vl != utils.VECTOR_LEFT ||
		vr != utils.VECTOR_RIGHT {
		t.Errorf("Base vector compromised, %v %v %v %v", utils.VECTOR_UP, utils.VECTOR_DOWN, utils.VECTOR_LEFT, utils.VECTOR_RIGHT)
	}
}
