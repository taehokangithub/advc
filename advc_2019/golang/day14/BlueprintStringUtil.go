package day14

import (
	"fmt"
	"strings"
)

const colorBlue string = "\033[34m"
const colorReset string = "\033[0m"

func (eqPart *EqPart) String() string {
	return fmt.Sprintf("[%s:%d]", eqPart.ingredient.name, eqPart.cnt)
}

func (eq *Equation) String() string {
	var builder strings.Builder

	for _, ingr := range eq.input {
		builder.WriteString(fmt.Sprint(ingr))
	}

	builder.WriteString(fmt.Sprintf("%s => %s", colorBlue, colorReset))
	builder.WriteString(fmt.Sprint(eq.output))

	return builder.String()
}

func (blueprints *Blueprints) String() string {
	var builder strings.Builder

	builder.WriteString(fmt.Sprintf("%d Ingredients : \n", len(blueprints.ingrMap)))
	for name, ingr := range blueprints.ingrMap {
		builder.WriteString(fmt.Sprintf("    [%s]%s %d target equations%s\n", name, colorBlue, len(ingr.eqs), colorReset))
	}

	builder.WriteString(fmt.Sprintf("%d Equations : \n", len(blueprints.eqs)))
	for _, eq := range blueprints.eqs {
		if eq == blueprints.fuelEq {
			builder.WriteString("***")
		} else {
			builder.WriteString("   ")
		}
		builder.WriteString(fmt.Sprintf("%v\n", eq))
	}

	return builder.String()
}
