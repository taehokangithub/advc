package main

import (
	"fmt"
	"io/ioutil"
	"os"
)

func get_fuel_requirement(mass int) int {
	return mass*3 - 2
}

func solve_19_01() {
	fmt.Println("Hello")
	file, _ := os.Open("../data/input01.txt")
	binary, _ := ioutil.ReadAll(file)

	fmt.Println(binary)
}
