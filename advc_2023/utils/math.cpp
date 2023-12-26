
#pragma once

#include <assert.h>
#include <sstream>

#include "math.h"

using namespace std;

namespace advc_2023::utils
{
    bool is_almost_zero(const double val)
    { 
        constexpr double ZERO_MARGIN = 0.000001;
        return std::abs(val) < ZERO_MARGIN; 
    }

    bool is_almost_same(const double val1, const double val2)
    {
        return is_almost_zero(std::abs(val1 / val2) - 1);
    }

    std::vector<double> gaussian_elimination(const std::vector<std::vector<double>>& input_vector)
    {
        // for example, ax + by + cw + dz = e;
        // requires 4 sets of a, b, c, d, e inputs, consisting 4 * 5 vector 

        auto my_vector = input_vector;

        const int rows = (int)my_vector.size();
        const int colums = (int)my_vector.front().size();

        assert(colums == rows + 1);
        
        auto ans = vector<double>();


        for (int y = 0; y < rows; y++)
        {
            auto& row = my_vector[y];

            double diagonal_column = row[y];
            if (is_almost_zero(diagonal_column))
            {
                // find a non-zero row and swap
                for (int another_row_index = y + 1; another_row_index < rows; another_row_index++)
                {
                    auto& another_row = my_vector[another_row_index];
                    if (!is_almost_zero(another_row[y]))
                    {
                        std::swap(row, another_row);
                        break;
                    }
                }

                diagonal_column = row[y];

                if (is_almost_zero(diagonal_column))
                {
                    return vector<double>(); // can't solve the equation
                }
            }

            // make the diagonal column value 1
            for (auto& row_val : row)
            {
                row_val /= diagonal_column;
            }

            // try to eliminate other rows 
            for (int other_row_index = 0; other_row_index < rows; other_row_index++)
            {
                if (y != other_row_index)
                {
                    auto& other_row = my_vector[other_row_index];
                    const auto divide = other_row[y];
                    for (int column = 0; column < colums; column ++)
                    {
                        other_row[column] -= row[column] * divide;
                    }
                }
            }
        }

        for (const auto& row : my_vector)
        {
            ans.push_back(row.back());
        }

#ifndef _DEBUG
        for (const auto& row : input_vector)
        {
            double sum = 0;
            for (int x = 0; x < (int)ans.size(); x++)
            {
                sum += row[x] * ans[x];
            }
            assert(is_almost_same(sum, row.back()));
        }
#endif

        return ans;
    }
}