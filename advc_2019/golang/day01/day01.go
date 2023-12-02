package day01

import (
	"os"
	"strconv"
	"strings"
	"taeho/advc19_go/etc"
)

func get_fuel_requirement(mass int) int {
	return mass/3 - 2
}

func solve_19_01_A(lines []string) int {
	result := 0
	for _, line := range lines {
		mass, _ := strconv.Atoi(line)
		fuel := get_fuel_requirement(mass)
		result += fuel
	}
	return result
}

func solve_19_01_B(lines []string) int {
	result := 0
	for _, line := range lines {
		mass, _ := strconv.Atoi(line)

		for {
			fuel := get_fuel_requirement(mass)
			if fuel <= 0 {
				break
			}
			result += fuel
			mass = fuel
		}
	}
	return result
}

func Solve() {
	content, err := os.ReadFile("../data/input01.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)
	str = strings.Replace(str, "\r", "", -1)
	lines := strings.Split(str, "\n")

	ans1 := solve_19_01_A(lines)
	etc.AnswerChecker("DAY01", ans1, 3223398)

	ans2 := solve_19_01_B(lines)
	etc.AnswerChecker("DAY01", ans2, 4832253)
}
