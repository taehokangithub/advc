package day14

import (
	"fmt"
	"strconv"
	"strings"
)

const (
	INGR_FUEL = "FUEL"
	INGR_ORE  = "ORE"
)

type Ingredient struct {
	name string
	eq   *Equation
}

type EqPart struct {
	ingredient *Ingredient
	cnt        int64
}

type Equation struct {
	input  []*EqPart
	output *EqPart
}

type Blueprints struct {
	ingrMap map[string]*Ingredient
	eqs     []*Equation
	fuelEq  *Equation
}

func newIngredient(name string) *Ingredient {
	return &Ingredient{
		name: name,
	}
}

func newEquation() *Equation {
	return &Equation{
		input: make([]*EqPart, 0),
	}
}

func (blueprints *Blueprints) GetOrCreateIngredient(name string) *Ingredient {
	if ingr, ok := blueprints.ingrMap[name]; ok {
		return ingr
	}

	ingr := newIngredient(name)
	blueprints.ingrMap[name] = ingr
	return ingr
}

func (blueprints *Blueprints) AddEquationFromString(str string) {
	strs := strings.Split(str, " => ")
	inputStrs := strings.Split(strs[0], ", ")

	eq := newEquation()
	for _, inputStr := range inputStrs {
		eqPart := blueprints.parseEqPart(inputStr)
		eq.input = append(eq.input, eqPart)
	}

	eq.output = blueprints.parseEqPart(strs[1])

	blueprints.eqs = append(blueprints.eqs, eq)
	outgr := eq.output.ingredient
	if outgr.name == "FUEL" {
		blueprints.fuelEq = eq
	}

	if outgr.eq != nil {
		panic(fmt.Sprintf("%s already has an eq %v", outgr.name, eq))
	}

	outgr.eq = eq
}

func (blueprints *Blueprints) parseEqPart(str string) *EqPart {
	strs := strings.Split(str, " ")
	cnt, err := strconv.Atoi(strs[0])
	if err != nil {
		panic(err)
	}
	name := strs[1]

	ingr := blueprints.GetOrCreateIngredient(name)
	eqPart := EqPart{
		ingredient: ingr,
		cnt:        int64(cnt),
	}
	return &eqPart
}

func ParseBlueprint(str string) *Blueprints {
	blueprints := &Blueprints{
		ingrMap: make(map[string]*Ingredient),
		eqs:     make([]*Equation, 0),
	}

	str = strings.Replace(str, "\r", "", -1)
	lines := strings.Split(str, "\n")

	for _, line := range lines {
		blueprints.AddEquationFromString(line)
	}

	return blueprints
}
