#include <chrono>
#include <iostream>

#include "solutions/day01/day01.h"
#include "solutions/day02/day02.h"

using namespace advc_2023;
using namespace std;

int main()
{
    const auto start_time = chrono::high_resolution_clock::now();

    cout << "Starting Advent of Code 2023" << endl;

    day01::solve();
    day02::solve();

    const auto end_time = chrono::high_resolution_clock::now();
    const auto duration = chrono::duration_cast<std::chrono::milliseconds>(end_time - start_time);

    cout << "Finished! took " << duration.count() << " ms" << endl;
}
