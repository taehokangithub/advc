package day18

import (
	"container/heap"
)

const QueueCapacity = 255

type searchMoveHeap struct {
	arr []*move
}

func newSearchMoveHeap() *searchMoveHeap {
	return &searchMoveHeap{
		arr: make([]*move, 0, QueueCapacity),
	}
}

func (s *searchMoveHeap) Clear() {
	s.arr = s.arr[0:0]
}

func (s *searchMoveHeap) Len() int {
	return len(s.arr)
}

func (s *searchMoveHeap) Less(i, j int) bool {
	return s.arr[i].steps < s.arr[j].steps
}

func (s *searchMoveHeap) Swap(i, j int) {
	s.arr[i], s.arr[j] = s.arr[j], s.arr[i]
}

func (s *searchMoveHeap) Push(x interface{}) {
	m := x.(*move)
	s.arr = append(s.arr, m)
}

func (s *searchMoveHeap) Pop() interface{} {
	index := len(s.arr) - 1
	m := s.arr[index]

	s.arr = s.arr[:index]
	return m
}

func (s *searchMoveHeap) PushMove(m *move) {
	heap.Push(s, m)
}

func (s *searchMoveHeap) PopMove() *move {
	return heap.Pop(s).(*move)
}

func (s *searchMoveHeap) IsEmpty() bool {
	return s.Len() == 0
}

// -------------------------------------------------------
// searchQueue : for comparison, this could be faster?
// -------------------------------------------------------
type searchQueue struct {
	q []*move
}

func newSearchQueue() *searchQueue {
	return &searchQueue{
		q: make([]*move, 0, QueueCapacity),
	}
}

func (sq *searchQueue) IsEmpty() bool {
	return len(sq.q) == 0
}

func (sq *searchQueue) PopMove() *move {
	ret := sq.q[0]
	sq.q = sq.q[1:]
	return ret
}

func (sq *searchQueue) PushMove(m *move) {
	insertAt := len(sq.q)
	for i := 0; i < insertAt; i++ {
		if m.steps < sq.q[i].steps {
			insertAt = i
			break
		}
	}

	if insertAt >= len(sq.q) {
		sq.q = append(sq.q, m)
	} else {
		sq.q = append(sq.q[:insertAt+1], sq.q[insertAt:]...)
		sq.q[insertAt] = m
	}
}
