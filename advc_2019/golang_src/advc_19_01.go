package main

import (
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"
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
			//fmt.Println("mass ", mass, "requires fuel", fuel, "total", result)
			mass = fuel
		}
	}
	return result
}

func solve_19_01() {
	content, _ := ioutil.ReadFile("../data/input01.txt")

	lines := strings.Split(string(content), "\n")

	fmt.Println("ans1", solve_19_01_A(lines))

	fmt.Println("ans2", solve_19_01_B(lines))
}
