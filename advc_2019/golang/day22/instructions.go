package day22

import (
	"fmt"
	"taeho/advc19_go/utils"
)

type InstType byte

const INST_REVERSE InstType = 0
const INST_CUT InstType = 1
const INST_INCRE InstType = 2

const CMD_REVERSE = "deal into new stack"
const CMD_CUT = "cut"
const CMD_INCRE = "deal with increment"

type Instruction struct {
	instType InstType
	param    int
}

func (i *Instruction) GetName() string {
	switch i.instType {
	case INST_CUT:
		return "CUT"
	case INST_INCRE:
		return "INCRE"
	case INST_REVERSE:
		return "REV"
	}
	panic(fmt.Sprintln("Unknown inst", i.instType))
}

func (i *Instruction) String() string {
	return fmt.Sprintf("[%s/%d]", i.GetName(), i.param)
}

func (i *Instruction) Run(deck []int) []int {
	switch i.instType {
	case INST_CUT:
		return i.RunCut(deck)
	case INST_INCRE:
		return i.RunIncre(deck)
	case INST_REVERSE:
		return i.RunReverse(deck)
	}
	panic(fmt.Sprintln("Unknown inst", i.instType))
}

func (i *Instruction) RunReverse(deck []int) []int {
	for i := range deck {
		j := len(deck) - i - 1
		if i >= j {
			break
		}
		deck[i], deck[j] = deck[j], deck[i]
	}
	return deck
}

func (i *Instruction) RunCut(deck []int) []int {
	param := i.param
	if param < 0 {
		param = len(deck) + param
	}
	front := deck[:param]
	back := deck[param:]
	return append(back, front...)
}

func (i *Instruction) RunIncre(deck []int) []int {
	l := len(deck)
	gcd := utils.Gcd(l, i.param)
	if gcd != 1 {
		panic(fmt.Sprintln("Len", l, "param", i.param, "divisible by", gcd))
	}
	newDeck := make([]int, l)
	index := 0
	for k := 0; k < l; k++ {
		newDeck[index] = deck[k]
		index += i.param
		if index >= l {
			index -= l
		}
	}
	return newDeck
}
