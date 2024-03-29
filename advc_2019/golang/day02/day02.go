package day02

import (
	"os"
	"taeho/advc19_go/computer"
	"taeho/advc19_go/etc"
)

func solve_19_02_A(data string) int {
	com := computer.NewComputer(data)
	com.Set(1, 12)
	com.Set(2, 2)
	com.RunProgram()
	ret := com.Get(0)
	return int(ret)
}

func solve_19_02_B(data string) int {
	const expected = 19690720
	for i := 0; i <= 99; i++ {
		for j := 0; j <= 99; j++ {
			com := computer.NewComputer(data)
			com.Set(1, i)
			com.Set(2, j)
			com.RunProgram()
			ret := com.Get(0)
			if ret == expected {
				return 100*i + j
			}
		}
	}
	panic("answer not found")
}

func Solve() {
	content, err := os.ReadFile("../data/input02.txt")
	if err != nil {
		panic(err)
	}

	data := string(content)

	ans1 := solve_19_02_A(data)
	etc.AnswerChecker("DAY02", ans1, 4023471)

	ans2 := solve_19_02_B(data)
	etc.AnswerChecker("DAY02", ans2, 8051)
}
