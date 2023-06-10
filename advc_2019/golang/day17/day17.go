package day17

import (
	"os"
	"taeho/advc19_go/computer"
	"taeho/advc19_go/etc"
)

func solve01(str string) int {
	m := NewMyGrid(str)
	return m.cntIntersection()
}

func solve02(str string) int {
	m := NewMyGrid(str)
	fullPaths := m.getFullPath()
	instructions := fullPaths.getInstructions()

	com := computer.NewComputer(str)
	com.Set(0, 2)
	for _, c := range instructions {
		com.AddInput(int64(c))
	}
	com.RunProgram()
	output := com.GetOutput()
	return int(output[len(output)-1])
}

func Solve() {
	content, err := os.ReadFile("../data/input17.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY17", ans1, 14332)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY17", ans2, 1034009)
}
