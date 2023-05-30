package day05

import (
	"fmt"
	"os"
	"taeho/advc19_go/computer"
)

func solve1(program string, input int64) int64 {
	com := computer.NewComputer(program)
	com.AddInput(input)
	com.RunProgram()

	output := com.GetOutput()
	return output[len(output)-1]
}

func solve2(program string, input int64) int64 {
	com := computer.NewComputer(program)
	com.AddInput(input)
	com.RunProgram()
	return com.GetOutput()[0]
}

func Solve() {
	content, e := os.ReadFile("../data/input05.txt")
	if e != nil {
		panic(e)
	}

	program := string(content)

	ans1 := solve1(program, 1)
	fmt.Println("DAY05 ans1", ans1, "expected", 13818007)

	ans2 := solve2(program, 5)
	fmt.Println("DAY05 ans2", ans2, "expected", 3176266)
}
