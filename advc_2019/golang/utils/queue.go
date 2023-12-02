package utils

type Queue[T interface{}] struct {
	data []T
}

func NewQueue[T interface{}]() *Queue[T] {
	q := Queue[T]{}
	q.data = make([]T, 0)
	return &q
}

func (q *Queue[T]) Push(val T) {
	q.data = append(q.data, val)
}

func (q *Queue[T]) Pop() T {
	t := q.data[0]
	q.data = q.data[1:]
	return t
}

func (q *Queue[T]) Len() int {
	return len(q.data)
}
