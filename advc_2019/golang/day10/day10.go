package day10

import (
	"fmt"
	"os"
)

func solve01(str string) int {
	ast := createAsteroidGrid(str)
	ans1, _ := ast.getAllVisibleCount()
	return ans1
}

func solve02(str string) int {
	ast := createAsteroidGrid(str)
	cnt, loc := ast.getAllVisibleCount()
	fmt.Printf("solve 2 mid, cnt %d, loc %v\n", cnt, loc)
	v := ast.findNthDestroyed(loc, 200)
	return v.X*100 + v.Y
}

func Solve() {
	content, err := os.ReadFile("../data/input10.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	fmt.Println("DAY10 ans1", ans1, "expected", 314)

	ans2 := solve02(str)
	fmt.Println("DAY10 ans2", ans2, "expected", 1513)
}
