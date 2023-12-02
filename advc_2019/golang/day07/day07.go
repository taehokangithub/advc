package day07

import (
	"os"
	"taeho/advc19_go/computer"
	"taeho/advc19_go/etc"
	"taeho/advc19_go/utils"
)

func pickNinternal(maxCnt int, curPicks []int) [][]int {
	curLen := len(curPicks)

	if curLen == maxCnt {
		return [][]int{curPicks}
	}

	myRet := make([][]int, 0)

	contains := func(arr []int, val int) bool {
		for _, v := range arr {
			if v == val {
				return true
			}
		}
		return false
	}

	for i := 0; i < maxCnt; i++ {
		if !contains(curPicks, i) {
			newPicks := make([]int, curLen, curLen+1)
			copy(newPicks, curPicks)
			newPicks = append(newPicks, i)
			ret := pickNinternal(maxCnt, newPicks)
			myRet = append(myRet, ret...)
		}
	}
	return myRet
}

func pickN(maxCnt int) [][]int {
	return pickNinternal(maxCnt, []int{})
}

func runThruster(memory string, settings []int) int {
	lastOutput := 0

	for _, s := range settings {
		com := computer.NewComputer(memory)
		com.AddInput(int64(s))
		com.AddInput(int64(lastOutput))
		com.RunProgram()
		lastOutput = int(com.GetOutput()[0])
	}

	return lastOutput
}

func runThrusterFeedback(memory string, settings []int) int64 {
	coms := make([]*computer.Computer, len(settings))

	for i := range coms {
		coms[i] = computer.NewComputer(memory)
		coms[i].AddInput(int64(settings[i] + 5))
	}
	lastOutput := int64(0)

	for {
		for _, com := range coms {
			com.AddInput(lastOutput)
			com.RunProgram()
			lastOutput = com.PopOutput()
		}
		if coms[len(coms)-1].Status == computer.COMSTATUS_FINISHED {
			break
		}
	}

	return lastOutput
}

func solve01(memory string) int {
	settings := pickN(5)

	maxOutput := 0
	for _, s := range settings {
		ret := runThruster(memory, s)
		maxOutput = utils.Max(ret, maxOutput)
	}
	return maxOutput
}

func solve02(memory string) int64 {
	settings := pickN(5)

	maxOutput := int64(0)
	for _, s := range settings {
		ret := runThrusterFeedback(memory, s)
		maxOutput = utils.Max(ret, maxOutput)
	}
	return maxOutput
}

func Solve() {
	content, err := os.ReadFile("../data/input07.txt")
	if err != nil {
		panic(err)
	}

	str := string(content)

	ans1 := solve01(str)
	etc.AnswerChecker("DAY07", ans1, 45730)

	ans2 := solve02(str)
	etc.AnswerChecker("DAY07", ans2, 5406484)
}
