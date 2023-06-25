package utils

import "container/heap"

type HeapData interface {
	GetUniqueKey() string
	GetCost() int
}

type heapNode[T HeapData] struct {
	data  T
	index int
}

type heapInternal[T HeapData] struct {
	heap    []*heapNode[T]
	dataMap map[string]*heapNode[T]
}

type Heap[T HeapData] struct {
	internalHeap *heapInternal[T]
}

func NewHeap[T HeapData]() *Heap[T] {
	internalHeap := &heapInternal[T]{
		heap:    make([]*heapNode[T], 0),
		dataMap: make(map[string]*heapNode[T]),
	}

	return &Heap[T]{
		internalHeap: internalHeap,
	}
}

func (m *heapInternal[T]) Swap(i, j int) {
	m.heap[i], m.heap[j] = m.heap[j], m.heap[i]
	m.heap[i].index = i
	m.heap[j].index = j
}

func (m *heapInternal[T]) Len() int {
	return len(m.heap)
}

func (m *heapInternal[T]) Less(i, j int) bool {
	return m.heap[i].data.GetCost() < m.heap[j].data.GetCost()
}

func (m *heapInternal[T]) Push(x interface{}) {
	node := x.(*heapNode[T])
	m.heap = append(m.heap, node)
}

func (m *heapInternal[T]) Pop() interface{} {
	node := m.heap[len(m.heap)-1]
	node.index = -1
	m.heap = m.heap[0 : len(m.heap)-1]
	return node
}

func (m *Heap[T]) Len() int {
	return m.internalHeap.Len()
}

func (m *Heap[T]) Push(data T) {
	uniqueKey := data.GetUniqueKey()
	if oldNode, ok := m.internalHeap.dataMap[uniqueKey]; ok {
		if oldNode.data.GetCost() > data.GetCost() {
			oldNode.data = data
			heap.Fix(m.internalHeap, oldNode.index)
		}
	} else {
		node := &heapNode[T]{
			data:  data,
			index: m.internalHeap.Len(),
		}
		m.internalHeap.dataMap[uniqueKey] = node
		heap.Push(m.internalHeap, node)
	}
}

func (m *Heap[T]) Pop() T {
	item := heap.Pop(m.internalHeap)
	node := item.(*heapNode[T])
	delete(m.internalHeap.dataMap, node.data.GetUniqueKey())
	return node.data
}
