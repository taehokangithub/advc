def defaultFishDic(bornTo):
	fishes = {}
	for i in range(bornTo + 1):
		fishes[i] = 0
	return fishes	

def solveOneFish(n, maxDate, resetTo, bornTo):
	fishes = defaultFishDic(bornTo)
	fishes[n] = 1

	#print("solving ", n, "initial fishes", fishes)
	for i in range(maxDate):
		newFishes = defaultFishDic(bornTo)

		for k in range(len(fishes)):
			if (k == 0) :
				newFishes[bornTo] += fishes[0]
				newFishes[resetTo] += fishes[0]
			else :
				newFishes[k - 1] += fishes[k]
		fishes = newFishes
		#print("day", i + 1, fishes)
	
	ret = sum(fishes.values())
	return ret

def buildSimulationResult(maxDate, resetTo, bornTo):
	results = [0]
	for i in range(resetTo):
		result = solveOneFish(i + 1, maxDate, resetTo, bornTo)
		results.append(result)

	print("Each simulation results : ", results)
	return results

def solve(data, maxDate):
	arr = map(int, data.split(","))
	results = buildSimulationResult(maxDate, 6, 8)

	ans = 0
	for v in arr:
		ans += results[v]

	print("Answer for", maxDate, "days =", ans)

def main(filename):
	sample = "3,4,3,1,2"
	fo = open(filename, "r")
	data = fo.read()

	solve(data, 80)
	solve(data, 256)


main("../../../data/advc21_06.txt")

