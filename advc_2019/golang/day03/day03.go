package day03

import (
	"fmt"
	"os"
	"strconv"
	"strings"
	"taeho/advc19_go/utils"
)

type Inst struct {
	dir  utils.Direction
	dist int
}

func (inst Inst) String() string {
	return fmt.Sprintf("[%v %d]", utils.DIR_NAMES[inst.dir], inst.dist)
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
	fmt.Printf("1 - %v\n2 - %v\n", inst1, inst2)
	return 0
}

func Solve() {
	content, e := os.ReadFile("../data/input03.txt")
	if e != nil {
		panic(e)
	}
	strs := strings.Split(string(content), "\n")
	ans1 := solve01(strs[0], strs[1])
	fmt.Println("ans1 = ", ans1)
}
