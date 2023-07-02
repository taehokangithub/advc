package day22

import (
	"fmt"
	"os"
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
}

func TestAnalyse(t *testing.T) {
	con, err := os.ReadFile("../../data/input22.txt")
	if err != nil {
		panic(err)
	}
	str := string(con)
	s := NewShuffler(10007, str)
	s.Analyse()
	t.Error(1)
}
