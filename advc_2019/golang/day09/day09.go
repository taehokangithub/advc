package day09

import (
	"fmt"
	"os"
	"taeho/advc19_go/computer"
)

func solve01(str string) int64 {
	com := computer.NewComputer(str)
	com.AddInput(1)
	com.RunProgram()
	ans1 := com.PopOutput()
	return ans1
}

func solve02(str string) int64 {
	com := computer.NewComputer(str)
	com.AddInput(2)
	com.RunProgram()
	ans2 := com.PopOutput()
	return ans2
}

func Solve() {
	content, err := os.ReadFile("../data/input09.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	fmt.Println("DAY09 ans1", ans1, "expected", 2955820355)

	ans2 := solve02(str)
	fmt.Println("DAY09 ans2", ans2, "expected", 46643)
}
