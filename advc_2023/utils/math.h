
#pragma once

#include <vector>

namespace advc_2023::utils
{
    bool is_almost_zero(double val);

    bool is_almost_same(double val1, double val2);

    std::vector<double> gaussian_elimination(const std::vector<std::vector<double>>& input_vector);
}