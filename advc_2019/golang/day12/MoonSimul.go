package day12

import (
	"fmt"
	"strconv"
	"strings"
	"taeho/advc19_go/utils"
)

type Moon struct {
	pos utils.Vector
	vel utils.Vector
}

type Bodies struct {
	data []*Moon
	turn int64
}

func applyGravityTemplate(posA, posB int, velA, velB *int) {
	if posA < posB {
		*(velA)++
		*(velB)--
	} else if posA > posB {
		*(velA)--
		*(velB)++
	}
}

func (m *Moon) String() string {
	return fmt.Sprintf("<[pos:%v][vel:%v]>", m.pos, m.vel)
}

func (m *Moon) applyGravity(other *Moon) {
	applyGravityTemplate(m.pos.X, other.pos.X, &m.vel.X, &other.vel.X)
	applyGravityTemplate(m.pos.Y, other.pos.Y, &m.vel.Y, &other.vel.Y)
	applyGravityTemplate(m.pos.Z, other.pos.Z, &m.vel.Z, &other.vel.Z)
}

func (m *Moon) applyVelocity() {
	m.pos.Add(m.vel)
}

func NewBodies(str string) *Bodies {
	str = strings.Replace(str, "\r", "", -1)
	lines := strings.Split(str, "\n")

	bodies := Bodies{
		data: make([]*Moon, len(lines)),
	}

	for moonIndex, line := range lines {
		line = strings.TrimSpace(line)
		line = strings.TrimPrefix(line, "<")
		line = strings.TrimSuffix(line, ">")
		parts := strings.Split(line, ", ")
		values := make([]int, 3)
		for i, part := range parts {
			split := strings.Split(part, "=")
			values[i], _ = strconv.Atoi(split[1])
		}
		bodies.data[moonIndex] = &Moon{
			pos: utils.NewVector3D(values[0], values[1], values[2]),
			vel: utils.NewVector3D(0, 0, 0),
		}
	}

	return &bodies
}

func (b *Bodies) String() string {
	builder := strings.Builder{}
	builder.WriteString("{\n")
	for _, m := range b.data {
		builder.WriteString(fmt.Sprintf("    %v\n", m))
	}
	builder.WriteString("}\n")
	return builder.String()
}

func (b *Bodies) TotalEnergy() int {
	ret := 0
	for _, m := range b.data {
		ret += m.pos.ManhattanDistance() * m.vel.ManhattanDistance()
	}
	return ret
}

func (b *Bodies) SimulateOneTurn() {
	b.applyGravity()
	b.applyVelocity()
	b.turn++
}

func (b *Bodies) Copy() *Bodies {
	ret := Bodies{data: make([]*Moon, len(b.data))}

	for i := range b.data {
		v := *b.data[i]
		ret.data[i] = &v
	}

	return &ret
}

func (b *Bodies) GetReturnToBase() int64 {
	ca := b.getReturnToBaseAt(utils.COORD_X)
	cb := b.getReturnToBaseAt(utils.COORD_Y)
	cc := b.getReturnToBaseAt(utils.COORD_Z)

	cab := ca * cb / utils.Gcd(ca, cb)
	ret := cab * cc / utils.Gcd(cab, cc)
	return ret
}

func (b *Bodies) getReturnToBaseAt(coord utils.CoordType) int64 {
	copied := b.Copy()

	for found := false; !found; {
		copied.SimulateOneTurn()
		found = true
		for i := range b.data {
			ma, mb := b.data[i], copied.data[i]
			if ma.pos.GetValueAt(coord) != mb.pos.GetValueAt(coord) ||
				ma.vel.GetValueAt(coord) != mb.vel.GetValueAt(coord) {
				found = false
				break
			}
		}
	}

	return copied.turn
}

func (b *Bodies) Simulate(turns int) {
	for i := 0; i < turns; i++ {
		b.SimulateOneTurn()
	}
}

func (b *Bodies) applyGravity() {
	nBodies := len(b.data)
	for i := 0; i < nBodies-1; i++ {
		for j := i + 1; j < nBodies; j++ {
			b.data[i].applyGravity(b.data[j])
		}
	}
}

func (b *Bodies) applyVelocity() {
	for _, b := range b.data {
		b.applyVelocity()
	}
}
