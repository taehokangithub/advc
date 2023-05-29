package day04

import "fmt"

func doesMeetCriteria1(a int) bool {
	str := fmt.Sprintf("%d", a)

	var prev int
	foundDouble := false

	for i, c := range str {
		current := int(c - '0')

		if i > 0 {
			if current < prev {
				return false
			}
			if current == prev {
				foundDouble = true
			}
		}
		prev = current
	}
	return foundDouble
}

func doesMeetCriteria2(a int) bool {
	str := fmt.Sprintf("%d", a)

	var prev int
	finding := -1
	cnt := 0
	foundDouble := false

	for i, c := range str {
		current := int(c - '0')

		if i > 0 {
			if current < prev {
				return false
			}
			if current == prev {
				if finding == -1 {
					finding = current
					cnt = 2
				} else {
					cnt++
				}
			} else if finding >= 0 {
				if cnt == 2 {
					foundDouble = true
				}
				finding = -1
			}
		}
		prev = current
	}

	if finding >= 0 && cnt == 2 {
		foundDouble = true
	}

	return foundDouble
}

func solvePart1(start, end int) int {
	sum := 0
	for i := start; i <= end; i++ {
		if doesMeetCriteria1(i) {
			sum++
		}
	}
	return sum
}

func solvePart2(start, end int) int {
	sum := 0
	for i := start; i <= end; i++ {
		if doesMeetCriteria2(i) {
			sum++
		}
	}
	return sum
}

func Solve() {
	start := 372304
	end := 847060

	ret1 := solvePart1(start, end)
	ret2 := solvePart2(start, end)

	fmt.Println("DAY04 ans1", ret1)
	fmt.Println("DAY04 ans2", ret2)
}
