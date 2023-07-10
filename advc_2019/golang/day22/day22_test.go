package day22

import (
	"fmt"
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
	type Case struct {
		inst string
		ans  string
	}
	cases := []Case{
		{T1, "[0 3 6 9 2 5 8 1 4 7]"},
		{T2, "[3 0 7 4 1 8 5 2 9 6]"},
		{T3, "[6 3 0 7 4 1 8 5 2 9]"},
		{T4, "[9 2 5 8 1 4 7 0 3 6]"},
	}

	for i, c := range cases {
		ans := Shuffle(10, c.inst)
		ansStr := fmt.Sprint(ans)
		if ansStr != c.ans {
			t.Error("Case", i, "got", ansStr, "expected", c.ans)
		}
	}

	for i, c := range cases {
		s := NewCircularShuffler(10, c.inst)
		ans := s.Shuffle()
		ansStr := fmt.Sprint(ans)
		if ansStr != c.ans {
			t.Error("Circular Case", i, "got", ansStr, "expected", c.ans)
		}
	}
}

func TestCircularBuff(t *testing.T) {
	b := NewCircularBuff(-10, 5)
	b.pos += 1 // -11 + 4

	val := int64(150)
	b.SetAt(7, val)
	ret := b.GetAt(-9)
	if ret != val {
		t.Error("got", ret, "expected", val)
	}
}

// TODO : WHY THIS SIMPLE 2 LINES FAILS? So do we need full length? set max length?
func TestRealData(t *testing.T) {
	str := `cut -135
deal with increment 38`

	b := NewCircularShuffler(10007, str)

	b.Shuffle()
}
