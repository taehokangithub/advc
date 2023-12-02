package utils

type strStream struct {
	str string
	ptr int
}

func NewStrStream(s string) *strStream {
	ret := strStream{str: s}
	return &ret
}

func (s *strStream) GetNext() (byte, bool) {
	if s.ptr == len(s.str) {
		return 0, false
	}
	s.ptr++
	return s.str[s.ptr-1], true
}
