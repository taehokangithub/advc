package day20

import (
	"fmt"
	"taeho/advc19_go/utils"
	"testing"
)

const MZ1 string = `         A           
         A           
  #######.#########  
  #######.........#  
  #######.#######.#  
  #######.#######.#  
  #######.#######.#  
  #####  B    ###.#  
BC...##  C    ###.#  
  ##.##       ###.#  
  ##...DE  F  ###.#  
  #####    G  ###.#  
  #########.#####.#  
DE..#######...###.#  
  #.#########.###.#  
FG..#########.....#  
  ###########.#####  
             Z       
             Z       `

func TestPortalNameContainer(t *testing.T) {
	p := NewPortalNameContainer()

	p.AddPartialName('A', utils.NewVector2D(3, 3))
	p.AddPartialName('G', utils.NewVector2D(10, 11))
	p.AddPartialName('H', utils.NewVector2D(10, 10))
	p.AddPartialName('I', utils.NewVector2D(20, 20))
	p.AddPartialName('K', utils.NewVector2D(19, 20))
	p.AddPartialName('I', utils.NewVector2D(30, 20))
	p.AddPartialName('B', utils.NewVector2D(3, 4))
	p.AddPartialName('J', utils.NewVector2D(31, 20))

	expectedNames := map[string]bool{
		"AB": true,
		"GH": true,
		"IK": true,
		"IJ": true,
	}

	for _, pg := range p.portalGates {
		if _, ok := expectedNames[pg.name]; !ok {
			t.Error("Unexpected name", pg.name)
		}
	}
}

func TestPortalCreation(t *testing.T) {
	m := NewMaze(MZ1)
	for v, portal := range m.portals {
		fmt.Println(v, portal)
	}
	fmt.Println(m.entrance, m.exit)
	if m.entrance.X != 9 || m.entrance.Y != 2 {
		t.Error("entrance loc is wrong", m.entrance, "expected 9,2")
	}
	if m.exit.X != 13 || m.exit.Y != 16 {
		t.Error("exit loc is wrong", m.exit, "expected 13, 16")
	}
	sampleLoc := utils.NewVector2D(11, 12)
	if _, ok := m.portals[sampleLoc]; !ok {
		t.Error("Sample loc", sampleLoc, "does not exist in portals")
	}
}
