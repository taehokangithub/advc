package day22

import (
	"testing"
)

const T1 string = `deal with increment 7
deal into new stack
deal into new stack`

const T2 string = `cut 6
deal with increment 7
deal into new stack`

const T3 string = `deal with increment 7
deal with increment 9
cut -2`

const T4 string = `deal into new stack
cut -2
deal with increment 7
cut 8
cut -4
deal with increment 7
cut 3
deal with increment 9
deal with increment 3
cut -1`

func TestShuffler(t *testing.T) {
	/*
		{T1, "[0 3 6 9 2 5 8 1 4 7]"},
		{T2, "[3 0 7 4 1 8 5 2 9 6]"},
		{T3, "[6 3 0 7 4 1 8 5 2 9]"},
		{T4, "[9 2 5 8 1 4 7 0 3 6]"},
	*/
	type Case struct {
		inst     string
		card     int64
		position int64
	}
	cases := []Case{
		{T1, 1, 7}, {T1, 2, 4}, {T1, 3, 1}, {T1, 4, 8}, {T1, 5, 5},
		{T1, 7, 9}, {T1, 8, 6}, {T1, 9, 3}, {T1, 0, 0}, {T1, 6, 2},
		{T2, 0, 1}, {T2, 1, 4}, {T2, 2, 7}, {T2, 3, 0}, {T2, 4, 3},
		{T2, 9, 8}, {T2, 8, 5}, {T2, 7, 2}, {T2, 6, 9}, {T2, 5, 6},
		{T3, 0, 2}, {T3, 1, 5}, {T3, 2, 8}, {T3, 3, 1}, {T3, 4, 4},
		{T3, 5, 7}, {T3, 6, 0}, {T3, 7, 3}, {T3, 8, 6}, {T3, 9, 9},
		{T4, 0, 7}, {T4, 1, 4}, {T4, 2, 1}, {T4, 3, 8}, {T4, 4, 5},
		{T4, 6, 9}, {T4, 7, 6}, {T4, 8, 3}, {T4, 9, 0}, {T4, 5, 2},
	}

	for i, c := range cases {
		s := NewShuffler(10, c.inst)
		ans := s.GetFinalPositionOf(c.card)
		if ans != c.position {
			t.Error("case", i, "fwd got", ans, "expected", c.position)
		}
	}

	for i, c := range cases {
		s := NewShuffler(10, c.inst)
		revAns := s.GetCardAtAfterIterations(c.position, 1)
		if revAns != c.card {
			t.Error("case", i, "rev got", revAns, "expected", c.card)
		}
	}

}

func TestLinear(t *testing.T) {
	type test struct {
		a, b, mod, x int64
	}

	cases := []test{
		{87, 64, 331, 11},
		{117, 23, 5341, 8},
		{23, 10, 51, 4},
	}

	for _, c := range cases {
		op := NewLinearOperation(c.a, c.b, c.mod)
		expected := (c.a*c.x + c.b) % c.mod
		ans := op.Operate(c.x)
		if ans != expected {
			t.Error("fwd: got", ans, "expected", expected)
		}
		ansRev := op.ReverseOperate(ans)
		if ansRev != c.x {
			t.Error("rev: got", ansRev, "expected", c.x)
		}
	}
}
