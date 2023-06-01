package day00

import (
	"fmt"
	"os"
)

func solve01(str string) int {
	ans1 := 0
	return ans1
}

func solve02(str string) int {
	return 0
}

func Solve() {
	content, err := os.ReadFile("../data/input00.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	fmt.Println("DAY00 ans1", ans1, "expected", 0)

	ans2 := solve02(str)
	fmt.Println("DAY00 ans2", ans2, "expected", 0)
}
