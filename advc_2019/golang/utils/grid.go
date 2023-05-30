package utils

import "fmt"

type Grid[T MapData] struct {
	data [][]T
	size Vector
}

func NewGrid[T MapData](size Vector) *Grid[T] {
	grid := Grid[T]{}
	grid.size = size

	if size.dimension != DIMENSION_2D {
		fmt.Printf("[NewGrid] incomding vector's dimension is not 2D, %d", size.dimension)
		grid.size.dimension = DIMENSION_2D
	}
	grid.data = make([][]T, size.y)
	for y := 0; y < size.y; y++ {
		grid.data[y] = make([]T, size.x)
	}
	return &grid
}

func (g *Grid[T]) Set(v Vector, val T) {
	if !g.IsValidVector(v) {
		panic(fmt.Sprintf("Grid.Set, invalid vector %v, whie size is %v", v, g.size))
	}
	g.data[v.y][v.x] = val
}

func (g *Grid[T]) Get(v Vector) T {
	if !g.IsValidVector(v) {
		panic(fmt.Sprintf("Grid.Get, invalid vector %v, whie size is %v", v, g.size))
	}
	return g.data[v.y][v.x]
}

func (g *Grid[T]) Foreach(cb func(Vector, T)) {
	for y := 0; y < g.size.y; y++ {
		for x := 0; x < g.size.x; x++ {
			v := NewVector2D(x, y)
			cb(v, g.Get(v))
		}
	}
}

func (g *Grid[T]) IsValidVector(v Vector) bool {
	return v.x < g.size.x && v.y < g.size.y && v.x >= 0 && v.y >= 0
}
