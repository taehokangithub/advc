package utils

type MapData interface {
	int | bool
}

type FreeMap[T MapData] struct {
	data map[string]T
}

func NewFreeMap[T MapData]() FreeMap[T] {
	m := FreeMap[T]{}
	m.data = make(map[string]T)
	return m
}

func (m *FreeMap[T]) Set(v Vector, val T) {
	m.data[v.String()] = val
}

func (m *FreeMap[T]) Get(v Vector) (val T, ok bool) {
	val, ok = m.data[v.String()]
	return
}
