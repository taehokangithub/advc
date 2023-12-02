package day24

import (
	"testing"
)

const Sample string = `....#
#..#.
#..##
..#..
#....`

func TestPart1(t *testing.T) {
	smg := NewSmallGrid(Sample)

	val := smg.BiodiversityRating()

	expected := 2129920
	if val != int64(expected) {
		t.Errorf("Part 1 expected %v, got %v", expected, val)
	}
}

func TestPart2(t *testing.T) {
	smg := NewSmallGridMultiLevel(Sample)

	val := smg.CntMultiLevelAfter(10)
	expected := 99
	if val != expected {
		t.Errorf("part 2 expected %v, got %v", expected, val)
	}

	for level := range smg.grid {
		if level > smg.maxLevel || level < smg.minLevel {
			t.Errorf("part 2 - level %d is out of range between %d and %d", level, smg.minLevel, smg.maxLevel)
		}
	}

	smg.PrintAll()
}
