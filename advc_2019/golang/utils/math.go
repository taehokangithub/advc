package utils

type Numbers interface {
	int | int64 | int32 | float32 | float64
}

type DivisableNumbers interface {
	int | int64 | int32
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

func Min[T Numbers](a, b T) T {
	if a < b {
		return a
	}
	return b
}

func Gcd[T DivisableNumbers](a, b T) T {
	a = Abs(a)
	b = Abs(b)
	big := Max(a, b)
	small := Min(a, b)

	if small == 0 {
		return 1
	}

	if big%small == 0 {
		return small
	}

	return Gcd(small, big-small)
}
