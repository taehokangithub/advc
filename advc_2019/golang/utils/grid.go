package utils

import (
	"fmt"
	"strings"
)

type MapData interface {
	bool | int | byte | int32 | int64
}

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

func (g *Grid[T]) Copy() *Grid[T] {
	grid := Grid[T]{
		Size:   g.Size,
		addPtr: g.addPtr,
		data:   make([][]T, len(g.data)),
	}
	for i := 0; i < len(g.data); i++ {
		grid.data[i] = make([]T, len(g.data[i]))
		copy(grid.data[i], g.data[i])

	}
	return &grid
}

func (g *Grid[T]) Set(v Vector, val T) {
	if !g.IsValidVector(v) {
		panic(fmt.Sprintf("Grid.Set, invalid vector %v, whie size is %v", v, g.Size))
	}
	g.SetFast(&v, val)
}

func (g *Grid[T]) SetFast(v *Vector, val T) {
	g.data[v.Y][v.X] = val
}

// sequential add to fill the grid one by one
func (g *Grid[T]) Add(val T) Vector {
	addedLoc := g.addPtr
	g.Set(g.addPtr, val)

	g.addPtr.X++
	if g.addPtr.X == g.Size.X {
		g.addPtr.X = 0
		g.addPtr.Y++
	}
	return addedLoc
}

func (g *Grid[T]) IsFilled() bool {
	return g.addPtr.Y == g.Size.Y
}

func (g *Grid[T]) Get(v Vector) T {
	if !g.IsValidVector(v) {
		panic(fmt.Sprintf("Grid.Get, invalid vector %v, whie size is %v", v, g.Size))
	}
	return g.GetFast(&v)
}

func (g *Grid[T]) GetFast(v *Vector) T {
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

func (g *Grid[T]) ForeachWithNewline(cb func(Vector, T, bool)) {
	for Y := 0; Y < g.Size.Y; Y++ {
		for X := 0; X < g.Size.X; X++ {
			v := NewVector2D(X, Y)
			cb(v, g.Get(v), (X == g.Size.X-1))
		}
	}
}

func (g *Grid[T]) IsValidVector(v Vector) bool {
	return v.X < g.Size.X && v.Y < g.Size.Y && v.X >= 0 && v.Y >= 0
}

func (g *Grid[T]) String() string {
	builder := strings.Builder{}

	for Y := 0; Y < g.Size.Y; Y++ {
		for X := 0; X < g.Size.X; X++ {
			builder.WriteString(fmt.Sprintf("[%v]", g.Get(NewVector2D(X, Y))))
		}
		builder.WriteRune('\n')
	}
	return builder.String()
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
