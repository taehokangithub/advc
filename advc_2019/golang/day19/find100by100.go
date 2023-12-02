package day19

import (
	"taeho/advc19_go/computer"
	"taeho/advc19_go/utils"
)

const FitSize = 100

type Mygrid struct {
	com     *computer.Computer
	visited map[string]bool
}

func (g *Mygrid) Get(x, y int) bool {
	thisStr := utils.NewVector2D(x, y).String()
	if v, ok := g.visited[thisStr]; ok {
		return v
	}
	com := g.com.Copy()
	com.AddInput(int64(x))
	com.AddInput(int64(y))
	com.RunProgram()
	val := com.PopOutput()
	ret := val == 1
	g.visited[thisStr] = ret
	return ret
}

func (g *Mygrid) DoesXFit(px, py int) bool {
	for x := 0; x < FitSize; x++ {
		if !g.Get(x+px, py) {
			return false
		}
	}
	return true
}

func (g *Mygrid) DoesYFit(px, py int) bool {
	for y := 0; y < FitSize; y++ {
		if !g.Get(px, py+y) {
			return false
		}
	}
	return true
}

func FindFit(str string) int {
	g := &Mygrid{
		com:     computer.NewComputer(str),
		visited: map[string]bool{},
	}

	// Takes 33secs without these... no idea how to optimise it atm
	lastXStart := 683
	startY := 936
	for y := startY; ; y++ {
		thisXStart := -1
		g.visited = map[string]bool{}
		for x := lastXStart; x < lastXStart+100; x++ {
			if g.Get(x, y) {
				if thisXStart == -1 {
					thisXStart = x
				}
				if g.DoesXFit(x, y) {
					if g.DoesYFit(x, y) {
						return x*10000 + y
					}
					thisXStart = x
				}
			} else {
				if thisXStart >= 0 {
					break
				}
			}
		}
		lastXStart = thisXStart
	}
}
