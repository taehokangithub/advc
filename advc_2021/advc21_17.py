import itertools

def getHighestY(minY):
    initialY = abs(minY) - 1
    return (sum(range(initialY + 1)))

def getMinSpdX(minX):
    for i in range(minX):
        if sum(range(i + 1)) >= minX:
            return i

def getAllPossible(minX, maxX, minY, maxY):

    resultList = set()

    def simulate(spdX, spdY):
        x, y = 0, 0
        initialSpdX, initialSpdY = spdX, spdY
        while y >= minY and x <= maxX:
            if x in range(minX, maxX + 1) and y in range (minY, maxY + 1):
                
                resultList.add((initialSpdX, initialSpdY))
                return True

            x += spdX
            y += spdY
            spdX -= 1 if spdX > 0 else 0
            spdY -= 1
        return False
    
    for x, y in itertools.product(range(getMinSpdX(minX), maxX + 1), range(minY, abs(minY))):
        simulate(x, y)

    return len(resultList)

def solveMain(minX,  maxX, minY, maxY):

    print("Solve1", getHighestY(minY))
    print("Solve2", getAllPossible(minX,  maxX, minY, maxY))

solveMain(20, 30, -10, -5)
solveMain(124, 174, -123, -86)
