package day22

import (
	"fmt"
	"strconv"
	"strings"
)

type Shuffler struct {
	instructions []*Instruction
	size         int64
}

func NewShuffler(size int64, str string) *Shuffler {
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

func (s *Shuffler) GetFinalPositionOf(card int64) int64 {
	op := s.GetSingleOperation()
	return op.Operate(card)
}

func (s *Shuffler) GetCardAtAfterIterations(position int64, iteration int64) int64 {
	op := s.GetSingleOperation()
	itop := op.SelfMerge(iteration)
	return itop.ReverseOperate(position)
}

func (s *Shuffler) GetSingleOperation() *LinearOperation {
	op := NewLinearOperationFromInstruction(s.instructions[0], s.size)

	for i := 1; i < len(s.instructions); i++ {
		other := NewLinearOperationFromInstruction(s.instructions[i], s.size)
		op.Merge(other)
	}

	return op
}
