package day20

import (
	"fmt"
	"taeho/advc19_go/utils"
)

type PortalGate struct {
	// Looks almost identical with Portal, but this is a single gate such as "AB" with locations of A and B
	// Whiles Portal's 2 locations are the portal-jumping endpoints
	locs [2]utils.Vector
	name string
}

func (g *PortalGate) String() string {
	return fmt.Sprint("[", g.name, ":", g.locs[0], g.locs[1], "]")
}

func (g *PortalGate) GetProperLoc(m *Maze) utils.Vector {
	locs := []utils.Vector{}
	for _, dvec := range utils.DIR_VECTORS {
		for _, loc := range g.locs {
			movedLoc := dvec.GetAdded(loc)
			if m.grid.IsValidVector(movedLoc) && m.grid.GetFast(&movedLoc) == TILE_ROAD {
				locs = append(locs, movedLoc)
			}
		}
	}
	if len(locs) != 1 {
		panic(fmt.Sprintln("Invalid number of roads", len(locs), "around portalGate", g.String()))
	}
	return locs[0]
}

type PortalNameContainer struct {
	locs        map[utils.Vector]byte
	portalGates []*PortalGate
}

func NewPortalNameContainer() *PortalNameContainer {
	return &PortalNameContainer{
		locs:        make(map[utils.Vector]byte),
		portalGates: make([]*PortalGate, 0),
	}
}

func (p *PortalNameContainer) AddPartialName(c byte, loc utils.Vector) {
	for _, dvec := range utils.DIR_VECTORS {
		movedVec := dvec.GetAdded(loc)
		if otherC, ok := p.locs[movedVec]; ok {
			name := fmt.Sprint(string(otherC), string(c))
			pn := &PortalGate{
				locs: [2]utils.Vector{movedVec, loc},
				name: name,
			}
			p.portalGates = append(p.portalGates, pn)
		} else {
			p.locs[loc] = c
		}
	}
}

func (p *PortalNameContainer) SetPortals(m *Maze) {
	localPortals := map[string]*Portal{}
	for _, g := range p.portalGates {
		loc := g.GetProperLoc(m)
		var tile byte
		if g.name == "AA" {
			tile = TILE_ENTRANCE
			m.entrance = loc
		} else if g.name == "ZZ" {
			tile = TILE_EXIT
			m.exit = loc
		} else {
			if portal, ok := localPortals[g.name]; ok {
				portal.locB = loc
			} else {
				localPortals[g.name] = &Portal{
					name: g.name,
					locA: loc,
				}
			}
			tile = TILE_PORTAL
		}
		m.grid.SetFast(&loc, tile)
	}
	for _, portal := range localPortals {
		m.portals[portal.locA] = portal
		m.portals[portal.locB] = portal
	}
}
