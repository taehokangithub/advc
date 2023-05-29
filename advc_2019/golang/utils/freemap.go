package utils

type MapData interface {
	int | bool
}

type FreeMap[T MapData] struct {
	data map[Vector]T
}

func NewFreeMap[T MapData]() FreeMap[T] {
	m := FreeMap[T]{}
	m.data = make(map[Vector]T)
	return m
}

func (m *FreeMap[T]) Set(v Vector, val T) {
	m.data[v] = val
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
