package day14

import (
	"strconv"
	"strings"
)

type Ingredient struct {
	name string
	eqs  []*Equation // Equations that produces itself
}

type EqPart struct {
	ingredient *Ingredient
	cnt        int
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
		eqs:  make([]*Equation, 0),
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
	if eq.output.ingredient.name == "FUEL" {
		blueprints.fuelEq = eq
	}

	eq.output.ingredient.eqs = append(eq.output.ingredient.eqs, eq)
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
		cnt:        cnt,
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
