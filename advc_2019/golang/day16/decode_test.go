package day16

import "testing"

func TestDecodePhase(t *testing.T) {
	tests := [][2]string{
		{"12345678", "48226158"},
		{"48226158", "34040438"},
	}

	for _, tuple := range tests {
		got := decodeOnePhase(tuple[0])
		if got != tuple[1] {
			t.Errorf("TestDecodePhase got %v expected %v", got, tuple[1])
		}
	}
}

func TestDecodeFullPhase(t *testing.T) {
	tests := [][2]string{
		{"80871224585914546619083218645595", "24176176"},
		{"19617804207202209144916044189917", "73745418"},
		{"69317163492948606335995924319873", "52432133"},
	}

	for _, tuple := range tests {
		got := decodeFullPhaseGetResultAt(tuple[0], 0)
		if got != tuple[1] {
			t.Errorf("TestDecodeFullPhase got %v expected %v", got, tuple[1])
		}
	}
}

func TestRepeatTest(t *testing.T) {
	decodeRepeatedFullPhaseGetResultAt("03036732577212944063491565474664", 1, 0)
}
