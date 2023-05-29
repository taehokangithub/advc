package main

import (
	"fmt"
	"os"
	"taeho/advc19_go/computer"
)

func solve_19_02_A(data string) {
	com := computer.NewComputer()
	com.InitMemory(data)
	com.Set(1, 12)
	com.Set(2, 2)
	com.RunProgram()
	ret := com.Get(0)
	fmt.Println("ans1", ret)
}

func solve_19_02_B(data string) {
	const expected = 19690720
	for i := 0; i <= 99; i++ {
		for j := 0; j <= 99; j++ {
			com := computer.NewComputer()
			com.InitMemory(data)
			com.Set(1, i)
			com.Set(2, j)
			com.RunProgram()
			ret := com.Get(0)
			if ret == expected {
				fmt.Println("ans2", 100*i+j)
			}
		}
	}
}

func solve_19_02() {
	content, _ := os.ReadFile("../data/input02.txt")
	data := string(content)
	solve_19_02_A(data)
	solve_19_02_B(data)
}
