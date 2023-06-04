package utils

import (
	"fmt"
	"math"
)

type Dimension byte

const (
	DIMENSION_2D Dimension = 2
	DIMENSION_3D Dimension = 3
	DIMENSION_4D Dimension = 4
)

type CoordType byte

const (
	COORD_X CoordType = 0
	COORD_Y CoordType = 1
	COORD_Z CoordType = 2
	COORD_W CoordType = 3
)

type Vector struct {
	X         int
	Y         int
	Z         int
	W         int
	dimension Dimension
}

func NewVector2D(X, Y int) Vector {
	return Vector{X, Y, 0, 0, DIMENSION_2D}
}

func NewVector3D(X, Y, Z int) Vector {
	return Vector{X, Y, Z, 0, DIMENSION_3D}
}

func NewVector4D(X, Y, Z, W int) Vector {
	return Vector{X, Y, Z, W, DIMENSION_4D}
}

func (v Vector) String() string {
	switch v.dimension {
	case 2:
		return fmt.Sprintf("[%d:%d]", v.X, v.Y)
	case 3:
		return fmt.Sprintf("[%d:%d:%d]", v.X, v.Y, v.Z)
	default:
		return fmt.Sprintf("[%d:%d:%d:%d]", v.X, v.Y, v.Z, v.Z)
	}
}

func (v *Vector) Add(other Vector) {
	v.X += other.X
	v.Y += other.Y
	v.Z += other.Z
	v.W += other.W
}

func (v Vector) GetAdded(other Vector) Vector {
	v.Add(other)
	return v
}

func (v Vector) GetValueAt(coord CoordType) int {
	switch coord {
	case COORD_X:
		return v.X
	case COORD_Y:
		return v.Y
	case COORD_Z:
		return v.Z
	case COORD_W:
		return v.W
	}
	panic(fmt.Sprintf("Unknown coord type %v", coord))
}

func (v Vector) GetSubtracted(other Vector) Vector {
	return v.GetAdded(other.GetMultiplied(-1))
}

func (v *Vector) Multiply(i int) {
	v.X *= i
	v.Y *= i
	v.Z *= i
	v.W *= i
}

func (v Vector) GetMultiplied(i int) Vector {
	v.Multiply(i)
	return v
}

func (v *Vector) Move(dir Direction, dist int) {
	switch dir {
	case DIR_UP:
		v.Add(VECTOR_UP.GetMultiplied(dist))
	case DIR_DOWN:
		v.Add(VECTOR_DOWN.GetMultiplied(dist))
	case DIR_LEFT:
		v.Add(VECTOR_LEFT.GetMultiplied(dist))
	case DIR_RIGHT:
		v.Add(VECTOR_RIGHT.GetMultiplied(dist))
	default:
		panic(fmt.Sprintf("Unknown dir %v", dir))
	}
}

func (v Vector) ManhattanDistance() int {
	return Abs(v.X) + Abs(v.Y) + Abs(v.Z) + Abs(v.W)
}

func (v Vector) GetBaseVector() Vector {
	getBaseValue := func(a int) int {
		if a == 0 {
			return 0
		}
		return a / Abs(a)
	}
	return Vector{
		dimension: v.dimension,
		X:         getBaseValue(v.X),
		Y:         getBaseValue(v.Y),
		Z:         getBaseValue(v.Z),
		W:         getBaseValue(v.W),
	}
}

func (v Vector) Atan2() float64 {
	return math.Atan2(float64(v.Y), float64(v.X))
}

func (v Vector) Atan2Reverse() float64 {
	return math.Atan2(float64(v.X), float64(v.Y))
}

func (v *Vector) Rotate(dir Direction) {
	if v.dimension != DIMENSION_2D {
		fmt.Println("Warning! Rotate is supported only for 2D vectors", dir)
		// but go on..
	}

	switch dir {
	case DIR_LEFT:
		v.X, v.Y = -v.Y, v.X
	case DIR_RIGHT:
		v.X, v.Y = v.Y, -v.X
	case DIR_DOWN:
		v.Y = -v.Y
	}
}

func (v *Vector) SetMin(other Vector) {
	v.X = Min(v.X, other.X)
	v.Y = Min(v.Y, other.Y)
	v.Z = Min(v.Z, other.Z)
	v.W = Min(v.W, other.W)
}

func (v *Vector) SetMax(other Vector) {
	v.X = Max(v.X, other.X)
	v.Y = Max(v.Y, other.Y)
	v.Z = Max(v.Z, other.Z)
	v.W = Max(v.W, other.W)
}

func (v *Vector) SetAll(val int) {
	v.X = val
	v.Y = val
	v.Z = val
	v.W = val
}
