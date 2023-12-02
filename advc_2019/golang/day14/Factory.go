package day14

import "fmt"

type Factory struct {
	bl      *Blueprints
	residue *Residue
	cost    int64
}

func NewFactory(str string) *Factory {
	fa := Factory{
		bl:      ParseBlueprint(str),
		residue: NewResidue(),
	}
	return &fa
}

func (f *Factory) Copy() *Factory {
	copied := Factory{
		bl:      f.bl,
		residue: f.residue.Copy(),
		cost:    f.cost,
	}
	return &copied
}

func (f *Factory) ProduceOneFuel() (consumedOre int64) {
	consumedOre = f.ProduceFuel(int64(1))
	return
}

func (f *Factory) ProduceFuel(cnt int64) (consumedOre int64) {
	f.produceInternal(f.bl.ingrMap[INGR_FUEL], cnt)
	consumedOre = int64(f.cost)
	return
}

func (f *Factory) produceInternal(ingr *Ingredient, cnt int64) {
	if ingr.name == INGR_ORE {
		f.cost += cnt
		return
	}

	residue := f.residue.GetCount(ingr)

	if residue >= cnt {
		f.residue.Remove(ingr, cnt)
		return
	}

	cnt -= residue
	f.residue.Remove(ingr, residue)

	if ingr.eq.output.ingredient != ingr {
		panic(fmt.Sprintf("produceIternal : ingr %s's eq is for %s", ingr.name, ingr.eq.output.ingredient.name))
	}

	requiredSets := int64(1)
	predefinedSetSize := int64(ingr.eq.output.cnt)
	if predefinedSetSize < cnt {
		requiredSets = 1 + (cnt-1)/predefinedSetSize
	}

	for _, eqPart := range ingr.eq.input {
		f.produceInternal(eqPart.ingredient, int64(eqPart.cnt)*requiredSets)
	}

	residue = requiredSets*predefinedSetSize - cnt

	if residue < 0 {
		panic(fmt.Sprintf("Residue < 0 !!! Ingr %s setCount %d requiredSets %d cnt %d => residue %d", ingr.name, predefinedSetSize, requiredSets, cnt, residue))
	} else if residue > 0 {
		f.residue.Add(ingr, residue)
	}
}

func (f *Factory) ConsumeOre(oreCnt int64) (fuels int64) {
	fuels = 0
	fptr := f
	magnitude := int64(1000000)

	for fptr.cost < oreCnt && magnitude > 0 {
		copied := fptr.Copy()
		copied.ProduceFuel(magnitude)

		if copied.cost > oreCnt {
			magnitude /= 10
			continue
		}

		fuels += magnitude
		fptr = copied
	}
	return
}
