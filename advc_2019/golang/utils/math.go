package utils

type Numbers interface {
	int | int64 | int32 | float32 | float64
}

func Abs[T Numbers](a T) (ret T) {
	if a >= 0 {
		return a
	}
	return -a
}

func Max[T Numbers](a, b T) T {
	if a > b {
		return a
	}
	return b
}
