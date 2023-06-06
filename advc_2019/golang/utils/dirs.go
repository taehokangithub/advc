package utils

type Direction int

const (
	DIR_UP    Direction = 0
	DIR_DOWN  Direction = 1
	DIR_LEFT  Direction = 2
	DIR_RIGHT Direction = 3
)

var VECTOR_UP Vector = Vector{0, 1, 0, 0, DIMENSION_2D}
var VECTOR_DOWN Vector = Vector{0, -1, 0, 0, DIMENSION_2D}
var VECTOR_LEFT Vector = Vector{-1, 0, 0, 0, DIMENSION_2D}
var VECTOR_RIGHT Vector = Vector{1, 0, 0, 0, DIMENSION_2D}

var DIR_DIRECTIONS []Direction = []Direction{DIR_UP, DIR_DOWN, DIR_LEFT, DIR_RIGHT}
var DIR_VECTORS []Vector = []Vector{VECTOR_UP, VECTOR_DOWN, VECTOR_LEFT, VECTOR_RIGHT}

var DIR_NAMES []string = []string{"Up", "Down", "Left", "Right"}
