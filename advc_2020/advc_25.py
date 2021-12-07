
#mod = 20201227
def findLoopCnt(subjectNumber, publicKey, mod):
	loop = 1
	curVal = subjectNumber
	while curVal != publicKey:
		loop += 1
		curVal = curVal * subjectNumber % mod

		#print (publicKey, "Loop", loop, "curVal", curVal)

	return loop

def getSharedKey(subjectNumber, loop, mod):
	ret = 1
	for i in range(loop) :
		ret = ret * subjectNumber % mod
	return ret

def main():
	subject = 7
	mod = 20201227
	#keys = [5764801, 17807724]
	keys = [11404017, 13768789]

	loop0 = findLoopCnt(subject, keys[0], mod)
	loop1 = findLoopCnt(subject, keys[1], mod)

	print("LoopCounts : ", loop0, loop1)

	shared1 = getSharedKey(keys[0], loop1, mod)
	shared2 = getSharedKey(keys[1], loop0, mod)

	print("shared keys", shared1, shared2)


main()
