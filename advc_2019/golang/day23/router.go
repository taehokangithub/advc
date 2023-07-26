package day23

import (
	"taeho/advc19_go/computer"
	"taeho/advc19_go/utils"
)

const (
	ADDR_NAT = 255
)

type Nat struct {
	hasPacket   bool
	hasEverSent bool
	x, y        int64
	prevSentY   int64
}

type Router struct {
	coms  []*computer.Computer
	queue map[int]*utils.Queue[int64]
	nat   Nat
}

func NewRouter(size int, str string) *Router {
	r := &Router{
		coms:  make([]*computer.Computer, size),
		queue: map[int]*utils.Queue[int64]{},
	}

	for i := range r.coms {
		com := computer.NewComputer(str)
		com.AddInput32(i)
		r.coms[i] = com
		r.queue[i] = utils.NewQueue[int64]()
	}
	return r
}

func (r *Router) Process(index int) {
	com := r.coms[index]
	myqueue := r.queue[index]

	if myqueue.Len() == 0 {
		com.AddInput32(-1)
	}
	for myqueue.Len() > 0 {
		data := myqueue.Pop()
		com.AddInput(data)
	}

	com.RunProgram()

	for com.HasOutput() {
		dst := com.PopOutput32()
		x := com.PopOutput()
		y := com.PopOutput()
		if dst == ADDR_NAT {
			r.nat.x = x
			r.nat.y = y
			r.nat.hasPacket = true
		} else {
			r.queue[dst].Push(x)
			r.queue[dst].Push(y)
		}
	}
}

func (r *Router) FindYOfFirstPacketToN(destAddr int) int64 {
	for {
		for i := range r.coms {
			r.Process(i)

			if r.nat.hasPacket {
				return r.nat.y
			}
		}
	}
}

func (r *Router) FindYOfFirstSameY_FromNat() int64 {
	idleCnt := 0
	loopCnt := 0
	for {
		loopCnt++

		for i := range r.coms {
			r.Process(i)
		}
		if r.IsAllQueueEmpty() {
			idleCnt++
		}

		if idleCnt >= 2 {
			idleCnt = 0
			if r.nat.hasEverSent && r.nat.prevSentY == r.nat.y {
				return r.nat.y
			}
			dstQueue := r.queue[0]
			dstQueue.Push(r.nat.x)
			dstQueue.Push(r.nat.y)
			r.nat.hasEverSent = true
			r.nat.prevSentY = r.nat.y
		}
	}
}

func (r *Router) IsAllQueueEmpty() bool {
	for _, q := range r.queue {
		if q.Len() > 0 {
			return false
		}
	}
	return true
}
