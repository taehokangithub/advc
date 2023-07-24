package day22

import (
	"fmt"
	"math/big"
	"taeho/advc19_go/utils"
)

type LinearOperation struct {
	a, b, mod int64 // y = (ax * b) % mod
}

func (o *LinearOperation) Clone() *LinearOperation {
	return &LinearOperation{o.a, o.b, o.mod}
}

func (o *LinearOperation) String() string {
	return fmt.Sprint("Op ", o.a, " x + ", o.b, " % ", o.mod)
}

func NewLinearOperation(a, b, mod int64) *LinearOperation {
	return &LinearOperation{a, b, mod}
}

func NewLinearOperationFromInstruction(inst *Instruction, size int64) *LinearOperation {
	o := &LinearOperation{
		mod: size,
	}
	switch inst.instType {
	case INST_CUT:
		o.a, o.b = 1, -int64(inst.param) // y = x - param
	case INST_INCRE:
		o.a, o.b = int64(inst.param), 0 // y = param * x
	case INST_REVERSE:
		o.a, o.b = -1, (size - 1) // y = size - 1 - x
	default:
		panic(fmt.Sprintln("Unknown inst type", inst.instType))
	}

	return o
}

func (o *LinearOperation) Operate(x int64) int64 {
	// y = (ax + b) % mod
	bigA := big.NewInt(o.a)
	bigB := big.NewInt(o.b)
	bigX := big.NewInt(x)
	bigM := big.NewInt(o.mod)

	bigA.Mul(bigA, bigX)
	bigA.Add(bigA, bigB)
	bigA.Mod(bigA, bigM)

	ans := bigA.Int64()
	for ans < 0 {
		ans += o.mod
	}
	return ans
}

func (o *LinearOperation) ReverseOperate(y int64) int64 {
	// y = (ax + b) % mod ===> find x for given y(val)
	// y = ax % mod + b % mod
	// ax % mod = y - b % mod
	// ax % mod = (y - b) % mod
	//
	// modular inversion : (A * B) % mod == 1
	// Now we get a's modular inversion inv
	inv := utils.ModularInversion(o.a, o.mod)

	// Now multiply inv to the both sides
	// inv * ax % mod = inv * (y - b) % mod
	// x = inv * (y - b) % mod
	// instead of direct calculation, use LinearOperation to avoid int64 overflow
	op := NewLinearOperation(y-o.b, 0, o.mod)
	ans := op.Operate(inv)

	return ans
}

func (o *LinearOperation) Merge(other *LinearOperation) {
	// (ax + b) into another (a'x + b')
	// a'ax + a'b + b'
	big1A := big.NewInt(o.a)
	big1B := big.NewInt(o.b)
	big2A := big.NewInt(other.a)
	big2B := big.NewInt(other.b)
	bigMod := big.NewInt(o.mod)

	big1A.Mul(big1A, big2A)
	big1A.Mod(big1A, bigMod)
	o.a = big1A.Int64()

	big1B.Mul(big1B, big2A)
	big1B.Add(big1B, big2B)
	big1B.Mod(big1B, bigMod)
	o.b = big1B.Int64()
}

func (o *LinearOperation) SelfMerge(iteration int64) *LinearOperation {
	cnt := int64(1)
	first := o.Clone()

	if iteration == 1 {
		return first
	}

	for cnt*2 <= iteration {
		first.Merge(first)
		cnt = cnt * 2
	}

	remainCnt := iteration - cnt
	if remainCnt > 0 {
		rest := o.SelfMerge(remainCnt)
		first.Merge(rest)
	}

	return first
}
