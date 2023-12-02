package day20

import "taeho/advc19_go/utils"

type VisitMap struct {
	data map[int]map[string]bool
}

func NewVisitMap() *VisitMap {
	return &VisitMap{
		data: make(map[int]map[string]bool),
	}
}

func (v *VisitMap) SetVisited(layer int, loc *utils.Vector) {
	if _, ok := v.data[layer]; !ok {
		v.data[layer] = map[string]bool{}
	}
	v.data[layer][loc.String()] = true
}

func (v *VisitMap) GetVisited(layer int, loc *utils.Vector) bool {
	if _, ok := v.data[layer]; ok {
		return v.data[layer][loc.String()]
	}
	return false
}
