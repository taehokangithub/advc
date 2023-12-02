package utils

type Stack[T interface{}] struct {
	data []T
}

func NewStack[T interface{}]() *Stack[T] {
	s := Stack[T]{}
	s.data = make([]T, 0)
	return &s
}

func (s *Stack[T]) Push(val T) {
	s.data = append(s.data, val)
}

func (s *Stack[T]) Pop() T {
	curLen := len(s.data)
	t := s.data[curLen-1]
	s.data = s.data[0 : curLen-1]
	return t
}
