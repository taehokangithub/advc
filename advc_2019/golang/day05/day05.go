package day05

import (
	"os"
	"taeho/advc19_go/computer"
	"taeho/advc19_go/etc"
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
	etc.AnswerChecker("DAY05", ans1, 13818007)

	ans2 := solve2(program, 5)
	etc.AnswerChecker("DAY05", ans2, 3176266)
}
