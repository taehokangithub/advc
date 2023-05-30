package day03

import (
	"fmt"
	"math"
	"os"
	"strconv"
	"strings"
	"taeho/advc19_go/utils"
)

type Inst struct {
	dir  utils.Direction
	dist int
}

type MoveMap = *utils.FreeMap[int]

func (inst Inst) String() string {
	return fmt.Sprintf("[%v %d]", utils.DIR_NAMES[inst.dir], inst.dist)
}

func drawOnMap(insts []Inst) MoveMap {
	ret := utils.NewFreeMap[int]()
	vector := utils.NewVector2D(0, 0)

	steps := 0
	for _, inst := range insts {
		for i := 0; i < inst.dist; i++ {
			vector.Move(inst.dir, 1)
			steps++
			ret.Set(vector, steps)
		}
	}
	return MoveMap(ret)
}

func findCrosses(insts []Inst, m MoveMap) MoveMap {
	ret := utils.NewFreeMap[int]()
	vector := utils.NewVector2D(0, 0)

	steps := 0
	for _, inst := range insts {
		for i := 0; i < inst.dist; i++ {
			vector.Move(inst.dir, 1)
			steps++
			if val, ok := m.Get(vector); ok {
				ret.Set(vector, steps+val)
			}
		}
	}

	return MoveMap(ret)
}

func parseInst(str string) []Inst {
	split := strings.Split(str, ",")
	ret := make([]Inst, len(split))
	for i, s := range split {
		dirchar := s[0]
		inst := Inst{}

		switch dirchar {
		case 'U':
			inst.dir = utils.DIR_UP
		case 'D':
			inst.dir = utils.DIR_DOWN
		case 'L':
			inst.dir = utils.DIR_LEFT
		case 'R':
			inst.dir = utils.DIR_RIGHT
		default:
			panic(fmt.Sprintf("Unknown dirchar %v", dirchar))
		}

		diststr := s[1:]
		dist, e := strconv.Atoi(diststr)
		if e != nil {
			panic(e)
		}
		inst.dist = dist
		ret[i] = inst
	}

	return ret
}

func solve01(str1 string, str2 string) int {
	inst1 := parseInst(str1)
	inst2 := parseInst(str2)

	m := drawOnMap(inst1)
	crosses := findCrosses(inst2, m)
	ans := math.MaxInt

	crosses.Foreach(func(v utils.Vector, val int) {
		dist := v.ManhattanDistance()
		if dist < ans {
			ans = dist
		}
	})

	return ans
}

func solve02(str1 string, str2 string) int {
	inst1 := parseInst(str1)
	inst2 := parseInst(str2)

	m := drawOnMap(inst1)
	crosses := findCrosses(inst2, m)
	ans := math.MaxInt

	crosses.Foreach(func(v utils.Vector, val int) {
		if ans > val {
			ans = val
		}
	})
	return ans
}

func Solve() {
	content, e := os.ReadFile("../data/input03.txt")
	if e != nil {
		panic(e)
	}

	contentStr := strings.Replace(string(content), "\r", "", -1)
	strs := strings.Split(contentStr, "\n")

	ans1 := solve01(strs[0], strs[1])
	fmt.Println("DAY03 ans1", ans1, "expected", 232)

	ans2 := solve02(strs[0], strs[1])
	fmt.Println("DAY03 ans2", ans2, "expected", 6084)
}
