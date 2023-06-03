package day11

import (
	"fmt"
	"os"
	"taeho/advc19_go/computer"
	"taeho/advc19_go/etc"
	"taeho/advc19_go/utils"
)

func runRobot(str string, input byte) *utils.FreeMap[byte] {

	com := computer.NewComputer(str)

	m := utils.NewFreeMap[byte]()

	curPos := utils.NewVector2D(0, 0)
	curDir := utils.DIR_VECTORS[utils.DIR_UP]
	m.Set(utils.NewVector2D(0, 0), input)

	for com.Status != computer.COMSTATUS_FINISHED {
		var color byte = 0
		if val, ok := m.Get(curPos); ok {
			color = val
		}

		com.AddInput(int64(color))
		com.RunProgram()

		color = byte(com.PopOutput())
		dirVal := byte(com.PopOutput())
		dir := utils.DIR_LEFT
		if dirVal == 1 {
			dir = utils.DIR_RIGHT
		}

		m.Set(curPos, color)

		curDir.Rotate(dir)
		curPos.Add(curDir)
	}

	return m
}

func solve01(str string) int {
	m := runRobot(str, 0)
	return m.Count()
}

func solve02(str string) int {
	m := runRobot(str, 1)

	for y := m.Max.Y; y >= m.Min.Y; y-- {
		for x := m.Min.X; x <= m.Max.X; x++ {
			c := " "
			if val, ok := m.Get(utils.NewVector2D(x, y)); ok {
				if val == 1 {
					c = "#"
				}
			}
			fmt.Print(c)
		}
		fmt.Println("")
	}
	return 0
}

func Solve() {
	content, err := os.ReadFile("../data/input11.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY11", ans1, 2160)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY11", ans2, 0)
}
