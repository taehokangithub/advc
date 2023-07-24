package utils

import "fmt"

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

func Lcm[T DivisableNumbers](a, b T) T {
	return a * b / Gcd(a, b)
}

func ModularInversion[T DivisableNumbers](a T, mod T) T {
	if a < 0 || mod <= 0 {
		panic(fmt.Sprintf("Invalid input: a(%v) must be non-negative, and mod(%v) must be positive\n", a, mod))
	}

	// Extended Euclidean Algorithm
	x, y, u, v := T(0), T(1), T(1), T(0)
	m := mod
	for a != 0 {
		q := m / a
		r := m % a
		m, a = a, r
		x, u = u, x-q*u
		y, v = v, y-q*v
	}

	// Ensure the result is positive
	result := (x + mod) % mod
	return result
}
