package utils

import "fmt"

type MemoryPool[T interface{}] struct {
	data  []T
	index int
}

func NewMemoryPool[T interface{}](maxSize int) *MemoryPool[T] {
	return &MemoryPool[T]{
		data: make([]T, maxSize),
	}
}

func (m *MemoryPool[T]) New() *T {
	if m.index == len(m.data) {
		panic(fmt.Sprint("Memory pool if full ", m.index))
	}
	item := &m.data[m.index]
	m.index++
	return item
}

func (m *MemoryPool[T]) Reset() {
	m.index = 0
}
