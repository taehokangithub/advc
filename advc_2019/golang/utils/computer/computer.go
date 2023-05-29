package computer

import (
	"fmt"
	"strconv"
	"strings"
)

const (
	INST_NIL  = 0
	INST_ADD  = 1
	INST_MUL  = 2
	INST_HALT = 99
)

type Computer struct {
	memory []int64
	pc     int
}

func NewComputer() Computer {
	com := Computer{}
	return com
}

func (c *Computer) InitMemory(memstr string) {
	memrunes := strings.Split(memstr, ",")

	c.memory = make([]int64, len(memrunes))

	for i, r := range memrunes {
		r = strings.TrimSpace(r)
		n, err := strconv.Atoi(r)
		if err != nil {
			panic(err.Error())
		}
		c.memory[i] = int64(n)
	}
}

func (c *Computer) Set64(i int64, v int64) {
	c.memory[i] = v
}

func (c *Computer) Set(i int, v int) {
	c.memory[i] = int64(v)
}

func (c *Computer) Get(i int) int64 {
	return c.memory[i]
}

func (c *Computer) DumpMemory() string {
	sb := strings.Builder{}

	for _, v := range c.memory {
		sb.WriteString(fmt.Sprint(v))
		sb.WriteRune(',')
	}

	str := sb.String()
	return str[:len(str)-1]
}

func (c *Computer) RunProgram() {
	var inst int64 = INST_NIL
	for inst != INST_HALT {
		inst = c.memory[c.pc]
		c.pc++

		c.runInst(inst)
	}
}

func (c *Computer) getParam() int64 {
	param := c.memory[c.pc]
	c.pc++
	return param
}

func (c *Computer) getParamIndirect() int64 {
	param := c.getParam()
	return c.Get(int(param))
}

func (c *Computer) runInst(inst int64) {
	switch inst {
	case INST_ADD:
		op1 := c.getParamIndirect()
		op2 := c.getParamIndirect()
		saveto := c.getParam()
		c.Set64(saveto, op1+op2)
	case INST_MUL:
		op1 := c.getParamIndirect()
		op2 := c.getParamIndirect()
		saveto := c.getParam()
		c.Set64(saveto, op1*op2)
	case INST_HALT:
		return
	default:
		panic(fmt.Sprintf("Unknown inst {%d} at {%d}", inst, c.pc))
	}
}
