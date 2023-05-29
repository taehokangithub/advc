package computer

import (
	"fmt"
	"strconv"
	"strings"
)

type Instruction int64
type ParameterMode int64

const (
	INST_NIL           Instruction = 0
	INST_ADD           Instruction = 1
	INST_MUL           Instruction = 2
	INST_INPUT         Instruction = 3
	INST_OUTPUT        Instruction = 4
	INST_JUMP_IF_TRUE  Instruction = 5
	INST_JUMP_IF_FALSE Instruction = 6
	INST_LESS_THAN     Instruction = 7
	INST_EQUALS        Instruction = 8
	INST_HALT          Instruction = 99
)

const (
	PARAM_MODE_POSITION  ParameterMode = 0
	PARAM_MODE_IMMEDIATE ParameterMode = 1
)

type Computer struct {
	memory []int64
	pc     int
	input  []int64
	output []int64
}

func NewComputer(memstr string) Computer {
	com := Computer{}
	com.input = make([]int64, 0)
	com.output = make([]int64, 0)
	com.InitMemory(memstr)
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

func (c *Computer) AddInputRange(input []int64) {
	c.input = append(c.input, input...)
}

func (c *Computer) AddInput(input int64) {
	c.AddInputRange([]int64{input})
}

func (c *Computer) GetOutput() []int64 {
	return c.output
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
	var inst Instruction = INST_NIL
	for inst != INST_HALT {
		inst = Instruction(c.memory[c.pc])
		c.pc++

		c.runInst(inst)
	}
}

func (c *Computer) getParamModes(inst *Instruction) []ParameterMode {
	const maxParams = 5
	ret := make([]ParameterMode, maxParams)
	str := fmt.Sprintf("%v", int64(*inst))

	if len(str) > 2 {
		str = str[:len(str)-2]
		index := 0

		for i := len(str) - 1; i >= 0; i-- {
			ret[index] = ParameterMode(str[i] - '0')
			index++
		}
	}

	*inst = (*inst) % 100

	return ret
}

func (c *Computer) getParamImmediate() int64 {
	param := c.memory[c.pc]
	c.pc++
	return param
}

func (c *Computer) getParamByPosition() int64 {
	param := c.getParamImmediate()
	return c.Get(int(param))
}

func (c *Computer) getParam(modes *[]ParameterMode) int64 {
	mode := (*modes)[0]
	*modes = (*modes)[1:]

	switch mode {
	case PARAM_MODE_IMMEDIATE:
		return c.getParamImmediate()
	case PARAM_MODE_POSITION:
		return c.getParamByPosition()
	default:
		panic(fmt.Sprintf("Unknown parameter mode %v", mode))
	}
}

func (c *Computer) runInst(inst Instruction) {
	modes := c.getParamModes(&inst)

	switch inst {
	case INST_ADD:
		op1 := c.getParam(&modes)
		op2 := c.getParam(&modes)
		saveto := c.getParamImmediate()
		c.Set64(saveto, op1+op2)
	case INST_MUL:
		op1 := c.getParam(&modes)
		op2 := c.getParam(&modes)
		saveto := c.getParamImmediate()
		c.Set64(saveto, op1*op2)
	case INST_INPUT:
		if len(c.input) == 0 {
			panic("Input buffer is empty!")
		}
		input := c.input[0]
		c.input = c.input[1:]
		saveto := c.getParamImmediate()
		c.Set64(saveto, input)
	case INST_OUTPUT:
		val := c.getParam(&modes)
		c.output = append(c.output, val)
	case INST_LESS_THAN:
		op1 := c.getParam(&modes)
		op2 := c.getParam(&modes)
		saveto := c.getParamImmediate()
		val := 0
		if op1 < op2 {
			val = 1
		}
		c.Set64(saveto, int64(val))
	case INST_EQUALS:
		op1 := c.getParam(&modes)
		op2 := c.getParam(&modes)
		saveto := c.getParamImmediate()
		val := 0
		if op1 == op2 {
			val = 1
		}
		c.Set64(saveto, int64(val))
	case INST_JUMP_IF_TRUE:
		op1 := c.getParam(&modes)
		op2 := c.getParam(&modes)
		if op1 != 0 {
			c.pc = int(op2)
		}
	case INST_JUMP_IF_FALSE:
		op1 := c.getParam(&modes)
		op2 := c.getParam(&modes)
		if op1 == 0 {
			c.pc = int(op2)
		}
	case INST_HALT:
		return
	default:
		panic(fmt.Sprintf("Unknown inst {%d} at {%d}", inst, c.pc))
	}
}
