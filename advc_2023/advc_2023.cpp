#include <chrono>
#include <iostream>

#include "solutions/day01/day01.h"
#include "solutions/day02/day02.h"
#include "solutions/day03/day03.h"
#include "solutions/day04/day04.h"
#include "solutions/day05/day05.h"
#include "solutions/day06/day06.h"
#include "solutions/day07/day07.h"
#include "solutions/day08/day08.h"
#include "solutions/day09/day09.h"
#include "solutions/day10/day10.h"
#include "solutions/day11/day11.h"
#include "solutions/day12/day12.h"
#include "solutions/day13/day13.h"
#include "solutions/day14/day14.h"
#include "solutions/day15/day15.h"
#include "solutions/day16/day16.h"
#include "solutions/day17/day17.h"
#include "solutions/day18/day18.h"

using namespace advc_2023;
using namespace std;

int main()
{
    const auto start_time = chrono::high_resolution_clock::now();

    cout << "Starting Advent of Code 2023" << endl;

    day01::solve();
    day02::solve();
    day03::solve();
    day04::solve();
    day05::solve();
    day06::solve();
    day07::solve();
    day08::solve();
    day09::solve();
    day10::solve();
    day11::solve();
    day12::solve();
    day13::solve();
    day14::solve();
    day15::solve();
    day16::solve();
    day17::solve();
    day18::solve();

    const auto end_time = chrono::high_resolution_clock::now();
    const auto duration = chrono::duration_cast<std::chrono::milliseconds>(end_time - start_time);

    cout << "Finished! took " << duration.count() << " ms" << endl;
}
