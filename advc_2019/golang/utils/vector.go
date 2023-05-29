package utils

import "fmt"

type Dimension byte

const (
	DIMENSION_2D Dimension = 2
	DIMENSION_3D Dimension = 3
	DIMENSION_4D Dimension = 4
)

type Vector struct {
	x         int
	y         int
	z         int
	w         int
	dimension Dimension
}

func NewVector2D(x, y int) Vector {
	return Vector{x, y, 0, 0, DIMENSION_2D}
}

func NewVector3D(x, y, z int) Vector {
	return Vector{x, y, z, 0, DIMENSION_3D}
}

func NewVector4D(x, y, z, w int) Vector {
	return Vector{x, y, z, w, DIMENSION_4D}
}

func (v Vector) String() string {
	switch v.dimension {
	case 2:
		return fmt.Sprintf("[%d:%d]", v.x, v.y)
	case 3:
		return fmt.Sprintf("[%d:%d:%d]", v.x, v.y, v.z)
	default:
		return fmt.Sprintf("[%d:%d:%d:%d]", v.x, v.y, v.z, v.z)
	}
}

func (v *Vector) Add(other Vector) {
	v.x += other.x
	v.y += other.y
	v.z += other.z
	v.w += other.w
}

func (v Vector) GetAdded(other Vector) Vector {
	v.Add(other)
	return v
}

func (v *Vector) Multiply(i int) {
	v.x *= i
	v.y *= i
	v.z *= i
	v.w *= i
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
	return Abs(v.x) + Abs(v.y) + Abs(v.z) + Abs(v.w)
}
