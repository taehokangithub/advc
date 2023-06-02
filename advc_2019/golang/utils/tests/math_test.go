package tests

import (
	"taeho/advc19_go/utils"
	"testing"
)

func TestMinMax(t *testing.T) {
	got := utils.Min(3, 12)
	if got != 3 {
		t.Errorf("min test got %d not %d", got, 3)
	}

	got = utils.Min(15, 7)
	if got != 7 {
		t.Errorf("min test got %d not %d", got, 7)
	}

	got = utils.Max(5, 18)
	if got != 18 {
		t.Errorf("max test got %d not %d", got, 18)
	}

	got = utils.Max(21, 13)
	if got != 21 {
		t.Errorf("max test got %d not %d", got, 21)
	}

	got = utils.Abs(-3)
	if got != 3 {
		t.Errorf("abs test got %d not %d", got, 3)
	}
}

func TestGcd(t *testing.T) {
	type Test struct {
		a, b, ret int
	}
	tests := []Test{
		{3, 12, 3},
		{1, 6, 1},
		{6, 22, 2},
		{6, 15, 3},
		{6, 21, 3},
		{20, 48, 4},
		{15, 45, 15},
		{15, 25, 5},
	}

	for i, c := range tests {
		got := utils.Gcd(c.a, c.b)
		if c.ret != got {
			t.Errorf("GCD case %d, got %d expected %d", i, got, c.ret)
		}
	}
}
