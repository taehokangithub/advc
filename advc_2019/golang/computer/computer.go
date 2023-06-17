package computer

import (
	"fmt"
	"sort"
	"strconv"
	"strings"
)

type Instruction int64
type ParameterMode int64
type ComStatus int

const (
	COMSTATUS_READY    ComStatus = 0
	COMSTATUS_PAUSED   ComStatus = 1
	COMSTATUS_FINISHED ComStatus = 2
)

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
	INST_SET_RELBASE   Instruction = 9
	INST_HALT          Instruction = 99
)

const (
	PARAM_MODE_POSITION  ParameterMode = 0
	PARAM_MODE_IMMEDIATE ParameterMode = 1
	PARAM_MODE_RELATIVE  ParameterMode = 2
)

type Computer struct {
	memory  map[int]int64
	pc      int64
	input   []int64
	output  []int64
	relBase int64
	Status  ComStatus
}

func NewComputer(memstr string) *Computer {
	com := Computer{
		input:   make([]int64, 0),
		output:  make([]int64, 0),
		memory:  make(map[int]int64),
		Status:  COMSTATUS_READY,
		relBase: 0,
	}
	com.InitMemory(memstr)
	return &com
}

func (c *Computer) Copy() *Computer {
	com := Computer{
		input:   make([]int64, len(c.input)),
		output:  make([]int64, len(c.output)),
		memory:  make(map[int]int64, len(c.memory)),
		Status:  c.Status,
		relBase: c.relBase,
	}
	copy(com.input, c.input)
	copy(com.output, c.output)
	for k, v := range c.memory {
		com.memory[k] = v
	}
	return &com
}

func (c *Computer) InitMemory(memstr string) {
	if len(memstr) == 0 {
		return
	}
	memrunes := strings.Split(memstr, ",")

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

func (c *Computer) HasOutput() bool {
	return len(c.output) > 0
}

func (c *Computer) PopOutput() int64 {
	ret := c.output[0]
	c.output = c.output[1:]
	return ret
}

func (c *Computer) Set64(i int64, v int64) {
	c.memory[int(i)] = v
}

func (c *Computer) Set(i int, v int) {
	c.memory[i] = int64(v)
}

func (c *Computer) Get(i int64) int64 {
	return c.memory[int(i)]
}

func (c *Computer) DumpMemory() string {
	keys := []int{}
	for k := range c.memory {
		keys = append(keys, k)
	}
	sort.Ints(keys)

	sb := strings.Builder{}
	for _, k := range keys {
		v := c.Get(int64(k))
		sb.WriteString(fmt.Sprint(v))
		sb.WriteRune(',')
	}
	str := sb.String()

	return str[:len(str)-1]
}

func (c *Computer) RunProgram() {
	var inst Instruction = INST_NIL

	if c.Status == COMSTATUS_PAUSED && len(c.input) > 0 {
		c.Status = COMSTATUS_READY
	}

	for c.Status == COMSTATUS_READY {
		inst = Instruction(c.Get(c.pc))
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
	param := c.Get(c.pc)
	c.pc++
	return param
}

func (c *Computer) getParamByPosition() int64 {
	addr := c.getParamImmediate()
	return c.Get(addr)
}

func (c *Computer) getParamByRelative() int64 {
	addr := c.getParamImmediate() + int64(c.relBase)
	return c.Get(addr)
}

func (c *Computer) getParam(modes *[]ParameterMode) int64 {
	mode := (*modes)[0]
	*modes = (*modes)[1:]

	switch mode {
	case PARAM_MODE_IMMEDIATE:
		return c.getParamImmediate()
	case PARAM_MODE_POSITION:
		return c.getParamByPosition()
	case PARAM_MODE_RELATIVE:
		return c.getParamByRelative()
	default:
		panic(fmt.Sprintf("Unknown parameter mode %v", mode))
	}
}

func (c *Computer) getSavePositionParam(modes *[]ParameterMode) int64 {
	mode := (*modes)[0]
	*modes = (*modes)[1:]

	addr := c.getParamImmediate()

	if mode == PARAM_MODE_RELATIVE {
		addr += c.relBase
	}

	return addr
}

func (c *Computer) runInst(inst Instruction) {
	modes := c.getParamModes(&inst)

	switch inst {
	case INST_ADD:
		op1 := c.getParam(&modes)
		op2 := c.getParam(&modes)
		saveto := c.getSavePositionParam(&modes)
		c.Set64(saveto, op1+op2)
	case INST_MUL:
		op1 := c.getParam(&modes)
		op2 := c.getParam(&modes)
		saveto := c.getSavePositionParam(&modes)
		c.Set64(saveto, op1*op2)
	case INST_INPUT:
		if len(c.input) == 0 {
			c.Status = COMSTATUS_PAUSED
			c.pc--
			return
		}
		input := c.input[0]
		c.input = c.input[1:]
		saveto := c.getSavePositionParam(&modes)
		c.Set64(saveto, input)
	case INST_OUTPUT:
		val := c.getParam(&modes)
		c.output = append(c.output, val)
	case INST_LESS_THAN:
		op1 := c.getParam(&modes)
		op2 := c.getParam(&modes)
		saveto := c.getSavePositionParam(&modes)
		val := 0
		if op1 < op2 {
			val = 1
		}
		c.Set64(saveto, int64(val))
	case INST_EQUALS:
		op1 := c.getParam(&modes)
		op2 := c.getParam(&modes)
		saveto := c.getSavePositionParam(&modes)
		val := 0
		if op1 == op2 {
			val = 1
		}
		c.Set64(saveto, int64(val))
	case INST_JUMP_IF_TRUE:
		op1 := c.getParam(&modes)
		op2 := c.getParam(&modes)
		if op1 != 0 {
			c.pc = op2
		}
	case INST_JUMP_IF_FALSE:
		op1 := c.getParam(&modes)
		op2 := c.getParam(&modes)
		if op1 == 0 {
			c.pc = op2
		}
	case INST_SET_RELBASE:
		c.relBase += c.getParam(&modes)
	case INST_HALT:
		c.Status = COMSTATUS_FINISHED
		return
	default:
		panic(fmt.Sprintf("Unknown inst {%d} at {%d}", inst, c.pc))
	}
}
