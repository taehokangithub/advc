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

func TestLcm(t *testing.T) {
	type Test struct {
		a, b, ret int
	}
	tests := []Test{
		{5, 10, 10},
		{6, 9, 18},
		{2, 5, 10},
		{22, 33, 66},
	}

	for i, c := range tests {
		got := utils.Lcm(c.a, c.b)
		if got != c.ret {
			t.Errorf("LCM case %d, got %d expected %d", i, got, c.ret)
		}
	}
}

func TestModularInversion(t *testing.T) {
	type Test struct {
		a   int64
		mod int64
		ans int64
	}
	tests := []Test{
		{7, 26, 15},
		{15, 26, 7},
		{9, 11, 5},
		{5, 11, 9},
		{8, 11, 7},
		{7, 11, 8},
		{2019, 119315717514047, 57323549276189},
		{57323549276189, 119315717514047, 2019},
		{1086238, 119315717514047, 26066090224459},
		{26066090224459, 119315717514047, 1086238},
	}

	for _, c := range tests {
		ans := utils.ModularInversion(c.a, c.mod)
		if ans != c.ans {
			t.Errorf("%v got %v expected %v", c, ans, c.ans)
		}
	}
}
