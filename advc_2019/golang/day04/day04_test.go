package day04

import "testing"

func TestDay04(t *testing.T) {
	type TestCase struct {
		isPart1  bool
		val      int
		expected bool
	}

	testCases := []TestCase{
		{true, 111111, true},
		{true, 223450, false},
		{true, 123789, false},
		{false, 112233, true},
		{false, 123444, false},
		{false, 111122, true},
	}

	for i, c := range testCases {
		var got bool
		if c.isPart1 {
			got = doesMeetCriteria1(c.val)
		} else {
			got = doesMeetCriteria2(c.val)
		}
		if got != c.expected {
			t.Errorf("Case[%d] got %v from %v", i, got, c)
		}
	}
}
