package utils

import "fmt"

type Grid[T MapData] struct {
	data   [][]T
	size   Vector
	addPtr Vector
}

func NewGrid[T MapData](size Vector) *Grid[T] {
	grid := Grid[T]{}
	grid.size = size
	grid.addPtr = NewVector2D(0, 0)

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

// sequential add to fill the grid one by one
func (g *Grid[T]) Add(val T) {
	g.Set(g.addPtr, val)

	g.addPtr.x++
	if g.addPtr.x == g.size.x {
		g.addPtr.x = 0
		g.addPtr.y++
	}
}

func (g *Grid[T]) IsFilled() bool {
	return g.addPtr.y == g.size.y
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

func (g *Grid[T]) DumpToString() {
	for y := 0; y < g.size.y; y++ {
		for x := 0; x < g.size.x; x++ {
			fmt.Printf("%v", g.Get(NewVector2D(x, y)))
		}
		fmt.Println("")
	}
}
func (g *Grid[T]) DumpToStringOnly(val T) {
	for y := 0; y < g.size.y; y++ {
		for x := 0; x < g.size.x; x++ {
			v := g.Get(NewVector2D(x, y))
			if v == val {
				fmt.Printf("%v", v)
			} else {
				fmt.Printf(" ")
			}
		}
		fmt.Println("")
	}
}
