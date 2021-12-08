declare var require: any
export {};

const fs = require("fs");

namespace advc21_08
{
    class PredefinedDigit
    {
        m_wires : string;
        m_value : number;
        static s_digitMap : { [key:string] : number } = {};
        static s_digits : PredefinedDigit[] = [];
        static s_lengthPattern : { [key:number] : string } = {};

        constructor(value : number, wires : string)
        {
            this.m_value = value;
            this.m_wires = wires;
        }

        static intersectString(a : string, b : string) : string 
        {
            if (a == undefined) 
                return b;

            if (b == undefined)
                return a;

            return a.split('').filter(x => b.indexOf(x) >= 0).join('');
        }

        static initialise()
        {
            PredefinedDigit.s_digits.push(new PredefinedDigit(0, 'ABCEFG'));    // 6
            PredefinedDigit.s_digits.push(new PredefinedDigit(1, 'CF'));        // 2
            PredefinedDigit.s_digits.push(new PredefinedDigit(2, 'ACDEG'));     // 5
            PredefinedDigit.s_digits.push(new PredefinedDigit(3, 'ACDFG'));     // 5
            PredefinedDigit.s_digits.push(new PredefinedDigit(4, 'BCDF'));      // 4
            PredefinedDigit.s_digits.push(new PredefinedDigit(5, 'ABDFG'));     // 5
            PredefinedDigit.s_digits.push(new PredefinedDigit(6, 'ABDEFG'));    // 6
            PredefinedDigit.s_digits.push(new PredefinedDigit(7, 'ACF'));       // 3
            PredefinedDigit.s_digits.push(new PredefinedDigit(8, 'ABCDEFG'));   // 7
            PredefinedDigit.s_digits.push(new PredefinedDigit(9, 'ABCDFG'));    // 6

            PredefinedDigit.s_digits.forEach(pd =>
                {
                    const pattern = pd.m_wires;
                    const length = pattern.length;
                    const s_lengthPattern = PredefinedDigit.s_lengthPattern;

                    s_lengthPattern[length] = PredefinedDigit.intersectString(s_lengthPattern[length], pattern);

                    PredefinedDigit.s_digitMap[pd.m_wires] = pd.m_value;
                });
            /*
            Object.keys(PredefinedDigit.s_lengthPattern).forEach((length : string) =>
                console.log(`[init] pre-digit pattern length ${length} = ${PredefinedDigit.s_lengthPattern[parseInt(length)]}`)
            );
            */
        }
    }

    class WireInQuestion
    {
        m_name : string;
        m_candidates : string = ""; // candidate characters

        constructor(name : string)
        {
            this.m_name = name;
        }

        addCandidate(candi : string)
        {
            if (this.m_candidates.length)
            {
                const result = PredefinedDigit.intersectString(candi, this.m_candidates);

                //console.log(`${this.m_name} adding ${candi} into ${this.m_candidates} result ${result}`);

                this.m_candidates = result;
            }
            else
            {
                //console.log(`${this.m_name} init with ${candi}`);

                this.m_candidates = candi;
            }
            
        }

        removeCandidate(candi : string)
        {
            const newCandi = this.m_candidates.replace(candi, "");

            if (newCandi != this.m_candidates)
            {
                this.m_candidates = newCandi;
            }
        }

        isConfirmed() : boolean
        {
            return this.m_candidates.length == 1;
        }
    };

    class WireGuesser
    {
        m_guessWires : {[key:string] : WireInQuestion} = {};
        m_guessLength : {[key:number] : string} = {};
        m_solved : number[] = [];
        m_text : string;

        constructor(text : string)
        {
            this.m_text = text;

            "abcdefg".split('').forEach(w =>
                {
                    this.m_guessWires[w] = new WireInQuestion(w);
                });
        }

        addPattern(pattern : string)
        {
            this.m_guessLength[pattern.length] = PredefinedDigit.intersectString(this.m_guessLength[pattern.length], pattern);
        }

        printAllGuesses()
        {
            Object.values(this.m_guessWires).forEach(g =>
                {
                    console.log(`[${g.m_name}] => ${g.m_candidates}`);
                });
        }

        guess()
        {
            //console.log(`Guessing line ${this.m_text}`);

            Object.keys(this.m_guessLength).forEach(lenstr =>
                {
                    const len = parseInt(lenstr);
                    const guessedByLen = this.m_guessLength[len];
                    const guessTarget = PredefinedDigit.s_lengthPattern[len];

                    guessedByLen.split('').forEach(c => 
                        {
                            this.m_guessWires[c].addCandidate(guessTarget);
                        });
                });

            let hasChanged = true;
            const visited = new Set();

            while(hasChanged)
            {
                hasChanged = false;

                Object.values(this.m_guessWires).forEach(guess =>
                    {
                        if (guess.isConfirmed() && !visited.has(guess.m_name))
                        {
                            hasChanged = true;
                            visited.add(guess.m_name);

                            Object.values(this.m_guessWires).forEach(other =>
                                {
                                    if (other.m_name != guess.m_name)
                                    {
                                        other.removeCandidate(guess.m_candidates);
                                    }
                                });
                        }
                    });
            }

            //this.printAllGuesses();
        }

        solveDigits(encodedDigits : string[])
        {
            encodedDigits.forEach(ed =>
                {
                    let decoded : string[] = [];
                    ed.split('').forEach(c => 
                        {
                            decoded.push(this.m_guessWires[c].m_candidates);
                        })

                    const decodedStr = decoded.sort().join('');

                    const digit = PredefinedDigit.s_digitMap[decodedStr];

                    //console.log(`Solving ${ed} => ${decodedStr} => ${digit}`);

                    this.m_solved.push(digit);
                });
        }

    }

    // ---------------------------------------------------------
    function solvePart1(guessers : WireGuesser[])
    {
        let sum = 0;

        guessers.forEach(guesser =>
            {
                console.log(guesser.m_solved);

                guesser.m_solved.forEach(v =>
                    {
                        if ([1,4,7,8].indexOf(v) >= 0)
                        {
                            sum ++;
                        }
                    })
            });

        console.log("Part 1 answer = " + sum);
    }

    // ---------------------------------------------------------
    function solvePart2(guessers : WireGuesser[])
    {
        let sum = 0;

        guessers.forEach(guesser =>
            {
                sum += parseInt(guesser.m_solved.map(x => x.toString()).join(''));
            });

        console.log("Part 2 answer = " + sum);
    }

    // ---------------------------------------------------------
    function main(path : string)
    {
        const text : string = fs.readFileSync(path,"utf8");
        const lines : string[] = text.split('\n');

        PredefinedDigit.initialise();

        const guessers : WireGuesser[] = [];

        lines.forEach((line : string) =>
            {
                if (line.length)
                {
                    const parts = line.split(' | ');
                    const patterns = parts[0].split(' ');
                    const digits = parts[1].replace(/\r/g, "") .split(' ' );
    
                    const wireGuesser = new WireGuesser(line);
                    patterns.forEach(p => wireGuesser.addPattern(p));
    
                    wireGuesser.guess();
                    wireGuesser.solveDigits(digits);
                    guessers.push(wireGuesser);
                }
            });

        solvePart1(guessers);
        solvePart2(guessers);
    }

    main("../../data/advc21_08_sample.txt");
    main("../../data/advc21_08.txt");
}

