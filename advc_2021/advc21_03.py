def solve1(lines):
	cntZeros = [0] * len(lines[0])
	for line in lines:
		for i in range(len(cntZeros)):
			if line[i] == '0':
				cntZeros[i] += 1
	
	gammaRate = 0
	epsilonrate = 0
	midValue = len(lines) / 2
	maxPos = len(cntZeros) - 1

	print("total lines", len(lines))
	print("count of zeros", cntZeros)
	print("Midvalue", midValue)
	
	for i, c in enumerate(cntZeros):
		val = 2 ** (maxPos - i)
		if c > midValue :
			gammaRate += val
		else:
			epsilonrate += val
		#print("i", i, "val", val, "c", c, "gamma", gammaRate, "epsilon", epsilonrate)
	
	print("Gamma", gammaRate, "Epsilon", epsilonrate, "ans", gammaRate * epsilonrate)


def elimination(lines, pos, doesNeedMostCommon):
	cntZero = 0
	for line in lines:
		if line[pos] == '0':
			cntZero += 1
	
	winner = '0' if (doesNeedMostCommon == (cntZero > len(lines) / 2)) else '1'

	#print("[Most Common]", doesNeedMostCommon, "pos", pos, "winner", winner)

	toRemove = []
	for line in lines:
		if line[pos] != winner:
			toRemove.append(line)

	for line in toRemove:
		lines.remove(line)

	#print("     remaining lines ", len(lines))
	return lines

def solve2(lines):
	width=len(lines[0])
	linesOrg = lines.copy()

	oxygen = ''
	co2 = ''

	for i in range(width):
		lines = elimination(lines, i, True)
		if len(lines) == 1 :
			oxygen = lines[0]
			break;
	
	lines = linesOrg
	for i in range(width):
		lines = elimination(lines, i, False)
		if len(lines) == 1 :
			co2 = lines[0]
			break;
	
	print("oxy", oxygen, "co2", co2)
	print("ans = ", int(oxygen, 2) * int(co2, 2))
	

def main(filename):
	fo = open(filename, "r")
	lines = fo.read().splitlines()
	solve1(lines)
	solve2(lines)


main("../../../data/advc21_03.txt");

