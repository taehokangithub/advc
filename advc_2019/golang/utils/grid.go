package utils

import "fmt"

type Grid[T MapData] struct {
	data   [][]T
	addPtr Vector
	Size   Vector
}

func NewGrid[T MapData](size Vector) *Grid[T] {
	grid := Grid[T]{}
	grid.Size = size
	grid.addPtr = NewVector2D(0, 0)

	if size.dimension != DIMENSION_2D {
		fmt.Printf("[NewGrid] incomding vector's dimension is not 2D, %d", size.dimension)
		grid.Size.dimension = DIMENSION_2D
	}
	grid.data = make([][]T, size.Y)
	for Y := 0; Y < size.Y; Y++ {
		grid.data[Y] = make([]T, size.X)
	}
	return &grid
}

func (g *Grid[T]) Set(v Vector, val T) {
	if !g.IsValidVector(v) {
		panic(fmt.Sprintf("Grid.Set, invalid vector %v, whie size is %v", v, g.Size))
	}
	g.data[v.Y][v.X] = val
}

// sequential add to fill the grid one by one
func (g *Grid[T]) Add(val T) {
	g.Set(g.addPtr, val)

	g.addPtr.X++
	if g.addPtr.X == g.Size.X {
		g.addPtr.X = 0
		g.addPtr.Y++
	}
}

func (g *Grid[T]) IsFilled() bool {
	return g.addPtr.Y == g.Size.Y
}

func (g *Grid[T]) Get(v Vector) T {
	if !g.IsValidVector(v) {
		panic(fmt.Sprintf("Grid.Get, invalid vector %v, whie size is %v", v, g.Size))
	}
	return g.data[v.Y][v.X]
}

func (g *Grid[T]) Foreach(cb func(Vector, T)) {
	for Y := 0; Y < g.Size.Y; Y++ {
		for X := 0; X < g.Size.X; X++ {
			v := NewVector2D(X, Y)
			cb(v, g.Get(v))
		}
	}
}

func (g *Grid[T]) IsValidVector(v Vector) bool {
	return v.X < g.Size.X && v.Y < g.Size.Y && v.X >= 0 && v.Y >= 0
}

func (g *Grid[T]) DumpToString() {
	for Y := 0; Y < g.Size.Y; Y++ {
		for X := 0; X < g.Size.X; X++ {
			fmt.Printf("%v", g.Get(NewVector2D(X, Y)))
		}
		fmt.Println("")
	}
}
func (g *Grid[T]) DumpToStringOnly(val T) {
	for Y := 0; Y < g.Size.Y; Y++ {
		for X := 0; X < g.Size.X; X++ {
			v := g.Get(NewVector2D(X, Y))
			if v == val {
				fmt.Printf("#")
			} else {
				fmt.Printf(" ")
			}
		}
		fmt.Println("")
	}
}
