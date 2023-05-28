package utils

type Direction int

const (
	DIR_UP    Direction = iota
	DIR_DOWN  Direction = iota
	DIR_LEFT  Direction = iota
	DIR_RIGHT Direction = iota
)

var VECTOR_UP Vector = Vector{0, 1, 0, 0, DIMENSION_2D}
var VECTOR_DOWN Vector = Vector{0, -1, 0, 0, DIMENSION_2D}
var VECTOR_LEFT Vector = Vector{-1, 0, 0, 0, DIMENSION_2D}
var VECTOR_RIGHT Vector = Vector{1, 0, 0, 0, DIMENSION_2D}

var DIR_VECTORS []Vector = []Vector{VECTOR_UP, VECTOR_DOWN, VECTOR_LEFT, VECTOR_RIGHT}
