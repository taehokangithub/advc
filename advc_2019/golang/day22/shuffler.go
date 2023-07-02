package day22

import (
	"fmt"
	"strconv"
	"strings"
	"taeho/advc19_go/utils"
)

type Shuffler struct {
	instructions []*Instruction
	size         int
}

func NewShuffler(size int, str string) *Shuffler {
	s := &Shuffler{
		instructions: make([]*Instruction, 0, 100),
		size:         size,
	}
	s.Parse(str)
	return s
}

func (s *Shuffler) Parse(str string) {
	str = strings.Replace(str, "\r", "", -1)
	lines := strings.Split(str, "\n")

	getParam := func(line string) int {
		fields := strings.Fields(line)
		ret, err := strconv.Atoi(fields[len(fields)-1])
		if err != nil {
			panic(err)
		}
		return ret
	}

	var inst *Instruction
	for _, line := range lines {
		if strings.HasPrefix(line, CMD_CUT) {
			inst = &Instruction{INST_CUT, getParam(line)}
		} else if strings.HasPrefix(line, CMD_INCRE) {
			inst = &Instruction{INST_INCRE, getParam(line)}
		} else if strings.HasPrefix(line, CMD_REVERSE) {
			inst = &Instruction{INST_REVERSE, 0}
		} else {
			panic(fmt.Sprintln("Unknown line", line))
		}
		s.instructions = append(s.instructions, inst)
	}
}

func (s *Shuffler) Shuffle() []int {
	deck := make([]int, s.size)
	for i := range deck {
		deck[i] = i
	}

	for _, inst := range s.instructions {
		deck = inst.Run(deck)
	}

	return deck
}

func (s *Shuffler) Analyse() {
	isPositive := true
	max := 0
	min := 0
	pos := 0

	for _, inst := range s.instructions {
		if inst.instType == INST_REVERSE {
			isPositive = !isPositive
			fmt.Println("Rev : ", isPositive, pos, "(", min, max, ")")
		} else if inst.instType == INST_CUT {
			param := inst.param
			if !isPositive {
				param = -param
			}
			pos += param
			max = utils.Max(max, pos)
			min = utils.Min(min, pos)
			fmt.Println("Cut ", inst.param, ": ", isPositive, pos, "(", min, max, ")")
		}
	}
	fmt.Println("Finished : ", isPositive, pos, "(", min, max, ")")
}

func Shuffle(size int, str string) []int {
	s := NewShuffler(size, str)
	return s.Shuffle()
}
