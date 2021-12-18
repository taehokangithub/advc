from typing import DefaultDict

def solve(numbers, maxIdx):
	occur = DefaultDict(lambda: -1)

	for i in range(len(numbers) - 1):
		occur[numbers[i]] = i

	for i in range(len(numbers), maxIdx):
		thisNum = numbers[-1]
		lastOccur = occur[thisNum]
		thisOccur = 0 if lastOccur < 0 else (i - lastOccur - 1)
		numbers.append(thisOccur)
		occur[thisNum] = i - 1

	print(numbers[-1])

solve([13,0,10,12,1,5,8], 2020)
solve([13,0,10,12,1,5,8], 30000000)