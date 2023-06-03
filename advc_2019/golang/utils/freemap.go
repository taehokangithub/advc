package utils

import "math"

type MapData interface {
	int64 | int32 | int | byte | bool
}

type FreeMap[T MapData] struct {
	data map[Vector]T
	Min  Vector
	Max  Vector
}

func NewFreeMap[T MapData]() *FreeMap[T] {
	m := FreeMap[T]{}
	m.data = make(map[Vector]T)
	m.Min.SetAll(math.MaxInt)
	m.Max.SetAll(math.MinInt)
	return &m
}

func (m *FreeMap[T]) Set(v Vector, val T) {
	m.data[v] = val
	m.Min.SetMin(v)
	m.Max.SetMax(v)
}

func (m *FreeMap[T]) Get(v Vector) (val T, ok bool) {
	val, ok = m.data[v]
	return
}

func (m *FreeMap[T]) Foreach(cb func(v Vector, val T)) {
	for v, val := range m.data {
		cb(v, val)
	}
}

func (m *FreeMap[T]) Count() int {
	return len(m.data)
}
