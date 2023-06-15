package utils

import "fmt"

type RingQueue[T interface{}] struct {
	data    []T
	head    int
	tail    int
	len     int
	maxSize int
}

func NewRingQueue[T interface{}](maxSize int) *RingQueue[T] {
	return &RingQueue[T]{
		data:    make([]T, maxSize),
		maxSize: maxSize,
	}
}

func (q *RingQueue[T]) getNextIndex(index int) int {
	// Avoided using modular operation
	index++
	if index >= q.maxSize {
		index -= q.maxSize
	}
	return index
}

func (q *RingQueue[T]) IsFull() bool {
	return q.len == q.maxSize
}

func (q *RingQueue[T]) IsEmpty() bool {
	return q.len == 0
}

func (q *RingQueue[T]) Push(item T) {
	if q.IsFull() {
		panic(fmt.Sprint("Queue is full. len ", q.len, " head ", q.head, " tail ", q.tail))
	}
	q.data[q.head] = item
	q.head = q.getNextIndex(q.head)
	q.len++
}

func (q *RingQueue[T]) Pop() T {
	item := q.data[q.tail]
	q.tail = q.getNextIndex(q.tail)
	q.len--
	return item
}

func (q *RingQueue[T]) Len() int {
	return q.len
}

func (q *RingQueue[T]) Clear() {
	q.head = 0
	q.tail = 0
	q.len = 0
}
